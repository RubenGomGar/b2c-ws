# Proyecto de ejemplo de autenticación B2C utilizando la librería de Microsoft “Microsoft.Identity.Web”.
De cara a poder hacerlo funcionar:

1- Generar el tenant de B2C

2- Crear flujos de usuario:
- Registro y login
- Editar perfil

3- Crear dos aplicaciones (Cliente y servicio)
- App servicio:
    - En la sección `Exponer una API` generar un ámbito (scope) y registrar el valor de este
- App cliente:
    - Sección Autenticación:
      - Configurar redirección del tipo `Web` y con las urls de redirección `https://localhost:5000/signin-oidc` y `https://localhost:5000`
      - Configurar url de cierre de sesión `https://localhost:5000/signout-oidc`
      - En la sección `Flujos de concesión implícita e híbridos` habilitar la opción de que la aplicación pueda pedir IdToken y AccessToken desde el endpoint de authorización
    - Sección Certificados y secretos: generar un secreto para la aplicación
    - Sección `Permisos de API`:
      - Click en agregar un permiso
      - Seleccionar la 3º opción `Mis API` y la aplicación que creada previamente
      - Marcar el permiso creado para la API
      - Una vez el permiso se ha añadido a la lista de permisos configurados, hacer click en `Conceder consentimiento de administrador` y accedemos a dar consentimiento en la ventana que aparece

4- Actualizar los valores correspondientes a estos en los ficheros appsettings.json de los proyectos
  - TodoListService (app servicio):
    - Instance: `https://XXXX.b2clogin.com`
    - ClientId: Id devuelto por B2C. Se puede comprobar desde el detalle de la aplicación generada.
    - Domain: `XXXX.onmicrosoft.com`
    - SignedOutCallbackPath: mantener el valor por defecto
    - SignUpSignInPolicyId: Nombre de la política de registro y login creada previamente
  - TodoListClient (app cliente):
    - Instance: `https://XXXX.b2clogin.com`
    - ClientId: Id devuelto por B2C. Se puede comprobar desde el detalle de la aplicación generada.
    - Domain: `XXXX.onmicrosoft.com`
    - SignedOutCallbackPath: mantener el valor por defecto
    - SignUpSignInPolicyId: Nombre de la política de registro y login creada previamente
    - EditProfilePolicyId: Nombre de la política de edición creada previamente
    - ClientSecret: Valor del secreto creado para la app cliente
    - TodoListScope: Valor del scope/ámbito creado para la aplicación servicio
