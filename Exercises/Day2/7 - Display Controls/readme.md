# "Custom policies" con "Display Controls" para verificar un determinado claim

En este ejercicio vamos a desarrollar una `Custom Policy` con "Display Controls" para verificar el email de un usuario antes de que este pueda realizar un cambio de contraseña.

_**Nota:** para este ejercicio partiremos de las políticas base del ejercicio anterior. Es importante haber completado los anteriores ejercicios y sus pasos._

## Creación de `Custom Policy` con "Display Controls"

En este caso, vamos a utilizar la política base `B2C_1A_TrustFrameworkExtensions` y utilizaremos como plantilla inicial el fichero `base.xml` de este ejercicio.

Este fichero `base.xml` ya incluye la estructura necesaria, junto a los `ClaimsProviders` necesarios para construir la verificación de un email para reseteo de contraseña enviando un simple código de un solo uso vía email.

A partir de este fichero, vamos a modificarlo para conseguir lo siguiente:
- Definir los claims necesarios para que el flujo funcione.
- Definir los `ContentDefinitions` necesarios para customizar el flujo por defecto de reseteo de contraseña para cuentas locales.
- Definir los `DisplayControls` necesarios para conseguir el flujo de reseteo de contraseña, dejando al usuario introducir un email, solicitar un código de un solo uso en el email y verificarlo para proceder a cambiar la contraseña.


Para ello copiamos el fichero `base.xml` a otra ubicación o la misma con diferente nombre (p.ej. `exercise_day2_7.xml`).
Abrimos el fichero en un editor y seguimos los siguientes pasos:

1. Definimos nuestro bloque `ClaimsSchema` el cual contendrá un solo `ClaimType` con id `verificationCode` en el cual el usuario introducirá su código de verificación de un solo uso.

2. En el bloque `ContentDefinitions` definimos un solo `ContentDefinition` haciendo referencia al flujo de reseteo de contraseña para usuarios locales.

3. Basándonos en la [documentación asociada](https://learn.microsoft.com/en-us/azure/active-directory-b2c/display-controls#display-control-actions), procedemos a declarar nuestros `DisplayControls` para la verificación de email.

_**Nota:** necesitaremos definir un solo `DisplayControl` para verificar el mail mandando un código de un solo uso. Los `ClaimProviders` necesarios ya han sido definidos, junto a los `TechnicalProfile` necesarios._

4. Iremos comprobando cada transformación subiendo nuestra política y validándola.

5. De esta manera, iremos iterando para cada localización comprobando cada una. Es importante ir paso a paso, debido a la complejidad inicial de observar detalles de errores al subir una política y ejecutarla.
