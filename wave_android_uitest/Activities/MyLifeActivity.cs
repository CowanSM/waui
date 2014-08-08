
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
        private bool _opponent = true;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
            SetContentView(Resource.Layout.my_life);

            // tab bar
            Android.App.FragmentTransaction transaction = FragmentManager.BeginTransaction ();
            TabBarFragment fragment = new TabBarFragment (this);
            transaction.Add (Resource.Id.my_life_root_layout, fragment);

            // calendar view
            CalendarPagerView calendarFrag = new CalendarPagerView(FindViewById<ViewFlipper>(Resource.Id.my_life_pager));
            calendarFrag.OnClick += HandleOnClick;
            calendarFrag.MonthChanged += HandleMonthChanged;
            transaction.Add(Resource.Id.my_life_pager, calendarFrag);
            transaction.Commit();

            var pparent = FindViewById<ViewGroup>(Resource.Id.my_life_pager_parent);
            ((RelativeLayout.LayoutParams)pparent.LayoutParameters).AddRule(LayoutRules.Above, Resource.Id.activity_tab_bar);

            // setup fonts
            var gbook = Android.Graphics.Typeface.CreateFromAsset(Assets, "fonts/GOTHAM_BOOK.TTF");

            if (_opponent) {
                FindViewById<TextView>(Resource.Id.my_life_opponent_challenge_text).SetTypeface(gbook, Android.Graphics.TypefaceStyle.Normal);
                FindViewById<TextView>(Resource.Id.my_life_opponent_remove_txt).SetTypeface(gbook, Android.Graphics.TypefaceStyle.Normal);
            } else {
                FindViewById<RelativeLayout>(Resource.Id.my_life_opponent_layout).Visibility = ViewStates.Invisible;
                ((RelativeLayout.LayoutParams)FindViewById<ViewGroup>(Resource.Id.my_life_pager_parent).LayoutParameters).AddRule(LayoutRules.Below, Resource.Id.my_life_header_frame);
            }

            ActionBar.Hide();
		}

        public void HandleMonthChanged (string obj) {
            FindViewById<TextView>(Resource.Id.my_life_month_text).Text = obj;
        }

        private void HandleOnClick(View view) {
            try {
                var txt = view.FindViewById<TextView>(Resource.Id.my_life_tile_fg_text);
                var day = Int32.Parse(txt.Text);

                var intent = new Intent(BaseContext, typeof(DayDetailActivity));
                intent.PutExtra("dayOfMonth", day);
                StartActivity(intent);
            } catch (Exception ex) {
                throw ex;
            }
        }
	}
}

