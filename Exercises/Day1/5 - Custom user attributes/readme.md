# Uso de atributos custom en B2C
Azure B2C permite personalizar la información que se recoje de los usuarios en el proceso de registro así como control total sobre que atributos se incluyen en el token de acceso generado.

Azure B2C cuenta con un listado de atributos de usuario globales. Después en cada uno de los distintos flujos de usuario se controla que atributos se quieren almacenar y que atributos (no tienen por qué ser los mismos) se quieren exponer como claims en el token.

## Definición de atributos custom
1. Desde el menú lateral de la página principal del tenant de Azure B2C, acceder a `User attributes`
2. En esta página se encuentra un listado global de todos los atributos definidos, los cuales pueden ser atributos predefinidos por el propio Azure B2C o customizados (identificados por la columna `Attribute type`)

**IMPORTANTE: Como buena práctica, no se debería almacenar información sensible de los usuarios. Es interesante mencionar que estos atributos pueden ser consumidos desde el API de Graph de Microsoft**

3. Seleccionar `Add` en el menú superior
4. Introducir la información correspondiente al nuevo atributo:
  - Name: nombre del atributo. Coincide con el nombre del claim generado si se incluye como parte del Access Token (departmentName)
  - Data type: tipo de dato del atributo. Si se recoje este atributo desde el registro del usuario, también se controla cómo se muestra el control asociado (string)
  - Description: Descripción del atributo, únicamente a nivel informativo. Nunca es mostrado al usuario
5. Pulsar el botón `Create`
6. Identificar el nuevo atributo en el listado de atributos de B2C y validar el tipo del mismo en la columna `Attribute type`

## Uso de un atributo custom en un flujo de usuario
1. Desde la página principal del tenant de Azure B2C seleccionar la opción `User flows` bajo la sección `Policies`
2. Click en `New user flow`
3. Elegir la opción `Sign up and sign in` y la versión `Recommended`
4. Proporcionar un nombre identificativo del flujo (SignUpSignInWDepartmentAttribute, por ejemplo)
5. En la sección `Identity providers` seleccionar la opción `Local accounts` a `Email signup`
6. Bajo la sección `User attributes and token claims` hacer click en `Show more`
7. Validar que el listado de atributos contiene el atributo custom creado previamente
8. Seleccionar sobre los atributos `Country/Region`, `departmentName` y `DisplayName` las dos columnas disponibles `Collect attribute` y `Return claim`
9. Pulsar el botón `Create`

## Prueba de flujo de usuario con atributo custom
1. Desde la sección de `User flows`, identificar y seleccionar el flujo creado en el paso previo
2. Pulsar en el botón `Run user flow` situado en el menú superior
3. Seleccionar la aplicación creada en el ejercicio 1 (webapp)
4. Click en `Run user flow`
5. Iniciar el proceso de registro pulsando en `Sign up now`
6. Validar que entre las propiedades que se recogen del usuario, se incluye el nuevo atributo definido `departmentName`
**Actualmente el atributo `departmentName` tiene un valor que no es amigable para el usuario final. Mediante el proceso de customización del branding se verá un ejemplo de gestión del label presentado al usuario para atributos custom**
7. Incluir los valores válidos y finalizar el proceso de registro
8. Validar que en el token se devuelve el claim con formato `extension_departmentName`