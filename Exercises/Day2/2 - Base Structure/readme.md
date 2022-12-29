# Creación de un recurso de Application Insights y "Custom policies" que registren en este
En este ejercicio, se realizará un ejemplo muy similar al del ejercicio 1 pero con la variante de registrar todos los eventos en Application Insights.

## Creación de un recurso de Application Insights

_**Nota:** si ya disponemos de un recurso de Application Insights creado, podemos utilizarlo para este caso._

_**Nota:** normalmente deberemos de cambiar de tenant al que tengamos una subscripción activa (no en el de Azure B2C) para desplegar este recurso._

1. En primer lugar accedemos al portal de Azure y cambiamos de directorio a cualquiera con una subscripción activa.

2. Buscamos en la barra superior por `Application Insights` y hacemos click en el primer servicio que se corresponda.

3. Hacemos click en create y rellenamos el formulario de la siguiente forma:
    - Subscription: la subscripción activa que consideremos conveniente.
    - Resource group: aquí podemos escoger entre un grupo de recursos existente o uno nuevo (si lo vemos conveniente).
    - Name: un nombre único para nuestro servicio de `Application Insights` (puede ser cualquiera).
    - Region: cualquier región es válida.
    - Resource mode: elegiremos `Workspace-based` ya que el modo clásico será retirado en 2024 (debemos de ir acostumbrándonos a desplegar en este modo).
    - Workspace details:"
        - Subscription: la subscripción activa que consideremos conveniente.
        - Log Analytics Workspace: podemos elegir crear un nuevo workspace si no disponemos de uno o reutilizar uno existente.

4. Seleccionamos `Review + create`.

5. Esperamos a la validación final y creamos el recurso finalmente haciendo click en `Create`.

6. Esperamos a que el despliegue termine, accedemos al nuevo recurso y copiamos la `Instrumentation Key` y `Connection String` para utilizarlos más adelante.

## Despliegue de `Custom policy` con Application Insights

_**Nota:** se considera que tenemos disponibles las aplicaciones y `Policy keys` que hemos creado en el ejercicio 1. De lo contrario, seguir los pasos desde `Creación de una aplicación en el tenant de Azure B2C` hasta `Identity Experience Framework: custom policy starter pack` (no inclusive)._

En este paso procederemos desde el `starter pack` de Azure B2C a desplegar una `Custom policy` que tenga habilitada la integración con Application Insights, para ello seguiremos pasos similares a los realizados en el primer ejercicio con algunas variaciones.

Para comenzar descargamos el repositorio de Git del `starter pack`:

```sh
git clone https://github.com/Azure-Samples/active-directory-b2c-custom-policy-starterpack
```

En este ejercicio haremos uso de la carpeta `LocalAccounts` que habilita el uso únicamente de cuentas locales.

Esta carpeta incluye los archivos:
- `TrustFrameworkBase.xml`: fichero base para las políticas, no necesitaremos apenas hacer cambios aquí.
- `TrustFrameworkLocalization.xml`: fichero donde haremos cambios relacionados con localización/idiomas.
- `TrustFrameworkExtensions.xml`: este será el fichero donde haremos la mayoría de los cambios.
- `SignUpOrSignin.xml`, `ProfileEdit.xml`, `PasswordReset.xml`: son ficheros llamados por la aplicación para realizar ciertas acciones (login, registro, edición de perfil y reseteo de contraseña).

Para el uso de las políticas del starter pack, debemos de hacer los siguientes cambios (dentro de la carpeta `LocalAccounts`):"

1. Cambiamos todas las referencias a `yourtenant` de todos los ficheros `.xml` dentro de la carpeta a el nombre de nuestro tenant (p.ej. `b2cworkshoppc`).

2. En el fichero `TrustFrameworkExtensions.xml` buscamos el elemento `<TechnicalProfile Id="login-NonInteractive">` y reemplazamos todas las instancias de `IdentityExperienceFrameworkAppId` por el `client ID` que hemos copiado antes referente a esta aplicación y las instancias de `ProxyIdentityExperienceFrameworkAppId` por el `client ID` que hemos copiado antes referente a esta aplicación.

3. Guardamos los cambios.

