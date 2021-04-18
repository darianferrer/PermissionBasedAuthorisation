Allows the dynamic creation of policies with a permission requirement. An implementation of `IPermissionVerificationService` must be provided, this is the service responsible of fulfilling the permission challenge.

### Get Started
Install this package using `dotnet` CLI.

```
dotnet add package PermissionBasedAuthorisation
```

Add the services to DI container.

```
services.AddPbacCore<CustomPermissionVerificationService>();
```

Use in controllers.

```
[Authorize("my-permission")]
public Task<IActionResult> Get(){}
```