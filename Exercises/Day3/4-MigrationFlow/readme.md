# Flujo de migración

En este enscenario queremos replicar un escenario de migración. 
Imaginemos que nos piden que una aplicación que ya tiene una solución B2C aplicada quieren movera a nuestro B2C.

El flujo debería estar comupesto por la siguiente lógica

1. Mostramos formulario de SignIn
2. Si el usuario existe en nuestro AAD validamos sus credenciales
3. Si el usuario no existe en el AAD entonces autenticamos contra un api externa (Simulando el otro sistema de auth)
4. Si el api de auth nos da paso, entonces creamos el usuario en nuestro AAD con el usuario y la password introducidos.


> Para este ejercicio las apis ya las tenemos expuestas en cloud y estas son las definiciones

### Remote Login Api

- Url: https://b2capidemo.azurewebsites.net/api/remote-login
- Action: GET
- Authentication: None
- SendClaimsIn: Header
- Modelo de entrada: Mediante headers las propiedades

```c#
    [FromHeader] string signInName
    [FromHeader] string password
```

- modelo de salida:

```c#
    public class RemoteLoginResponse
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string DisplayName { get; set; }
    }
```


# Para construir el flujo necesitaríamos los siguientes pasos:

1. Creamos una claim para saber si el usuario es local
    - El Identificador de la claim deberá ser `isLocalUser`
    - DisplayName isLocalUser
    - Tipo de dato. boolean
    - Textos de ayuda. `isLocalUser`
2. Creamos una claim transformation que copie la claim de email
    - Identificador: `copyEmailFromSignin`.
    - Tipo: `CopyClaim`.
    - Claim de entrada: `signInName`, tipo de transformación `inputClaim`.
    - Claim de salida: `email`, tipo de transformación `outputClaim`
3. Creamos un technical profile que valide la autenticación remota
    - Identificador `login-Remote`
    - Protocolo: `RestfulProvider`
    - Metadata:
        + La definida en el api remote
        + `AllowInsecureAuthInProduction` true
        + `DefaultUserMessageIfRequestFailed` Invalid Remote User Login.
    - Claims de entrada, `signInName` y `password` como requeridas.
    - Cliams de salida: el modelo de salida del api definida.
    - Sesión gestionada por `SM-Noop`
4. Creamos un technical profile que valide si el usuario existe en nuestro AAD
    - Identificador `AAD-UserExists`
    - Metadata:
        + Operation: `Read`
        + RaiseErrorIfClaimsPrincipalDoesNotExist: `true`
    - Claims de entrada, `signInName` cogiendo como referencia padre la claim `signInNames.emailAddress`, requerida.
    - Cliams de salida: `isLocalUser`, valor por defecto `true` y configurada como AlwaysUseDefaultValue
    - Sesión gestionada por `AAD-Common`
5. Creamos un technical profile que escriba el usuario en el AAD
    - Identificador `Create-RemoteUserLocally`
    - Input Claim transformation `copyEmailFromSignin`
    - Persisted Claims `password`
    - Included Technical Profile `AAD-UserWriteUsingLogonEmail`
6. Consumimos el technical profile `SelfAsserted-LocalAccountSignin-Email` para añadir validaciones nuevas
    - Identificador `SelfAsserted-LocalAccountSignin-Email`
    - OutputClaim necesaria `isLocalUser`
    - Validation technical profile `AAD-UserExists` Continue `true` Error/Success
    - Validation technical profile `login-NonInteractive` bajo la precondición de que `isLocalUser` no exista.
    - Validation technical profile `login-Remote` bajo la precondición de que `isLocalUser` exista.
    - Validation technical profile `Create-RemoteUserLocally` bajo la precondición de que `isLocalUser` exista. Continue on Seccess False
7. Añadimos los steps de nuestra User Journey
    - STEP 1: Llamamos a nuestro technical profile `SelfAsserted-LocalAccountSignin-Email`
    - STEP 2: Enviamos las claims

