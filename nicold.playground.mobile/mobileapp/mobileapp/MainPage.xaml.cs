using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private async void Button_SigninClicked(object sender, EventArgs e)
        {
            try
            {
                //AuthenticationResult ar = await App.PCA.AcquireTokenSilentAsync(App.Scopes, GetUserByPolicy(App.PCA.Users, App.PolicySignUpSignIn), App.Authority, false);
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

        private void Button_SumClicked(object sender, EventArgs e)
        {

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
    }
}
