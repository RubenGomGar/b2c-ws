# Flujo Base SignIn

Vamos a empezar con un flujo sencillo. Lo que queremos conseguir es un flujo de solo Login. 

La idea es tomar como referencia el flujo prefedinido de `SignUpOrSignIn` que encontramos en nuestros ficheros base y quitar todo lo que no tenga relación con el Login. Esto nos va a valer para confirmar nuestra habilidad de navegación entre referencias en nuestros ficheros y afirmar lo aprendido en días anteriores.

Deberemos rellenar nuestro user Journey con los siguientes steps:

1. SingIn Page. Solo necesitamos mostrar la página de login.
2. Azure AAD Read Using ObjectId. Tenemos que validar si en el AAD tenemos un usuario con esas credenciales.
3. Send Claims. Enviamos la bolsa de claims.



<details>
   <summary>STEP 1 SPOILER</summary>
   <div class="description">

```xml
    <OrchestrationStep Order="1" Type="ClaimsExchange">
        <ClaimsExchanges>
            <ClaimsExchange Id="LocalAccountSigninEmailExchange" TechnicalProfileReferenceId="SelfAsserted-LocalAccountSignin-Email" />
        </ClaimsExchanges>
    </OrchestrationStep>
```

   </div>
</details>

<details>
   <summary>STEP 2 SPOILER</summary>
   <div class="description">

```xml
    <OrchestrationStep Order="2" Type="ClaimsExchange">
        <ClaimsExchanges>
            <ClaimsExchange Id="AADUserReadWithObjectId" TechnicalProfileReferenceId="AAD-UserReadUsingObjectId" />
        </ClaimsExchanges>
    </OrchestrationStep>
```
    
   </div>
</details>

<details>
   <summary>STEP 1 SPOILER</summary>
   <div class="description">

```xml
    <OrchestrationStep Order="3" Type="SendClaims" CpimIssuerTechnicalProfileReferenceId="JwtIssuer" />
```
    
   </div>
</details>