<details>
   <summary>Creamos una claim para saber si el usuario es local: SPOILER</summary>
   <div class="description">

```xml
  <ClaimType Id="isLocalUser">
    <DisplayName>isLocalUser</DisplayName>
    <DataType>boolean</DataType>
    <UserHelpText />
  </ClaimType>
```
   </div>
</details>

<details>
   <summary>Creamos una claim transformation que copie la claim de email: SPOILER</summary>
   <div class="description">

```xml
  <ClaimsTransformation Id="copyEmailFromSignin" TransformationMethod="CopyClaim">
    <InputClaims>
      <InputClaim ClaimTypeReferenceId="signInName" TransformationClaimType="inputClaim" />
    </InputClaims>
    <OutputClaims>
      <OutputClaim ClaimTypeReferenceId="email" TransformationClaimType="outputClaim" />
    </OutputClaims>
  </ClaimsTransformation>
```
   </div>
</details>

<details>
   <summary>Creamos un technical profile que valide la autenticación remota: SPOILER</summary>
   <div class="description">

```xml
    <TechnicalProfile Id="login-Remote">
      <DisplayName>Remote Account SignIn</DisplayName>
      <Protocol Name="Proprietary" Handler="Web.TPEngine.Providers.RestfulProvider, Web.TPEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" />
      <Metadata>
        <Item Key="ServiceUrl">https://b2capidemo.azurewebsites.net/api/remote-login</Item>
        <Item Key="AuthenticationType">None</Item>
        <Item Key="SendClaimsIn">Header</Item>
        <Item Key="AllowInsecureAuthInProduction">true</Item>
        <Item Key="DefaultUserMessageIfRequestFailed">Invalid Remote User Login.</Item>
      </Metadata>
      <InputClaims>
        <InputClaim ClaimTypeReferenceId="signInName" Required="true" />
        <InputClaim ClaimTypeReferenceId="password" Required="true" />
      </InputClaims>
      <OutputClaims>
        <OutputClaim ClaimTypeReferenceId="email" PartnerClaimType="email" />
        <OutputClaim ClaimTypeReferenceId="displayName" PartnerClaimType="displayName" />
        <OutputClaim ClaimTypeReferenceId="givenName" PartnerClaimType="name" />
        <OutputClaim ClaimTypeReferenceId="surname" PartnerClaimType="surname" />
      </OutputClaims>
      <UseTechnicalProfileForSessionManagement ReferenceId="SM-Noop" />
    </TechnicalProfile>
    <TechnicalProfile Id="Create-RemoteUserLocally">
      <DisplayName>Create Remote Account Locally</DisplayName>
      <InputClaimsTransformations>
        <InputClaimsTransformation ReferenceId="copyEmailFromSignin" />
      </InputClaimsTransformations>
      <PersistedClaims>
        <PersistedClaim ClaimTypeReferenceId="password" PartnerClaimType="password" />
      </PersistedClaims>            
      <IncludeTechnicalProfile ReferenceId="AAD-UserWriteUsingLogonEmail" />
    </TechnicalProfile>
```
   </div>
</details>

<details>
   <summary>Creamos un technical profile que valide si el usuario existe en nuestro AAD: SPOILER</summary>
   <div class="description">

```xml
    <TechnicalProfile Id="AAD-UserExists">
      <Metadata>
        <Item Key="Operation">Read</Item>
        <Item Key="RaiseErrorIfClaimsPrincipalDoesNotExist">true</Item>
      </Metadata>
      <IncludeInSso>false</IncludeInSso>
      <InputClaims>
        <InputClaim ClaimTypeReferenceId="signInName" PartnerClaimType="signInNames.emailAddress" Required="true" />
      </InputClaims>
      <OutputClaims>
        <OutputClaim ClaimTypeReferenceId="isLocalUser" DefaultValue="true" AlwaysUseDefaultValue="true" />
      </OutputClaims>
      <IncludeTechnicalProfile ReferenceId="AAD-Common" />
    </TechnicalProfile>
```
   </div>
