# Creación de flujos de usuario predefinidos

Azure B2C permite dos posibilidades de definir la experiencia del usuario:
  - Flujos de usuario: políticas predefinidas y configurables que permiten gestionar el registro, login y edición del perfil de manera casi inmediata
  - Políticas custom: usadas para definir escenarios mas complejos o flujos completamente customizados los cuales no están soportados con flujos de usuario.

En este ejemplo veremos como definir un flujo de usuario predefinido que gestione el registro de un nuevo usuario.

## Creación del flujo de registro de usuario
1. Desde la página principal del portal de Azure, buscar y seleccionar `Azure AD B2C`
2. En el menu lateral, buscar el valor `User flows` bajo la sección `Policies`
3. Seleccionar la opcion `New user flow` y en el siguiente menú, el flujo de usuario deseado `Sign up and Sign in`
4. Elegir la versión del flujo recomendada. Los flujos legacy corresponden a la antigua versión v1 de Azure B2C y no reciben soporte ni actualizaciones.
5. Introducir un nombre para el flujo

**Es importante mencionar que Azure B2C proporciona un prefijo para estos flujos con el formato `B2C_1_`. Este prefijo permite distinguir entre flujos de usuario o custom policies (las cuales tienen como prefijo `B2C_1A_`)**
6. Seleccionar `Email signup` como proveedor de identidad (en futuros ejercicios se verá mas información al respecto)
7. Mantener la autenticación multifactor activada
8. Bajo la sección `User attributes and claims` seleccionar la opción `Show more` y marcar como `Collect attribute` y `Return claim` los siguientes valores:
 - Display name
 - Job title
 - Postal code
 - Country/region

**Azure B2C proporciona control total sobre que información/atributos se quieren registrar para cada usuario `Collect attribute` y que información se quiere devolver como parte del token de acceso `Return claim`**

9. Seleccionar `Create` para registrar el fujo de usuario

## Prueba del flujo de registro de usuario
- Seleccionar el flujo creado en la sección anterior
- En el menú superior, click en `Run user flow`
- Seleccionar la aplicación web registrada en el ejercicio previo `webapp` y validar que la url de redirección corresponde a `https://jwt.ms`
- En la pestaña del navegador abierta, completar el fujo de registro. Tras la creación del mismo, el token generado con los atributos requeridos se muestra bajo la página `https://jwt.ms`. 