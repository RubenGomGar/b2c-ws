# Api connectors en Azure B2C
Los conectores de Api permiten customizar el flujo de login/registro del usuario mediante llamadas a endpoints que devuelvan o reciban datos. Ejemplos:
- Validar ciertos valores que el usuario haya introduccido en el registro (número de póliza válido en nuestro sistema)
- Devolver información extra del usuario (mediante el email del usuario, nuestro sistema devuelve información mas extensa del usuario sin necesidad de que el mismo introduzca todos los datos)

## Añadir atributo custom para rellenar desde el conector
1. Desde el menú lateral de la página principal del tenant de Azure B2C, acceder a `User attributes`
2. Seleccionar `Add` en el menú superior
3. Introducir la información correspondiente al nuevo atributo:
  - Name: appRoles
  - Data type: string
  - Description: appRoles por user
4. Pulsar el botón `Create`

## Crear conector que devuelva atributos extra en el registro de usuarios
1. Publicar la función de Azure de la carpeta src del ejemplo (si no se desea publicar, se muestra la alternativa mas adelante)
2. Desde la página principal del tenant de Azure B2C seleccionar la opción `Api Connectors` bajo la sección `Manage`
3. Click en `New API connector`
4. Rellenar los valores correspondientes:
    - DisplayName: Nombre identificativo del conector
    - Endpoint URL: Endpoint público de la función publicada (si no se ha subido, usar el endpoint `https://b2cworkshoppcfunction.azurewebsites.net/api/Function1?code=N4YWdr3lLIk9yh9gMOVX3ArJiqQEj_pKIFp0biW9DyLnAzFuz_xJmA==`)
    - Authentication Type: Basic
    - Username: Usuario configurado en la función bajo la clave `BASIC_AUTH_USERNAME` (admin) 
    - Password: Contraseña configurada en la función bajo la clave `BASIC_AUTH_PASSWORD` (admin)
5. Guardar los cambios con el botón `Save`

