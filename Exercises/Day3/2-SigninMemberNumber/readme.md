# Flujo SingIn Member Number

Para este flujo queremos hacer una validación previa antes de dar la posibilidad de registrarnos en nuestro tenant.

Imaginamos que somos una asociación y que solo permirimos el registro en nuestra plaforma si tiene un número de socio correcto. Si permitimos el registro también queremos recuperar inforamción de usuario de nuestra app para rellenar parcialmente el formulario de registro y si acepta o no los términos de uso de nuestra plataforma.

> Para este ejercicio las apis ya las tenemos expuestas en cloud y estas son las definiciones

### Api Validación Número de socio.

- Url: https://b2capidemo.azurewebsites.net/api/validate-member-number
- Action: POST
- Authentication: None
- Modelo de entrada:

```c#
    public class UserModel
    {
        public int MemberNumber { get; set; }
    }
```

- modelo de salida:

```c#
    public class ValidateWDNIResponse
    {
        public bool IsValidUser { get; set; }
    }
```


### Api Guardado de Usuario

- Url: https://b2capidemo.azurewebsites.net/api/save-user
- Action: POST
- Authentication: None
- Modelo de entrada:

```c#
    public class SaveUserRequest
    {
        public int MemberNumber { get; set; }
    }
```

- modelo de salida:

```c#
    public class SaveUserResponse
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
    }
```

### Api Términos de usuario

- Url: https://b2capidemo.azurewebsites.net/api/terms-of-use
- Action: POST
- Authentication: None
- Modelo de entrada:

```c#
    public class TermsOfUseModelRequest
    {
        public int MemberNumber { get; set; }
        public string Email { get; set; }
    }
```

El flujo debería estar comupesto por la siguiente lógica

1. Mostramos una página previa de validación donde esté como input el campo de número de socio.
2. Validaríamos ese número de socio frente a un Api externa
3. Si el Api nos devuelve un Error mostramos feedback al usuario que no puede registrarse.
4. Si el Api nos devuelve un Success rederigimos al formulario de registro.
4. Obtenemos información adicional del usuario en nuestra Api Externa.
5. Mostramos formulario de registro, pre-rellenando la información que ya tenemos.
5. Registamos el usuario en el AAD.
7. Enviamos los términos y condiciones a nuestra Api externa.
6. Enviamos Claims.



Para construir el flujo necesitaríamos los siguientes pasos:

# 1. ¿Cómo pedir el número de socio?

1. Creamos una claim para el Nº de socio:
    - El Identificador de la claim deberá ser `extension_MemberNumber`
    - DisplayName Número de socio
    - Tipo de dato. String
    - Tipo TextBox
    - Textos de ayuda. `Número de socio`
2. Creamos la claim para el mensaje requerido de número de socio.
    - Identificador `MemberNumber_required`
    - Tipo de dato. String.
    - Tipo Paragraph
    - Display Name `De cara a completar el proceso de registro, es necesario validar tu identidad. Por favor introduce tu Número de socio a continuación`
3. Creamos la claim para el mensaje de error de validación de número de socio
    - Identificador `MemberNumber_ValidationError`
    - Tipo de dato. String.
    - Tipo Paragraph
    - Display Name `El número de socio no está en el sistema. Reviselo y vuelva a intentarlo`
4. Creamos una claim de tipo json para almacenar el json de entrada el api de validación de Nº de socio
    - Identificador `validateUserBody`
    - Tipo de dato. String
    - DisplayName `Json`
    - Textos de ayuda `Json`
5. Creamos una claim transformation que coja la claim 'extension_MemberNumber' y genera el json en la output claim `validateUserBody`
    - Tipo de transformación `GenerateJson`
    - Identificador `GenerateValidateUserBody`
6. Creamos un technical profile de tipo SelfAsserted donde incluiremos nuestra output claim `extension_MemberNumber`
    - Identificador: `ReadMemberNumber`
    - Tipo de contenido. `api.selfasserted`
    - Referencia a output claims `MemberNumber_required, extension_MemberNumber`
    - Ponemos una output cliam transformation para formar el body de llamada al api custom de validación de Nº de socio `GenerateValidateUserBody`
7. Creamos una User Journey con los siguientes pasos
    - 1. OrchestationStep de tipo `ClaimsExchange` que hace referencia la technicalProfile `ReadMemberNumber`
8. Podemos probar hasta aquí si se nos muestra el formulario de obtención de Nº de Socio.



