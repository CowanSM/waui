
using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Jjoe64.Graphview;

namespace wave_android_uitest
{
    [Activity (Label = "MyLifeActivity", ScreenOrientation=Android.Content.PM.ScreenOrientation.Portrait)]			
	public class MyLifeActivity : SubPrimaryActivity
	{
        private bool _opponent = false;
        private bool _calToggle = true;

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

            var pparent = FindViewById<ViewGroup>(Resource.Id.my_life_calendar_switcher);
            ((RelativeLayout.LayoutParams)pparent.LayoutParameters).AddRule(LayoutRules.Above, Resource.Id.activity_tab_bar);

            // setup stats toggle button
            var btn = FindViewById<ImageView>(Resource.Id.my_life_calendar_toggle);
            btn.Clickable = true;
            btn.Click += (sender, e) => {
                if (_calToggle) {
                    // we are currently viewing the calendar, switch to stats
                    btn.SetImageResource(Resource.Drawable.StatsButton);
                    FindViewById<ViewSwitcher>(Resource.Id.my_life_calendar_switcher).ShowNext();
                } else {
                    // we are currently viewing stats, switch to the calendar
                    btn.SetImageResource(Resource.Drawable.StatsIcon_Selected);
                    FindViewById<ViewSwitcher>(Resource.Id.my_life_calendar_switcher).ShowPrevious();
                }
                _calToggle = !_calToggle;
            };

            // setup the graph view
            GraphingView.GraphViewSeries series = new GraphingView.GraphViewSeries(new GraphingView.GraphView.GraphViewData[]
                {
                        new GraphingView.GraphView.GraphViewData(1,2000),
                        new GraphingView.GraphView.GraphViewData(2,1860),
                        new GraphingView.GraphView.GraphViewData(3,1750),
                        new GraphingView.GraphView.GraphViewData(4,500),
                        new GraphingView.GraphView.GraphViewData(5,1260),
                        new GraphingView.GraphView.GraphViewData(6,1500),
                        new GraphingView.GraphView.GraphViewData(7,2100),
                        new GraphingView.GraphView.GraphViewData(8,1600),
                        new GraphingView.GraphView.GraphViewData(9,1000),
                        new GraphingView.GraphView.GraphViewData(10,1223),
                        new GraphingView.GraphView.GraphViewData(11,1432),
                        new GraphingView.GraphView.GraphViewData(12,1333),
                        new GraphingView.GraphView.GraphViewData(13,540)
                });
            GraphingView.GraphView gView = new GraphingView.LineGraphView(this, "");
            gView.SetManualYAxisBounds(2200, 0);
            gView.SetViewPort(1, 31);
            gView.SetHorizontalLabels(new string[] { "1", "", "", "", "", "", "", "", "", "", "", "", "today", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "31" });
            gView.AddSeries(series);
            var pgView = FindViewById<LinearLayout>(Resource.Id.my_life_stats_graph_parent);
            pgView.AddView(gView);
            pgView.SetBackgroundColor(Android.Graphics.Color.DimGray);
//            ((RelativeLayout.LayoutParams)gView.LayoutParameters).AddRule(LayoutRules.Below, Resource.Id.mylifestats

//            ((RelativeLayout.LayoutParams)gView.LayoutParameters).AddRule(LayoutRules.Above, Resource.Id.my_life_stats_button_layout);
//            ((RelativeLayout.LayoutParams)gView.LayoutParameters).AddRule(LayoutRules.Below, Resource.Id.my_life_stats_header);

            var statsHeader = FindViewById<LinearLayout>(Resource.Id.my_life_stats_month_bar);
            statsHeader.SetBackgroundColor(Android.Graphics.Color.DimGray);

            // setup fonts
            var gbook = Android.Graphics.Typeface.CreateFromAsset(Assets, "fonts/GOTHAM_BOOK.TTF");

