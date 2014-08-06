
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

namespace wave_android_uitest
{
	[Activity (Label = "MyLifeActivity")]			
	public class MyLifeActivity : SubPrimaryActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
            SetContentView(Resource.Layout.my_life);

            // tab bar
            Android.App.FragmentTransaction transaction = FragmentManager.BeginTransaction ();
            TabBarFragment fragment = new TabBarFragment (this);
            transaction.Add (Resource.Id.my_life_root_layout, fragment);
            transaction.Commit ();

            var pager = FindViewById<ViewGroup>(Resource.Id.my_life_pager_parent);
            ((RelativeLayout.LayoutParams)pager.LayoutParameters).AddRule(LayoutRules.Above, Resource.Id.activity_tab_bar);
		}
	}
}

