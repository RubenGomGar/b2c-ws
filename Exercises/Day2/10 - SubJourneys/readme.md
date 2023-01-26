# SubJourneys

Las `SubJourneys` son conjuntos de pasos de orquestación que pueden ser invocados en cualquier punto de un `UserJourney`, esto permite la reutilización de pasos comunes entre varios flujos. Un `SubJourney` siempre será invocado desde un `UserJourney`.

Hay dos tipos de `SubJourneys`:
- Call: Devuelve siempre el control al paso de orquestación desde el que ha sido invocado
- Transfer: El control se transfiere al `SubJourney` y no vuelve nunca al paso de orquestación desde el que se ha invocado. El `SubJourney` requerirá siempre de un paso de orquestación del tipo `SendClaims`

Para este ejercicio vamos a reutilizar el escenario anterior y aplicaremos los distintos tipos de `SubJourneys` disponibles.

## Escenario
Un cliente nos pide un flujo en el que el usuario tenga que escribir la ciudad de procedendcia antes de realizar el registro.
Al obtener la ciudad, nos piden que validemos lo siguiente:

1. El valor Ciudad esté relleno
2. El valor Ciudad se Londres, Berlin o Seattle

Si se pasan las validaciones, procedemos a mostrar el formulario de registro con la claim de ciudad y el envío de claims.


## Generación del SubJourney de tipo Call

1. Definimos la cabecera de un `SubJourney` encargado de realizar el proceso de recogida y validación del campo `Ciudad`. Como queremos que el control una vez realizada la validación vuelva al flujo de usuario principal, lo decoramos con el atributo `Type="call"`
```xml
<SubJourney Id="ReadCityWValidation" Type="Call">
```
2. Incluimos en este `SubJourney` todos los pasos de orquestación propios de la recogida y validación del campo `Ciudad`. Es importante tener claro que al utilizar un `SubJourney` de tipo `Call` no necesitamos declarar un paso de orquestación del tipo `SendClaims`
```xml
<SubJourney Id="ReadCityWValidation" Type="Call">
      <OrchestrationSteps>
        <OrchestrationStep Order="1" Type="ClaimsExchange">
          <ClaimsExchanges>
            <ClaimsExchange Id="ReadCityValue" TechnicalProfileReferenceId="ReadCityValue" />
          </ClaimsExchanges>
        </OrchestrationStep>
        <OrchestrationStep Order="2" Type="ClaimsExchange">
          <Preconditions>
            <Precondition Type="ClaimsExist" ExecuteActionsIf="false">
              <Value>extension_City</Value>
              <Action>SkipThisOrchestrationStep</Action>
            </Precondition>
            <Precondition Type="ClaimEquals" ExecuteActionsIf="true">
              <Value>extension_City</Value>
              <Value>Londres</Value>
              <Action>SkipThisOrchestrationStep</Action>
            </Precondition>
            <Precondition Type="ClaimEquals" ExecuteActionsIf="true">
              <Value>extension_City</Value>
              <Value>Seattle</Value>
              <Action>SkipThisOrchestrationStep</Action>
            </Precondition>
            <Precondition Type="ClaimEquals" ExecuteActionsIf="true">
              <Value>extension_City</Value>
              <Value>Berlin</Value>
              <Action>SkipThisOrchestrationStep</Action>
            </Precondition>
          </Preconditions>
          <ClaimsExchanges>
            <ClaimsExchange Id="ShowError" TechnicalProfileReferenceId="ShowValidationError" />
          </ClaimsExchanges>
        </OrchestrationStep>
      </OrchestrationSteps>
    </SubJourney>
```

## Generación del SubJourney de tipo Transfer

1. Definimos la cabecera de un `SubJourney` encargado de realizar el proceso de registro de usuario. Como queremos que `SubJourney` sea el encargado de finalizar el proceso de registro lo decoramos con el atributo `Type="Call"`
```xml
<SubJourney Id="ReadCityWValidation" Type="Call">
```

2. Incluimos en este `SubJourney` todos los pasos de orquestación referentes al registro del usuario, así como un último paso del tipo `SendClaims` para marcar la finalización del proceso.

```xml
<SubJourney Id="RegisterUser" Type="Transfer">
    <OrchestrationSteps>
        <OrchestrationStep Order="1" Type="ClaimsExchange">
          <ClaimsExchanges>
            <ClaimsExchange Id="SignUpWithLogonEmailExchange" TechnicalProfileReferenceId="LocalAccountSignUpWithLogonEmail" />
          </ClaimsExchanges>
        </OrchestrationStep>
        <OrchestrationStep Order="2" Type="SendClaims" CpimIssuerTechnicalProfileReferenceId="JwtIssuer" />
      </OrchestrationSteps>
    </SubJourney>
```

## Uso de los SubJourneys en el UserJourney de la política
1. La declararación de los pasos de orquestación varía un poco respecto a lo visto anteriormente. En primer lugar hay que decorar el paso con el atributo `Type="InvokeSubJourney"` para indicar que se desea hacer la llamada. Como paso adicional, en lugar de incluir el elemento `ClaimsExchanges` haremos uso de `JourneyList` y `Candidate` para indicar el `SubJourney` al que queremos llamar. Los pasos de orquestación quedarían:
```xml
<OrchestrationStep Order="1" Type="InvokeSubJourney">
  <JourneyList>
    <Candidate SubJourneyReferenceId="ReadCityWValidation" />
  </JourneyList>
</OrchestrationStep>
<OrchestrationStep Order="2" Type="InvokeSubJourney">
  <JourneyList>
    <Candidate SubJourneyReferenceId="RegisterUser" />
  </JourneyList>
</OrchestrationStep>
```

2. Como se ha mencionado inicialmente, todo `UserJourney` debe tener un último paso de orquestación del tipo `SendClaims`. Esto implica que en este flujo tenemos dos pasos de orquestacion del tipo `SendClaims`, el declarado en el flujo principal y un segundo en el `SubJourney` encargado del registro del usuario. El uso de `SubJourneys` del tipo `Transfer` requiere de un atributo adicional en el elemento `UserJourney` denominado `DefaultCpimIssuerTechnicalProfileReferenceId` el cual indica el identificador de referencia del `Technical Profile` emisor de tokens por defecto (JwtIssuer, SAML Token issuer u OAuth2 custom error).
```xml
<UserJourney Id="CustomizeUserInput" DefaultCpimIssuerTechnicalProfileReferenceId="JwtIssuer">
```