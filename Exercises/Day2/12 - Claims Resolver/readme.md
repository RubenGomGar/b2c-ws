# Claims Resolvers

Los `Claims Resolvers` proporcionan información sobre una petición de autorización. Ejemplos de esta información pueden ser:
- Nombre de la política
- CorrelationId
- Idioma utilizado

Estos claims pueden ser consumidos por la propia aplicación cliente o por la propia política, como por ejemplo customizar la interfaz del flujo en función del idioma mostrado mediante la carga de un `ContentDefinition` cuya url es construida dinámicamente:
```xml
<ContentDefinition Id="api.signuporsignin">
  <LoadUri>https://TENANT.blob.core.windows.net/{Culture:LanguageName}/unified.html</LoadUri>
  ...
</ContentDefinition>
```

Existen también claims propios del protocolo de autenticación (OIDC o SAML). La lista completa de claims se puede consultar en [Azure B2C Claims Resolvers](https://learn.microsoft.com/en-us/azure/active-directory-b2c/claim-resolver-overview). Es importante identificar si es posible usar `ClaimsResolvers` en el elemento deseado, en el link anterior bajo la sección `Using claim resolvers` podremos realizar dicha confirmación

## Uso de Claims Resolvers como claims de token
1. Definimos los claims a exponer junto al token. En este caso se usarán:
    1. CorrelationId
    2. TenantId
    3. ClientId
    4. IpAddress
2. La sección ClaimsSchema tiene que quedar:
```xml
<ClaimType Id="correlationId">
    <DisplayName>correlationId</DisplayName>
    <DataType>string</DataType>
    <AdminHelpText>correlationId</AdminHelpText>
</ClaimType>
<ClaimType Id="clientId">
    <DisplayName>clientId</DisplayName>
    <DataType>string</DataType>
    <AdminHelpText>clientId</AdminHelpText>
</ClaimType>
<ClaimType Id="tenantId">
    <DisplayName>tenantId</DisplayName>
    <DataType>string</DataType>
    <AdminHelpText>tenantId</AdminHelpText>
</ClaimType>
<ClaimType Id="ipAddress">
    <DisplayName>ipAddress</DisplayName>
    <DataType>string</DataType>
    <AdminHelpText>ipAddress</AdminHelpText>
</ClaimType>
```
3. Definimos los claims a exponer junto al token en el `TechnicalProfile` del elemento `RelyingParty` como claims de salida:
    1. TenantId - utilizando el attributo `AlwaysUseDefaultValue` y como valor `DefaultValue="{Policy:TenantObjectId}"`
    2. CorrelationId - utilizando el attributo `AlwaysUseDefaultValue` y como valor `DefaultValue="{Context:CorrelationId}"`
    3. ClientId - utilizando el attributo `AlwaysUseDefaultValue` y como valor `DefaultValue="{OIDC:ClientId}"`
    4. IpAddress - utilizando el attributo `AlwaysUseDefaultValue` y como valor `DefaultValue="{Context:IPAddress}"`
4. Ejecutamos el flujo y verificamos que los claims son expedidos junto al token