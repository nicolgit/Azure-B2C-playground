# Setup NicolWebCalculator

This web app is a SPA authenticated via Azure B2C that allows both all operation provided by NicolAPICalculator and NicolAPIScientificCalculator. The user needs to authenticate to AzureB2c and then with the bearer obtained call all the API to execute operations.

# (1) Azure B2C Configuration
Create an application on Azure B2C.

* Name: **WebCalculator**
* WebApp/Include WebAPI: **YES**
* WebApp/allow Implicit Flow: **YES**
* Reply URL: TBD
* App ID URI: WebCalculator
* Include Native client: **NO**

After creating it, select "Api access". Click the "Add" button. In the next blade, add both ApiCalculator and ApiScientificCalculator) this will allow to call with the same authorization bearer both ApiScientificCalculator **and** ApiCalculator.

![Set API Access](assets/img12.png)

# (2) create a ASP Net Core Web Application of type ANGULAR
Via visual studio create a solution of type "ASP.NET Core Web Application", type ANGULARE, with NO Authentication
![create vs project](assets/img13.png)

# (3) Create a policy: B2C\_1\_signin-default
**You don't need to create an additional policy, just use the policy already created** (B2C_1_signin-default).

# (4) Reference Microsoft Authentication Library
use the following command to add a reference Microsoft Authentication Library:

    npm install @types/node
    npm install msal

# (4) Configure appsettings.json and solution for authentication

In appsettings.config add the following information:

	"AzureAdB2C": {
	"Tenant": "nicolb2c.onmicrosoft.com",
	"ClientId": "<the application id (guid)>",
	"Policy": "B2C_1_signin-default"
	},

also add following NuGet packages to the solution

	Microsoft.AspNetCore.Authentication.JwtBearer
	Microsoft.AspNetCore.Mvc
	Microsoft.Extensions.Configuration.UserSecrets
	Microsoft.Extensions.SecretManager.Tools

# (5) Update the code 

Update [program.cs](nicold.playground/nicold.APICalculator/program.cs) and [startup.cs](nicold.playground/nicold.APICalculator/startup.cs) as shown in this repository, then create a controller for the home page [Controllers/HomeController.cs](nicold.playground/nicold.APICalculator/Controllers/HomeController.cs) with the corresponding view [View/Home/index.shtml](nicold.playground/nicold.APICalculator/View/Home/index.shtml).

It is now the time to build the real API: we will implement the API in the controller [Controllers/CalcController.cs](nicold.playground/nicold.APIScientificCalculator/Controllers/CalcController.cs)

The sequence of operation needed to authenticate the API is similar to APICalculatator. The interesting part is the API to API portion of the code.
Here the main attention points:

### Make the Get Asyncronous
 	public async Task<IActionResult> Get(string op, [FromQuery] double param1, [FromQuery] double param2)

### Get the Bearer from the context
	var header_bearer = HttpContext.Request.Headers["Authorization"];
    string bearer = header_bearer.FirstOrDefault().Split(" ")[1];

### Use it to call ApiCalculator
	string CALL_MULTIPLY = "http://nicolapicalculator.azurewebsites.net/api/calc/sum?param1={0}&param2={1}";
	
	_client.DefaultRequestHeaders.Accept.Clear();
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);

    var httpResponse = await _client.GetAsync(string.Format(CALL_MULTIPLY, param1, param2));

# (6) Update to APICalculator and APIScientificCalculator to avoid ValidateAudience check

By default Microsoft.AspNetCore.Authentication.JwtBearer middleware verifies on call if the token and the API audience match. This means that in a cross API call, if you try to use the same bearer, when ScientificAPI Calls Calculator receive the following message:

	AuthenticationFailed: IDX10214: Audience validation failed. Audiences: '9f3d61b2-e38e-4c22-88ed-3f6735e40e0a'. Did not match: validationParameters.ValidAudience: '27339a64-0c55-4ba0-8632-aaaa81030814' or validationParameters.ValidAudiences: 'null'.

This because both clientdi "9f3d61b2-e38e-4c22-88ed-3f6735e40e0a" and clientid "27339a64-0c55-4ba0-8632-aaaa81030814" want to access to same API.

In order to avoid this, in ValidAudiences we have to enumerate all the Audiences authorized to use the API. 

![architecture](assets/architecture.png)

Looking at schema above, API Calculator and API Scientific Calculator are backend API called by different services, so more valid audiences must be set.


| API        | Valid Audiences |
|------------|-----------------|
| Calculator | Calculator - ScientificCalculator - WebCalculator - postman    |
| ScientificCalculator |    ScientificCalculator - WebCalculator - postman  |


For the Calculator API, In calculatorAPI>Startup>ConfigureServices, you need to add the following

```csharp
var tokenValidationParameters = new TokenValidationParameters
            {
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                SaveSigninToken = false,
                ValidateActor = false,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = false,
                ValidateLifetime = true,
                ValidAudiences = new string[] {
                    Configuration["AzureAdB2C:ClientId"],   // API Calculator
                    "9f3d61b2-e38e-4c22-88ed-3f6735e40e0a", // API Scientific Calculator
                    "c07391de-3205-4496-a704-4607b18b64f9",  // WebCalculator 
                    "d668afda-f613-43f7-89e4-5425496ebdf2", // postman
                }
            };

services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtOptions =>
              {
                  jwtOptions.Authority = $"https://login.microsoftonline.com/tfp/{Configuration["AzureAdB2C:Tenant"]}/{Configuration["AzureAdB2C:Policy"]}/v2.0/";
                  jwtOptions.Audience = Configuration["AzureAdB2C:ClientId"];
                  jwtOptions.TokenValidationParameters = tokenValidationParameters;
                  jwtOptions.Events = new JwtBearerEvents
                  {
                      OnAuthenticationFailed = AuthenticationFailed
                  };
              });
```

While for the Scientific Calculator API, you need to add:


```csharp
var tokenValidationParameters = new TokenValidationParameters
            {
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                SaveSigninToken = false,
                ValidateActor = false,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = false,
                ValidateLifetime = true,
                ValidAudiences = new string[] {
                    Configuration["AzureAdB2C:ClientId"],    // API Scientific Calculator
                    "c07391de-3205-4496-a704-4607b18b64f9",  // WebCalculator 
                    "d668afda-f613-43f7-89e4-5425496ebdf2",  // postman
                }
            };

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtOptions =>
            {
                jwtOptions.Authority = $"https://login.microsoftonline.com/tfp/{Configuration["AzureAdB2C:Tenant"]}/{Configuration["AzureAdB2C:Policy"]}/v2.0/";
                jwtOptions.Audience = Configuration["AzureAdB2C:ClientId"];
                jwtOptions.TokenValidationParameters = tokenValidationParameters; // ADD THIS LINE TOO
                jwtOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = AuthenticationFailed
                };
            });
```

# Retrieve the bearer and call the API
That's all. In order to call the API, you can go to Azure Portal > Azure B2C > Policies > Sign-up or Sign-In User Policy > B2C_1_signin-default

![retrieve the bearer](assets/img10.png)

* Application: **ApiScientificCalculator**
* ReplyURL: **https://jwt.ws**

Click [RUN NOW], copy the bearer from the page and use it in Postman.