<details>
   <summary>Creamos una claim para el Nº de socio: SPOLIER</summary>
   <div class="description">

    ```xml
      <ClaimType Id="extension_MemberNumber">
        <DisplayName>Número de socio</DisplayName>
        <DataType>string</DataType>
        <AdminHelpText>Número de socio</AdminHelpText>
        <UserHelpText>Número de socio</UserHelpText>
        <UserInputType>TextBox</UserInputType>
      </ClaimType>
    ```

   </div>
</details>

<details>
   <summary>Creamos la claim para el mensaje requerido de número de socio: SPOLIER</summary>
   <div class="description">

    ```xml
      <ClaimType Id="MemberNumber_required">
        <DisplayName>De cara a completar el proceso de registro, es necesario validar tu identidad. Por favor introduce tu Número de socio a continuación</DisplayName>
        <DataType>string</DataType>
        <UserInputType>Paragraph</UserInputType>
      </ClaimType>
    ```
    
   </div>
</details>

<details>
   <summary>Creamos la claim para el mensaje de error de validación de número de socio: SPOLIER</summary>
   <div class="description">

    ```xml
      <ClaimType Id="MemberNumber_ValidationError">
        <DisplayName>El número de socio no está en el sistema. Reviselo y vuelva a intentarlo</DisplayName>
        <DataType>string</DataType>
        <UserInputType>Paragraph</UserInputType>
      </ClaimType>
    ```
    
   </div>
</details>

<details>
   <summary>Creamos una claim de tipo json: SPOLIER</summary>
   <div class="description">

    ```xml
      <ClaimType Id="validateUserBody">
        <DisplayName>Json</DisplayName>
        <DataType>string</DataType>
        <AdminHelpText>Json</AdminHelpText>
        <UserHelpText>Json</UserHelpText>
      </ClaimType> 
    ```
    
   </div>
</details>

<details>
   <summary>Creamos una claim transformation: SPOLIER</summary>
   <div class="description">

    ```xml
      <ClaimsTransformation Id="GenerateValidateUserBody" TransformationMethod="GenerateJson">
        <InputClaims>
          <InputClaim ClaimTypeReferenceId="extension_MemberNumber" TransformationClaimType="MemberNumber"/>
        </InputClaims>
        <OutputClaims>
          <OutputClaim ClaimTypeReferenceId="validateUserBody" TransformationClaimType="outputClaim"/>
        </OutputClaims>
      </ClaimsTransformation>
    ```
    
   </div>
</details>

<details>
   <summary>Creamos un technical profile: SPOLIER</summary>
   <div class="description">

    ```xml
        <TechnicalProfile Id="ReadMemberNumber">
          <DisplayName>Read Member Number</DisplayName>
          <Protocol Name="Proprietary" Handler="Web.TPEngine.Providers.SelfAssertedAttributeProvider, Web.TPEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" />
          <Metadata>
            <Item Key="ContentDefinitionReferenceId">api.selfasserted</Item>
            <Item Key="setting.showCancelButton">false</Item>
          </Metadata>
          <OutputClaims>
            <OutputClaim ClaimTypeReferenceId="MemberNumber_required" />
            <OutputClaim ClaimTypeReferenceId="extension_MemberNumber" />
          </OutputClaims>
          <OutputClaimsTransformations>
            <OutputClaimsTransformation ReferenceId="GenerateValidateUserBody" />
          </OutputClaimsTransformations>
          <UseTechnicalProfileForSessionManagement ReferenceId="SM-Noop" />
        </TechnicalProfile>
    ```
    
   </div>
</details>

<details>
   <summary>Creamos una userJourney: SPOLIER</summary>
   <div class="description">

    ```xml
    <UserJourney Id="SignInMemberNumber">
      <OrchestrationSteps>
        <OrchestrationStep Order="1" Type="ClaimsExchange">
          <ClaimsExchanges>
            <ClaimsExchange Id="ReadMemberNumber" TechnicalProfileReferenceId="ReadMemberNumber" />
          </ClaimsExchanges>
        </OrchestrationStep>
      </OrchestrationSteps>
    </UserJourney>
    ```
    
   </div>
</details>


# 2. ¿Cómo validar el número de socio?

1. Crearemos las claims necesarias para la validación de Nº de socio.
    - El Identificador de la claim deberá ser `extension_isValidUser`
    - DisplayName Is Valid User
    - Tipo Boolean
    - Textos de ayuda. `Is Valid User`
