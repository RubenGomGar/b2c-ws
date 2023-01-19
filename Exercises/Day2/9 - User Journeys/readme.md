# User Journey

Para este ejercicio vamos a partir del siguiente escenario.

Un cliente nos pide un flujo en el que el usuario tenga que escribir la ciudad de procedendcia antes de realizar el registro.
Al obtener la ciudad, nos piden que validemos lo siguiente:

1. El valor Ciudad esté relleno
2. El valor Ciudad se Londres, Berlin o Seattle

Si se pasan las validaciones, procedemos a mostrar el formulario de registro con la claim de ciudad y el envío de claims.


## Generación de los Orchestation Steps

1. El primer paso de orquestación tendrá que ser la llamada a nuestro technical profile `ReadCityValue`. Este technical profile muestra y recoge el valor de nuestra claim `extension_City`. El tipo de Orchestation Step será ClaimsExange ya que el tipo nos indica que el paso de orquestación intercambia claims con un proveedor de claims.

```xml
        <OrchestrationStep Order="1" Type="ClaimsExchange">
          <ClaimsExchanges>
            <ClaimsExchange Id="ReadCityValue" TechnicalProfileReferenceId="ReadCityValue" />
          </ClaimsExchanges>
        </OrchestrationStep>
```


2. Lo siguiente que queremos hacer es añadir un paso que valide que nuesta claim exista, y que el valor esté comprendido. Si no se cumplen las condiciones, llamaremos al technical profile `ShowValidationError` que muestra nuestro error de validación.

Para aplicar las precondiciones utilizaremos el elemento `Preconditions` con los tipos `ClaimsExist` para validar si la claim existe y `ClaimEquals` para validar el contenido de la claim `extension_City`

```xml
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
```

3. Llamamos a nuestro technical profile `LocalAccountSignUpWithLogonEmail`

```xml
        <OrchestrationStep Order="3" Type="ClaimsExchange">
          <ClaimsExchanges>
            <ClaimsExchange Id="SignUpWithLogonEmailExchange" TechnicalProfileReferenceId="LocalAccountSignUpWithLogonEmail" />
          </ClaimsExchanges>
        </OrchestrationStep>
```

4. Enviamos nuestrlas claims con un orchestation step de tipo `SendClaims`

```xml
    <OrchestrationStep Order="4" Type="SendClaims" CpimIssuerTechnicalProfileReferenceId="JwtIssuer" />
```