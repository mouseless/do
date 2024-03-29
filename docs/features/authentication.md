# Authentication

This feature provides a mechanism which authenticates incoming http requests.

Add this feature using `AddAuthentication()` extension;

```csharp
app.Features.AddAuthentication(...);
```

## FixedToken

This feature adds a middleware which authenticates requests using the token 
provided `Authorization` header. Middleware tests the token against the value 
configured in settings. Multiple tokens can be used by providing key names when
adding the feature and `Authentication:FixedToken:Default` value from settings 
will be used if no token key specified

```csharp
c => c.FixedToken(tokenNames: ["ServiceA", "ServiceB"])
```

```json
"Authentication": {
  "FixedToken": {
    "ServiceA": "SERVICE_A_TOKEN",
    "ServiceB": "SERVICE_B_TOKEN"
  }
}
```

The feature also provides a form post authentication mechanism. The middleware
looks for a form parameter named `hash` than validates the request using form 
parameters and token. The expected hash should match the value which is 
generated by combining form parameters with token value, computing a hash using
`SHA256` and converting to `Base64` string. 

> :information_source:
>
> Form post authentication will only work if there is no authorization header
> exists in the request.
