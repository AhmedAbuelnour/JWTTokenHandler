# JWTTokenHandler
Is just a simple library that handles generating JWT access tokens and refresh tokens, in an easy way.
You can clone it and add your modifications according to your case.

# Getting started

> Install from [Nuget](https://www.nuget.org/packages/JWTTokenHandler/)

> Write your data in the appsettings.json or in any secure file you want.

```
"Jwt": {
    "Key": "U2VsYWhlbHRlbG1lZXsdfwerwerwrwer==",
    "Issuer": "Issuer_Domain",
    "AccessTokenExpiration": 60,
    "RefreshTokenExpiration": 60,
    "Audience": "Issuer_Domain"
  }
  
```
> Inject into your Services 
```
using JWTGenerator.EntityModel;
using JWTGenerator.TokenHandler;

builder.Services.AddJWTTokenHandlerExtension(new JWTConfiguration
{
    Audience = builder.Configuration["Jwt:Audience"],
    Issuer = builder.Configuration["Jwt:Issuer"],
    Key = builder.Configuration["Jwt:Key"],
    AccessTokenExpiration = TimeSpan.FromDays(int.Parse(builder.Configuration["Jwt:AccessTokenExpiration"])),
    RefreshTokenExpiration = TimeSpan.FromDays(int.Parse(builder.Configuration["Jwt:RefreshTokenExpiration"]))
    ClearCliamTypeMap = true,
});

```
> Using the handler in your code

```
public readonly TokenHandlerManager _jwtAccessGenerator;
public AuthController(IMediator mediator, TokenHandlerManager jwtAccessGenerator)
{
     _jwtAccessGenerator = jwtAccessGenerator;
}

 [HttpPost("[action]"), AllowAnonymous]
 public async Task<IActionResult> Login([FromBody] LoginUser loginUser, CancellationToken token)
 {
   ...      
   Dictionary<string, string> claims = new Dictionary<string, string>()
   {
       {"Key", "Value"},
       {"Key", "Value"},
       {"Key", "Value"},

   };
   AccessToken accessToken = _jwtAccessGenerator.GetAccessToken(claims);
   RefreshToken refreshToken = _jwtAccessGenerator.GetRefreshToken();
   
   // Save into the your database the refresh token
   _dbContext.RefreshTokens.Add(refreshToken);
   // Return your model, containing the access token and refreshToken
   return Ok(..);
}

 [HttpPost("[action]"), AllowAnonymous]
 public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest, CancellationToken token)
 {
   // Check the existing of the old reference
   RefreshToken oldRefreshToken = await _dbContext.RefreshTokens.SingleOrDefaultAsync(a=> a.Token == refreshRequest.RefreshToken && a.IsActive);
   // if true carry on
   
   AccessToken accessToken = _jwtAccessGenerator.GetRefreshAccessToken(refreshRequest.ExpiredToken);
   RefreshToken refreshToken = _jwtAccessGenerator.GetRefreshToken();
   
   // Save into the your database the new refresh token and revoke the old one
   _dbContext.RefreshTokens.Add(refreshToken);
   oldRefreshToken.RevokedOn = DateTime.UTCNow;
   await _dbContext.SaveChangesAsync();
   
   // Return your model, containing the access token and refreshToken
   return Ok(..);
}



```