2. Creamos el technical profile que llamará al api
    - El identificador del technical profile será `CUSTOM-ValidateMemeberUser`
    - Utilizaremos el protocolo `RestfulProvider`
    - Configuraremos en los metadatos la información de nuestra api externa de validación de nº de socio.
    - La claim que vamos a usar como vody será la definida como `validateUserBody`
    - Necesitaremos como InputClaim `validateUserBody`
    - Recogeremos el resultado en la OutputClaim `extension_isValidUser` tomando como referencia la PartnerClaimType `isValidUser` que devuelve nuestro api.
    - Uso de Caché `SM-Noop`
3. Creamos technical profile para mostrar un error cuando el usuario no sea válido.
    - El identificador del technical profile será `ShowValidationError`.
    - Protocolo `SelfAssertedAttributeProvider`
    - Metadata necesaria
        + Referencia a content definition `api.selfasserted`
        + No queremos mostrar botón de continuar.
        + No queremos mostrar botón de cancelar.
    - Tomaremos como InputClaim `MemberNumber_ValidationError` con Default Value `Error al validar el número de socio` y usar tiempre el valor por defecto a `true`
    - Necesitaremos una Display Claim que muestre `MemberNumber_ValidationError`
    - Uso de Caché `SM-Noop`
4. Completamos nuestro UserJourney para añadir los siguientes pasos
    - Step 2: Llamamos a nuestro `CUSTOM-ValidateMemeberUser`
    - Step 3: Llamamos a nuestro `ShowValidationError` Si la `extension_isValidUser` no existe o es false.


<details>
   <summary>Crearemos las claims necesarias: SPOLIER</summary>
   <div class="description">

    ```xml
      <ClaimType Id="extension_isValidUser">
        <DisplayName>Is Valid User</DisplayName>
        <DataType>boolean</DataType>
        <AdminHelpText>Is Valid User</AdminHelpText>
        <UserHelpText>Is Valid User</UserHelpText>
      </ClaimType> 
    ```
    
   </div>
</details>

<details>
   <summary>Creamos el technical profile que llamará al api: SPOLIER</summary>
   <div class="description">

    ```xml
        <TechnicalProfile Id="CUSTOM-ValidateMemeberUser">
          <DisplayName>Get user data</DisplayName>
          <Protocol Name="Proprietary" Handler="Web.TPEngine.Providers.RestfulProvider, Web.TPEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" />
          <Metadata>
            <Item Key="ServiceUrl">https://b2capidemo.azurewebsites.net/api/validate-member-number</Item>
            <Item Key="AuthenticationType">None</Item>
            <Item Key="SendClaimsIn">Body</Item>
            <Item Key="AllowInsecureAuthInProduction">true</Item>
            <Item Key="DefaultUserMessageIfRequestFailed">Cannot process your request right now, please try again later.</Item>
            <Item Key="ClaimUsedForRequestPayload">validateUserBody</Item>
          </Metadata>
          <InputClaims>
            <InputClaim ClaimTypeReferenceId="validateUserBody" />
          </InputClaims>
          <OutputClaims>
            <OutputClaim ClaimTypeReferenceId="extension_isValidUser" PartnerClaimType="isValidUser" />
          </OutputClaims>
          <UseTechnicalProfileForSessionManagement ReferenceId="SM-Noop" />
        </TechnicalProfile>
    ```
    
   </div>
</details>

<details>
   <summary>Creamos technical profile para mostrar un error cuando el usuario no sea válido: SPOLIER</summary>
   <div class="description">

    ```xml
        <TechnicalProfile Id="ShowValidationError">
          <DisplayName>Show Validation Error</DisplayName>
          <Protocol Name="Proprietary" Handler="Web.TPEngine.Providers.SelfAssertedAttributeProvider, Web.TPEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" />
          <Metadata>
            <Item Key="ContentDefinitionReferenceId">api.selfasserted</Item>
            <Item Key="setting.showContinueButton">false</Item>
            <Item Key="setting.showCancelButton">false</Item>
          </Metadata>
          <InputClaims>
            <InputClaim ClaimTypeReferenceId="MemberNumber_ValidationError"  DefaultValue="Error al validar el número de socio" AlwaysUseDefaultValue="true"/>
          </InputClaims>
          <DisplayClaims>
            <DisplayClaim ClaimTypeReferenceId="MemberNumber_ValidationError" />
          </DisplayClaims>
          <UseTechnicalProfileForSessionManagement ReferenceId="SM-Noop" />
        </TechnicalProfile>
    ```
    
   </div>
