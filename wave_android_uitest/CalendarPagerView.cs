/* CalendarPagerAdapter and CalendarPagerView are adapted from the code at
 * https://github.com/dbrain/android-calendarview
 */

using System;

using Java.Util;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.App;

namespace wave_android_uitest {
    public class SwipeGesture : Android.Views.GestureDetector.SimpleOnGestureListener {
        private int _swipeMinDistance, _swipeThresholdVel;
        // events for swipes
        public event Action<float> Next;
        public event Action<float> Previous;

        public SwipeGesture(Context context) {
            var viewConfig = ViewConfiguration.Get(context);
            _swipeMinDistance = viewConfig.ScaledTouchSlop;
            _swipeThresholdVel = viewConfig.ScaledMinimumFlingVelocity;
        }

        public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY) {
            if (e1 == null || e2 == null) return false; // early exit to avoid null refs

            var difx = Math.Abs(e1.GetX() - e2.GetX());
            var dify = Math.Abs(e1.GetY() - e2.GetY());
            if (dify > (difx * .95)) return false; // early exit if y movement is significant more than x

            if (e1.GetX() - e2.GetX() > _swipeMinDistance && Math.Abs(velocityX) > _swipeThresholdVel) {
                Next(Math.Abs(velocityX)/100f);
            } else if (e2.GetX() - e1.GetX() > _swipeMinDistance && Math.Abs(velocityX) > _swipeThresholdVel) {
                Previous(Math.Abs(velocityX)/100f);
            }

            return false;
        }
    }

    public class GridTouchListener : Java.Lang.Object, Android.Views.View.IOnTouchListener {
        private GestureDetector _detect;

        public GridTouchListener(GestureDetector detect) {
            _detect = detect;
        }

        public bool OnTouch(View v, MotionEvent e) {
            return _detect.OnTouchEvent(e);
        }
    }

    public class CalendarPagerView : Fragment {
        private const int DURATION = 1400;

        protected Calendar _calendar;
        private Calendar _today;
        private Java.Util.Locale _locale;
        private CalendarPagerAdapter _adapter;
//        private ViewSwitcher _switcher;
        private ViewFlipper _flipper;

        public event Action<string> MonthChanged;
        public event Action<View> OnClick;

        public CalendarPagerView(ViewFlipper view) {
            _locale = Java.Util.Locale.Default;
            _calendar = Calendar.GetInstance(_locale);
            _flipper = view;
            _today = Calendar.GetInstance(_locale);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState) {
            var layout = inflater.Inflate(Resource.Layout.my_life_page, container, false);
            var grid = layout.FindViewById<GridView>(Resource.Id.my_life_page_grid);

            // setup swiping
            var swipeGes = new SwipeGesture(Activity);
            swipeGes.Next += PreviousMonth;
            swipeGes.Previous += NextMonth;
            var swipeDet = new GestureDetector(Activity, swipeGes);

            _adapter = new CalendarPagerAdapter(Activity, _calendar);
            _adapter.OnClick += (view) => {
                if (OnClick != null) {
                    OnClick(view);
                }
            };
            UpdateCurrentMonth();

            grid.Adapter = _adapter;
            grid.SetOnTouchListener(new GridTouchListener(swipeDet));

            return layout;
        }

        public void NextMonth(float velocity) {
            // exit early if the next month is past current month
            if (_calendar.Get(CalendarField.Year) == _today.Get(CalendarField.Year) && _calendar.Get(CalendarField.Month) == _today.Get(CalendarField.Month)) {
                return;
            }

            _flipper.SetInAnimation(Activity, Resource.Animation.in_from_right);
            _flipper.SetOutAnimation(Activity, Resource.Animation.out_to_left);
            _flipper.InAnimation.Duration = (long)Math.Max((DURATION / velocity), 500f);
            _flipper.ShowNext();
            if (_calendar.Get(CalendarField.Month) == Calendar.December) {
                _calendar.Set((_calendar.Get(CalendarField.Year) + 1), Calendar.January, 1);
            } else {
                _calendar.Set(CalendarField.Month, _calendar.Get(CalendarField.Month) + 1);
            }
            UpdateCurrentMonth();
        }

        public void PreviousMonth(float velocity) {
            // exit early if we are at January 2000
            if (_calendar.Get(CalendarField.Year) == 2000 && _calendar.Get(CalendarField.Month) == Calendar.January) {
                return;
            }

            _flipper.SetInAnimation(Activity, Resource.Animation.in_from_left);
            _flipper.SetOutAnimation(Activity, Resource.Animation.out_to_right);
            _flipper.InAnimation.Duration = (long)Math.Max((DURATION / velocity), 500f);
            _flipper.ShowPrevious();
            if (_calendar.Get(CalendarField.Month) == Calendar.January) {
                _calendar.Set((_calendar.Get(CalendarField.Year) - 1), Calendar.December, 1);
            } else {
                _calendar.Set(CalendarField.Month, _calendar.Get(CalendarField.Month) - 1);
            }
            UpdateCurrentMonth();
        }

        void UpdateCurrentMonth() {
            _adapter.RefreshDays();
            var txt = Java.Lang.String.Format(_locale, "%tB", _calendar);
            if (_calendar.Get(CalendarField.Year) != Calendar.GetInstance(_locale).Get(CalendarField.Year))  {
                txt += " " + _calendar.Get(CalendarField.Year);
            }
            if (MonthChanged != null)
                MonthChanged(txt);
        }
    }
}

