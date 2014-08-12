using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace wave_android_uitest {
    public class MyLifeListAdapter : ArrayAdapter {
        public struct Item {
            public int Id;
            public string Date, Count;
            public int BgDrawableRes;
            public Android.Graphics.Drawables.Drawable BgDrawable;
        }

        private Context _context;
        private LayoutInflater _inflator;
        private List<Item> _list;
        private Action<View> _dayOnClick;

        public MyLifeListAdapter(Context context, Action<View> onClick, int textResId = Resource.Id.my_life_tile_fg_text)
            : base(context, textResId) {
            _context = context;
            _inflator = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            _list = new List<Item>();
            _dayOnClick = onClick;
        }

        public void AddItem(Item toAdd) {
            _list.Add(toAdd);
            this.Add("");
        }

        public override View GetView(int position, View convertView, ViewGroup parent) {
            View view;

            if (convertView != null) {
                view = convertView;
            } else {
                view = _inflator.Inflate(Resource.Layout.my_life_tile, parent, false);
            }

            return BindData(view, position);
        }

        private View BindData(View view, int position) {
            var item = _list[position];

            var click = view.FindViewById<FrameLayout>(Resource.Id.my_life_tile_root);
            click.Click += (sender, e) => {
                if (_dayOnClick != null) {
                    _dayOnClick(click);
                }
            };

            var txt = view.FindViewById<TextView>(Resource.Id.my_life_tile_fg_text);
            txt.Text = item.Date;

            if (item.Date == "Today") {
                var img = view.FindViewById<ImageView>(Resource.Id.my_life_tile_fg_image);
                img.SetImageResource(Resource.Drawable.dateBgWide);
            }

            txt = view.FindViewById<TextView>(Resource.Id.my_life_tile_count_txt);
            txt.Text = item.Count;

            if (item.BgDrawable != null) {
                var img = view.FindViewById<ImageView>(Resource.Id.my_life_tile_bg_image);
                img.SetImageDrawable(item.BgDrawable);
            } else if (item.BgDrawableRes > 0) {
                var img = view.FindViewById<ImageView>(Resource.Id.my_life_tile_bg_image);
                img.SetImageResource(item.BgDrawableRes);
            }

            return view;
        }
    }
}