</details>

<details>
   <summary>Completamos nuestro UserJourney: SPOLIER</summary>
   <div class="description">

    ```xml
        <OrchestrationStep Order="2" Type="ClaimsExchange">
          <ClaimsExchanges>
            <ClaimsExchange Id="ValidateUser" TechnicalProfileReferenceId="CUSTOM-ValidateMemeberUser" />
          </ClaimsExchanges>
        </OrchestrationStep>
        <OrchestrationStep Order="3" Type="ClaimsExchange">
          <Preconditions>
            <Precondition Type="ClaimsExist" ExecuteActionsIf="false">
              <Value>extension_isValidUser</Value>
              <Action>SkipThisOrchestrationStep</Action>
            </Precondition>
            <Precondition Type="ClaimEquals" ExecuteActionsIf="true">
              <Value>extension_isValidUser</Value>
              <Value>true</Value>
              <Action>SkipThisOrchestrationStep</Action>
            </Precondition>
          </Preconditions>
          <ClaimsExchanges>
            <ClaimsExchange Id="ShowError" TechnicalProfileReferenceId="ShowValidationError" />
          </ClaimsExchanges>
        </OrchestrationStep>
    ```
    
   </div>
</details>


# 3. ¿Cómo pre-rellenar la información de usuario?

1. Creamos una claim de tipo json para almacenar el json de entrada el api de validación de Nº de socio
    - Identificador `saveUserBody`
    - Tipo de dato. String
    - DisplayName `Json`
    - Textos de ayuda `Json`
2. Creamos una claim transformation que coja la claim 'extension_MemberNumber' y genera el json en la output claim `saveUserBody`
    - Tipo de transformación `GenerateJson`
    - Identificador `GenerateSaveUserDataBody`
3. Creamos el technical profile que llamará al api y rellenará la información a mostrar
    - El identificador del technical profile será `CUSTOM-SaveUserData`
    - Utilizaremos el protocolo `RestfulProvider`
    - Configuraremos en los metadatos la información de nuestra api externa de salvado de dato de usuario.
    - La claim que vamos a usar como vody será la definida como `saveUserBody`
    - Necesitaremos una InpuntClaimTransformation `GenerateSaveUserDataBody`.
    - Necesitaremos como InputClaim `saveUserBody`
    - Recogeremos el resultado en la OutputClaim
        + `email` tomando como referencia la PartnerClaimType `email` que devuelve nuestro api.
        + `displayName` tomando como referencia la PartnerClaimType `username` que devuelve nuestro api.
        + `givenName` tomando como referencia la PartnerClaimType `name` que devuelve nuestro api.
        + `surname` tomando como referencia la PartnerClaimType `surname` que devuelve nuestro api.
    - Uso de Caché `SM-Noop`
4. Incluimos el step 4 que llama a nuestro nuevo technical profile


<details>
   <summary>Creamos una claim de tipo json para almacenar el json: SPOLIER</summary>
   <div class="description">

    ```xml
      <ClaimType Id="saveUserBody">
        <DisplayName>Json</DisplayName>
        <DataType>string</DataType>
        <AdminHelpText>Json</AdminHelpText>
        <UserHelpText>Json</UserHelpText>
      </ClaimType>
    ```
    
   </div>
</details>

<details>
   <summary>Creamos una claim transformation: SPOLIER</summary>
   <div class="description">

    ```xml
      <ClaimsTransformation Id="GenerateSaveUserDataBody" TransformationMethod="GenerateJson">
        <InputClaims>
          <InputClaim ClaimTypeReferenceId="extension_MemberNumber" TransformationClaimType="MemberNumber"/>
        </InputClaims>
        <OutputClaims>
          <OutputClaim ClaimTypeReferenceId="saveUserBody" TransformationClaimType="outputClaim"/>
        </OutputClaims>
      </ClaimsTransformation>
    ```
    
   </div>
</details>

