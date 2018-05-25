# Setup NicolAPICalculator

Objective of this API is to expose the standard 4 operations (+, -, *, /) as authenticathed API.
In this walktrough we will configure a dot net core api including the configuration for Azure B2C tenant.


#(1) Azure B2C Configuration
creat an application on Azure B2C as shown follow.

![Create Application](assets/img03.png)
  
Note: https://jws.ms allows you to read easily the bearer, so that you can copy it in Postman or curl.
  
#(2) create a ASP Net Core API not authenticated
via visual studio create a solution of type "ASP.NET Core Web Application", type API, with NO Authentication
![create vs project](assets/img04.png)


#(2) Create a policy: B2C\_1\_signin-default

On Azure Portal, on Azure AD B2C Service go to

* Sign-up or sign-in policies
* Add and name it "B2C\_1\_signin-default"


In Azure AD B2C, every user experience is defined by a policy. You have to create a policy in order to control the specific look and feel, use of MFA, information the app receive from AD B2C etc.

#(3) Configure appsettings.json and solution for authentication

in appsettings.config add the following information:

	"AzureAdB2C": {
	"Tenant": "nicolb2c.onmicrosoft.com",
	"ClientId": "<the tenant id>",
	"Policy: "B2C_1_signin-default"
	},

also add following nuget packages to the solution

	Microsoft.AspNetCore.Authentication.JwtBearer
	Microsoft.AspNetCore.Mvc
	Microsoft.Extensions.Configuration.UserSecrets
	Microsoft.Extensions.SecretManager.Tools

#(4) update the code 

Update [program.cs](nicold.playground/nicold.APICalculator/program.cs) and [startup.cs](nicold.playground/nicold.APICalculator/startup.cs) as shown in this repository. 

Then create a controller for the home page [Controllers/HomeController.cs](nicold.playground/nicold.APICalculator/Controllers/HomeController.cs) with the corresponding view [View/Home/index.shtml](nicold.playground/nicold.APICalculator/View/Home/index.shtml)

It is noe the time to build the real API. We will implement the API with the controller **Controllers/CalcController.cs**

The anonymous API looks like the following:

	public class CalcController : Controller
	{
		// GET api/values/
		[HttpGet("{op}")]
		public IActionResult Get(string op, [FromQuery] double param1, [FromQuery] double param2)
		{
		double result = 0;
		// omiss...

		result = param1 + param2;
		return new ObjectResult(result);
		}
	}

if we want to enable the authentication, we need to add the **\[Authorize\]** attribute to the class, and everything is ready. In this sample we just require authentication, but if you need also to verify the presence of a specific claim or somthing, you can filter within the get() method on attribute in **HttpContext.User** and/or **HttpContext.User.Claims**.

That's all. In order to call the API, you can call the following link [https://login.microsoftonline.com/nicolb2c.onmicrosoft.com/oauth2/v2.0/authorize?p=B2C_1_signin-default&client_id=27339a64-0c55-4ba0-8632-aaaa81030814&nonce=defaultNonce&redirect_uri=https%3A%2F%2Fjwt.ms&scope=https%3A%2F%2Fnicolb2c.onmicrosoft.com%2FApiCalculator%2Fuser_impersonation&response_type=token&prompt=login](https://login.microsoftonline.com/nicolb2c.onmicrosoft.com/oauth2/v2.0/authorize?p=B2C_1_signin-default&client_id=27339a64-0c55-4ba0-8632-aaaa81030814&nonce=defaultNonce&redirect_uri=https%3A%2F%2Fjwt.ms&scope=https%3A%2F%2Fnicolb2c.onmicrosoft.com%2FApiCalculator%2Fuser_impersonation&response_type=token&prompt=login) it gives you the bearer you can put in the header to call the api.
