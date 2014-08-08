
using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace wave_android_uitest {
	public class TabBarFragment : Fragment {
		View _rootView;
		SubPrimaryActivity _parentActivity;

		public TabBarFragment(SubPrimaryActivity parent) {
			_parentActivity = parent;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);
			// Create your fragment here

		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			_rootView = inflater.Inflate (Resource.Layout.tab_bar, container, false);
			((RelativeLayout.LayoutParams)_rootView.LayoutParameters).AddRule (LayoutRules.AlignParentBottom);

			// not a great place for it, but let's setup the buttons
			SetupButton (Resource.Id.tab_bar_activity_layout, Resource.Id.tab_bar_activity_text,
				Resource.Id.tab_bar_activity_image, Resource.Drawable.ActivityTab_Selected2, _parentActivity is ActivityActivity);
			SetupButton (Resource.Id.tab_bar_my_life_layout, Resource.Id.tab_bar_my_life_text,
                Resource.Id.tab_bar_my_life_image, Resource.Drawable.CalendarTab_Selected2, _parentActivity is MyLifeActivity || _parentActivity is DayDetailActivity);
			SetupButton (Resource.Id.tab_bar_challenges_layout, Resource.Id.tab_bar_challenge_text,
				Resource.Id.tab_bar_challenge_image, Resource.Drawable.ChallengesTab_Selected2, _parentActivity is ChallengesActivity);
			SetupButton (Resource.Id.tab_bar_people_layout, Resource.Id.tab_bar_people_text,
				Resource.Id.tab_bar_people_image, Resource.Drawable.FriendsTab_Selected2, _parentActivity is PeopleActivity);

			// setup clickables
			var layout = _rootView.FindViewById<FrameLayout> (Resource.Id.tab_bar_activity_layout);
			layout.Click += (sender, e) => {
				// laod activity activity
				_parentActivity.SwitchActivity(typeof(ActivityActivity));
			};

			layout = _rootView.FindViewById<FrameLayout> (Resource.Id.tab_bar_my_life_layout);
			layout.Click += (sender, e) => {
				// load my life activity
				_parentActivity.SwitchActivity(typeof(MyLifeActivity));
			};

			layout = _rootView.FindViewById<FrameLayout> (Resource.Id.tab_bar_challenges_layout);
			layout.Click += (sender, e) => {
				// load challenges activity
				_parentActivity.SwitchActivity(typeof(ChallengesActivity));
			};

			layout = _rootView.FindViewById<FrameLayout> (Resource.Id.tab_bar_people_layout);
			layout.Click += (sender, e) => {
				// load people activity
				_parentActivity.SwitchActivity(typeof(PeopleActivity));
			};

			return _rootView;
		}

		private void SetupButton(int layoutResource, int textResource, int imageResource, int imageDrawable, bool selected) {
			var layout = _rootView.FindViewById<FrameLayout> (layoutResource);
			var text = _rootView.FindViewById<TextView> (textResource);
			var image = _rootView.FindViewById<ImageView> (imageResource);

			if (selected) {
				layout.SetBackgroundColor (Android.Graphics.Color.Argb (50, 255, 255, 255));
				layout.Clickable = false;
				text.SetTextColor (Android.Graphics.Color.Black);
				image.SetImageResource (imageDrawable);
			} else {
				layout.SetBackgroundColor (Android.Graphics.Color.Argb (0, 0, 0, 0));
				layout.Clickable = true;
			}
		}
	}
}

