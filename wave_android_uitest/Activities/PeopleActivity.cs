
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
            listview.Adapter = new OpponentListAdapter(LayoutInflater, new List<OpponentListAdapter.Opponent>() {
                new OpponentListAdapter.Opponent() {
                    Name = "Lindsay Bluth",
                    Level = 17
                },
                new OpponentListAdapter.Opponent() {
                    Name = "J. Walter Weatherman",
                    Level = 21
                },
                new OpponentListAdapter.Opponent() {
                    Name = "Barry Zuckercorn",
                    Level = 11
                },
                new OpponentListAdapter.Opponent() {
                    Name = "Gene Parmesan",
                    Level = 2
                },
                new OpponentListAdapter.Opponent() {
                    Name = "Judge Reinholt",
                    Level = 13
                },
                new OpponentListAdapter.Opponent() {
                    Name = "Stanley Sitwell",
                    Level = 1
                },
                new OpponentListAdapter.Opponent() {
                    Name = "George Bluth",
                    Level = 7
                },
                new OpponentListAdapter.Opponent() {
                    Name = "Tobias Funke",
                    Level = 1
                }
            });

            var friends = FindViewById<View>(Resource.Id.people_friends);
            ((RelativeLayout.LayoutParams)friends.LayoutParameters).AddRule(LayoutRules.Above, Resource.Id.activity_tab_bar);
		}
	}
}

