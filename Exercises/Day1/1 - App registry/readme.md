# Creación de aplicación de test
En este ejemplo se creará una aplicación web la cual permitirá realizar las validaciones de los ejemplos de este taller
1. Desde el portal de Azure, buscar y acceder a `Azure AD B2C`
2. En el menu lateral, seleccionar `App registrations`
3. Elegir la opción `New registration`
4. Introducir el nombre de la aplicación, por ejemplo `webapp`
5. En el menú `Supported account types`, seleccionar la opción `Accounts in any identity provider or organizational directory`
6. Bajo el menú `Redirect Uri` elegir la opción `Web` e introducir `https://jwt.ms` como url de redirección
**IMPORTANTE: Azure B2C define las aplicaciones Web como aplicaciones que realizar la mayor parte de gestiones en el servidor**
7. En la sección de `Permissions`, marcar el checkbox `Grant admin consent to openid and offline_access permissions`
8. Seleccionar `Register`

## Creación de un secreto para la aplicación de ejemplo
1. Bajo la aplicación creada en el paso anterior, seleccionar el menú `Certificates & secrets`
2. Click en `New secret`
3. Incluir una descripción que permita identificar al secreto y una validez del mismo (mantener el valor por defecto si se desea)
4. Click en `Add`
5. Registrar el valor del secreto (bajo la columna `Value`). Este valor no volverá a ser mostrado una vez se abandone la página.

## Habilitar el flujo implícito
- Seleccionar la opción `Authentication` bajo la sección `Manage`
- En la sección `Implicit grants and hybrid flows`, marcar los valores `Access tokens` y `ID tokens`
- Click en `Save` para registrar los cambios