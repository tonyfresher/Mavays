using System.Net.Http;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Firebase.Xamarin.Auth;
using System;
using Android.Content.PM;

namespace SMWHR
{
    [Activity(Label = "SMWHR", Theme = "@style/AppTheme.Dark",
        ScreenOrientation = ScreenOrientation.SensorPortrait, WindowSoftInputMode = Android.Views.SoftInput.StateHidden)]
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
            // _signupLink.Click += (sender, e) => { } TODO: Активити регистрации
        }

        private async void Login()
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

            
            try
            {
                var apiKey = GetString(Resource.String.ApiKey); // Он реально меняется
                using (var authProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey)))
                {
                    var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
                    SaveUserId(auth.FirebaseToken);
                    OnLoginSuccess();
                }
            }
            catch (HttpRequestException)
            {
                progressDialog.Cancel();
                OnLoginFailed(GetString(Resource.String.SignInError));
            }
            catch (Exception)
            {
                progressDialog.Cancel();
                OnLoginFailed(GetString(Resource.String.Error));
            }
        }

        public override void OnBackPressed()
        {
            MoveTaskToBack(true);
        }

        private void OnLoginFailed(string message = null)
        {
            if (message != null)
                Toast.MakeText(this, message, ToastLength.Long).Show();

            _loginButton.Enabled = true;
        }

        private void OnLoginSuccess()
        {
            _loginButton.Enabled = true;

            var intent = new Intent(this, typeof(MenuActivity));
            StartActivity(intent);

            Finish();
        }

        private void SaveUserId(string userId)
        {
            var sharedPref = GetSharedPreferences(
                GetString(Resource.String.PreferencesFile), FileCreationMode.Private);
            var editor = sharedPref.Edit();
            editor.PutString(GetString(Resource.String.UserIdPreference), userId);
            editor.Commit();
        }

        private bool Validate()
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

