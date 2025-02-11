# Tareas pendientes Workshop B2C

El repo está compuesto de la siguiente forma:

 - B2CApiConnectorRecaptcha: ejemplo de Api Connector que compruba reCAPTCHAv2 en una función de Azure hecha en Node.JS.
 - B2CAuthDemo: Aplicación web de ejemplo en .NET 7 con ASP.NET para autenticarse con Azure AD B2C mediante Microsoft.Identity.
 - B2CDemo: Api de NetCore que usamos en algunos technical profiles
 - Exercises: Contenido de los ejercicios del los tres días. Sería interesante tener un readme.md con el paso a paso que deben seguir e igual publicar en una rama del repo con la nomenclatura features/d{n}-ex{n} (ej. features/d2-ex9) con el ejercicio resuelto (Si aplica tener ejercicio).
 - Policies: Aquí tengo las políticas Base para el día 3 con los casos de uso E2E del día 3. Esta parte estaría casi completa.
 - B2C Workshop.pptx -> Aquí tenemos la PPT de los tres días.
 - assets: diferentes archivos conteniendo diagramas, imágenes y logos para utilizar con Azure AD B2C.


## DAY 1

Aunque la idea principal es hacer este día más interactivo, si sería necesario un mínimo de slides. EL contenido inicial sería el que propuso Victor.

- a.	OverView
- b.	Basic Flows
- c.	Applications
- d.	Identity Providers
- e.	Attributes
- f.	Personalization
- g.	Users
- h.	Api connectors


## DAY 2

- a.	Identity Framework Experience
- b.	Diferencias básico y custom
- c.	Building Blocks
    - i.	Claims Schemas
    - ii.	Claims Transformations
    - iii.	Predicates
    - iv.	Content Definitions
    - v.	Localization
    - vi.	Display Controls
- d.	Claims Providers
- i.	Technical Profiles
    - 1.	AAD Technical Profiles
    - 2.	Claims Transformations
    - 3.	Self – Asserted
    - 4.	SSO Session
- e.	User Journeys
- f.	Sub Journeys
- g.	Relying Party
- h.	Claim Resolvers


## DAY 3

Este día está mas enfocado a hacer la practica de escenarios complejos. Los ejemplos los tengo ya listos y funcionando. Solo me quedan los readme.md con el stepByStep.

- 1 - CustomSignIn
- 2 - CustomSignUp
- 3 - CustomizeUserInput
- 4 - SignInMemberUser
- 5 - FirstLogin
- 6 - MigrationFlow
- 7 - MultiBrandFlow
- 8 - TermsOfService


# Documentación

Para visualizar la documentación de forma sencilla en una web, se hace uso de [MkDocs](https://www.mkdocs.org/) para levantar una web en local y visualizar de manera sencilla y rápida la documentación (que podemos exponer para los asistentes, con Ngrok por ejemplo).

Para levantar la web hace falta tener instalado Python 3.x e instalar MkDocs:
```shell
pip install mkdocs
```

Los ficheros relacionados con este proyecto son los siguientes:
- mkdocs.yml
- Exercies/index.md
- Todos los ficheros `*.md` dentro de la carpeta `Exercises`

Para levantar el servidor web con la documentación ejecutar:
```shell
mkdocs serve
```
Esto expone en localhost:8000 la página principal.

Para construir el sitio web para ser publicado como sitio estático ejecutar:
```shell
mkdocs build
```
