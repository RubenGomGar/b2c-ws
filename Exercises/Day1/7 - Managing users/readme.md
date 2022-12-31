# Gestión de usuarios en Azure B2C
Desde Azure B2C los administradores tienen un control total de los usuarios registrados en las aplicaciones del tenant. Además, es posible crear nuevos usuarios de manera manual.

1. Desde la página principal del tenant de Azure B2C seleccionar la opción `Users` bajo la sección `Manage`
2. Validar que los usuarios creados en los ejemplos previos se muestran en el tenant con el valor de la columna `User type ` del tipo `Member`

## Cambio de contraseña de un usuario
1. Desde el listado de usuarios de la sección `Users`, seleccionar mediante el check de la primera columna cualquier usuario creado en un flujo previo
2. Click en la opción `Reset password` del menú superior
3. En la ventana lateral abierta, click en el botón `Reset password`
4. Al usuario seleccionado se le asigna una contraseña aleatoria. La siguiente vez que inicie sesión con la contraseña proporcionada se le requerirá un reseteo de contraseña para futuros logins

### Revocación de sesión de usuarios
Una práctica habitual tras forzar el cambio de una contraseña de un usuario (por ejemplo si se ha encontrado alguna vulnerabilidad en alguna aplicación y el usuario ha sido afectado), es forzar a que inicide de nuevo todas las sesiones que este usuario tuviese activas.
1. Desde el listado de usuarios de la sección `Users`, seleccionar mediante un usuario creado en los flujos previos
2. En la página de detalle, bajo `User Sign-ins` es posible ver información sobre la actividad de login del usuario. Si se hace click 
sobre `User Sign-ins` se obtiene información mas detallada con posibilidad de filtrarla
3. Hacer click en el botón `Revoke sessions`
4. Hacer click en `Yes` para confirmar la acción

## Borrado de un usuario registrado
1. Desde el listado de usuarios de la sección `Users`, seleccionar mediante el check de la primera columna cualquier usuario creado en un flujo previo
2. Click en la opción `Delete user` del menú superior

## Creación de usuarios

### Creación de usuario en la organización
Azure B2C permite generar usuarios asociados al tenant (se verá reflejado en el dominio del correo usado)
1. Desde el listado de usuarios de la sección `Users`, hacer click en el botón `New user` en la barra superior
2. En la sección `Select template` elegir la opción `Create user`
3. Introducir el nombre de usuario para generar un email válido con dominio del tenant
4. Introducir el resto de campos
    - Name
    - First Name
    - Last Name
5. De cara a generar una contraseña para el nuevo usuario, B2C genera por defecto un valor aleatorio pero también permite introducir la contraseña de manera manual. Elegir la opción de generarla manualmente.
6. Click en el botón `Create` para confirmar el alta del usuario

### Invitar a un usuario externo al tenant
En este caso, se generará una invitación a un usuario con email externo al tenant. Se mantendrá el email original y el propio usuario generará la contraseña en el proceso de login.
Es importante conocer que aunque se mantiene el email original indicado en la invitación, el usuario generado tiene un `UPN` generado por el tenant. Si el correo es `test@dominio.com` y el tenant es `tenanttest.onmicrosoft.com` el valor del `UPN` será `test_dominio.com#EXT#@tenanttest.onmicrosoft.com`
1. Desde el listado de usuarios de la sección `Users`, hacer click en el botón `New user` en la barra superior
2. En la sección `Select template` elegir la opción `Invite user`
3. Introducir el nombre de usuario y un email válido para generar la invitación
4. Click en el botón `Create` para confirmar el alta del usuario
5. Validar que se recibe el email con la invitación en el buzón correspondiente y completar el registro
6. En el listado de usuarios del tenant, validar que el usuario invitado está marcado como `Guest` y en la columna `Source` tiene el valor `Invite` si no se ha aceptado la petición

## Crear un usuario que se autentique contra el tenant de B2C
La principal diferencia entre crear el usuario en la organización y crearlo en el tenant de B2C es que se permite que el login se haga por medio del username en lugar de por email (o combinación de ambas)
1. Desde el listado de usuarios de la sección `Users`, hacer click en el botón `New user` en la barra superior
2. En la sección `Select template` elegir la opción `Create Azure AD B2C user`
3. En la sección `Identity` bajo la columna `Sign in method`, elegir la opción `User Name` e introducir un valor válido
4. Proporcionar un valor para el `Name` del usuario
5. Generar la contraseña manualmente
6. Click en el botón `Create` para confirmar el alta del usuario
7. Validar que el usuario creado tiene el valor `Member`