# Flujo First Login Reset Password

Para este flujo queremos detectar cuando es el primer login de un usuario para forzarle a reiniciar la contraseña.

El flujo debería estar comupesto por la siguiente lógica

1. Mostramos el formulario de SignUp conbinado.
2. Registramos a un usuario añadiendo una claim nueva de reseteo contraseña por defecto a true en registro.
3. Nos logamos con el usuario nuevo.
4. Validamos contra el AAD las credenciales introducidas.
5. Si es el primer login del usario.
    - Forzamos cambio de contraseña.
    - Reseteamos claim de reinicio de contraseña.
6. Login al AAD y acceso a la aplicación.


# Para construir el flujo necesitaríamos los siguientes pasos:

1. Creamos una claim para saber si debe reiniciar la contraseña:
    - El Identificador de la claim deberá ser `extension_mustResetPassword`
    - DisplayName Must reset password
    - Tipo de dato. boolean
    - Textos de ayuda. `Specifies whether user must reset the password`
2. Consumimos el technical profile `AAD-UserWriteUsingLogonEmail` para añadir una claim nueva en la bolsa de claims de registro.
    - Identificador `AAD-UserWriteUsingLogonEmail`
    - PersistedClaim `extension_mustResetPassword` con Valor por defecto a `true`.
3. Consumimos el technical profile `AAD-UserReadUsingObjectId` para pedirle una Claim adicional
    - Identificador `AAD-UserReadUsingObjectId`
    - OutputClaim a consumir `extension_mustResetPassword`
4. Creamos un technical profile que elimine la claim de reseteo de contraseña
    - Identificador `AAD-UserRemoveMustResetPasswordUsingObjectId`
    - Metadata necesaria. Key `Operation` con valor `DeleteClaims`
    - Claims de entrada como requerida `objectId`. 
    - Persisted Claims necesarias. `objectId` y `extension_mustResetPassword`
    - Incluimos el techical profile `AAD-Common`.
5. Añadimos los steps de nuestra User Journey
    - STEP 1: CombinedSignInAndSignUp
    - STEP 2: Si no detectamos login llamamos a LocalAccountSignUpWithLogonEmail
    - STEP 3: Si hemos hecho login validamos las credenciales de AAD con `AAD-UserReadUsingObjectId`
    - STEP 4: Si hemos hecho login y si existe y es true la claim `extension_mustResetPassword` llamaremos a nuestro techincal profile de reseteo de contraseña `LocalAccountWritePasswordUsingObjectId`
    - STEP 5: Si hemos hecho login y reseteado la contraseña llamamos a nuestro technical profile de reseteo a false de claim `AAD-UserRemoveMustResetPasswordUsingObjectId`
    - STEP 6: Enviamos claims

<details>
   <summary>Creamos una claim para saber si debe reiniciar la contraseña: SPOLIER</summary>
   <div class="description">

    ```xml
  	<ClaimType Id="extension_mustResetPassword">
	    <DisplayName>Must reset password</DisplayName>
	    <DataType>boolean</DataType>
	    <UserHelpText>Specifies whether user must reset the password</UserHelpText>
	</ClaimType>
    ```

   </div>
</details>

<details>
   <summary>Consumimos el technical profile `AAD-UserWriteUsingLogonEmail` para añadir una claim nueva en la bolsa de claims de registro: SPOLIER</summary>
   <div class="description">

    ```xml
        <TechnicalProfile Id="AAD-UserWriteUsingLogonEmail">
          <PersistedClaims>
            <PersistedClaim ClaimTypeReferenceId="extension_mustResetPassword" DefaultValue="true" />
          </PersistedClaims>
        </TechnicalProfile>
    ```

   </div>
</details>

<details>
   <summary>Consumimos el technical profile `AAD-UserReadUsingObjectId` para pedirle una Claim adicional: SPOLIER</summary>
   <div class="description">

    ```xml
        <TechnicalProfile Id="AAD-UserReadUsingObjectId">
          <OutputClaims>
            <!--Demo: Read the 'must reset password' extension attribute -->
            <OutputClaim ClaimTypeReferenceId="extension_mustResetPassword" />
          </OutputClaims>
        </TechnicalProfile>
    ```

   </div>
</details>