## Añadir flujo que use el conector
1. Desde la página principal del tenant de Azure B2C seleccionar la opción `User flows` bajo la sección `Policies`
2. Click en `New user flow`
3. Elegir la opción `Sign up and sign in` y la versión `Recommended`
4. Proporcionar un nombre identificativo del flujo (SignUpSignInWApiConnectorAppRoles, por ejemplo)
5. En la sección `Identity providers` seleccionar la opción `Local accounts` a `Email signup`
6. Bajo la sección `User attributes and token claims` hacer click en `Show more`
8. Seleccionar sobre el atributo `appRoles` las dos columnas disponibles `Collect attribute` y `Return claim`
9. Pulsar el botón `Create`
**Para poder rellenar automáticamente un atributo desde un conector de Api, es necesario incluir el atributo como Collect attribute y Return claim. Esto implica que se muestra el input al usuario, pero podemos usar css o javascript para ocultar esta información. En este caso usaremos el template custom de la página de registro usado en el ejemplo 6 el cual contiene una regla css para ocultar este campo**
10. Sobre el flujo creado, acceder a la seccion `Page Layouts`
11. Buscar el layout `Local account sign page` y marcar la opción `Use custom page content`
12. Introducir la url pública de acceso a la página para apuntar al template generado. Podéis usar el [link](https://strworkshopb2c.blob.core.windows.net/main/susi.cshtml)
13. Guardar los cambios con el botón `Save`
14. Acceder a la sección `Api connectors` del menú lateral
15. En el paso `Before creating the user`, seleccionar en el combo el conector creado previamente
16. Guardar los cambios con el botón `Save`
17. Lanzar el flujo y realizar un proceso de registro
18. Validar que no aparece el campo para el atributo `appRoles`
19. En el token generado, validar que aparece el claim con el atributo `extension_appRoles` y el valor generado

## Crear conector que valide un captcha usando ReCaptcha

### Crear Azure Function que valide un token ReCaptcha
1. Con una cuenta de google, acceder a la consola de administración y configurar una aplicación para usar ReCaptcha v2: `https://ayudapanel.com/temas-ayuda/servicios-externos/google-recaptcha.html`

2. Subir todos los archivos de la carpeta `blobs` salvo el fichero `selfAsserted.html` y obtener los links de acceso a cada uno de ellos.

3. Modificar el fichero `selfAsserted.html` de la carpeta `blobs`:
    - Linea 16 - Modificar la ruta de acceso al fichero `global.css` subido previamente
    - Linea 31 - Modificar la ruta de acceso al fichero `logo.png` subido previamente
    - Linea 51 - Modificar la clave del sitio por la creada en el paso 1. **IMPORTANTE: Introducimos la clave pública, no la secreta**

4. Subir el fichero `selfAsserted.html` al storage account. Si no se dispone de un storage account para subir los ficheros, utilizad la url `https://strworkshopb2c.blob.core.windows.net/main/selfAsserted.html`

6. Ejecutar el comando `npm install` en la carpeta de la función para instalar las dependencias

7. Crear un zip con todo el contenido de la carpeta `function` (desde dentro de esa misma carpeta)

8. Publicar la función incluida en la carpeta `function` a Azure. Para subirla utilizando un terminal, es necesario crear desde el portal de Azure una `Azure Function` de `Node` y sistema operativo `Windows`. Posteriormente, en la máquina local instalamos [Azure Functions Core Tools](https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local) y desde un terminal 
realizar las acciones:
    - az login - introducir las credenciales correctas. Tras realizar el proceso de autenticación se muestran todas las suscripciones asociadas a la cuenta. Identificad la suscripción correcta y anotad el identificador de la misma (valor "id")
    - az account set --subscription "IDENTIFICADOR DE LA SUSCRIPCION"
    - az functionapp deployment source config-zip -g NOMBREGRUPORECURSOS -n NOMBREAZUREFUNCTION --src ZIPGENERADO.zip

9. En la sección `Configuration` de la Azure Function añadir las siguientes configuraciones:
    - B2C_EXTENSIONS_APP_ID: Identificador de la aplicación `b2c-extensions-app`. Para obtener el valor, desde el registro de aplicaciones, acceder a `Todas las aplicaciones`. La aplicación ya estará creada de antemano. Copiar el IdCliente y eliminar los guiones
    - BASIC_AUTH_USERNAME: Usuario usado en la configuración del conector de B2C, `admin`
    - BASIC_AUTH_PASSWORD: Contraseña utilizada en la configuración del conector de B2C, `admin`
    - CAPTCHA_SECRET_KEY: Clave secreta de ReCaptcha generada previamente

### Crear conector de Azure B2C que llame a la función de Azure
1. Desde la sección `Api connectors` pulsar el boton `New Api Connector`
    - Configurar el nombre, dirección a la Azure Function creada previamente. Si no se ha creado, utilizar `https://recaptchafunctionpc.azurewebsites.net/api/function?code=Zs1Wc5_w87uHw7XzkJMEfa3IGRlXQtqdMP5UmLflZbKbAzFu97QvVw==`
    - Tipo de autenticación: básico
    - Usuario y contraseña incluidos en la configuración de la Azure Function (admin, admin)

2. Añadir un atributo en la sección `Custom attributes`:
    - Name: CaptchaUserResponseToken
    - Data Type: string

### Crear flujo de usuario que use el conector
1. Desde la página principal del tenant de Azure B2C seleccionar la opción `User flows` bajo la sección `Policies`
2. Click en `New user flow`
3. Elegir la opción `Sign up and sign in` y la versión `Recommended`
4. Proporcionar un nombre identificativo del flujo (SignUpSignInWRecaptchaConnector, por ejemplo)
5. En la sección `Identity providers` seleccionar la opción `Local accounts` a `Email signup`
6. Bajo la sección `User attributes and token claims` hacer click en `Show more`
7. Seleccionar sobre los atributos `CaptchaUserResponseToken` únicamente sobre la columna `Collect attribute`
8. Pulsar el botón `Create`
9. Desde la sección `Properties` activar la opción `Enable JavaScript enforcing page layout` y guardar los cambios con el botón `Save`
10. Seleccionar la opción `Page layouts` desde el menú lateral, bajo la sección `Customize`
11. Click sobre la opción `Local account sign up page`
12. En la parte inferior de la página, activar la opción `Use custom page content`
13. Modificar la sección `Custom page URI` con la url pública al archivo `selfAsserted.html`. Podéis usar el [link](https://strworkshopb2c.blob.core.windows.net/main/selfAsserted.html)
14. Guardar los cambios con el botón `Save`
15. Configurar el conector desde la sección `API Connectors` del menú lateral
16. Seleccionar el conector creado previamente bajo la sección `Before creating the user`
17. Guardar los cambios con el botón `Save`
18. Lanzar el flujo con el botón `Run user flow` de la barra superior
19. Pulsar el botón de registro `Sign up now`
20. Introducir unas credenciales válidas de registro y completar el Captcha mostrado
21. Validar que el usuario se crea correctamente
