
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
	[Activity (Label = "PeopleActivity")]			
	public class PeopleActivity : SubPrimaryActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
            SetContentView(Resource.Layout.people);

            // tab bar
            Android.App.FragmentTransaction transaction = FragmentManager.BeginTransaction ();
            TabBarFragment fragment = new TabBarFragment (this);
            transaction.Add (Resource.Id.people_root, fragment);
            transaction.Commit();

			// Create your application here
            // add friend requests to the view below
            var requestView = FindViewById<LinearLayout>(Resource.Id.people_requests);
            var request1 = LayoutInflater.Inflate(Resource.Layout.user_row, requestView, false);
            request1.FindViewById<TextView>(Resource.Id.user_row_name).Text = "Oscar Bluth";
            request1.FindViewById<TextView>(Resource.Id.user_row_xp_text).Text = "4";
            request1.FindViewById<View>(Resource.Id.user_row_buttons).Visibility = ViewStates.Visible;
            requestView.AddView(request1);

            var request2 = LayoutInflater.Inflate(Resource.Layout.user_row, requestView, false);
            request2.FindViewById<TextView>(Resource.Id.user_row_name).Text = "Carl Weathers";
            request2.FindViewById<TextView>(Resource.Id.user_row_xp_text).Text = "8";
            request2.FindViewById<View>(Resource.Id.user_row_buttons).Visibility = ViewStates.Visible;
            requestView.AddView(request2);

            // listview should be set to above tab bar
            // add list adapter which will have a list of all friends (opponents)
            var listview = FindViewById<ListView>(Resource.Id.people_friends_list);
            listview.Adapter = new OpponentListAdapter(this, new List<OpponentListAdapter.Opponent>() {
                new OpponentListAdapter.Opponent() {
                    Name = "Lindsay Bluth",
                    Level = 17,
                    Selectable = true
                },
                new OpponentListAdapter.Opponent() {
                    Name = "J. Walter Weatherman",
                    Level = 21,
                    Selectable = true
                },
                new OpponentListAdapter.Opponent() {
                    Name = "Barry Zuckercorn",
                    Level = 11,
                    Selectable = true
                },
                new OpponentListAdapter.Opponent() {
                    Name = "Gene Parmesan",
                    Level = 2
                },
                new OpponentListAdapter.Opponent() {
                    Name = "Judge Reinholt",
                    Level = 13,
                    Selectable = true
                },
                new OpponentListAdapter.Opponent() {
                    Name = "Stanley Sitwell",
                    Level = 1,
                    Selectable = true
                },
                new OpponentListAdapter.Opponent() {
                    Name = "George Bluth",
                    Level = 7,
                    Selectable = true
                },
                new OpponentListAdapter.Opponent() {
                    Name = "Tobias Funke",
                    Level = 1,
                    Selectable = true
                }
            }, ClickOnUser);

            var friends = FindViewById<View>(Resource.Id.people_friends);
            ((RelativeLayout.LayoutParams)friends.LayoutParameters).AddRule(LayoutRules.Above, Resource.Id.activity_tab_bar);

            // setup request button
            var req = FindViewById<TextView>(Resource.Id.people_header_request);
            req.Click += (sender, e) => {
                var intent = new Intent(BaseContext, typeof(FriendRequest));
                StartActivity(intent);
            };

            // font(s)
            var gbold = Android.Graphics.Typeface.CreateFromAsset(Assets, "fonts/GOTHAM_BOLD.TTF");
            var gbook = Android.Graphics.Typeface.CreateFromAsset(Assets, "fonts/GOTHAM_BOOK.TTF");
            var gmed = Android.Graphics.Typeface.CreateFromAsset(Assets, "fonts/GOTHAM_MEDIUM.TTF");
            var segsb = Android.Graphics.Typeface.CreateFromAsset(Assets, "fonts/seguisb.ttf");

            FindViewById<TextView>(Resource.Id.people_header_title).SetTypeface(gbold, Android.Graphics.TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.people_header_request).SetTypeface(gbook, Android.Graphics.TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.people_tabs_friends).SetTypeface(gmed, Android.Graphics.TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.people_tabs_top_performers).SetTypeface(gmed, Android.Graphics.TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.people_friends_title).SetTypeface(segsb, Android.Graphics.TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.people_requests_title).SetTypeface(segsb, Android.Graphics.TypefaceStyle.Normal);

            ActionBar.Hide();
		}

        public void ClickOnUser(object sender, EventArgs args) {
            if (sender is View) {
                var view = (View)sender;
                var name = view.FindViewById<TextView>(Resource.Id.user_row_name);
                if (name != null) {
                    var intent = new Intent(BaseContext, typeof(OpponentLifeActivity));
                    intent.PutExtra("name", name.Text);
                    StartActivity(intent);
                }
            }
        }
	}
}

