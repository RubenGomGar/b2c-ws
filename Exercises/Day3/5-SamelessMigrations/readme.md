# Flujo de migración sameless

En este enscenario queremos replicar un escenario de migración diferente.
En este caso no quieren de un escenario de migración progresiva donde el alta del usuario en el nuevo tenant recaiga sobre el.

En este caso, lo que se busca es mediante importación de fichero, migrar todos los usuarios sin la contraseña en nuestro tenant de B2C con una claim custom que indica si el usuario requiero o no de migración.

Si requiere se le validará contra el antigui Idp y escribimos su contraseña para futuros logins

1. Mostramos formulario de SignIn
2. Si el usuario existe en nuestro AAD con la claim requireMigration validamos contra el antiguo Idp, sino logamos en nuestro AAD
3. Si logamos contra el Idp Legacy entonces posteriormente seteamos la contraseña y limpiamos el flag de requireMigration.


> Para este ejercicio las apis ya las tenemos expuestas en cloud y estas son las definiciones

### Remote Login Api

- Url: https://b2capidemo.azurewebsites.net/api/sameless-remote-login
- Action: POST
- Authentication: None
- Modelo de entrada: 

```c#
        public class SamelessRemoteLoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
```

- modelo de salida:

```c#
        public class SamelessRemoteLoginResponse
        {
            public bool tokenSuccess { get; set; }
            public bool migrationRequired { get; set; }
        }
```


# Para construir el flujo necesitaríamos los siguientes pasos:

1. Creamos una claim para almacenar requireMigration en la cuenta de B2C
    - El Identificador de la claim deberá ser `extension_requiresMigration`
    - DisplayName extension_requiresMigration
    - Tipo de dato. boolean
    - Textos de ayuda. `extension_requiresMigration`
2. Creamos una claim para gestionar el campo requireMigration en el flujo
    - El Identificador de la claim deberá ser `requiresMigration`
    - DisplayName extension_requiresMigration
    - Tipo de dato. boolean
    - Textos de ayuda. `extension_requiresMigration`
3. Creamos una claim para almacenar el primer campo de respuesta del api de Auth
    - El Identificador de la claim deberá ser `tokenSuccess`
    - DisplayName tokenSuccess
    - Tipo de dato. boolean
    - Textos de ayuda. `tokenSuccess`
4. Creamos una claim para almacenar el segundo campo de respuesta del api de Auth
    - El Identificador de la claim deberá ser `migrationRequired`
    - DisplayName migrationRequired
    - Tipo de dato. boolean
    - Textos de ayuda. `migrationRequired`
5. Creamos un technical profile que lea el usuario del AAD y popule el campo requireMigration en nuestro claim pipeline
    - Identificador: `Get-requiresMigration-status-signin`
    - Metadata:
        + Operation: `Read`
        + RaiseErrorIfClaimsPrincipalDoesNotExist: `true`
        + UserMessageIfClaimsPrincipalDoesNotExist: `An account could not be found for the provided user ID.`
    - IncludeInSso `false`
    - Claim de entrada. `signInName` como referencia padre `signInNames.emailAddress` requerida.
    - Claims de salida: `objectId` y `requiresMigration` como referencia padre de `extension_requiresMigration` Valor por defecto `false`
    - Incluimos techincal profile `AAD-Common`
6. Creamos un techincal profile que gestione la autenticación contra el Idp Legacy:
    - Identificador: `UserMigrationViaLegacyIdp`
    - Protocol: `RestfulProvider`
    - Metadata:
        + Las necesarias para llamar al Api definidca
        + AllowInsecureAuthInProduction `true`
    - Claims de entrada:
        + `signInName` referencia a la parent `email`
        + `password`
    - Claims de salida: 
        + `tokenSuccess` default value `true`
        + `migrationRequired`
    - Technical Profile para la caché `SM-Noop`
7. Creamos technical profile que escriba password en usuario y limpie el campo require migration.
    - Identificador: `AAD-WritePasswordAndFlipMigratedFlag`
    - Metadata:
        + Operation: `Write`
        + RaiseErrorIfClaimsPrincipalAlreadyExists: `false`
    - IncludeInSso: `false`
    - Claims de entrada. `objectId` required.
    - Claims de salida:
        + objectId
        + userPrincipalName
        + displayName
        + password
        + `passwordPolicies` default values `DisablePasswordExpiration, DisableStrongPassword` Always default value.
        + `extension_requiresMigration` valor por defacto `false` Always default value.
    - Include TechnicalProfile `AAD-Common`
    - TechnicalProfile for session `SM-AAD`
8. Definimos los steps de nuestra User Journey:
    - STEP 1: Llamamos a nuestro technical profile `SelfAsserted-LocalAccountSignin-Email`
    - STEP 2: Llamamos a nuestro techincal profile `AAD-UserReadUsingObjectId`
    - STEP 2: Enviamos las claims.


<details>
   <summary>Creamos una claim para almacenar requireMigration en la cuenta de B2C: SPOLIER</summary>
   <div class="description">

    ```xml
			<ClaimType Id="extension_requiresMigration">
				<DisplayName>extension_requiresMigration</DisplayName>
				<DataType>boolean</DataType>
				<AdminHelpText>extension_requiresMigration</AdminHelpText>
				<UserHelpText>extension_requiresMigration</UserHelpText>
			</ClaimType>
    ```
   </div>
</details>

<details>
   <summary>Creamos una claim para gestionar el campo requireMigration en el flujo: SPOLIER</summary>
   <div class="description">

    ```xml
			<ClaimType Id="requiresMigration">
				<DisplayName>extension_requiresMigration</DisplayName>
				<DataType>boolean</DataType>
				<AdminHelpText>extension_requiresMigration</AdminHelpText>
				<UserHelpText>extension_requiresMigration</UserHelpText>
			</ClaimType>
    ```
   </div>
