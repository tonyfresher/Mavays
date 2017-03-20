using System.Net.Http;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Firebase.Xamarin.Auth;
using System;

namespace SMWHR
{
    [Activity(Label = "SMWHR", MainLauncher = true, Theme = "@style/AppTheme.Dark",
        WindowSoftInputMode = Android.Views.SoftInput.StateHidden)]
    public class LoginActivity : Activity
    {
        private EditText _emailText;
        private EditText _passwordText;
        private Button _loginButton;
        private TextView _signupLink;

        protected override void OnCreate(Bundle bundle)
        {
            if (TryReadUserId() != null)
                Finish(); // TODO: Тут поменять на пререход в другое активити

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
            Finish(); // TODO: Тут поменять на пререход в другое активити
        }

        private string TryReadUserId()
        {
            var sharedPref = GetPreferences(FileCreationMode.Private);
            return sharedPref.GetString(GetString(Resource.String.UserIdPreference), null);
        }

        private void SaveUserId(string userId)
        {
            var sharedPref = GetPreferences(FileCreationMode.Private);
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

