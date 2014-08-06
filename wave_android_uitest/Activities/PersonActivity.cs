
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
	[Activity (Label = "PersonActivity")]			
	public class PersonActivity : FragmentActivity {
		public override bool OnCreateOptionsMenu (IMenu menu) {
			MenuInflater.Inflate (Resource.Menu.activity_actions, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
			case Resource.Id.action_activity:
				Finish ();
				break;
			case Resource.Id.action_challenges:
				Finish ();
				break;
			case Resource.Id.action_my_life:
				Finish ();
				break;
			case Resource.Id.action_people:
				// do nothing
				break;
			default:
				break;
			}

			return base.OnOptionsItemSelected (item);
		}

		protected override void OnCreate (Bundle bundle) {
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.friend_stats);

			// Create your application here
			// get name from intent
			Java.Lang.String name = new Java.Lang.String ("Somebody Someone");
			Bundle extras = Intent.Extras;
			if (extras != null) {
				name = new Java.Lang.String (extras.GetString ("person_name").ToCharArray ());
			}

			// setup pager
			var pager = FindViewById<ViewPager> (Resource.Id.fs_pager);
			var adaptor = new GenericFragmentPagerAdaptor(SupportFragmentManager);

			adaptor.AddFragmentView ((i, v, b) => {
				var layout = i.Inflate(Resource.Layout.friend_stats_insert, v, false);

				return layout;
			});

			pager.Adapter = adaptor;
			pager.OffscreenPageLimit = 3;

			// Setup actionbar
			ActionBar.TitleFormatted = name;
			ActionBar.SetIcon (Resource.Drawable.person_outline);
			ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

			// tabs
			var tab = ActionBar.NewTab ();
			tab.SetText ("Month");
			tab.TabSelected += (sender, e) => {};
			ActionBar.AddTab (tab);

			tab = ActionBar.NewTab ();
			tab.SetText ("Week");
			tab.TabSelected += (sender, e) => {};
			ActionBar.AddTab (tab);

			tab = ActionBar.NewTab ();
			tab.SetText ("Day");
			tab.TabSelected += (sender, e) => {};
			ActionBar.AddTab (tab);

			ActionBar.Show ();
		}
	}
}

