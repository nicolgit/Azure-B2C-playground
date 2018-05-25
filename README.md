# Azure-B2C-playground
Azure AD B2C playground ehre test authentication and integration scenarios

Objective for this lab is build a playground where test various authentication and integration scenarios on Azure B2C.

We will configure a Tenant B2C for the company my NicolCorp company(!) where we will store all customer's user profiles. Once configured we will configure the following Web Application:

* 	**NicolAPICalculator**: an authenticated WebAPI that allows simple calculation
* 	**NicolAPIScientificCalculator**: an authenticated WebAPI that allows basic scientific calculation. It uses Api exposed by NicolAPICalculator
* 	**NicolWebCalculator**: an ASP.NET MVC Web Application that allows user to perform simple calculations. It uses NicolAPICalculator via backend.
* 	**NicolWebScientificCalculator**:  an ASP.NET MVC Web Application that allows user to perform simple calculations. It uses NicolAPICalculator via backend.

As shown in the image below

TODO: Disegno dell'architettura

# Setup Azure AD B2C Instance

The first step is provision a B2C instances. Go to https://portal.azure.com/#create/hub, find and select "Azure Active Directory B2C". You'll need to enter an organization name, the domain name (.onmicrosoft.com), and the country or region of your organization. This determines the datacenter for your directory.

In my environment:

* 	**Organization**: NicolCorp-B2C-Playground
* 	**Domain**: nicolb2c.onmicrosoft.com
* 	**Contry**: Italy (yes i'm italian :-)
	
Once configured you can switch from your B2C tenant to another using the user menu on the top right of the Azure Portal.

![change directory](assets/img01.png)

# Azure Resources Setup

In order to create this lab you have to creare:

* **Resource Group**: nicolcorp-b2c-playground - will contain all the resources involved in the plan
* **AppService Plan**: nicolplan-b2c - I used the free F1 ideal for dev/testing
* **WebApp**: NicolWebCalculator.azurewebsites.net (OS: Windows)
* **WebApp**: NicolWebScientificCalculator.azurewebsites.net (OS: Windows)
* **WebApp**: NicolAPICalculator.azurewebsites.net (OS: Windows)
* **WebApp**: NicolAPIScientificCalculator.azurewebsites.net (OS: Windows)
 
the result will the following:

![resource group](assets/img02.png)

# Setup NicolAPICalculator

Objective of this API is to expose the standard 4 operations (+, -, *, /) as authenticathed API.
  
(1)  ASP.NET Core API with NO Authentication


(2) configure an application on Azure B2C as shown follow

![Create Application](assets/img03.png)
  
  remember to add reply URL for both appService and localhost in order to test also locally
  
(3) Create a policy: B2C 1 signin-default

In Azure AD B2C, every user experience is defined by a policy. You will need to create a policy to communicate with Azure AD B2C. 
  
(4) add to web.config the following:

	"AzureAdB2C": {
	"Tenant": "nicolb2c.onmicrosoft.com",
	"ClientId": "<the tenant id>",
	"SignUpSignInPolicyId": "B2C_1_signin-default"
	},

(5) add following nuget packages to the solution

	Microsoft.AspNetCore.Authentication.JwtBearer
	Microsoft.AspNetCore.Mvc
	Microsoft.Extensions.Configuration.UserSecrets
	Microsoft.Extensions.SecretManager.Tools

(6) update the following files:
change in program.cs and startup.cs as shown in this repository (inser link). Then create a controller for the home page (Controllers/HomeController.cs) with the corresponding view (View/Home/index.cshtml)

(7) now is the time to build the API. We will implement the API with the controller "Controllers/CalcController.cs"

an anonymous API looks like the following:

		// GET api/values/
        [HttpGet("{op}")]
        public IActionResult Get(string op, [FromQuery] double param1, [FromQuery] double param2)
        {
            double result = 0;
			// omiss...
			
			result = param1 + param2;
            return new ObjectResult(result);
        }

if we want to enable 


 
how to register a webapi on B2C: 
https://docs.microsoft.com/en-us/azure/active-directory-b2c/active-directory-b2c-devquickstarts-api-dotnet 
  
  add AD B2C to an API
  https://docs.microsoft.com/en-us/azure/active-directory-b2c/active-directory-b2c-app-registration#register-a-web-api
  
  
  
  
  how to enable B2C authentication:  
  1 create application on Azure B2c Tenant
  2 Configure reply-url
  3 application settings > create key
 
 how to configure API to authenticate
 
  
  
  
https://azure.microsoft.com/en-us/resources/samples/active-directory-b2c-dotnetcore-webapi/ 

https://github.com/Azure-Samples/active-directory-b2c-dotnetcore-webapi