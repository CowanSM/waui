/* CalendarPagerAdapter and CalendarPagerView are adapted from the code at
 * https://github.com/dbrain/android-calendarview
 */

using System;
using System.Collections.Generic;

using Java.Util;
using Android.Views;
using Android.Widget;
using Android.Content;

namespace wave_android_uitest {
    public class CalendarItem : Java.Lang.Object{
        public int year, month, day;
        public string text;
        public long id;

        public CalendarItem(Calendar calendar)
            : this(calendar.Get(CalendarField.Year), calendar.Get(CalendarField.Month), calendar.Get(CalendarField.DayOfMonth)) {}

        public CalendarItem(int year, int month, int day) {
            this.year = year;
            this.month = month;
            this.day = day;
            this.text = day.ToString();
            this.id = Int64.Parse(year + "" + month + "" + day);
        }

        public override bool Equals(Java.Lang.Object o) {
            CalendarItem cv = (CalendarItem)o;

            if (cv != null) {
                return cv.day == this.day && cv.month == this.month && cv.year == this.year;
            }

            return false;
        }

        public override bool Equals(object obj) {
            var cv = obj as CalendarItem;

            if (cv != null) {
                return cv.day == this.day && cv.month == this.month && cv.year == this.year;
            }

            return false;
        }
    }

    public class CalendarPagerAdapter : Android.Widget.BaseAdapter {
        private const int FIRSTDAYOFWEEK = Calendar.Monday;
        private Calendar _calendar;
        private LayoutInflater _inflater;
        private CalendarItem _today, _selected;
        private List<CalendarItem> _list;

        public event Action<View> OnClick;

        public class TileClickListener : Java.Lang.Object, Android.Views.View.IOnClickListener {
            private CalendarPagerAdapter _parent;

            public TileClickListener(CalendarPagerAdapter parent) {
                _parent = parent;
            }

            public void OnClick(View v) {
                if (_parent != null && _parent.OnClick != null) {
                    _parent.OnClick(v);
                }
            }
        }

        public CalendarPagerAdapter(Context context, Calendar monthCalendar) {
            _calendar = monthCalendar;
            _today = new CalendarItem(monthCalendar);
            _selected = new CalendarItem(monthCalendar);
            _calendar.Set(CalendarField.DayOfMonth, 1);
            _inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
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
                return _list[position].id;
            }

            return -1;
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent) {
            View view;

            if (convertView != null) {
                view = convertView;
                view.Visibility = ViewStates.Visible;
            } else {
                view = _inflater.Inflate(Resource.Layout.my_life_tile, parent, false);
                //                view.SetOnClickListener(new TileClickListener(this));
                view.Click += (sender, e) => {
                    if (OnClick != null) {
                        OnClick(view);
                    }
                };
            }

            var txt = view.FindViewById<TextView>(Resource.Id.my_life_tile_fg_text);

            if (_list.Count <= position) {
                // null it out?
                txt.Text = "NULL";
            } else {
                var day = _list[position];
                if (day.year == 0) {
                    view.Visibility = ViewStates.Invisible;
                    view.Clickable = false;
                }
                else if (day.Equals(this._today)) {
                    txt.Text = "Today";
                    var img = view.FindViewById<ImageView>(Resource.Id.my_life_tile_fg_image);
                    img.SetImageResource(Resource.Drawable.dateBgWide);
                    view.Clickable = true;
                } else {
                    txt.Text = day.text;
                    var img = view.FindViewById<ImageView>(Resource.Id.my_life_tile_fg_image);
                    img.SetImageResource(Resource.Drawable.dateBgCircle);
                    view.Clickable = true;
                }
            }

            // need to fill in step count or something as well...

            return view;
        }

        public override int Count {
            get
            {
                return _list.Count;
            }
        }

        #endregion

        public void SetSelected(int year, int month, int day) {
            _selected.year = year;
            _selected.month = month;
            _selected.day = day;
            NotifyDataSetChanged();
        }

        public void RefreshDays() {
            int year = _calendar.Get(CalendarField.Year);
            int month = _calendar.Get(CalendarField.Month);
            int firstDay = _calendar.Get(CalendarField.DayOfWeek);
            int lastDay = _calendar.GetActualMaximum(CalendarField.DayOfMonth);
            int blankies;
            List<CalendarItem> days;

            if (firstDay == FIRSTDAYOFWEEK) {
                blankies = 0;
            } else if (firstDay < FIRSTDAYOFWEEK) {
                blankies = Calendar.Saturday - (FIRSTDAYOFWEEK - 1);
            } else {
                blankies = firstDay - FIRSTDAYOFWEEK;
            }

            days = new List<CalendarItem>(lastDay + blankies);
            for (int i = 0; i < days.Capacity; i++) {
                days.Add(new CalendarItem(0, 0, 0));
            }

            for (int day = 1, position = blankies; position < days.Capacity; position++) {
                days[position] = new CalendarItem(year, month, day++);
            }

            this._list = days;
            NotifyDataSetChanged();
        }

    }
}

