# "Custom policies" con "Claims" y "Predicates"

En este ejercicio vamos a desarrollar una `Custom Policy` con claims personalizados de nuestra elección y algunos predicados que
nos ayudarán a establecer condiciones y validaciones a estos claims.

_**Nota:** para este ejercicio partiremos de las politicas base del ejercicio anterior. Es importante haber completado los anteriores ejercicios y sus pasos._

## Creación de `Custom Policy` con `Predicates`

En este caso, vamos a utilizar la política base `B2C_1A_TrustFrameworkExtensions` y utilizaremos como plantilla inicial el fichero
`base.xml` de este ejercicio.

A partir de este fichero, vamos a modificarlo para conseguir lo siguiente:
- Creación de una claim de tipo `country` que sea un enumerado.
- Creación de una claim de tipo `fecha`.
- Creación de una claim de tipo `contraseña`.
- Crear predicado que compruebe que la contraseña tenga entre 8 y 12 y que tenga mínimo una minúscula, una mayúscula, un número y un dígito.
- Crear un predicado que compruebe que la fecha sea mayor que hoy hasta final de año.


Para ello copiamos el fichero `base.xml` a otra ubicación o la misma con diferente nombre (p.ej. `exercise_day2_3.xml`).
Abrimos el fichero en un editor y seguimos los siguientes pasos:
1. Localizamos el nodo `<ClaimsSchema>` que debería de encontrarse como hijo del nodo `<BuildingBlocks>`.

2. Añadimos un nodo `<ClaimType>` con un `Id` para el campo de contraseña.
Tenemos que tener en cuenta que su `<DataType>` tendra que ser de tipo `string` y su `<UserInputType>` de tipo `Password`. A su vez es recomendable añadir los campos `DisplayName`, `AdminHelpText` y `UserHelpText`.

3. Añadimos otro nodo `<ClaimType>` con un `Id` para el campo de país (`country`) debajo del anterior. En este caso su `<DataType>` tendra que ser de tipo `string` y su `<UserInputType>` de tipo `DropdownSingleSelect`. Esto último nos obligará a tener un nodo `<Restriction>` con varias enumeraciones ([documentación asociada](https://learn.microsoft.com/en-us/azure/active-directory-b2c/claimsschema#enumeration)).

4. Continuamos añadiendo un último `<ClaimType>` con un `Id` para el campo de fecha. En este caso su `<DataType>` tendra que ser de tipo `date` y su `<UserInputType>` de tipo `DateTimeDropdown`.

5. Procedemos a añadir cada `ClaimType` añadido su referencia a `InputClaims` y `OutputClaims` de los dos `TechnicalProfile` que disponemos. Para ello nos basaremos en la sintaxis y flujo ya establecido.

    _**Nota:** podemos ir subiendo y reemplazando nuestra política para ir iterando sobre la misma y validando._

6. Una vez tengamos establecidos todos los claims y seamos capaces de obtenerlos en nuestro token y visualizarlos en `jwt.ms`, empezarmos definiendo nuestros predicados según las condiciones iniciales. Estos `Predicates` se encontrarán después del nodo `ClaimsSchema`.

7. Tras establecer cada predicado por separado, siempre tendremos que tener un nodo `PredicateValidations` tras `Predicates` que siga la siguiente estructura:

```xml
<PredicateValidations>
    <PredicateValidation Id="{{CustomID}}">
        <PredicateGroups>
            <PredicateGroup Id="{{CustomGroupID}}">
            <PredicateReferences>
                <PredicateReference Id="{{ReferenceID}}" />
            </PredicateReferences>
            </PredicateGroup>
        </PredicateGroups>
    </PredicateValidation>
</PredicateValidations>
```

De este modo crearemos un nodo `PredicateValidation` con al menos una referencia para luego utilizarlos en nuestros `ClaimType` ([documentación de referencia](https://learn.microsoft.com/en-us/azure/active-directory-b2c/predicates)).

8. Tras haber definido toda la estructura de `PredicateValidations` debemos de iterar cada uno de los `ClaimType` y añadir un nodo al final del tipo `PredicateValidationReference` con un `Id` que haga referencia a un `PredicateValidation`. Al hacer esto, estaremos asociando a nuestros claims grupos de predicados y así añadiendo nuestras validaciones y restricciones.

9. Por último, subimos nuestra política e intentamos hacer que las validaciones impidan seguir con el flujo, comprobando que el comportamiento es el correcto.
