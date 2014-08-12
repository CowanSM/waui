using System;
using System.Collections.Generic;

using Android.Content;
using Android.Widget;
using Android.Views;

namespace wave_android_uitest {
    public class ChallengesPagerAdapter : Android.Support.V4.App.FragmentPagerAdapter {
        private List<ChallengesPagerView> _views;

//        public ChallengesPagerAdapter(Android.Support.V4.App.FragmentManager fm)
//            : base(fm) {
//            _views = new List<ChallengesPagerView>();
//        }

        public ChallengesPagerAdapter(Android.Support.V4.App.FragmentManager fm, IEnumerable<ChallengesPagerView> initial)
            : base(fm) {
            _views = new List<ChallengesPagerView>(initial);
        }

        #region implemented abstract members of PagerAdapter

        public override int Count {
            get
            {
                return _views.Count;
            }
        }

        #endregion

        #region implemented abstract members of FragmentPagerAdapter

        public override Android.Support.V4.App.Fragment GetItem(int position) {
            if (position < _views.Count) {
                return (Android.Support.V4.App.Fragment)_views[position];
            }
            return null;
        }

        #endregion
    }

    public class ChallengesListAdapter : BaseAdapter {
        public class Item : Java.Lang.Object {
            private static long currentId = 0;

            public string Name, Goal, Bet, Time;
            public int UserImageRes;
            public Android.Graphics.Color CircleColor;
            public long Id;
            public bool ShowButtons;

            public Item() {
                UserImageRes = Resource.Drawable.Icon;
                CircleColor = Android.Graphics.Color.Transparent;
                Id = currentId++;
                ShowButtons = true;
            }
        }

        private List<Item> _list;
        private LayoutInflater _inflator;

        public ChallengesListAdapter(LayoutInflater inflator) {
            _list = new List<Item>();
            _inflator = inflator;
        }

        public ChallengesListAdapter(LayoutInflater inflator, IEnumerable<Item> initialList) {
            _inflator = inflator;
            _list = new List<Item>(initialList);
        }

        public void AddItem(Item toAdd) {
            _list.Add(toAdd);
            NotifyDataSetChanged();
        }

        public void RemoveAt(int index) {
            _list.RemoveAt(index);
            NotifyDataSetChanged();
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


        public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent) {
            View view;

            if (convertView != null) {
                view = convertView;
            } else {
                view = _inflator.Inflate(Resource.Layout.challenges_item, parent, false);
            }

            // set the data
            var item = _list[position];

            var txt = view.FindViewById<TextView>(Resource.Id.challenges_item_inner_name);
            txt.Text = item.Name;
            txt = view.FindViewById<TextView>(Resource.Id.challenges_item_inner_bet);
            txt.Text = item.Bet;
            txt = view.FindViewById<TextView>(Resource.Id.challenges_item_inner_middle_goal);
            txt.Text = item.Goal;
            txt = view.FindViewById<TextView>(Resource.Id.challenges_item_inner_middle_time);
            txt.Text = item.Time;

            var circle = view.FindViewById<ImageView>(Resource.Id.challenges_item_rimage);
            circle.SetBackgroundColor(item.CircleColor);

            var userimg = view.FindViewById<ImageView>(Resource.Id.challenges_item_limage);
            userimg.SetImageResource(item.UserImageRes);

            var xbtn = view.FindViewById<ImageView>(Resource.Id.challenges_item_inner_x);
            var chkbtn = view.FindViewById<ImageView>(Resource.Id.challenges_item_inner_check);

            if (item.ShowButtons) {
                xbtn.Visibility = ViewStates.Visible;
                xbtn.Clickable = true;
                chkbtn.Visibility = ViewStates.Visible;
                chkbtn.Clickable = true;
            } else {
                xbtn.Visibility = ViewStates.Invisible;
                xbtn.Clickable = false;
                chkbtn.Visibility = ViewStates.Invisible;
                chkbtn.Clickable = false;
            }

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

