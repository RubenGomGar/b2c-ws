# Flujo Customize User Inpit

Para este ejercicio lo que necesitamos es añadir un campo nuevo en nuestro flujo de registro. Para ello vamos a tomar como base el `0.1-CustomSignUp` que ya hemos hecho anteriormente.

Lo que necesitamos hacer es añadir es una definición de la claim nueva e incluirta en nuestro terchnical profile de SignUp:


1. Definición de la claim.
    - La vamos a llamar  `city`
    - Va a ser de tipo DropdownSingleSelect
    - Los valores van a ser de tipo string
    - El contenido de los valores serán `Berlin,London,Seattle`
    - El valor por defecto debería ser `London`

2. Añadimos la nueva claim en nuestro techincal profile
    - Buscamos el techical profile de registro
    - Lo añadimos en nuestro flujo y solo definimos las secciones de InputClaims y OutputClaims
    
    > Cuando usamos un techical profile existente en nuestros flujos custom, lo que hacemos es ampliarlo con más funcionalidad.
    > (Lo tomamos como herencia o como referencia base)

<details>
   <summary>STEP 1 SPOLIER</summary>
   <div class="description">

    ```xml
      <ClaimType Id="city">
        <DisplayName>City where you work</DisplayName>
        <DataType>string</DataType>
        <UserInputType>DropdownSingleSelect</UserInputType>
        <Restriction>
            <Enumeration Text="Berlin" Value="berlin" />
            <Enumeration Text="London" Value="london" SelectByDefault="true" />
            <Enumeration Text="Seattle" Value="seattle" />
        </Restriction>
      </ClaimType>
    ```

   </div>
</details>

<details>
   <summary>STEP 2 SPOLIER</summary>
   <div class="description">

    ```xml
        <TechnicalProfile Id="LocalAccountSignUpWithLogonEmail">

          <InputClaims>
            <InputClaim ClaimTypeReferenceId="email"/>
            <InputClaim ClaimTypeReferenceId="newPassword" />
            <InputClaim ClaimTypeReferenceId="reenterPassword" />
            <InputClaim ClaimTypeReferenceId="displayName" />
            <InputClaim ClaimTypeReferenceId="givenName" />
            <InputClaim ClaimTypeReferenceId="surname" />
            <InputClaim ClaimTypeReferenceId="city" />
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
            <OutputClaim ClaimTypeReferenceId="city"/>
          </OutputClaims>

        </TechnicalProfile>
    ```
    
   </div>
</details>