<details>
   <summary>Creamos el technical profile que llamará al api: SPOLIER</summary>
   <div class="description">

    ```xml
        <TechnicalProfile Id="CUSTOM-SaveUserData">
          <DisplayName>Get user data</DisplayName>
          <Protocol Name="Proprietary" Handler="Web.TPEngine.Providers.RestfulProvider, Web.TPEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" />
          <Metadata>
            <Item Key="ServiceUrl">https://b2capidemo.azurewebsites.net/api/save-user</Item>
            <Item Key="AuthenticationType">None</Item>
            <Item Key="SendClaimsIn">Body</Item>
            <Item Key="AllowInsecureAuthInProduction">true</Item>
            <Item Key="DefaultUserMessageIfRequestFailed">Cannot process your request right now, please try again later.</Item>
            <Item Key="ClaimUsedForRequestPayload">saveUserBody</Item>
          </Metadata>
          <InputClaimsTransformations>
            <InputClaimsTransformation ReferenceId="GenerateSaveUserDataBody" />
          </InputClaimsTransformations>
          <InputClaims>
            <InputClaim ClaimTypeReferenceId="saveUserBody" />
          </InputClaims>
          <OutputClaims>
            <OutputClaim ClaimTypeReferenceId="email" PartnerClaimType="email" />
            <OutputClaim ClaimTypeReferenceId="displayName" PartnerClaimType="username" />
            <OutputClaim ClaimTypeReferenceId="givenName" PartnerClaimType="name" />
            <OutputClaim ClaimTypeReferenceId="surname" PartnerClaimType="surname" />
          </OutputClaims>
          <UseTechnicalProfileForSessionManagement ReferenceId="SM-Noop" />
        </TechnicalProfile>
    ```
    
   </div>
</details>

<details>
   <summary>Completamos nuestro UserJourney: SPOLIER</summary>
   <div class="description">

    ```xml
        <OrchestrationStep Order="4" Type="ClaimsExchange">
          <ClaimsExchanges>
            <ClaimsExchange Id="GetData" TechnicalProfileReferenceId="CUSTOM-SaveUserData" />
          </ClaimsExchanges>
        </OrchestrationStep>
    ```
    
   </div>
</details>


# 4. ¿Cómo almacenar los términos de uso?

1. Creamos una claim de tipo json para almacenar el json de entrada el api de envío de términos de uso.
    - Identificador `termsOfUseBody`
    - Tipo de dato. String
    - DisplayName `Json`
    - Textos de ayuda `Json`
2. Creamos una claim para el valor de términos de uso
    - Identificador `termsOfUse`
    - Tipo de dato. String
    - DisplayName `Términos de uso`
    - Textos de ayuda `Términos de uso`
    - Tipo `CheckboxMultiSelect`
    - Restricción valor por defecto `false`
    - Restricción texto `Acepto los términos de uso`
    - Restricción valor `AgreeToTermsOfUseConsentYes`
3. Creamos un techincal profile para mostrar los datos de registro y mostrar la nueva claim de términos de uso
    - El identificador del technical profile será `GetUserInfo`
    - Utilizaremos el protocolo `SelfAssertedAttributeProvider`
    - Metadatas necesarias
        + Content Definition `selfasserted`
        + Raise Errors `true`
    - Mostramos las Input y Output claims de registro y añadimos como output nuestra nueva claim `termsOfUse` como requerida
    - Llamamos a nuestro Output Claim Transformation `GenerateSendTermsOfUseBody`
    - Gestionamos la cacheé como `SM-AAD`
4. Creamos un technical profile que envíe los términos de uso a nuestra api
    - El identificador del technical profile será `CUSTOM-SendTermsOfUse`
    - Utilizaremos el protocolo `RestfulProvider`
    - Configuraremos en los metadatos la información de nuestra api externa de salvado de dato de usuario.
    - La claim que vamos a usar como vody será la definida como `termsOfUseBody`
    - Necesitaremos una InpuntClaimTransformation `termsOfUseBody`.
    - Uso de Caché `SM-Noop`
5. Creamos un techincal profile para guardar el usuario en el AAD con las claims extras
    - El identificador del technical profile será `SignupUser`
    - Utilizaremos el protocolo `SelfAssertedAttributeProvider`
    - Metadatas necesarias
        + Claim Reference Ip Address `IpAddress`
        + Content Definition `localaccountsignup`
    - Utilizaremos nuesto sitema de encriptado `B2C_1A_TokenSigningKeyContainer`
    - Claims de entrada
        + email
        + newPassword
        + displayName
        + givenName
        + surname
        + termsOfUse
    - Claims de salida
        + objectId
        + email como requerido
        + newPassword como requerido
        + reenterPassword como requerido
        + executed-SelfAsserted-Input como requerido
        + authenticationSource
        + newUser
        + displayName
        + givenName
        + surName
        + termsOfUse
    - Llamamos a un Validation Technical Profile llamado `AAD-UserWriteUsingLogonEmail`
    - Gestionamos la cacheé como `SM-AAD`
