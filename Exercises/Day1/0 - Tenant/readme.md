# Creación de un tenant de Azure B2C

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