
using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.Support.V4.App;

namespace wave_android_uitest {
    [Activity (Label = "SubPrimaryActivity", ScreenOrientation=Android.Content.PM.ScreenOrientation.Portrait)]			
	public class SubPrimaryActivity : FragmentActivity {

		public void SwitchActivity(Type nextActivity) {
			MainActivity.NextActivity = nextActivity;
			Finish ();
		}
	}
}

