# Relying party

El elemento `RelyingParty` declara el recorrido del usuario para la petición actual, la lista de claims que la aplicación necesita como parte del token generado o gestiones sobre la sesión.

## Generación de endpoint de UserInfo
El endpoint de UserInfo es parte del estándar la especificación de OIDC. Este endpoint devuelve claims del usuario actual y es utilizado cuando se requiere de una mayor cantidad de claims/información del usuario, dado que incluir todos estos claims en el token puede influir negativamente en el rendimiento de la aplicación.

### Añadir un proveedor de claims que valide el token y devuelva los claims necesarios
1. Generamos un nuevo `ClaimsProvider` bajo la sección `ClaimsProviders`
2. Creamos un `TechnicalProfile` encargado de validar el token. Los elementos mas importantes son:
    1. Protocol:
        1. Name: None
    2. Metadata:
      1. issuer: Entidad emisora del token,  https://b2cworkshoppc.b2clogin.com/567593d1-d2c8-4e1f-9a1d-b1527669ab11/v2.0/
      2. audience: receptor para el cual ha sido generado el token, [ "85377b08-d3ed-488b-b57c-391efa687573"]
      3. client_assertion_type: formato de la asercion (token JWT), urn:ietf:params:oauth:client-assertion-type:jwt-bearer
    3. CryptographicKeys:
        1. issuer_secret: StorageReferenceId="B2C_1A_TokenSigningKeyContainer"
    4. OutputClaims - listado de claims que se van a leer del AT
        1. ClaimTypeReferenceId="objectId" PartnerClaimType="sub"
        2. ClaimTypeReferenceId="signInNames.emailAddress" PartnerClaimType="email"
3. La `TechnicalProfile` tiene que ser similar a:
```xml
<TechnicalProfile Id="UserInfoAuthorization">
    <DisplayName>UserInfo authorization</DisplayName>
    <Protocol Name="None" />
    <InputTokenFormat>JWT</InputTokenFormat>
    <Metadata>
        <Item Key="issuer">https://b2cworkshoppc.b2clogin.com/567593d1-d2c8-4e1f-9a1d-b1527669ab11/v2.0/</Item>
        <Item Key="audience">[ "85377b08-d3ed-488b-b57c-391efa687573"]</Item>
        <Item Key="client_assertion_type">urn:ietf:params:oauth:client-assertion-type:jwt-bearer</Item>
    </Metadata>
    <CryptographicKeys>
        <Key Id="issuer_secret" StorageReferenceId="B2C_1A_TokenSigningKeyContainer" />
    </CryptographicKeys>
    <OutputClaims>
        <OutputClaim ClaimTypeReferenceId="objectId" PartnerClaimType="sub"/>
        <OutputClaim ClaimTypeReferenceId="signInNames.emailAddress" PartnerClaimType="email"/>
    </OutputClaims>
</TechnicalProfile>
```
4. Generamos un `TechnicalProfile` encargado de leer los claims que se quieren incluir del token de la bolsa de claims. Los elementos mas importantes son:
**IMPORTANTE: Los claims que se quieran incluir en el token han de ser persistidos o insertados de propio en la bolsa de claims**          
    1. Protocol:
        1. Name: None
    2. OutputTokenFormat: JSON
    3. CryptographicKeys:
        1. issuer_secret: StorageReferenceId="B2C_1A_TokenSigningKeyContainer"
    4. InputClaims: listado de claims a devolver en el endpoint
        1. objectId
        2. givenName
        3. surname
        4. displayName
        5. signInNames.emailAddress
5. La `TechnicalProfile` tiene que ser similar a:
```xml
<TechnicalProfile Id="UserInfoIssuer">
    <DisplayName>JSON Issuer</DisplayName>
    <Protocol Name="None" />
    <OutputTokenFormat>JSON</OutputTokenFormat>
    <CryptographicKeys>
        <Key Id="issuer_secret" StorageReferenceId="B2C_1A_TokenSigningKeyContainer" />
    </CryptographicKeys>
    <InputClaims>
        <InputClaim ClaimTypeReferenceId="objectId"/>
        <InputClaim ClaimTypeReferenceId="givenName"/>
        <InputClaim ClaimTypeReferenceId="surname"/>
        <InputClaim ClaimTypeReferenceId="displayName"/>
        <InputClaim ClaimTypeReferenceId="signInNames.emailAddress"/>
    </InputClaims>
    </TechnicalProfile>
```
### Generar UserJourney para el endpoint
1. En el elemento de `UserJourney`, incluimos como identificador de referencia del `Technical Profile` emisor de tokens por defecto la `TechnicalProfile` generada encargada de devolver los claims deseados con el atributo: `DefaultCpimIssuerTechnicalProfileReferenceId="UserInfoIssuer"`
2. Dentro del `UserJourney` declaramos un elemento `Authorization` con la `TechnicalProfile` encargada de validar el token JWT:
```xml
<Authorization>
    <AuthorizationTechnicalProfiles>
        <AuthorizationTechnicalProfile ReferenceId="UserInfoAuthorization" />
    </AuthorizationTechnicalProfiles>
</Authorization>
```
3. Declaramos dos pasos en el flujo. Primero leeremos los valores del usuario por medio del claim `objectId` el cual ha sido introducido en la bolsa de claims desde la `TechnicalProfile` que valida el token y como segundo y último paso incluiremos el paso `SendClaims` requerido
```xml
<OrchestrationSteps>
    <OrchestrationStep Order="1" Type="ClaimsExchange">
    <Preconditions>
        <Precondition Type="ClaimsExist" ExecuteActionsIf="false">
        <Value>objectId</Value>
        <Action>SkipThisOrchestrationStep</Action>
        </Precondition>
    </Preconditions>
    <ClaimsExchanges UserIdentity="false">
        <ClaimsExchange Id="AADUserReadWithObjectId" TechnicalProfileReferenceId="AAD-UserReadUsingObjectId" />
    </ClaimsExchanges>
    </OrchestrationStep>
    <OrchestrationStep Order="2" Type="SendClaims" CpimIssuerTechnicalProfileReferenceId="UserInfoIssuer" />
</OrchestrationSteps>
```

### Incluir el endpoint en la sección RelyingParty
Para exponer este endpoint en la política, incluimos la sección endpoint dentro de `RelyingParty`
```xml
<Endpoints>
    <Endpoint Id="UserInfo" UserJourneyReferenceId="UserInfoJourney" />
  </Endpoints>
```

## Prueba del endpoint
1. Validamos que el endpoint de autodiscovery muestra el endpoint de UserInfo en https://TENANT.b2clogin.com/TENANT.onmicrosoft.com/POLITICA/v2.0/.well-known/openid-configuration
1. Ejecutamos la política custom
2. Copiamos el token generado por la política
3. Desde Postman/Curl u otra herramienta similar, hacemos una petición a https://TENANT.b2clogin.com/TENANT.onmicrosoft.com/POLITICA/openid/v2.0/userinfo
4. Validamos que el endpoint devuelve los datos correctos
        
