# Ejemplo de conector para B2C que realiza la validación de ReCaptcha utilizando ApiConnector + Azure Functions
1- Con una cuenta de google, acceder a la consola de administración y configurar una aplicación para usar ReCaptcha v2: `https://ayudapanel.com/temas-ayuda/servicios-externos/google-recaptcha.html`

2- Subir todos los archivos de la carpeta `blobs` salvo el fichero `selfAsserted.html` y obtener los links de acceso a cada uno de ellos.

3- En la carpeta `B2CApiConnectorRecaptcha\blobs` modificar el fichero `selfAsserted.html`
  - Linea 16 - Modificar la ruta de acceso al fichero `global.css` subido previamente
  - Linea 31 - Modificar la ruta de acceso al fichero `logo.png` subido previamente
  - Linea 51 - Modificar la clave del sitio por la creada en el paso 1. **IMPORTANTE: Introducimos la clave pública, no la secreta**

4- Subir el fichero `selfAsserted.html` al storage account

5- Publicar la función incluida en la carpeta `function` a Azure

6- En la sección `Configuration` de la Azure Function añadir las siguientes configuraciones:
  - B2C_EXTENSIONS_APP_ID: Identificador de la aplicación `b2c-extensions-app`. Para obtener el valor, desde el registro de aplicaciones, acceder a `Todas las aplicaciones`. La aplicación ya estará creada de antemano. Copiar el IdCliente
  - BASIC_AUTH_USERNAME: Usuario usado en la configuración del conector de B2C, `admin`
  - BASIC_AUTH_PASSWORD: Contraseña utilizada en la configuración del conector de B2C, `admin`
  - CAPTCHA_SECRET_KEY: Clave secreta de ReCaptcha generada previamente

7- Configurar el conector de API en B2C

  - Sección Conectores de API -> Nuevo conector de API
  - Configurar el nombre, dirección a la Azure Function creada previamente
  - Tipo de autenticación: básico
  - Usuario y contraseña incluidos en la configuración de la Azure Function

8- Crear un flujo de usuario de registro

9- Configurar el conector desde el detalle del flujo de usuario de registro - Seleccionamos el paso `Antes de crear el usuario` y elegimos el conector creado en el paso 7

10 - Ejecutar el flujo de usuario y validar el uso de ReCaptcha