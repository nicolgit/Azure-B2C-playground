using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace mobileapp
{
	public partial class MainPage : ContentPage
	{
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            IsSignedIn = false;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            // Check to see if we have a User
            // in the cache already.
            try
            {
                AuthenticationResult ar = await App.PCA.AcquireTokenSilentAsync(App.Scopes, GetUserByPolicy(App.PCA.Users, App.PolicySignUpSignIn), App.Authority, false);
                IsSignedIn = true;
            }
            catch (Exception ex)
            {
                // Doesn't matter the answer, we are offline
                IsSignedIn = false;
            }
        }

        #region PROPERTIES

        private bool _isSignedIn;
        public bool IsSignedIn
        {
            get
            {
                return _isSignedIn;
            }
            set
            {
                _isSignedIn = value;
                OnPropertyChanged("IsSignedIn");
                OnPropertyChanged("IsNotSignedIn");
            }
        }
        public bool IsNotSignedIn
        {
            get
            {
                return !IsSignedIn;
            }
        }

        private string _output = "";
        public string Output {
            get
            {
                return _output;
            }
            set {
                _output = value;
                OnPropertyChanged("Output");
            }
        }

        private string _param1 = "4";
        public string Parameter1
        {
            get
            {
                return _param1;
            }
            set
            {
                _param1 = value;
                OnPropertyChanged("Parameter1");
            }
        }

        private string _param2 = "3";
        public string Parameter2
        {
            get
            {
                return _param2;
            }
            set
            {
                _param2 = value;
                OnPropertyChanged("Parameter2");
            }
        }

        #endregion

        private async void Button_SigninClicked(object sender, EventArgs e)
        {
            try
            {
                AuthenticationResult ar = await App.PCA.AcquireTokenAsync(App.Scopes, GetUserByPolicy(App.PCA.Users, App.PolicySignUpSignIn), App.UiParent);

                UpdateUserInfo(ar);

                IsSignedIn = true;
            }
            catch (Exception ex)
            {
                Output = ex.ToString();
                IsSignedIn = false;
            }
        }

        private void Button_SignOutClicked(object sender, EventArgs e)
        {
            foreach (var user in App.PCA.Users)
            {
                App.PCA.Remove(user);
            }

            IsSignedIn = false;
            Output = "";
        }

        private async void Button_SumClicked(object sender, EventArgs e)
        {
            await CallCalculatorAPI("sum");
        }

        private async void Button_SubClicked(object sender, EventArgs e)
        {
            await CallCalculatorAPI("subtract");
        }

        private async void Button_MulClicked(object sender, EventArgs e)
        {
            await CallCalculatorAPI("multiply");
        }

        private async void Button_SplitClicked(object sender, EventArgs e)
        {
            await CallCalculatorAPI("split");
        }

        private async Task CallCalculatorAPI(string operation)
        {
            try
            {
                Output = $"Calling API ...";

                string parameters = $"{operation}?param1={Parameter1}&param2={Parameter2}";

                string apicall = App.ApiEndpoint + parameters;

                AuthenticationResult ar = await App.PCA.AcquireTokenSilentAsync(App.Scopes, GetUserByPolicy(App.PCA.Users, App.PolicySignUpSignIn), App.Authority, false);
                string token = ar.AccessToken;

                // Get data from API
                HttpClient client = new HttpClient();
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, apicall);
                message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.SendAsync(message);
                string responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Output = $"Response from API {apicall}\r\n\r\n{responseString}";
                }
                else
                {
                    Output = $"Error calling API {apicall}";
                }
            }

            catch (MsalUiRequiredException ex)
            {
                await DisplayAlert($"Session has expired, please sign out and back in.", ex.ToString(), "Dismiss");
            }
            catch (Exception ex)
            {
                await DisplayAlert($"Exception:", ex.ToString(), "Dismiss");
            }
        }

        private IUser GetUserByPolicy(IEnumerable<IUser> users, string policy)
        {
            foreach (var user in users)
            {
                string userIdentifier = Base64UrlDecode(user.Identifier.Split('.')[0]);
                if (userIdentifier.EndsWith(policy.ToLower())) return user;
            }

            return null;
        }

        private string Base64UrlDecode(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/');
            s = s.PadRight(s.Length + (4 - s.Length % 4) % 4, '=');
            var byteArray = Convert.FromBase64String(s);
            var decoded = Encoding.UTF8.GetString(byteArray, 0, byteArray.Count());

            return decoded;
        }

        public void UpdateUserInfo(AuthenticationResult ar)
        {
            JObject user = ParseIdToken(ar.IdToken);

            StringBuilder sb = new StringBuilder();
            foreach (var item in user)
            {
                sb.Append($"{item.Key}={item.Value}\r\n");
            }
            Output = sb.ToString();
        }
        JObject ParseIdToken(string idToken)
        {
            // Get the piece with actual user info
            idToken = idToken.Split('.')[1];
            idToken = Base64UrlDecode(idToken);
            return JObject.Parse(idToken);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}
