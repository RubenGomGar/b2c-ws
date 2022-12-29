# Personalización del branding
Azure B2C cuenta con distintos niveles de personalización de la interfaz presentada al usuario final en los flujos de usuario

## Personalización del branding de la compañía
1. Desde el menú lateral de la página principal del tenant de Azure B2C, acceder a `Company branding`
2. Si se muestra un mensaje `STATUS: Not configured`, hacer click en el boton `Configure` del menú superior
3. La página mostrada presenta una configuración general de las ventanas de login, registro y edición de contraseña.
4. En la sección `Sign-in page background image` seleccionar el archivo `background.png`
5. En la sección `Banner logo` seleccionar el archivo `banner.jpg`
6. Guardar los cambios con el botón `Save`
7. Lanzar cualquier flujo de usuario y validar que la interfaz ha cambiado acorde a los cambios realizados
8. Una vez validado, eliminar las customizaciones para futuros ejercicios

**Esta configuración es básica y aplica de manera global a todos los flujos de usuario, aunque se puede separar por idiomas.**

## Personalización del branding desde un flujo de usuario
1. Desde la página principal del tenant de Azure B2C seleccionar la opción `User flows` bajo la sección `Policies`
2. Click en `New user flow`
3. Elegir la opción `Sign up and sign in` y la versión `Recommended`
4. Proporcionar un nombre identificativo del flujo (SignUpSignInWCustomUI, por ejemplo)
5. En la sección `Identity providers` seleccionar la opción `Local accounts` a `Email signup`
6. Pulsar el botón `Create`
7. Seleccionar la opción `Page layouts` desde el menú lateral, bajo la sección `Customize`
8. La página muestra diferentes configuraciones según el tipo de flujo de usuario creado. Al usar el flujo de login y registro, contamos con todas las pantallas de login, registro de usuario, autenticación multifactor, reseteo de contraseña...
9. Validar que la opción seleccionada es `Unified sign up or sign in page`
10. En la parte inferior de la página, activar la opción `Use custom page content`
11. Descargar la página desde la [web de Microsoft](https://learn.microsoft.com/en-us/azure/active-directory-b2c/customize-ui-with-html?pivots=b2c-user-flow) con el template `Classic` o utilizar el archivo `susi.html`
12. Modificar la página descargada para incluir como imagen de fondo `background.png` (línea 40). Podéis usar el [link](https://strworkshopb2c.blob.core.windows.net/main/background.png) o subir la imagen a algún lugar público
**IMPORTANTE: NO se debe modificar la estructura generada en los templates ya que es usada por B2C para generar dinámicamente los controles necesarios según el tipo de página. Se pueden añadir elementos pero nunca eliminarlos ni modificar los identificadores de los mismos**
13. Cambiar la url en `Custom page URI` para apuntar al template generado. Podéis usar el [link](https://strworkshopb2c.blob.core.windows.net/main/susi.cshtml) o subir la página a algún lugar público
14. Guardar los cambios con el botón `Save`
15. Lanzar el flujo de usuario y validar los cambios

## Personalización de los idiomas y sus valores desde un flujo de usuario
1. Desde la página principal del tenant de Azure B2C seleccionar la opción `User flows` bajo la sección `Policies`
2. Click en `New user flow`
3. Elegir la opción `Sign up and sign in` y la versión `Recommended`
4. Proporcionar un nombre identificativo del flujo (SignUpSignInWCustomLanguageValues, por ejemplo)
5. En la sección `Identity providers` seleccionar la opción `Local accounts` a `Email signup`
6. Bajo la sección `User attributes and token claims` hacer click en `Show more`
8. Seleccionar sobre los atributos `Country/Region`, `departmentName` y `DisplayName` las dos columnas disponibles `Collect attribute` y `Return claim`
9. Pulsar el botón `Create`
10. Seleccionar la opción `Languages` desde el menú lateral, bajo la sección `Customize`
11. Por defecto los flujos de usuario no soportan multi-idioma. Para activar el soporte hacer click en el botón `Enable language customization` de la barra superior
12. Tras habilitar el soporte multi-idioma, en la tabla inferior, bajo la sección `All` se muestran todos los idiomas con soporte por defecto en Azure B2C.
13. Buscar el valor `español`, hacer click sobre el y marcar la opción `Enabled`
14. Marcar el valor `Default` a true para incluirlo como idioma por defecto
14. En la parte inferior a la opción `Default` se muestran las distintas páginas para las que se dispone de traducciones para el flujo correspondiente. Azure B2C ya proporciona en cada una de ellas unas traducciones por defecto, aunque se permite customizarlas.
15. Buscar la opción `Local account sign up page` y abrir el desplegable
16. Click en `Download defaults (es)` para descargar la lista de traducciones
17. Añadir al final del archivo una nueva entrada o utilizar el archivo `spanish.json`:
```json
{
    "ElementType": "ClaimType",
    "ElementId": "extension_departmentName",
    "StringId": "DisplayName",
    "Override": true,
    "Value": "Nombre del departamento"
}
```
18. Subir el archivo modificado
19. Guardar los cambios con el botón `Save`
19. Ejecutar el flujo con la opcion `Run user flow`
20. Una vez seleccionada la aplicación, identificar una nueva pestaña `Localization` donde se permite indicar el idioma del flujo a probar
21. Activar la opción `Specify ui_locales`
22. Marcar como idioma `es - español`
23. Lanzar el flujo. Todo el flujo ha sido traducido automáticamente al idioma especificado
24. Click en `Registrarse ahora`
25. En la página de registro, validar que el campo del atributo `departmentName` contiene el valor introducido en el fichero de recursos