</details>

<details>
   <summary>Creamos un technical profile que escriba el usuario en el AAD: SPOILER</summary>
   <div class="description">

```xml
    <TechnicalProfile Id="Create-RemoteUserLocally">
      <DisplayName>Create Remote Account Locally</DisplayName>
      <InputClaimsTransformations>
        <InputClaimsTransformation ReferenceId="copyEmailFromSignin" />
      </InputClaimsTransformations>
      <PersistedClaims>
        <PersistedClaim ClaimTypeReferenceId="password" PartnerClaimType="password" />
      </PersistedClaims>            
      <IncludeTechnicalProfile ReferenceId="AAD-UserWriteUsingLogonEmail" />
    </TechnicalProfile>
```
   </div>
</details>

<details>
   <summary>Consumimos el technical profile `SelfAsserted-LocalAccountSignin-Email` para añadir validaciones nuevas: SPOILER</summary>
   <div class="description">

```xml
<TechnicalProfile Id="SelfAsserted-LocalAccountSignin-Email">
      <DisplayName>Local Account Signin</DisplayName>
      <OutputClaims>
        <OutputClaim ClaimTypeReferenceId="isLocalUser" />
      </OutputClaims>
      <ValidationTechnicalProfiles>
        <ValidationTechnicalProfile ReferenceId="AAD-UserExists" ContinueOnError="true" ContinueOnSuccess="true" />
        <!-- If 'isLocalUser' equals 'True' Login Locally -->
        <ValidationTechnicalProfile ReferenceId="login-NonInteractive">
          <Preconditions>
            <Precondition Type="ClaimsExist" ExecuteActionsIf="false">
              <Value>isLocalUser</Value>
              <Action>SkipThisValidationTechnicalProfile</Action>
            </Precondition>
          </Preconditions>
        </ValidationTechnicalProfile>
        <!-- If 'isLocalUser' equals 'False' Login Remotely (Migrate User) -->
        <ValidationTechnicalProfile ReferenceId="login-Remote">
          <Preconditions>
            <Precondition Type="ClaimsExist" ExecuteActionsIf="true">
              <Value>isLocalUser</Value>
              <Action>SkipThisValidationTechnicalProfile</Action>
            </Precondition>
          </Preconditions>
        </ValidationTechnicalProfile>
        <!-- Set to Continue on success - false so as not rto try to re-apply login-Noninteractive from inhertance -->
        <ValidationTechnicalProfile ReferenceId="Create-RemoteUserLocally" ContinueOnSuccess="false"> 
          <Preconditions>
            <Precondition Type="ClaimEquals" ExecuteActionsIf="true">
              <Value>isLocalUser</Value>
              <Value>True</Value>
              <Action>SkipThisValidationTechnicalProfile</Action>
            </Precondition>
          </Preconditions>
        </ValidationTechnicalProfile>
      </ValidationTechnicalProfiles>
    </TechnicalProfile>
```
   </div>
</details>

<details>
   <summary>Añadimos los steps de nuestra User Journey: SPOILER</summary>
   <div class="description">

```xml
    <OrchestrationStep Order="1" Type="ClaimsExchange">
      <ClaimsExchanges>
        <ClaimsExchange Id="LocalAccountSigninEmailExchange" TechnicalProfileReferenceId="SelfAsserted-LocalAccountSignin-Email" />
      </ClaimsExchanges>
    </OrchestrationStep>
    <!-- This step reads any user attributes that we may not have received when authenticating using ESTS so they can be sent 
      in the token. -->
    <OrchestrationStep Order="2" Type="ClaimsExchange">
      <ClaimsExchanges>
        <ClaimsExchange Id="AADUserReadWithObjectId" TechnicalProfileReferenceId="AAD-UserReadUsingObjectId" />
      </ClaimsExchanges>
    </OrchestrationStep>

    <OrchestrationStep Order="3" Type="SendClaims" CpimIssuerTechnicalProfileReferenceId="JwtIssuer" />
```
   </div>
</details>