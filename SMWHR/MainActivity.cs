using Android.App;
using Android.OS;
using Android.Content;

namespace SMWHR
{
    [Activity(Label = "SMWHR", MainLauncher = true)]
    public class MainActivty : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var intent = (TryReadUserId() == null) ? 
                new Intent(this, typeof(LoginActivity)) : new Intent(this, typeof(MenuActivity));
            
            StartActivity(intent);
            Finish();
        }

        private string TryReadUserId()
        {
            var sharedPref = GetSharedPreferences(
                GetString(Resource.String.PreferencesFile), FileCreationMode.Private);
            return sharedPref.GetString(GetString(Resource.String.UserIdPreference), null);
        }
    }
}

