# "Custom policies" con "Claims Transformations"

En este ejercicio vamos a desarrollar una `Custom Policy` con claims personalizados de nuestra elección y algunas transformaciones que
nos ayudarán a extender y establecer lógica de negocio.

_**Nota:** para este ejercicio partiremos de las politicas base del ejercicio anterior. Es importante haber completado los anteriores ejercicios y sus pasos._

## Creación de `Custom Policy` con `Claim Transformations`

En este caso, vamos a utilizar la política base `B2C_1A_TrustFrameworkExtensions` y utilizaremos como plantilla inicial el fichero
`base.xml` de este ejercicio.

A partir de este fichero, vamos a modificarlo para conseguir lo siguiente:

- Creación de una claim con Id `employeeId` de tipo `string`.
- Creación de una claim con Id `employeeIdNew` de tipo `string`.
- Creación de una claim con Id `termsOfUse` de tipo `checkbox`.
- Creación de una claim con Id `privacyPolicy` de tipo `checkbox`.
- Creación de una claim con Id `readAccountRules` de tipo `checkbox`. 
- Creación de una claim con Id `isAgeOver21Years` de tipo `checkbox`.
- Creamos una transformación que compare claims (`CompareClaims`) entre `employeeId` y `employeeIdNew`.
- Creamos una transformación que compare claims (`AndClaims`) entre `termsOfUse` y `privacyPolicy`.
-  Creamos una transformación para asertar que un claim es true (`AssertBooleanClaimIsEqualToValue`) `readAccountRules`.
- Creamos una transformación para asertar que el claim `isAgeOver21Years` es true (`CompareBooleanClaimToValue`).
- Creamos una transformación para negar el claim `isAgeOver21Years` (`NotClaims`).


Para ello copiamos el fichero `base.xml` a otra ubicación o la misma con diferente nombre (p.ej. `exercise_day2_3.xml`).
Abrimos el fichero en un editor y seguimos los siguientes pasos:

1. Añadimos todos los `ClaimType` anteriores de la misma forma que los añadimos en el ejercicio anterior ([documentación asociada](https://learn.microsoft.com/en-us/azure/active-directory-b2c/claimsschema)).

2. Estableceremos todos los `ClaimTransformation` necesarios (uno a uno), dentro de un nodo `ClaimTransformations` después del cierre del nodo de `ClaimsSchema`. Para ello, tendremos en cuenta la [documentación asociada](https://learn.microsoft.com/en-us/azure/active-directory-b2c/claimstransformations) buscando para cada caso la operación y su ejemplo de implementación en la documentación.

3. Iremos comprobando cada transformación subiendo nuestra política y validándola. Es importante tener en cuenta los `InputClaim` y `OutputClaim` para cada transformación, ya que los tendremos que declarar correctamente para poderlos visualizar en nuestro token.

4. De esta manera, iremos iterando entre los pasos 2 y 3 para cada transformación comprobando cada una. Es importante ir paso a paso, debido a la complejidad inicial de observar detalles de errores al subir una política y ejecutarla.
