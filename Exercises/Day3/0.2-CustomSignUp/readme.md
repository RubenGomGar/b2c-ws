# Flujo Base SignUp

Continuamos con otro flujo sencillo. Esta vez queremos construir un flujo de solo SignUp

La idea es tomar como referencia el flujo prefedinido de `SignUpOrSignIn` que encontramos en nuestros ficheros base y quitar todo lo que no tenga relación con el SignUp. Esto nos va a valer para confirmar nuestra habilidad de navegación entre referencias en nuestros ficheros y afirmar lo aprendido en días anteriores.

Deberemos rellenar nuestro user Journey con los siguientes steps:

1. SingUp Page. Solo necesitamos mostrar la página de registro.
3. Send Claims. Enviamos la bolsa de claims.



<details>
   <summary>STEP 1 SPOILER</summary>
   <div class="description">

```xml
<OrchestrationStep Order="1" Type="ClaimsExchange">
  <ClaimsExchanges>
    <ClaimsExchange Id="SignUpWithLogonEmailExchange" TechnicalProfileReferenceId="LocalAccountSignUpWithLogonEmail" />
  </ClaimsExchanges>
</OrchestrationStep>
```

   </div>
</details>

<details>
   <summary>STEP 2 SPOILER</summary>
   <div class="description">

```xml
    <OrchestrationStep Order="2" Type="SendClaims" CpimIssuerTechnicalProfileReferenceId="JwtIssuer" />
```
    
   </div>
</details>