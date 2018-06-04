# Azure-B2C-playground
Azure AD B2C playground ehre test authentication and integration scenarios

Objective for this lab is build a playground where test various authentication and integration scenarios on Azure B2C.

We will configure a Tenant B2C for the company my NicolCorp company(!) where we will store all customer's user profiles. Once configured we will configure the following Web Application:

* 	**NicolAPICalculator**: an authenticated WebAPI that allows simple calculation
* 	**NicolAPIScientificCalculator**: an authenticated WebAPI that allows basic scientific calculation. It uses Api exposed by NicolAPICalculator
* 	**NicolWebCalculator**: an ASP.NET MVC Web Application that allows user to perform simple calculations. It uses NicolAPICalculator via backend.
* 	**NicolWebScientificCalculator**:  an ASP.NET MVC Web Application that allows user to perform simple calculations. It uses NicolAPICalculator via backend.

As shown in the image below

![architecture](assets/architecture.png)

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

# Next Steps

1. [setup NicolAPICalculator](setup-apicalculator.md)
	1. Access API from Xamarin Forms Client App
2. Configure NicolAPIScientificCalculator
3. Configure NicolWebCalculator
4. Configure NicolWEBScientificCalculator
