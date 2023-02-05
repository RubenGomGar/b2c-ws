# Praparación escenarios prácticos

Para poder preparar los escenarios vamos a necesitar de los flujos por defecto que nos ofrede Microsoft. En este caso vamos a utilizar los ficheros

- TrustFrameworkBase.xml
- TrustFrameworkExtensions.xml
- TrustFrameworkLocalization.xml

Para que cada uno trabaje con su propio fichero vamos a renombrar las políticas.

1. Modificamos el fichero TrustFrameworkBase.xml añadiendo nuestras iniciales con formato `_[INICIALES]` en los atributos `PolicyId` y `PublicPolicyUrl`

Debería quedar así:

```xml
    <TrustFrameworkPolicy
        xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
        xmlns:xsd="http://www.w3.org/2001/XMLSchema"
        xmlns="http://schemas.microsoft.com/online/cpim/schemas/2013/06"
        PolicySchemaVersion="0.3.0.0"
        TenantId="Hellofreshgotest.onmicrosoft.com"
        PolicyId="B2C_1A_TrustFrameworkBase_RLGG"
        PublicPolicyUri="http://Hellofreshgotest.onmicrosoft.com/B2C_1A_TrustFrameworkBase_RLGG">
```

2. Repetimos los pasos para el fichero TrustFrameworkExtensions.xml y TrustFrameworkLocalization.xml
3. Reemplazamos los identificadores de los techincal progiles `AAD-Common` y `login-NonInteractive` del fichero TrustFrameworkExtensions.xml con nuestras aplicaciones `b2c-extensions-app`, `ProxyIdentityExperienceFramework` y `IdentityExperienceFramework`

El código está comentado con las propiedades de donde sacarlo:

```xml
    <TechnicalProfile Id="AAD-Common">
      <Metadata>
        <!--Insert b2c-extensions-app application ID here, for example: 11111111-1111-1111-1111-111111111111-->  
        <Item Key="ClientId">13471245-4337-4c57-8be6-5bf7d7ac2d17</Item>
        <!--Insert b2c-extensions-app application ObjectId here, for example: 22222222-2222-2222-2222-222222222222-->
        <Item Key="ApplicationObjectId">861310f1-fc65-4651-8930-a2a8c8be161a</Item>
      </Metadata>
    </TechnicalProfile>


    <TechnicalProfile Id="login-NonInteractive">
      <Metadata>
        <Item Key="client_id">f62dc048-00bb-46c1-b856-4b1ef2d9c189</Item> <!-- ProxyIdentityExperienceFramework ApplicationId  -->
        <Item Key="IdTokenAudience">b57d1d03-4b79-4be5-a9d9-b42d11bee83c</Item> <!-- IdentityExperienceFramework ApplicationId -->
      </Metadata>
      <InputClaims>
        <InputClaim ClaimTypeReferenceId="client_id" DefaultValue="f62dc048-00bb-46c1-b856-4b1ef2d9c189" /> <!-- ProxyIdentityExperienceFramework ApplicationId  -->
        <InputClaim ClaimTypeReferenceId="resource_id" PartnerClaimType="resource" DefaultValue="b57d1d03-4b79-4be5-a9d9-b42d11bee83c" /> <!-- IdentityExperienceFramework ApplicationId -->
      </InputClaims>
    </TechnicalProfile>
```

4. Subimos las nuevas políticas a nuestro tenant de B2C.