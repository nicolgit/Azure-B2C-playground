# Azure-B2C-playground

The objective of this lab is to build a playground in testing authentication and integration scenarios on Azure B2C.

We will configure a Tenant B2C for the company _NicolCorp_ where we will store all customer's user profiles. Once configured we will set up the following Web Applications: 

* 	**NicolAPICalculator**: an authenticated WebAPI that implements simple calculation
* 	**NicolAPIScientificCalculator**: an authenticated WebAPI that allows basic scientific calculation. It uses API exposed by NicolAPICalculator
* 	**NicolWebCalculator**: an ASP.NET MVC Web Application that allows the user to perform simple calculations. It uses NicolAPICalculator via backend.

we will also show how to access these API from: 

* A Mobile App developed with Xamarin
* Postman

As shown in the image below:

![architecture](assets/architecture.png)

where there is a "double jump" (i.e. user access to WebApp that accesses the Scientific Calculator API that accesses the Calculator API), user impersonation will be kept on all the chain.

# Setup Azure AD B2C Instance

The first step is providing a B2C instance. Go to https://portal.azure.com/#create/hub, find and select "Azure Active Directory B2C". You'll need to enter an organization name, the domain name (.onmicrosoft.com), and the country or region of your organization. This determines the datacenter for your directory.

In my environment I have used:

* 	**Organization**: NicolCorp-B2C-Playground
* 	**Domain**: nicolb2c.onmicrosoft.com
* 	**Country**: Italy
	
Once configured, remember that you can switch from your B2C tenant to another using the user menu on the top right of the Azure Portal.

![change directory](assets/img01.png)


# Configure UserFlow (Policy)

AzureB2C comes with a default sign-up|sign-in user policy. In order to show the user's "Display Name" on the the Web and mobile UI, we need to return the Application Claim. Application claims are returned to the application and the JWS token is returned at the authorized time. To make sure this information in the list of claims:
* Open Azure B2C Service
* Select "User flows (policy)"
* Select Default policy
* Select "Application Claims"
* from the claims list, select "Display Name"
* Click on Save button

![update User Flow](assets/img14.png)

# Azure Resources Setup

The next step is to create the remaining objects in Azure:

* **Resource Group**: nicolcorp-b2c-playground - will contain all the resources involved in the plan
* **AppService Plan**: nicolplan-b2c 
* **WebApp**: NicolAPICalculator.azurewebsites.net (OS: Windows)
* **WebApp**: NicolAPIScientificCalculator.azurewebsites.net (OS: Windows)
* **WebApp**: NicolAPICalculator.azurewebsites.net (OS: Windows)

the result is the following:

![resource group](assets/img02.png)

# Next Steps

1. [Setup NicolAPICalculator](setup-apicalculator.md)
2. [Setup NicolAPIScientificCalculator](setup-apiscientificcalculator.md)
3. [Setup NicolWebCalculator](setup-webcalculator.md)
4. [Xamarin Forms Mobile App](setup-apicalculator-xamarin.md)
5. Configure Postman [to call NicolAPICalculator](setup-postman.md)