<details>
   <summary>Creamos un technical profile que elimine la claim de reseteo de contraseña: SPOLIER</summary>
   <div class="description">

    ```xml
        <TechnicalProfile Id="AAD-UserRemoveMustResetPasswordUsingObjectId">
          <Metadata>
            <Item Key="Operation">DeleteClaims</Item>
          </Metadata>
          <InputClaims>
            <InputClaim ClaimTypeReferenceId="objectId" Required="true" />
          </InputClaims>
          <PersistedClaims>
            <PersistedClaim ClaimTypeReferenceId="objectId" />
            <PersistedClaim ClaimTypeReferenceId="extension_mustResetPassword" />            
          </PersistedClaims>
          <IncludeTechnicalProfile ReferenceId="AAD-Common" />
        </TechnicalProfile>
    ```

   </div>
</details>

<details>
   <summary>Añadimos los steps de nuestra User Journey: SPOLIER</summary>
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

        <!-- Check if the user has selected to sign in using one of the social providers -->
        <OrchestrationStep Order="2" Type="ClaimsExchange">
          <Preconditions>
            <Precondition Type="ClaimsExist" ExecuteActionsIf="true">
              <Value>objectId</Value>
              <Action>SkipThisOrchestrationStep</Action>
            </Precondition>
          </Preconditions>
          <ClaimsExchanges>
           <ClaimsExchange Id="SignUpWithLogonEmailExchange" TechnicalProfileReferenceId="LocalAccountSignUpWithLogonEmail" />
          </ClaimsExchanges>
        </OrchestrationStep>

        <!-- This step reads any user attributes that we may not have received when authenticating using ESTS so they can be sent 
          in the token. -->
        <OrchestrationStep Order="3" Type="ClaimsExchange">
          <Preconditions>
            <Precondition Type="ClaimEquals" ExecuteActionsIf="true">
              <Value>authenticationSource</Value>
              <Value>socialIdpAuthentication</Value>
              <Action>SkipThisOrchestrationStep</Action>
            </Precondition>
          </Preconditions>
          <ClaimsExchanges>
            <ClaimsExchange Id="AADUserReadWithObjectId" TechnicalProfileReferenceId="AAD-UserReadUsingObjectId" />
          </ClaimsExchanges>
        </OrchestrationStep>

        <!--Demo: check if change password is required. If yes, ask the user to reset the password-->
        <OrchestrationStep Order="4" Type="ClaimsExchange">
          <Preconditions>
            <Precondition Type="ClaimEquals" ExecuteActionsIf="true">
              <Value>authenticationSource</Value>
              <Value>socialIdpAuthentication</Value>
              <Action>SkipThisOrchestrationStep</Action>
            </Precondition>
            <Precondition Type="ClaimsExist" ExecuteActionsIf="false">
              <Value>extension_mustResetPassword</Value>
              <Action>SkipThisOrchestrationStep</Action>
            </Precondition>            
            <Precondition Type="ClaimEquals" ExecuteActionsIf="false">
              <Value>extension_mustResetPassword</Value>
              <Value>True</Value>
              <Action>SkipThisOrchestrationStep</Action>
            </Precondition>            
          </Preconditions>        
          <ClaimsExchanges>
            <ClaimsExchange Id="NewCredentials" TechnicalProfileReferenceId="LocalAccountWritePasswordUsingObjectId" />
          </ClaimsExchanges>
        </OrchestrationStep>

          <!--Demo: check if change password is required. If yes remove the value of the extension attribute. 
              So, on the next time user dons' t need to update the password-->
        <OrchestrationStep Order="5" Type="ClaimsExchange">
          <Preconditions>
            <Precondition Type="ClaimEquals" ExecuteActionsIf="true">
              <Value>authenticationSource</Value>
              <Value>socialIdpAuthentication</Value>
              <Action>SkipThisOrchestrationStep</Action>
            </Precondition>
            <Precondition Type="ClaimsExist" ExecuteActionsIf="false">
              <Value>extension_mustResetPassword</Value>
              <Action>SkipThisOrchestrationStep</Action>
            </Precondition>            
            <Precondition Type="ClaimEquals" ExecuteActionsIf="false">
              <Value>extension_mustResetPassword</Value>
              <Value>True</Value>
              <Action>SkipThisOrchestrationStep</Action>
            </Precondition>            
          </Preconditions>        
          <ClaimsExchanges>
            <ClaimsExchange Id="AADUserRemoveMustResetPasswordUsingObjectId" TechnicalProfileReferenceId="AAD-UserRemoveMustResetPasswordUsingObjectId" />
          </ClaimsExchanges>
        </OrchestrationStep>
    ```

   </div>
</details>