            if (_opponent) {
                FindViewById<TextView>(Resource.Id.my_life_opponent_challenge_text).SetTypeface(gbook, Android.Graphics.TypefaceStyle.Normal);
                FindViewById<TextView>(Resource.Id.my_life_opponent_remove_txt).SetTypeface(gbook, Android.Graphics.TypefaceStyle.Normal);
            } else {
                FindViewById<RelativeLayout>(Resource.Id.my_life_opponent_layout).Visibility = ViewStates.Invisible;
                ((RelativeLayout.LayoutParams)FindViewById<ViewGroup>(Resource.Id.my_life_calendar_switcher).LayoutParameters).AddRule(LayoutRules.Below, Resource.Id.my_life_header_frame);
            }

            // actions for the stats clickables
            var stbtn = FindViewById<LinearLayout>(Resource.Id.my_life_stats_steps_layout);
            stbtn.SetBackgroundColor(Android.Graphics.Color.Argb(75, 75, 75, 75));
            var mibtn = FindViewById<LinearLayout>(Resource.Id.my_life_stats_miles_layout);
            var calbtn = FindViewById<LinearLayout>(Resource.Id.my_life_stats_calories_layout);
            stbtn.Click += (sender, e) => {
                // set other buttons to transparent background
                stbtn.SetBackgroundColor(Android.Graphics.Color.Argb(75, 75, 75, 75));
                mibtn.SetBackgroundColor(Android.Graphics.Color.Transparent);
                calbtn.SetBackgroundColor(Android.Graphics.Color.Transparent);
                // show the arrow
                var arrow = stbtn.FindViewById<ImageView>(Resource.Id.my_life_stats_steps_arrow);
                arrow.Visibility = ViewStates.Visible;
                // hide the others
                arrow = mibtn.FindViewById<ImageView>(Resource.Id.my_life_stats_miles_arrow);
                arrow.Visibility = ViewStates.Invisible;
                arrow = calbtn.FindViewById<ImageView>(Resource.Id.my_life_stats_cals_arrow);
                arrow.Visibility = ViewStates.Invisible;
            };
            mibtn.Click += (sender, e) => {
                mibtn.SetBackgroundColor(Android.Graphics.Color.Argb(75, 75, 75, 75));
                stbtn.SetBackgroundColor(Android.Graphics.Color.Transparent);
                calbtn.SetBackgroundColor(Android.Graphics.Color.Transparent);
                // show the arrow
                var arrow = mibtn.FindViewById<ImageView>(Resource.Id.my_life_stats_miles_arrow);
                arrow.Visibility = ViewStates.Visible;
                // hide the others
                arrow = stbtn.FindViewById<ImageView>(Resource.Id.my_life_stats_steps_arrow);
                arrow.Visibility = ViewStates.Invisible;
                arrow = calbtn.FindViewById<ImageView>(Resource.Id.my_life_stats_cals_arrow);
                arrow.Visibility = ViewStates.Invisible;
            };
            calbtn.Click += (sender, e) => {
                calbtn.SetBackgroundColor(Android.Graphics.Color.Argb(75, 75, 75, 75));
                mibtn.SetBackgroundColor(Android.Graphics.Color.Transparent);
                stbtn.SetBackgroundColor(Android.Graphics.Color.Transparent);
                // show the arrow
                var arrow = calbtn.FindViewById<ImageView>(Resource.Id.my_life_stats_cals_arrow);
                arrow.Visibility = ViewStates.Visible;
                // hide the others
                arrow = mibtn.FindViewById<ImageView>(Resource.Id.my_life_stats_miles_arrow);
                arrow.Visibility = ViewStates.Invisible;
                arrow = stbtn.FindViewById<ImageView>(Resource.Id.my_life_stats_steps_arrow);
                arrow.Visibility = ViewStates.Invisible;
            };

            ActionBar.Hide();
		}

        public void HandleMonthChanged (string obj) {
            FindViewById<TextView>(Resource.Id.my_life_month_text).Text = obj;
        }

        private void HandleOnClick(View view) {
            try {
                var txt = view.FindViewById<TextView>(Resource.Id.my_life_tile_fg_text);
                var day = txt.Text;
                txt = FindViewById<TextView>(Resource.Id.my_life_month_text);
                var info = day + " - " + txt.Text;

                var intent = new Intent(BaseContext, typeof(DayDetailActivity));
                intent.PutExtra("dateText", info);
                StartActivity(intent);
            } catch (Exception ex) {
                throw ex;
            }
        }
	}
}

