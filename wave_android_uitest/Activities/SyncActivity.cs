
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
    [Activity(Label = "SyncActivity")]			
    public class SyncActivity : Activity {
        private bool _start;
        private Button _btn;

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.sync);
            _start = true;

            _btn = FindViewById<Button>(Resource.Id.sync_button);
            _btn.Click += HandleClick;

            // back closes activity
            var txt = FindViewById<TextView>(Resource.Id.sync_back);
            txt.Click += (object sender, EventArgs e) => {
                Finish();
            };

            ActionBar.Hide();
        }

        void HandleClick (object sender, EventArgs e)
        {
            if (_start) {
                Start(sender, e);
                _btn.Text = "Stop";
            } else {
                Stop(sender, e);
                _btn.Text = "Start";
            }
            _start = !_start;
        }

        public void Start(object sender, EventArgs args) {

        }

        public void Stop(object sender, EventArgs args) {

        }
    }

}

