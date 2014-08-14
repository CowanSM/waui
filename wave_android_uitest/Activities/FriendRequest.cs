
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
    [Activity(Label = "FriendRequest", ScreenOrientation=Android.Content.PM.ScreenOrientation.Portrait)]			
    public class FriendRequest : Activity {
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.friend_request);

            // setup back button for now
            var btn = FindViewById<TextView>(Resource.Id.friend_request_button_cancel);
            btn.Click += (sender, e) => {
                Finish();
            };
        }
    }
}

