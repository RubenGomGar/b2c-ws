# Modos de login contra Azure B2C
Por defecto el tenant generado de Azure B2C tiene habilitado el login mediante correo electrónico, pero B2C cuenta con la posibilidad de extender o limitar los login a combinaciones de:
- Email: opción por defecto. El valor introducido es el identificador único del usuario
- Username: se sigue manteniendo la necesidad de registrar un email y validarlo, pero el identificador es generado por el propio usuario
- Phone: no se requiere de un email ni su validación. En su lugar el teléfono introducido es el identificador único del usuario, tras la validación del mismo

## Configuración de login por nombre de usuario y teléfono
1. Desde la página principal del tenant de Azure B2C seleccionar `Identity Providers`
2. Buscar y seleccionar el proveedor `Local Account`, es interesante ver que en la columna `Configuration` aparece el valor `Email` indicando el tipo de login habilitado actualmente
3. Habilitar todos los tipos de login permitidos, `Email`, `Username` y `Phone`
4. Guardar los cambios

## Creación de un flujo de login y registro con teléfono
1. Desde la página principal del tenant de Azure B2C seleccionar la opción `User flows` bajo la sección `Policies`
2. Click en `New user flow`
3. Elegir la opción `Sign up and sign in` y la versión `Recommended`
4. Proporcionar un nombre identificativo del flujo (SignUpSignInWPhone, por ejemplo)
5. En la sección `Identity providers` donde en ejercicios previos únicamente aparecía la opción `Email signup` ahora aparecen nuevas opciones. En este ejemplo se usará la opción `Phone signup`
6. Deshabilitar la opción de forzar MFA en la sección `Multifactor authentication`
7. En la sección `User attributes and tokens` pulsar la opción `Show more` para mostrar la lista completa de atributos del usuario y marcar las opciones (en ambas columnas siempre que sea posible):
- Country/Region
- Given Name
8. Seleccionar la opción `Create` al final del formulario

## Prueba de registro y login con teléfono
1. Desde la sección de `User flows`, identificar y seleccionar el flujo creado en el paso previo
2. Verificar en la sección `Identity Providers` que la opción seleccionada para cuentas locales es `Phone signup`
3. Pulsar en el botón `Run user flow` situado en el menú superior
4. Seleccionar la aplicación creada en el ejercicio 1 (webapp)
5. Click en `Run user flow`
6. Realizar un nuevo registro introduciendo y validando un teléfono móvil válido
7. Introducir un email de recuperación válido (sólo será utilizado para gestiones de recuperación de cuenta, no identificará al usuario ni se requiere que sea único)
8. Validar que los claims devueltos son correctos
9. Lanzar nuevamente el flujo de usuario y validar que el comportamiento del flujo de login es el esperado

