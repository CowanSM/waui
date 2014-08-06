using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace wave_android_uitest {
	public class ActivityListAdapter : ArrayAdapter {
		public struct Item {
            public int Id, ImageResLeft, ImageResRight;
            public string TopText, BottomText, TimeText;
            public Android.Text.SpannableString TopSequence;
		}

        private List<Item> _list;
        private int _layoutResId, _nextId;
        private LayoutInflater _inflater;

        public ActivityListAdapter (Context context, int layoutId, int textViewId)
            : base(context, layoutId, textViewId) {

            _list = new List<Item>();
            _inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            _nextId = 0;
            _layoutResId = layoutId;
		}

        public void AddItem(Item toAdd) {
            toAdd.Id = _nextId++;
            _list.Add(toAdd);
            // placeholder to add to actual list view, actual view will be overwritten when the view is drawn
            this.Add("");
        }

        public override View GetView(int position, View convertView, ViewGroup parent) {
            View view;

            if (convertView != null) {
                view = convertView;
            } else {
                view = _inflater.Inflate(_layoutResId, parent, false);
            }

            return BindData(view, position);
        }

        private View BindData(View view, int position) {
            var item = _list[position];

            var limage = view.FindViewById<ImageView>(Resource.Id.activity_limage);
            var rimage = view.FindViewById<ImageView>(Resource.Id.activity_rimage);
            var ttext = view.FindViewById<TextView>(Resource.Id.activity_text1);
            var btext = view.FindViewById<TextView>(Resource.Id.activity_text2);
            var timeText = view.FindViewById<TextView>(Resource.Id.activity_time);

            if (!string.IsNullOrEmpty(item.BottomText)) {
                btext.Text = item.BottomText;
            }

            if (!string.IsNullOrEmpty(item.TopText)) {
                ttext.Text = item.TopText;
            }

            if (item.TopSequence != null) {
                ttext.TextFormatted = item.TopSequence;
                ttext.MovementMethod = new Android.Text.Method.LinkMovementMethod();
            }

            if (!string.IsNullOrEmpty(item.TimeText)) {
                timeText.Text = item.TimeText;
            }

            return view;
        }
	}
}