6. Incluimos los siguientes steps en nuestra USerJourney
    - Step 5: Llamamos a nuestro techincal profile `GetUserInfo`
    - Step 6: Llamamos a nuestro technical profile `CUSTOM-SendTermsOfUse`
    - Step 7: Llamamos a nuestro technical profile `SignupUser`
    - Step 8: Enviamos Claims

<details>
   <summary>Creamos una claim de tipo json para almacenar el json de entrada el api de envío de términos de uso: SPOLIER</summary>
   <div class="description">

    ```xml
      <ClaimType Id="termsOfUseBody">
        <DisplayName>Json</DisplayName>
        <DataType>string</DataType>
        <AdminHelpText>Json</AdminHelpText>
        <UserHelpText>Json</UserHelpText>
      </ClaimType>
    ```
    
   </div>
</details>

<details>
   <summary>Creamos una claim para el valor de términos de uso: SPOLIER</summary>
   <div class="description">

    ```xml
      <ClaimType Id="termsOfUse">
        <DisplayName>Términos de uso</DisplayName>
        <DataType>string</DataType>
        <AdminHelpText>Términos de uso</AdminHelpText>
        <UserHelpText>Términos de uso</UserHelpText>
        <UserInputType>CheckboxMultiSelect</UserInputType>
        <Restriction>
          <Enumeration Text="Acepto los términos de uso" Value="AgreeToTermsOfUseConsentYes" SelectByDefault="false" />
        </Restriction>
      </ClaimType> 
    ```
    
   </div>
</details>

<details>
   <summary>Creamos un techincal profile para mostrar los datos de registro: SPOLIER</summary>
   <div class="description">

    ```xml
        <TechnicalProfile Id="GetUserInfo">
          <DisplayName>Get User info</DisplayName>
          <Protocol Name="Proprietary" Handler="Web.TPEngine.Providers.SelfAssertedAttributeProvider, Web.TPEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" />
          <Metadata>
            <Item Key="ContentDefinitionReferenceId">api.selfasserted</Item>
            <Item Key="RaiseErrorIfClaimsPrincipalDoesNotExist">true</Item>
          </Metadata>
          <InputClaims>
            <InputClaim ClaimTypeReferenceId="email"/>
            <InputClaim ClaimTypeReferenceId="newPassword" />
            <InputClaim ClaimTypeReferenceId="reenterPassword" />
            <InputClaim ClaimTypeReferenceId="displayName" />
            <InputClaim ClaimTypeReferenceId="givenName" />
            <InputClaim ClaimTypeReferenceId="surname" />
          </InputClaims>
           <OutputClaims>
            <OutputClaim ClaimTypeReferenceId="email" Required="true"/>
            <OutputClaim ClaimTypeReferenceId="newPassword" Required="true"/>
            <OutputClaim ClaimTypeReferenceId="reenterPassword" Required="true"/>
            <OutputClaim ClaimTypeReferenceId="displayName" />
            <OutputClaim ClaimTypeReferenceId="givenName" />
            <OutputClaim ClaimTypeReferenceId="surname" />
            <OutputClaim ClaimTypeReferenceId="termsOfUse" Required="true"/>
          </OutputClaims>
          <OutputClaimsTransformations>
            <OutputClaimsTransformation ReferenceId="GenerateSendTermsOfUseBody" />
          </OutputClaimsTransformations>
          <UseTechnicalProfileForSessionManagement ReferenceId="SM-AAD" />
        </TechnicalProfile>
    ```
    
   </div>
</details>

