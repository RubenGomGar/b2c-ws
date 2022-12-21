# Proyecto de ejemplo de autenticación B2C desde una SPA y con validación de la autenticación desde el Api.
De cara a poder hacerlo funcionar:

1- Generar el tenant de B2C

2- Crear un flujo de usuario de registro y login

3- Crear aplicación
 - Sección Autenticación:
    - Configurar redirección del tipo `SPA` y con la url de redirección `https://localhost:5022/callback.html`
    - En la sección `Configuración avanzada` habilitar la opción de flujos móviles y de escritorio
    - Sección `Exponer una API`: generar un ámbito (scope) y registrar el valor de este
    - Sección `Permisos de API`:
      - Click en agregar un permiso
      - Seleccionar la 3º opción `Mis API` y la aplicación actual
      - Marcar el permiso creado para la API
      - Una vez el permiso se ha añadido a la lista de permisos configurados, hacer click en `Conceder consentimiento de administrador` y accedemos a dar consentimiento en la ventana que aparece

4- Proyecto Spa.Vue
  - En la ruta `B2COIDC\Spa.Vue\spa` hacer un `npm install`
  - En el fichero `B2COIDC\Spa.Vue\spa\src\main.ts`
    - Authority: `https://NOMBRE TENANT B2C.b2clogin.com/NOMBRE TENANT B2C.onmicrosoft.com/NOMBRE DEL FLUJO CREADO/v2.0`
    - ClientId: Id devuelto por B2C. Se puede comprobar desde el detalle de la aplicación generada.
    - Redirect_Uri: Mantener el valor por defecto
    - Response_Type: Mantener el valor por defecto
    - Scope: Valor del scope/ámbito creado para la aplicación
5- Proyecto Api
  - En el fichero `Startup`, en la llamada a `AddJwtBearer`
    - Authority: `https://NOMBRE TENANT B2C.b2clogin.com/NOMBRE TENANT B2C.onmicrosoft.com/NOMBRE DEL FLUJO CREADO/v2.0`
    - Audience: Id devuelto por B2C. Se puede comprobar desde el detalle de la aplicación generada. Tiene que ser el mismo valor que el ClientId del proyecto SPA