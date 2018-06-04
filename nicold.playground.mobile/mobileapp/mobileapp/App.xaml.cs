using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.Identity.Client;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace mobileapp
{
	public partial class App : Application
	{
        public static PublicClientApplication PCA = null;
        public static UIParent UiParent = null;

        // Azure AD B2C Coordinates
        public static string Tenant = "nicolb2c.onmicrosoft.com";
        public static string ClientID = "27339a64-0c55-4ba0-8632-aaaa81030814";
        public static string PolicySignUpSignIn = "B2C_1_signin-default";

        public static string[] Scopes = { "https://nicolb2c.onmicrosoft.com/ApiCalculator/user_impersonation" };
        public static string ApiEndpoint = "https://nicolwebcalculator.azurewebsites.net/api/calc/";
        public static string AuthorityBase = $"https://login.microsoftonline.com/tfp/{Tenant}/";
        public static string Authority = $"{AuthorityBase}{PolicySignUpSignIn}";
        //public static string Authority = $"https://login.microsoftonline.com/nicolb2c.onmicrosoft.com/v2.0/.well-known/openid-configuration?p=B2C_1_signin-default";
        

        public App ()
		{
			InitializeComponent();

            // default redirectURI; each platform specific project will have to override it with its own
            PCA = new PublicClientApplication(ClientID, Authority);
            PCA.RedirectUri = $"msal{ClientID}://auth";

            MainPage = new MainPage();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
