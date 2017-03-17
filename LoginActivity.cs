using System.Linq;
using System.IO;
using System.Net.Http;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Nfc;
using Android.Util;
using Android.Views;
using Firebase.Xamarin.Auth;
using Firebase.Xamarin.Database;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace SMWHR
{
    [Activity(Label = "SMWHR", MainLauncher = true, Theme = "@style/AppTheme.Dark")]
    public class LoginActivity : Activity
    {
        private EditText _emailText;
        private EditText _passwordText;
        private Button _loginButton;
        private TextView _signupLink;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.LoginActivity);

            _emailText = FindViewById<EditText>(Resource.Id.input_email);
            _passwordText = FindViewById<EditText>(Resource.Id.input_password);
            _loginButton = FindViewById<Button>(Resource.Id.btn_login);
            _signupLink = FindViewById<TextView>(Resource.Id.link_signup);

            _loginButton.Click += (sender, e) => Login();
        }

        public async void Login()
        {
            if (!Validate())
            {
                OnLoginFailed();
                return;
            }

            _loginButton.Enabled = false;

            var progressDialog = new ProgressDialog(this, Resource.Style.AppTheme_Dark_Dialog);
            progressDialog.Indeterminate = true;
            progressDialog.SetMessage(GetString(Resource.String.SigningIn));
            progressDialog.Show();

            var email = _emailText.Text;
            var password = _passwordText.Text;

            var apiKey = "AIzaSyBuQuTvg0NeJUhWghWfhWuLLsATeRmWtHk";
            using (var authProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey)))
            {
                try
                {
                    var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
                    OnLoginSuccess();
                }
                catch (HttpRequestException)
                {
                    progressDialog.Cancel();
                    OnLoginFailed();
                }
            }
        }

        public override void OnBackPressed()
        {
            MoveTaskToBack(true);
        }

        public void OnLoginFailed()
        {
            Toast.MakeText(this, GetString(Resource.String.SignInError), ToastLength.Long).Show();
            _loginButton.Enabled = true;
        }

        public void OnLoginSuccess()
        {
            _loginButton.Enabled = true;
            Finish();
        }

        public bool Validate()
        {
            var valid = true;

            var email = _emailText.Text;
            var password = _passwordText.Text;

            if (email == "" || !Android.Util.Patterns.EmailAddress.Matcher(email).Matches())
            {
                _emailText.Error = GetString(Resource.String.EmailError);
                valid = false;
            }
            else
            {
                _emailText.Error = null;
            }

            if (password == "" || password.Length < 4)
            {
                _passwordText.Error = GetString(Resource.String.PasswordError);
                valid = false;
            }
            else
            {
                _passwordText.Error = null;
            }

            return valid;
        }
    }
}

