
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace wave_android_uitest {
    [Activity(Label = "HomeActivity", ScreenOrientation=Android.Content.PM.ScreenOrientation.Portrait)]			
    public class HomeActivity : SubPrimaryActivity {
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.home);

            // tab bar
            Android.App.FragmentTransaction transaction = FragmentManager.BeginTransaction ();
            TabBarFragment fragment = new TabBarFragment (this);
            transaction.Add (Resource.Id.home_root, fragment);
            transaction.Commit();

            // setup audio sync button here
            var btn = FindViewById<TextView>(Resource.Id.home_options_sync);
            btn.Click += (sender, e) => {
                // need to go to sync page...
                // start new activity
                var intent = new Intent(BaseContext, typeof(SyncActivity));
                StartActivity(intent);
            };

            ActionBar.Hide();
        }
    }
}

