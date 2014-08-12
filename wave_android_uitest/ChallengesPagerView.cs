
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
    public class ChallengesPagerView : Android.Support.V4.App.Fragment {
        private IListAdapter _adapter;
        private Context _context;

        public ChallengesPagerView(Context context, IListAdapter adapter) {
            _adapter = adapter;
            _context = context;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            base.OnCreateView(inflater, container, savedInstanceState);
            // create a list view and return it
            var view = new ListView(_context);
            view.Adapter = _adapter;
            return view;
        }
    }

    public class ChallengesPagerChangeListener : Android.Support.V4.View.ViewPager.SimpleOnPageChangeListener {
        private TextView[] _tabResources;

        public ChallengesPagerChangeListener(TextView[] resIds) {
            _tabResources = resIds;
        }

        public override void OnPageSelected(int position) {
            if (position < _tabResources.Length) {
                for (int i = 0; i < _tabResources.Length; i++) {
                    if (i == position) {
                        _tabResources[i].SetTextColor(Android.Graphics.Color.White);
                    } else {
                        _tabResources[i].SetTextColor(Android.Graphics.Color.LightGray);
                    }
                }
            }
        }

    }
}