4. Abrimos el fichero `SignUpOrSignin.xml` y en la etiqueta `<TrustFrameworkPolicy>` establecemos los siguientes valores:
    ```xml
    DeploymentMode="Development"
    UserJourneyRecorderEndpoint="urn:journeyrecorder:applicationinsights"
    ```

    _**Nota:** en el caso de quue no exista debemos añadir un nodo hijo `<UserJourneyBehaviors>` al nodo `<RelyingParty>` (debería de encontrarse despúes del nodo `<DefaultUserJourney>` o después de `<Endpoints>` si existe la clave)._


5. Añadimos el siguiente nodo como hijo del nodo `<UserJourneyBehaviors>` (reemplazando {{APP_INSIGHTS_KEY}} con la clave de instrumentación de `Application Insights`):

    ```xml
    <JourneyInsights TelemetryEngine="ApplicationInsights" InstrumentationKey="{{APP_INSIGHTS_KEY}}" DeveloperMode="true" ClientEnabled="true" ServerEnabled="true" TelemetryVersion="1.0.0" />
    <ScriptExecution>Allow</ScriptExecution>
    ```

    _**Nota:** el orden de los nodos es muy importante, sino la validación de las políticas falla._

    De los anteriores nodos y atributos:
    - `<ScriptExecution>Allow</ScriptExecution>`: permite la ejecución de JavaScript en los flujos, lo cual es requerido para habilitar `Application Insights` del lado del cliente.
    - `DeveloperMode="true"`: establece enviar telemetría a traves del flujo principal de procesamiento. **En entornos productivos esto se debe deshabilitar debido a los posibles grandes volúmenes de peticiones.**
    - `ClientEnabled="true"`: registra el script del lado de cliente de `Application Insights` para "tracking" del cliente y errores por parte del usuario (además de tiempos de llamadas, contadores, detalles de excepciones, etc.).
    - `ServerEnabled="true"`: envía el JSON correspondiente a `UserJourneyRecorder` como un `Custom event` a `Application Insights`.

6. **(Opcional)** si estamos compartiendo un tenant de Azure B2C o queremos que nuestras políticas tengan un nombre único y que no colisionen con otras:
    - Buscamos a en todos los ficheros `.xml` la palabra clave `PolicyId` y `PublicPolicyUri` y añadimos un pequeño sufijo con nuestras iniciales o un identificador (p.ej. `_RPA_AI`).

7. Volvemos a nuestro recurso/tenant de Azure B2C en el portal de Azure y seleccionamos `Identity Experience Framework` desde el menú.

8. Seleccionamos `Upload custom policy` y vamos subiendo todos los fichero `.xml` **en el siguiente orden**:
    - `TrustFrameworkBase.xml`
    - `TrustFrameworkLocalization.xml`
    - `TrustFrameworkExtensions.xml`
    - `SignUpOrSignin.xml`
    - `ProfileEdit.xml`
    - `PasswordReset.xml`

    _**Nota:** el orden de subida de los ficheros es muy importante ya que se hacen referencia unos a otros._

    _**Nota:** al subir cada fichero, Azure les establecera un prefijo del tipo `B2C_1A_` a cada uno._


9. Ya estamos preparados para probar la política custom, para ello dento de `Identity Experience Framework` en el recurso de Azure B2C, debajo de `Custom policies` seleccionamos la que comienza por `B2C_1A_signup_signin`.

10. Se abrira un pequeño panel en el que seleccionaremos la aplicación que hemos creado en primera instancia al principio del ejercicio y nos aseguramos que la URL de respuesta (`Reply URL`) está establecida a `https://jwt.ms`.

11. Hacemos click en `Run now` y seguimos el flujo de registro de usuario. Deberíamos de acabar en `https://jwt.ms` viendo nuestro token.

12. Volvemos a repetir el paso 9 pero usando la cuenta que ya hemos creado (haciendo login directo).

13. Accedemos al recurso de `Application Insights` (cambiando de directorio) y comprobamos que se han volcado todas las trazas correspondientes a estos pasos realizados.

_**Nota:** también resulta de gran utilidad utilizar la extensión de [Azure AD B2C para VSCode](https://marketplace.visualstudio.com/items?itemName=AzureADB2CTools.aadb2c), en la cual podemos visualizar todo de forma sencilla y rápida (aquí [pasos para configurarla](https://learn.microsoft.com/en-us/azure/active-directory-b2c/troubleshoot-with-application-insights?pivots=b2c-custom-policy#see-the-logs-in-vs-code-extension))._

