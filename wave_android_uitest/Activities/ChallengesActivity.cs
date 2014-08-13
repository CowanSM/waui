
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
	[Activity (Label = "ChallengesActivity")]			
	public class ChallengesActivity : SubPrimaryActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
            SetContentView(Resource.Layout.challenges);

			// Create your application here
            // tab bar
            Android.App.FragmentTransaction transaction = FragmentManager.BeginTransaction ();
            TabBarFragment fragment = new TabBarFragment (this);
            transaction.Add (Resource.Id.challenges_root, fragment);
            transaction.Commit();

            // create button
            var btn = FindViewById<TextView>(Resource.Id.challenges_header_create);
            btn.Click += (sender, e) => {
                StartActivity(new Intent(BaseContext, typeof(NewChallengeActivity)));
            };

            var pager = FindViewById<Android.Support.V4.View.ViewPager>(Resource.Id.challenges_pager);
            ((RelativeLayout.LayoutParams)pager.LayoutParameters).AddRule(LayoutRules.Above, Resource.Id.activity_tab_bar);

            // create pages
            pager.Adapter = new ChallengesPagerAdapter(SupportFragmentManager, new List<ChallengesPagerView>()
                {
                        new ChallengesPagerView(this, new ChallengesListAdapter(this, new List<ChallengesListAdapter.Item>() {
                            new ChallengesListAdapter.Item() {
                                Name = "Oscar Bluth",
                                Bet = "Wear underwear outside pants for one day.",
                                Goal = "Steps in 2 hours",
                                Time = "45m"
                            },
                            new ChallengesListAdapter.Item() {
                                Name = "Carl Weathers",
                                Bet = "$20",
                                Goal = "Distance in 1 week.",
                                Time = "1d 50m"
                            }
                        })),
                        new ChallengesPagerView(this, new ChallengesListAdapter(this, new List<ChallengesListAdapter.Item>() {
                            new ChallengesListAdapter.Item() {
                                Name = "Oscar Bluth",
                                Bet = "Wear underwear outside pants for one day.",
                                Goal = "Steps in 2 hours",
                                Time = "45m",
                                ShowButtons = false
                            },
                            new ChallengesListAdapter.Item() {
                                Name = "Carl Weathers",
                                Bet = "$20",
                                Goal = "Distance in 1 week.",
                                Time = "1d 50m",
                                ShowButtons = false
                            },
                            new ChallengesListAdapter.Item() {
                                Name = "Bob Loblaw",
                                Bet = "Loser buys us both tickets to Coachella",
                                Goal = "Calories in 1 day",
                                Time = "2d 21m",
                                ShowButtons = false
                            }
                        })),
                        new ChallengesPagerView(this, new ChallengesListAdapter(this, new List<ChallengesListAdapter.Item>() {
                            new ChallengesListAdapter.Item() {
                                Name = "Oscar Bluth",
                                Bet = "Wear underwear outside pants for one day.",
                                Goal = "Steps in 2 hours",
                                Time = "45m",
                                ShowButtons = false
                            },
                            new ChallengesListAdapter.Item() {
                                Name = "Carl Weathers",
                                Bet = "$20",
                                Goal = "Distance in 1 week.",
                                Time = "1d 50m",
                                ShowButtons = false
                            },
                            new ChallengesListAdapter.Item() {
                                Name = "Bob Loblaw",
                                Bet = "Loser buys us both tickets to Coachella",
                                Goal = "Calories in 1 day",
                                Time = "2d 21m",
                                ShowButtons = false
                            },
                            new ChallengesListAdapter.Item() {
                                Name = "Bob Loblaw",
                                Bet = "Loser buys us both tickets to Coachella",
                                Goal = "Calories in 1 day",
                                Time = "2d 21m",
                                ShowButtons = false
                            },
                            new ChallengesListAdapter.Item() {
                                Name = "Bob Loblaw",
                                Bet = "Loser buys us both tickets to Coachella",
                                Goal = "Calories in 1 day",
                                Time = "2d 21m",
                                ShowButtons = false
                            },
                            new ChallengesListAdapter.Item() {
                                Name = "Bob Loblaw",
                                Bet = "Loser buys us both tickets to Coachella",
                                Goal = "Calories in 1 day",
                                Time = "2d 21m",
                                ShowButtons = false
                            },
                            new ChallengesListAdapter.Item() {
                                Name = "Bob Loblaw",
                                Bet = "Loser buys us both tickets to Coachella",
                                Goal = "Calories in 1 day",
                                Time = "2d 21m",
                                ShowButtons = false
                            }
                        }))
                });

            pager.SetOnPageChangeListener(new ChallengesPagerChangeListener(new TextView[]
                    {
                        FindViewById<TextView>(Resource.Id.challenges_tab_new_txt),
                        FindViewById<TextView>(Resource.Id.challenges_tab_active_txt),
                        FindViewById<TextView>(Resource.Id.challenges_tab_completed_txt)
                    }));

            // setup fonts
            var gmed = Android.Graphics.Typeface.CreateFromAsset(Assets, "fonts/GOTHAM_MEDIUM.TTF");
            var gbold = Android.Graphics.Typeface.CreateFromAsset(Assets, "fonts/GOTHAM_BOLD.TTF");
            var gbook = Android.Graphics.Typeface.CreateFromAsset(Assets, "fonts/GOTHAM_BOOK.TTF");

            FindViewById<TextView>(Resource.Id.challenges_header_create).SetTypeface(gbook, Android.Graphics.TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.challenges_title_text).SetTypeface(gbold, Android.Graphics.TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.challenges_tab_new_txt).SetTypeface(gmed, Android.Graphics.TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.challenges_tab_active_txt).SetTypeface(gmed, Android.Graphics.TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.challenges_tab_completed_txt).SetTypeface(gmed, Android.Graphics.TypefaceStyle.Normal);

            ActionBar.Hide();
		}
	}
}

