## Logical Steps
For a user to be able to sign up and sign in, the following user experience must be translated into logical steps with a custom policy.

### Handling Sign Up:

1 - Display a page that allows users to enter their email, password, and name.
2 - Verify their email with a Timed One Time Passcode sent to their email address.
3 - When the user completes a sign up, we must create their account.
4 - Prevent a user to sign up with an existing email address.
5 - Issue an id token.

### Handling Sign In:

1 - Display a page where the user can enter their email and password.
2 - On the sign in page, display a link to sign up.
3 - If the user submits their credentials (signs in), we must validate the credentials.
4 - Issue an id token.



## Translating this into custom policies


### Handling Sign Up

1- This requires a Self-Asserted technical profile. It must present output claims to obtain the email, password, and name claims.
2 - Make use of a special claim, which enforces email verification.
3 - Use a Validation technical profile to write the account to the directory. This Validation technical profile will be of type Azure Active Directory.
4 - As part of writing the account configures the technical profile to throw an error if the account exists.
5 - Read any additional information from the directory user object.
6 - Call a technical profile to issue a token.

### Handling Sign In:

1 - This requires a Self-Asserted technical profile. It must present output claims to obtain the email and password claims.
2 - Use the combined sign in and sign up content definition, which provides this for us.
3 - Run a Validation technical profile to validate the credentials.
4 - Read any additional information from the directory user object.
5 - Call a technical profile to issue a token.