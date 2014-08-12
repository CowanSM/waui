
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
    [Activity(Label = "NewChallengeActivity")]			
    public class NewChallengeActivity : SubPrimaryActivity {
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.new_challenge);

            // Create your application here
            // tab bar
            Android.App.FragmentTransaction transaction = FragmentManager.BeginTransaction ();
            TabBarFragment fragment = new TabBarFragment (this);
            transaction.Add (Resource.Id.new_challenge_root, fragment);
            transaction.Commit();

            var pager = FindViewById<Android.Support.V4.View.ViewPager>(Resource.Id.new_challenge_pager);
            ((RelativeLayout.LayoutParams)pager.LayoutParameters).AddRule(LayoutRules.Above, Resource.Id.activity_tab_bar);

            var adapter = new NewChallengePagerAdapter(SupportFragmentManager);
            adapter.AddFragment(new NewChallengePages.PageOne(this));
            adapter.AddFragment(new NewChallengePages.PageTwo(this));
            adapter.AddFragment(new NewChallengePages.PageThree(this));
            adapter.AddFragment(new NewChallengePages.PageFour(this));

            pager.Adapter = adapter;
        }
    }

    public class NewChallengePages {
        public class PageFour : Android.Support.V4.App.Fragment {
            private Context _context;

            public PageFour(Context context) {
                _context = context;
            }

            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
                var view = inflater.Inflate(Resource.Layout.new_challenge_page_four, container, false);

                // set style for edit text
                var txt = view.FindViewById<EditText>(Resource.Id.new_challenge_page_four_input);
                txt.ClearFocus();

                // finish button
                var btn = view.FindViewById<Button>(Resource.Id.new_challenge_page_four_finished);
                btn.Click += (sender, e) => {
                    Activity.Finish();
                };

                return view;
            }
        }

        public class PageThree : Android.Support.V4.App.Fragment {
            private Context _context;

            public PageThree(Context context) {
                _context = context;
            }

            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
                var view = inflater.Inflate(Resource.Layout.new_challenge_page_three, container, false);
                // add the pickers
                var timepicker = new TimePicker(_context);
                var parent = view.FindViewById<LinearLayout>(Resource.Id.new_challenge_page_three_time_parent);
                parent.AddView(timepicker);

                var datepicker = new DatePicker(_context);
                parent = view.FindViewById<LinearLayout>(Resource.Id.new_challenge_page_three_date_parent);
                parent.AddView(datepicker);

                return view;
            }
        }

        public class PageTwo : Android.Support.V4.App.Fragment {
//            public enum TrackType {
//                Steps, Miles, Calories
//            }
//
//            public enum TrackTime {
//                Minutes, Hours, Days, Weeks, Months
//            }

            private Context _context;
            private TextView _type, _time;


            public PageTwo(Context context) {
                _context = context;
            }

            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
                var view = inflater.Inflate(Resource.Layout.new_challenge_page_two, container, false);
                // setup the buttons
                view.FindViewById<TextView>(Resource.Id.new_challenge_page_two_top_steps).Click += TrackButton;
                view.FindViewById<TextView>(Resource.Id.new_challenge_page_two_top_miles).Click += TrackButton;
                view.FindViewById<TextView>(Resource.Id.new_challenge_page_two_top_calories).Click += TrackButton;
                view.FindViewById<TextView>(Resource.Id.new_challenge_page_two_bottom_days).Click += TimeButton;
                view.FindViewById<TextView>(Resource.Id.new_challenge_page_two_bottom_hours).Click += TimeButton;
                view.FindViewById<TextView>(Resource.Id.new_challenge_page_two_bottom_minutes).Click += TimeButton;
                view.FindViewById<TextView>(Resource.Id.new_challenge_page_two_bottom_months).Click += TimeButton;
                view.FindViewById<TextView>(Resource.Id.new_challenge_page_two_bottom_weeks).Click += TimeButton;


                // add number picker
                var parent = view.FindViewById<LinearLayout>(Resource.Id.new_challenge_page_two_number_parent);

                var picker1 = new NumberPicker(_context);
                picker1.MinValue = 0;
                picker1.MaxValue = 9;
                parent.AddView(picker1);
                var picker2 = new NumberPicker(_context);
                picker2.MinValue = 0;
                picker2.MaxValue = 9;
                parent.AddView(picker2);
                var picker3 = new NumberPicker(_context);
                picker3.MinValue = 0;
                picker3.MaxValue = 9;
                picker3.Value = 1;
                parent.AddView(picker3);

                return view;
            }

            private void TrackButton(object sender, EventArgs e) {
                if (sender is TextView) {
                    if (_type != null) {
                        _type.SetBackgroundColor(Android.Graphics.Color.Transparent);
                    }
                    _type = sender as TextView;
                    _type.SetBackgroundColor(Android.Graphics.Color.Red);
                }
            }

            private void TimeButton(object sender, EventArgs e) {
                if (sender is TextView) {
                    if (_time != null) {
                        _time.SetBackgroundColor(Android.Graphics.Color.Transparent);
                    }
                    _time = sender as TextView;
                    _time.SetBackgroundColor(Android.Graphics.Color.Red);
                }
            }
        }

        public class PageOne : Android.Support.V4.App.Fragment {
            private Context _context;

            public PageOne(Context context) {
                _context = context;
            }

            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
                var view = new ListView(_context);
                view.Adapter = new OpponentListAdapter(inflater, new List<OpponentListAdapter.Opponent>() {
                    new OpponentListAdapter.Opponent() {
                        Name = "Oscar Bluth",
                        Level = 4
                    },
                    new OpponentListAdapter.Opponent() {
                        Name = "Carl Weathers",
                        Level = 8
                    },
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

                return view;
            }
        }
    }

    public class NewChallengePagerAdapter : Android.Support.V4.App.FragmentPagerAdapter {
        private List<Android.Support.V4.App.Fragment> _list;

        public NewChallengePagerAdapter(Android.Support.V4.App.FragmentManager fm)
            : base(fm) {
            _list = new List<Android.Support.V4.App.Fragment>();
        }

        public void AddFragment(Android.Support.V4.App.Fragment fragment) {
            _list.Add(fragment);
        }

        #region implemented abstract members of PagerAdapter

        public override int Count {
            get
            {
                return _list.Count;
            }
        }

        #endregion

        #region implemented abstract members of FragmentPagerAdapter

        public override Android.Support.V4.App.Fragment GetItem(int position) {
            if (position < _list.Count) {
                return _list[position];
            }
            return null;
        }

        #endregion


    }

    // list adapter for first page
    public class OpponentListAdapter : BaseAdapter {
        // temporary list, will want to actually base it off of the list of user contacts
        List<Opponent> _list;
        LayoutInflater _inflator;

        public class Opponent : Java.Lang.Object {
            public int Level, UserImageRes;
            public string Name;
            public long Id { get; private set; }
            private static int _currentId = 0;

            public Opponent() {
                Id = _currentId++;
                UserImageRes = Resource.Drawable.Icon;
            }
        }

        public OpponentListAdapter(LayoutInflater inflater, IEnumerable<Opponent> initial) {
            _list = new List<Opponent>(initial);
            _inflator = inflater;
        }

        #region implemented abstract members of BaseAdapter

        public override Java.Lang.Object GetItem(int position) {
            if (position < _list.Count) {
                return (Java.Lang.Object)_list[position];
            }
            return null;
        }

        public override long GetItemId(int position) {
            if (position < _list.Count) {
                return _list[position].Id;
            }
            return -1;
        }

        public override View GetView(int position, View convertView, ViewGroup parent) {
            View view;

            if (convertView != null) {
                view = convertView;
            } else {
                view = _inflator.Inflate(Resource.Layout.user_row, parent, false);
                // setup fonts and the like
            }

            var user = _list[position];

            // setup the row
            var txt = view.FindViewById<TextView>(Resource.Id.user_row_xp_text);
            txt.Text = user.Level.ToString();
            txt = view.FindViewById<TextView>(Resource.Id.user_row_name);
            txt.Text = user.Name;

            var img = view.FindViewById<ImageView>(Resource.Id.user_row_image);
            img.SetImageResource(user.UserImageRes);

            return view;
        }

        public override int Count {
            get
            {
                return _list.Count;
            }
        }

        #endregion


    }
}

