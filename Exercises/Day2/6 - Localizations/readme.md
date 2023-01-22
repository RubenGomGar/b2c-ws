# "Custom policies" con soporte para múltiples idiomas

En este ejercicio vamos a desarrollar una `Custom Policy` con claims personalizados de nuestra elección y diferentes entradas de "localización" para personalizar la experiencia del usuario en varios idiomas.

_**Nota:** para este ejercicio partiremos de las politicas base del ejercicio anterior. Es importante haber completado los anteriores ejercicios y sus pasos._


## Creación de `Custom Policy` con soporte para múltiples idiomas

En este caso, vamos a utilizar la política base `B2C_1A_TrustFrameworkExtensions` y utilizaremos como plantilla inicial el fichero
`base.xml` de este ejercicio.

Este fichero `base.xml` ya incluye un claim personalizado de tipo `gender` que indica el género del usuario, el cual utilizaremos para informarlo en diferentes idiomas (inglés y español).

A partir de este fichero, vamos a modificarlo para conseguir lo siguiente:
- Proporcionar soporte para inglés y español.
- Proporcionar texto localizado para cada valor del combo de género, además de el propio título del campo.
- Cambiar el texto del botón de continuar (hint: el id es `button_continue`) a algo distinto para ambos idiomas.
- Cambiar el mensaje de error que se produce si el email ya existe (hint: el id es `UserMessageIfClaimsPrincipalAlreadyExists`) a algo distinto para ambos idiomas.
- De esta manera seremos capaces de aprender como localizar varios elementos de la interfaz de usuario.

Para ello copiamos el fichero `base.xml` a otra ubicación o la misma con diferente nombre (p.ej. `exercise_day2_6.xml`).
Abrimos el fichero en un editor y seguimos los siguientes pasos:

1. Basándonos en la [documentación asociada](https://learn.microsoft.com/en-us/azure/active-directory-b2c/localization), procedemos a dar soporte a dos idiomas (inglés y español).

2. Definimos los siguientes `LocalizedResources` dentro de `ContentDefinitions` para el flujo que vamos a modificar (justo después de cerrar `SupportedLanguages`):

```xml
<LocalizedResources Id="api.localaccountsignup.en">
...
</LocalizedResources>
<LocalizedResources Id="api.localaccountsignup.es">
...
</LocalizedResources>
```

3. Siguiendo la documentación, y dentro de cada `LocalizedResources`, definimos una `LocalizedCollection` y varios `LocalizedString` dentro de un bloque `LocalizedStrings` para conseguir el objetivo del ejercicio.

3. Iremos comprobando cada transformación subiendo nuestra política y validándola. Podemos usar una extensión como ["Locale Switcher"](https://chrome.google.com/webstore/detail/locale-switcher/kngfjpghaokedippaapkfihdlmmlafcc) para Chrome o cambiar el idioma por defecto del navegador para comprobar que los textos cambian según el idioma (debemos de refrescar la página).

4. De esta manera, iremos iterando para cada localización comprobando cada una. Es importante ir paso a paso, debido a la complejidad inicial de observar detalles de errores al subir una política y ejecutarla.