</details>

<details>
   <summary>Creamos una claim para almacenar el primer campo de respuesta del api de Auth: SPOLIER</summary>
   <div class="description">

    ```xml
			<ClaimType Id="tokenSuccess">
				<DisplayName>tokenSuccess</DisplayName>
				<DataType>boolean</DataType>
				<AdminHelpText>tokenSuccess</AdminHelpText>
				<UserHelpText>tokenSuccess</UserHelpText>
			</ClaimType>
    ```
   </div>
</details>

<details>
   <summary>Creamos una claim para almacenar el segundo campo de respuesta del api de Auth: SPOLIER</summary>
   <div class="description">

    ```xml
			<ClaimType Id="migrationRequired">
				<DisplayName>migrationRequired</DisplayName>
				<DataType>boolean</DataType>
				<AdminHelpText>migrationRequired</AdminHelpText>
				<UserHelpText>migrationRequired</UserHelpText>
			</ClaimType>
    ```
   </div>
</details>

<details>
   <summary>Creamos un technical profile que lea el usuario del AAD y popule el campo requireMigration en nuestro claim pipeline: SPOLIER</summary>
   <div class="description">

    ```xml
				<TechnicalProfile Id="Get-requiresMigration-status-signin">
					<Metadata>
						<Item Key="Operation">Read</Item>
						<Item Key="RaiseErrorIfClaimsPrincipalDoesNotExist">true</Item>
						<Item Key="UserMessageIfClaimsPrincipalDoesNotExist">An account could not be found for the provided user ID.</Item>
					</Metadata>
					<IncludeInSso>false</IncludeInSso>
					<InputClaims>
						<InputClaim ClaimTypeReferenceId="signInName" PartnerClaimType="signInNames.emailAddress" Required="true" />
					</InputClaims>
					<OutputClaims>
						<OutputClaim ClaimTypeReferenceId="objectId" />
						<!-- Set a default value (false) in the case the account does not have this attribute defined -->
						<OutputClaim ClaimTypeReferenceId="requiresMigration" PartnerClaimType="extension_requiresMigration" DefaultValue="false" />
					</OutputClaims>
					<IncludeTechnicalProfile ReferenceId="AAD-Common" />
				</TechnicalProfile>
    ```
   </div>
</details>

<details>
   <summary>Creamos un techincal profile que gestione la autenticación contra el Idp Legacy: SPOLIER</summary>
   <div class="description">

    ```xml
			<TechnicalProfile Id="UserMigrationViaLegacyIdp">
					<DisplayName>REST API call to communicate with Legacy IdP</DisplayName>
					<Protocol Name="Proprietary" Handler="Web.TPEngine.Providers.RestfulProvider, Web.TPEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" />
					<Metadata>
						<Item Key="ServiceUrl">https://b2capidemo.azurewebsites.net/api/sameless-remote-login</Item>
						<Item Key="AuthenticationType">None</Item>
						<Item Key="SendClaimsIn">Body</Item>
						<Item Key="AllowInsecureAuthInProduction">True</Item>
					</Metadata>
					<InputClaims>
						<InputClaim ClaimTypeReferenceId="signInName" PartnerClaimType="email" />
						<InputClaim ClaimTypeReferenceId="password" />
					</InputClaims>
					<OutputClaims>
						<OutputClaim ClaimTypeReferenceId="tokenSuccess" DefaultValue="false"/>
						<OutputClaim ClaimTypeReferenceId="migrationRequired"/>
					</OutputClaims>
					<UseTechnicalProfileForSessionManagement ReferenceId="SM-Noop" />
				</TechnicalProfile>
    ```
   </div>
</details>

<details>
   <summary>Creamos technical profile que escriba password en usuario y limpie el campo require migration: SPOLIER</summary>
   <div class="description">

    ```xml
				<TechnicalProfile Id="AAD-WritePasswordAndFlipMigratedFlag">
					<Metadata>
						<Item Key="Operation">Write</Item>
						<Item Key="RaiseErrorIfClaimsPrincipalAlreadyExists">false</Item>
					</Metadata>
					<IncludeInSso>false</IncludeInSso>
					<InputClaims>
						<InputClaim ClaimTypeReferenceId="objectId" Required="true" />
					</InputClaims>
					<PersistedClaims>
						<PersistedClaim ClaimTypeReferenceId="objectId" />
						<PersistedClaim ClaimTypeReferenceId="userPrincipalName" />
						<PersistedClaim ClaimTypeReferenceId="displayName" />
						<PersistedClaim ClaimTypeReferenceId="password" PartnerClaimType="password"/>
						<PersistedClaim ClaimTypeReferenceId="passwordPolicies" DefaultValue="DisablePasswordExpiration, DisableStrongPassword" AlwaysUseDefaultValue="true"/>
						<PersistedClaim ClaimTypeReferenceId="extension_requiresMigration" DefaultValue="false" AlwaysUseDefaultValue="true"/>
					</PersistedClaims>
					<IncludeTechnicalProfile ReferenceId="AAD-Common" />
					<UseTechnicalProfileForSessionManagement ReferenceId="SM-AAD" />
				</TechnicalProfile>
    ```
   </div>
</details>

<details>
   <summary>Definimos los steps de nuestra User Journey: SPOLIER</summary>
   <div class="description">

    ```xml
			<OrchestrationStep Order="1" Type="CombinedSignInAndSignUp" ContentDefinitionReferenceId="api.signuporsignin">
			<ClaimsProviderSelections>
				<ClaimsProviderSelection ValidationClaimsExchangeId="LocalAccountSigninEmailExchange" />
			</ClaimsProviderSelections>
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