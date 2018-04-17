# AspNetCore.LdapAuthentication

Fork of Justin.AspNetCore.LdapAuthentication.

Provides LDAP password authentication for an existing ASP.NET Core Identity user store via an LDAP bind. I created this for a project I'm working on with a very basic need, so it is not an exchaustive provider by any means.

## License

MIT

## Features

- LDAP password authentication via a custom UserManager against any existing UserManager/UserStore combination.
- Does not (yet) provide an LDAP based UserStore implementation
- Does not support password changes or resets.

## Possible Future Features

- Full implementation of IUserStore and applicable interfaces around LDAP.

## Dependencies

- NETStandard 2.0.0
- Novell.Directory.Ldap.NETStandard 2.3.8
- Microsoft.AspNetCore.Identity 2.0.2

## Getting Started

Setup ASP.NET Identity Core

Install the NuGet Package 

```
Install-Package AspNetCore.LdapAuthentication
```

Create LdapAuth settings in appsettings.json:

```json
  "LdapAuth": {
    "url": "ldap.local",
    "bindDn": "CN=user,OU=branch,DC=contoso,DC=local",
    "bindCredentials": "secret_password",
    "searchBase": "DC=contoso,DC=local",
    "searchFilter": "(&(objectClass=user)(objectClass=person)(sAMAccountName={0}))",
    "adminCn": "CN=Admins,OU=branch,DC=contoso,DC=local"
  },
```

Configure options and the custom User Manager in Startup *before* Identity:

```csharp

// You can use services.AddLdapAuthentication(setupAction => {...}) to configure the 
// options manually instead of loading the configuration from Configuration.
services.Configure<AspNetCore.LdapAuthentication.LdapAuthenticationOptions>(this.Configuration.GetSection("LdapAuth"));

// Add the custom user manager.
services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddUserManager<AspNetCore.LdapAuthentication.LdapUserManager<ApplicationUser>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
```

Use the normal sign-in method, and it will valid the user's passwod via an LDAP bind.

```csharp
...
result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
...
```

## Other Notes

By default, the result of the user store GetNormalizedUserNameAsync() method on the UserStore as the value for the distguished name when performing an LDAP bind. You can customize this by implementing a custom user store and the interface AspNetCore.LdapAuthentication.IUserLdapStore, which provides a GetDistinguishedNameAsync method that will be used instead of the normalized username.

