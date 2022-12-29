# Creación de un tenant de Azure B2C con una aplicación, claves y  "Custom policies" de inicio
En este ejercicio, crearemos nuestro tenant de Azure B2C con una aplicación inicial (preferiblemente de tipo Web) y desplegaremos un conjunto
de "Custom policies" base para poder ir trabajando sobre las mismas.

_**Nota:** si ya disponemos de un tenant (p.ej. el creado el día 1) podemos pasar directamente al apartado de creación de aplicación._

_**Nota:** podemos automatizar todo el proceso de este ejercicio siguiendo las instrucciones en [Deploy AAD B2C Custom Policies](https://b2ciefsetupapp.azurewebsites.net/), seleccionado `Remove Facebook references` y `Enable JavaScript`._

## Creación de un tenant de Azure B2C

Requisitos previos:
- Suscripción de Azure con al menos el permiso de `Contributor` sobre la suscripcion o en su defecto en un grupo de recursos de la misma
- Importante validar que el usuario tenga la creación de tenants activada. En el caso de que no sea así, será necesario que el usuario tenga asignado el rol de `Tenant Creator`

## Asignar el proveedor de recursos `Microsoft.ActiveDirectory` a la suscripción actual

1. Desde el portal de Azure, acceder al menu de directorios y suscripciones

2. Verificar que el directorio actual corresponda al directorio donde se tenga asignada la suscripción

3. Desde la página principal del portal, buscar `Subscriptions` y seleccionar la suscripción actual

4. Acceder a `Resource Providers` en el menú lateral

5. Validar que la fila correspondiente a `Microsoft.ActiveDirectory` se marca como `Registered`. En el caso de no ser asi, seleccionar la fila y `Register`.

## Crear el tenant de Azure B2C

1. Desde la página principal del portal, buscar `Azure Active Directory B2C` y seleccionar `Create`

2. En el menú mostrado, elegir la opción `Create new Azure AD B2C Tenant`

3. Desde la página de creación de nuevo directorio, rellenar la información necesaria:
    - Organization name: nombre del tenant de B2C
    - Initial domain name: nombre de dominio para el tenant de B2c
    - Country or region: **Este valor no podrá ser cambiado posteriormente**
    - Subscription
    - Resource group: grupo de recursos asociado al tenant de B2C
4. Cuando toda la información esté correctamente rellena, hacer click en `Review + create`

5. Validar que la información es correcta y seleccionar `Create`

  **IMPORTANTE: Al crear un nuevo tenant de Azure B2C se crea por defecto una aplicación llamada `b2c-extensions-app`. No se debe modificar o editar esta aplicacion bajo ningún concepto ya que es usada por Azure B2C para almacenar información de los usuarios y atributos custom**

## Creación de una aplicación en el tenant de Azure B2C

1. Desde la página inicial que se muestra al acceder al recurso principal de Azure B2C, hacemos click en `App registrations` (en la sección `Manage` en la barra lateral izquierda).

2. Una vez dentro, veremos todas las aplicaciones de las que somos "dueños", teniendo también la posibilidad de ver todas las aplicaciones registradas y las borradas. Desde este panel, hacemos click en `New registration`.

3. Veremos un formulario de registro de aplicación, en el que tendremos que especificar un nombre único (p.ej. "B2CWorkshop_Day2_1") y las siguientes opciones:
    - Supported account types: opción 3 - `Accounts in any identity provider or organizational directory (for authenticating users with user flows)`.
    - Redirect URI: establecemos el primer combobox a `Web` y establecemos la URL de redirección a `https://jwt.ms`.
    - Permissions: marcamos el check que menciona `Grant admin consent to openid and offline_access permissions`.

4. Tras crear la aplicación accedemos a ella y en el apartado `Authentication` bajo `Manage` en el panel lateral izquierdo, habilitamos los dos checks correspondientes al apartado `Implicit grant and hybrid flows` y guardamos los cambios.

5. Tras guardar los cambios volvemos a la página principal de nuestro tentant de Azure B2C.

## Identity Experience Framework: Policy Keys

1. En el panel principal de nuestro tenant de Azure B2C, hacemos click en `Identity Experience Framework` bajo `Policies` en la barra lateral izquierda.

2. Una vez dentro, hacemos click en `Policy Keys` y seleccionamos `Add`.

3. En opciones seleccionamos `Generate`.

4. Rellenamos los campos correspondientes del formulario y creamos la clave:
   - Name: establecemos el nombre a `TokenSigningKeyContainer` (debido a las políticas que desplegaremos después). Se añadirá el prefijo `B2C_1A_` automáticamente.
   - Key Type: seleccionamos `RSA`.
   - Key usage: seleccionamos `Signature`.

5. Volvemos a repetir los pasos 2 al 4 pero esta vez con los siguientes valores para la clave:
   - Name: establecemos el nombre a `TokenEncryptionKeyContainer` (debido a las políticas que desplegaremos después). Se añadirá el prefijo `B2C_1A_` automáticamente.
   - Key Type: seleccionamos `RSA`.
   - Key usage: seleccionamos `Encryption`.

## Identity Experience Framework: aplicaciones

Después de la creación de claves, deberemos de registrar dos aplicaciones que Azure B2C requiere para registrar y hacer login a usuarios con cuentas locales:
 - `IdentityExperienceFramework`: una web API.
 - `ProxyIdentityExperienceFramework`: una app nativa con un permiso delegado a la primera apicación.

Con esto, los usuarios pueden registrarse con su cuenta de correo o usuario y una contraseña para acceder a las diferentes aplicaciones registradas en nuestro tenant (creando una cuenta local).

_**Nota:** las cuentas locales **solo existen en nuestro tenant de B2C.**_

### Creación de la primera aplicación: `IdentityExperienceFramework`

1. Desde la página inicial que se muestra al acceder al recurso principal de Azure B2C, hacemos click en `Identity Experience Framework` (en la sección `Policies` en la barra lateral izquierda).

2. Una vez dentro, seleccionamos `App registrations` y veremos todas las aplicaciones de las que somos "dueños", teniendo también la posibilidad de ver todas las aplicaciones registradas y las borradas. Desde este panel, hacemos click en `New registration`.

3. Veremos un formulario de registro de aplicación, en el que tendremos que especificar un nombre único (en este caso "IdentityExperienceFramework") y las siguientes opciones:
    - Supported account types: opción 1 - `Accounts in this organizational directory only ({{TENANT_NAME}} only - Single tenant)` (donde {{TENANT_NAME}} es el nombre de nuestro tenant).
    - Redirect URI: establecemos el primer combobox a `Web` y establecemos la URL de redirección a `https://your-tenant-name.b2clogin.com/your-tenant-name.onmicrosoft.com` (donde "your-tenant-name" es nuestro nombre de tenant, p.ej. "b2cworkshoppc").
    - Permissions: marcamos el check que menciona `Grant admin consent to openid and offline_access permissions`.

4. Tras crear la aplicación accedemos a ella y copiamos el campo `Application (client) ID` que usaremos después.

5. Posteriormente, exprondremos una API añadiendo un scope. Para ello, seleccionamos `Expose an API` bajo el apartado de `Manage` en la barra lateral izquierda.

6. Hacemos click en `Add a scope` y después `Save and continue` aceptando la URI por defecto.

7. Cuando se abra el formulario para añadir el scope lo rellenamos con los siguientes datos y guardamos:
    - Scope name: `user_impersonation`
    - Admin constent display name: `Access IdentityExperienceFramework`
    - Admin consent description: `Allow the application to access IdentityExperienceFramework on behalf of the signed-in user.`

8. Volvemos a la página principal de nuestro tentant de Azure B2C.

### Creación de la segunda aplicación: `ProxyIdentityExperienceFramework`

1. Desde la página inicial que se muestra al acceder al recurso principal de Azure B2C, hacemos click en `Identity Experience Framework` (en la sección `Policies` en la barra lateral izquierda).

2. Una vez dentro, seleccionamos `App registrations` y veremos todas las aplicaciones de las que somos "dueños", teniendo también la posibilidad de ver todas las aplicaciones registradas y las borradas. Desde este panel, hacemos click en `New registration`.

3. Veremos un formulario de registro de aplicación, en el que tendremos que especificar un nombre único (en este caso "ProxyIdentityExperienceFramework") y las siguientes opciones:
    - Supported account types: opción 1 - `Accounts in this organizational directory only ({{TENANT_NAME}} only - Single tenant)` (donde {{TENANT_NAME}} es el nombre de nuestro tenant).
    - Redirect URI: establecemos el primer combobox a `Public client/native (mobile & desktop)` y establecemos la URL de redirección a `myapp://auth`.
    - Permissions: marcamos el check que menciona `Grant admin consent to openid and offline_access permissions`.

4. Tras crear la aplicación accedemos, copiamos el campo `Application (client) ID` que usaremos después.

5. Posteriormente en el apartado `Authentication` bajo `Manage` en el panel lateral izquierdo, habilitamos los flujos públicos de cliente bajo `Advanced settings` y guardamos los cambios.

6. Volviendo a la página principal de la aplicación que acabamos de crear, navegamos a `API permissions` dentro de la sección `Manage` en la barra lateral izquierda.

7. Una vez dentro, seleccionamos `Add a permission` bajo `Configured permissions`.

8. Seleccionamos la pestaña `My APIs`, seleccionamos la aplicación `IdentityExperienceFramework` y bajo permisos seleccionamos `user_impersonation` que hemos creado antes.

9. Hacemos click en `Add permissions` y posteriormente en la lista de `Configured permissions` hacemos click en `Grant admint consent for {{TENANT_NAME}}` y aceptamos.

10. Volvemos a la página principal de nuestro tentant de Azure B2C.

## Identity Experience Framework: custom policy starter pack

Las custom policies son un conjunto de ficheros XML que subimos a nuestro tenant de Azure B2C y que definen `technical profiles` y `user journeys`.

Para este caso, vamos a hacer uso de un `starter pack` para empezar con las políticas. Este `starter pack` contiene lo mínimo para que podamos alcanzar ciertos escenarios.

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

4. **(Opcional)** si estamos compartiendo un tenant de Azure B2C o queremos que nuestras políticas tengan un nombre único y que no colisionen con otras:
    - Buscamos a en todos los ficheros `.xml` la palabra clave `PolicyId` y `PublicPolicyUri` y añadimos un pequeño sufijo con nuestras iniciales (p.ej. `_RPA`).

5. Volvemos a nuestro recurso/tenant de Azure B2C en el portal de Azure y seleccionamos `Identity Experience Framework` desde el menú.

6. Seleccionamos `Upload custom policy` y vamos subiendo todos los fichero `.xml` **en el siguiente orden**:
    - `TrustFrameworkBase.xml`
    - `TrustFrameworkLocalization.xml`
    - `TrustFrameworkExtensions.xml`
    - `SignUpOrSignin.xml`
    - `ProfileEdit.xml`
    - `PasswordReset.xml`

    _**Nota:** el orden de subida de los ficheros es muy importante ya que se hacen referencia unos a otros._

    _**Nota:** al subir cada fichero, Azure les establecera un prefijo del tipo `B2C_1A_` a cada uno._


7. Ya estamos preparados para probar la política custom, para ello dento de `Identity Experience Framework` en el recurso de Azure B2C, debajo de `Custom policies` seleccionamos la que comienza por `B2C_1A_signup_signin`.

8. Se abrira un pequeño panel en el que seleccionaremos la aplicación que hemos creado en primera instancia al principio del ejercicio y nos aseguramos que la URL de respuesta (`Reply URL`) está establecida a `https://jwt.ms`.

9. Hacemos click en `Run now` y seguimos el flujo de registro de usuario. Deberíamos de acabar en `https://jwt.ms` viendo nuestro token.

10. Volvemos a repetir el paso 9 pero usando la cuenta que ya hemos creado (haciendo login directo).
