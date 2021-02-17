using Android.App;
using Android.OS;
using Android.Content;
using Android.Util;
using AndroidX.AppCompat.App;

namespace PrismSample.Droid
{
    [Activity(Theme = "@style/MainTheme.Splash", 
              MainLauncher = true, 
              NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
            Log.Debug(TAG, "SplashActivity.OnCreate");
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}