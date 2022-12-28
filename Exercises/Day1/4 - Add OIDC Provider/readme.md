# Configuración de IDP en Azure B2C
Azure B2C permite configurar diferentes proveedores de identidad (IDP) para permitir al usuario realizar el flujo de login a través de ellos. Existen ciertos proveedores predefinidos para los cuales B2C facilita su configuración, aunque se puede configurar de forma fácil y sencilla cualquier proveedor que siga el protocolo OIDC.

## Configuración de IDP predefinido - Google
### Creación de la aplicación en la consola Google
1. Acceder a la consola de google con credenciales válidas
2. Crear una nueva credencial con el botón `Create credentials` situado en el menú superior
3. Seleccionar el tipo `OAuth client ID`
4. Seleccionar el tipo de aplicación `Web`
5. Proporcionar un nombre descriptivo (b2cworkshopapp)
6. En la sección `Authorized redirect URIs` introducir el valor correspondiente al tenant de B2C con formato `XXXX.b2clogin.com` o `https://XXXX.b2clogin.com/XXXX.onmicrosoft.com/oauth2/authresp`
7. Pulsar el botón `Create` y en la pantalla que aparece anotar el valor del `Client ID` y `Client Secret` correspondiente

### Configuración del IDP en B2C
1. Acceder a la página principal del tenant de Azure B2C
2. Seleccionar en el menú lateral la opción `Identity Providers`
3. Buscar y seleccionar en el listado la opción `Google`
4. En el formulario mostrado introducir los datos:
  - Nombre: Identificador descriptivo de la configuración (podemos usar el nombre de la app de Google, b2cworkshopapp)
  - Client ID: Valor obtenido tras la creación de la aplicación en la consola de Google
  - Client Secert: Valor obtenido tras la creación de la aplicación en la consola de Google
5. Guardamos la configuración

### Creación de un flujo de usuario que permita el login con Google
1. Desde la página principal del tenant de Azure B2C seleccionar la opción `User flows` bajo la sección `Policies`
2. Click en `New user flow`
3. Elegir la opción `Sign up and sign in` y la versión `Recommended`
4. Proporcionar un nombre identificativo del flujo (SignUpSignInWGoogle, por ejemplo)
5. En la sección `Identity providers` bajo `Social identity providers` seleccionar la opción `Google` y mantener `Local accounts` a `None`
6. Crear el flujo de usuario

### Prueba de un flujo de usuario que permita el login con Google
1. Desde la sección de `User flows`, identificar y seleccionar el flujo creado en el paso previo
2. Pulsar en el botón `Run user flow` situado en el menú superior
3. Seleccionar la aplicación creada en el ejercicio 1 (webapp)
4. Click en `Run user flow`
5. Acceder con la cuenta de Google correspondiente y validar que el token devuelto contiene la información esperada

### Configuración de un IDP custom en B2C
1. Acceder a la página principal del tenant de Azure B2C
2. Seleccionar en el menú lateral la opción `Identity Providers`
3. Seleccionar la opción `New OpenID Connect provider`
5. Introducir los valores correspondientes en la ventana del tenant de B2C:
  - Name: Identificador del proveedor (playground)
  - Metadata url: url al endpoint de autodiscovery, `https://orein.azurewebsites.net/.well-known/openid-configuration`
  - Client ID: `b2cworkshop`
  - Client Secret: `8ee24ee3-d1f0-4a42-b548-7b5192934a9e`
  - Scope: `openid`
  - Identity provider claims mapping
    - User ID: `sub`
    - Display name: `name`
6. Guardar los cambios
7. Crear un nuevo flujo de usuario y habilitar el proveedor creado
8. Ejecutar el flujo y validar que el token devuelto contiene los valores esperados. Para loguear con el IDP usar el usuario `testuser` password `Test1234567!`