<details>
   <summary>Creamos un technical profile que envíe los términos de uso a nuestra api: SPOLIER</summary>
   <div class="description">

    ```xml
        <TechnicalProfile Id="CUSTOM-SendTermsOfUse">
          <DisplayName>Send terms of use acceptation</DisplayName>
          <Protocol Name="Proprietary" Handler="Web.TPEngine.Providers.RestfulProvider, Web.TPEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" />
          <Metadata>
            <Item Key="ServiceUrl">https://b2capidemo.azurewebsites.net/api/terms-of-use</Item>
            <Item Key="AuthenticationType">None</Item>
            <Item Key="SendClaimsIn">Body</Item>
            <Item Key="AllowInsecureAuthInProduction">true</Item>
            <Item Key="DefaultUserMessageIfRequestFailed">Cannot process your request right now, please try again later.</Item>
            <Item Key="ClaimUsedForRequestPayload">termsOfUseBody</Item>
          </Metadata>
          <InputClaims>
            <InputClaim ClaimTypeReferenceId="termsOfUseBody" />
          </InputClaims>
          <UseTechnicalProfileForSessionManagement ReferenceId="SM-Noop" />
        </TechnicalProfile>
    ```
    
   </div>
</details>

<details>
   <summary>Creamos un techincal profile para guardar el usuario en el AAD con las claims extras: SPOLIER</summary>
   <div class="description">

    ```xml
        <TechnicalProfile Id="SignupUser">
          <DisplayName>Sign-up user</DisplayName>
          <Protocol Name="Proprietary" Handler="Web.TPEngine.Providers.SelfAssertedAttributeProvider, Web.TPEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" />
          <Metadata>
            <Item Key="IpAddressClaimReferenceId">IpAddress</Item>
            <Item Key="ContentDefinitionReferenceId">api.localaccountsignup</Item>
          </Metadata>
          <CryptographicKeys>
            <Key Id="issuer_secret" StorageReferenceId="B2C_1A_TokenSigningKeyContainer" />
          </CryptographicKeys>
          <InputClaims>
            <InputClaim ClaimTypeReferenceId="email"/>
            <InputClaim ClaimTypeReferenceId="newPassword" />
            <InputClaim ClaimTypeReferenceId="reenterPassword" />
            <InputClaim ClaimTypeReferenceId="displayName" />
            <InputClaim ClaimTypeReferenceId="givenName" />
            <InputClaim ClaimTypeReferenceId="surname" />
            <InputClaim ClaimTypeReferenceId="termsOfUse" />
          </InputClaims>
           <OutputClaims>
            <OutputClaim ClaimTypeReferenceId="objectId" />
            <OutputClaim ClaimTypeReferenceId="email" Required="true" />
            <OutputClaim ClaimTypeReferenceId="newPassword" Required="true" />
            <OutputClaim ClaimTypeReferenceId="reenterPassword" Required="true" />
            <OutputClaim ClaimTypeReferenceId="executed-SelfAsserted-Input" DefaultValue="true" />
            <OutputClaim ClaimTypeReferenceId="authenticationSource" />
            <OutputClaim ClaimTypeReferenceId="newUser" />

            <!-- Optional claims, to be collected from the user -->
            <OutputClaim ClaimTypeReferenceId="displayName" />
            <OutputClaim ClaimTypeReferenceId="givenName" />
            <OutputClaim ClaimTypeReferenceId="surName" />
            <OutputClaim ClaimTypeReferenceId="termsOfUse"/>
          </OutputClaims>
          <ValidationTechnicalProfiles>
            <ValidationTechnicalProfile ReferenceId="AAD-UserWriteUsingLogonEmail" />
          </ValidationTechnicalProfiles>
          <UseTechnicalProfileForSessionManagement ReferenceId="SM-AAD" />
        </TechnicalProfile>
    ```
    
   </div>
</details>

<details>
   <summary>Incluimos los siguientes steps en nuestra USerJourney: SPOLIER</summary>
   <div class="description">

    ```xml
        <OrchestrationStep Order="5" Type="ClaimsExchange">
          <ClaimsExchanges>
            <ClaimsExchange Id="GetUserInfo" TechnicalProfileReferenceId="GetUserInfo" />
          </ClaimsExchanges>
        </OrchestrationStep>
        <OrchestrationStep Order="6" Type="ClaimsExchange">
          <ClaimsExchanges>
            <ClaimsExchange Id="TermsOfUse" TechnicalProfileReferenceId="CUSTOM-SendTermsOfUse" />
          </ClaimsExchanges>
        </OrchestrationStep>
        <OrchestrationStep Order="7" Type="ClaimsExchange">
          <ClaimsExchanges>
            <ClaimsExchange Id="RegisterUser" TechnicalProfileReferenceId="SignupUser" />
          </ClaimsExchanges>
        </OrchestrationStep>
        <OrchestrationStep Order="8" Type="SendClaims" CpimIssuerTechnicalProfileReferenceId="JwtIssuer" />
    ```
    
   </div>
</details>