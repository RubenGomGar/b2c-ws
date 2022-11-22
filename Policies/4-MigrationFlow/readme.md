We need to build the following flow.

1 - Sign-In
2 - Read USer Object
3 - User Exists
3.1 - True - Login User Locally
3.2 - False - Login User Remotelly
            - Write User Locally
4 - Issue Token and Redirect











# Body Creation

{
    "displayName": "PRE MIGration USer",
    "identities": [
        {
            "signInType": "emailAddress",
            "issuer": "Hellofreshgotest.onmicrosoft.com",
            "issuerAssignedId": "migration@finalmigration.com"
        }
    ],
    "passwordProfile": {
        "password": "Password123!!",
        "forceChangePasswordNextSignIn": false
    },
    "passwordPolicies": "DisablePasswordExpiration",
    "$extName": true
}