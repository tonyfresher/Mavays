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
    [Activity(Label = "SMWHR", ScreenOrientation = ScreenOrientation.SensorPortrait,
        WindowSoftInputMode = Android.Views.SoftInput.StateHidden)]
    public class MenuActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Menu);
        }

        public override void OnBackPressed()
        {
            MoveTaskToBack(true);
        }
    }
}

