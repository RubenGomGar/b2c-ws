# "Custom policies" con "Content definitions"

En este ejercicio vamos a desarrollar una `Custom Policy` con `Content definitions` que nos ayudarán a customizar la experiencia
de usuario y personalizar la apariencia de las páginas.

_**Nota:** para este ejercicio partiremos de las politicas base del ejercicio anterior. Es importante haber completado los anteriores ejercicios y sus pasos._

## Creación de `Custom Policy` con `Content definitions`

En este caso, vamos a utilizar la política base `B2C_1A_TrustFrameworkExtensions` y utilizaremos como plantilla inicial el fichero
`base.xml` de este ejercicio.

A partir de este fichero, vamos a modificarlo para conseguir lo siguiente:
- Customizar el flujo de `SignUp` con cuenta local con nuestro propio HTML.

1. Para empezar debemos de descargar el fichero correspondiente al flujo que vamos a customizar, en este caso se trata de la página `Self-Asserted` que podemos [descargar su base desde la documentación (con diferentes temas)](https://learn.microsoft.com/en-us/azure/active-directory-b2c/customize-ui-with-html?pivots=b2c-custom-policy#customize-the-default-azure-ad-b2c-pages).

2. Realizamos algunas modificaciones al HTML como cambiar la referencia al logo (buscando por `<img class="companyLogo"`) y añadiendo algún elemento HTML o CSS.

3. Creamos un storage account o reutilizamos uno existente para subir este fichero HTML. Tendremos que subirlo a un container público de nombre `root` y en su base. Copiaremos su ruta absoluta y la reemplazaremos en el fichero `base.xml` (podemos copiarlo y renombrarlo para tener una referencia base).

4. Modificamos las configuraciones `Resource sharing (CORS)` de nuestro Storage account para permitir todos los orígenes desde el dominio de nuestro tenant (que tendra la forma `https://{{TENANT_NAME}}.b2clogin.com`). Para ello, en las pestañas `Blob Service` y `File Service` rellenamos los campos de la siguiente manera (por cada pestaña):
    - Allowed origins: `https://{{TENANT_NAME}}.b2clogin.com` (reemplazando `{{TENANT_NAME}}`).
    - Allowed methods: GET, OPTIONS
    - Allowed headers: *
    - Exposed headers: *
    - Max age: 200

5. Subimos nuestra política y comprobamos que se carga correctamente con nuestra template modificada y podemos completar el flujo de manera correcta.
