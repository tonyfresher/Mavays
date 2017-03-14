using Android.App;
using Android.Widget;
using Android.OS;

namespace Mavays
{
    [Activity(Label = "Mavays", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView (Resource.Layout.Main);
        }
    }
}

