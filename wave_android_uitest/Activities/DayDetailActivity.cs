
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
    [Activity(Label = "DayDetailActivity")]			
    public class DayDetailActivity : SubPrimaryActivity {
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.day_detail);

            // tab bar
            Android.App.FragmentTransaction transaction = FragmentManager.BeginTransaction ();
            TabBarFragment fragment = new TabBarFragment (this);
            transaction.Add (Resource.Id.day_detail_txt_layout, fragment);
            transaction.Commit ();

            // setup the fonts
            var glight = Android.Graphics.Typeface.CreateFromAsset(Assets, "fonts/GOTHAM_LIGHT.TTF");
            var gmed = Android.Graphics.Typeface.CreateFromAsset(Assets, "fonts/GOTHAM_MEDIUM.TTF");
            var gthin = Android.Graphics.Typeface.CreateFromAsset(Assets, "fonts/GOTHAM_THIN.TTF");

            FindViewById<TextView>(Resource.Id.day_detail_step_count).SetTypeface(gthin, Android.Graphics.TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.day_detail_step_label).SetTypeface(gmed, Android.Graphics.TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.day_detail_cal_count).SetTypeface(glight, Android.Graphics.TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.day_detail_cal_label).SetTypeface(gmed, Android.Graphics.TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.day_detail_mi_count).SetTypeface(glight, Android.Graphics.TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.day_detail_mi_label).SetTypeface(gmed, Android.Graphics.TypefaceStyle.Normal);

            // tab bar alignment
            var txt = FindViewById<TextView>(Resource.Id.day_detail_step_count);
            ((RelativeLayout.LayoutParams)txt.LayoutParameters).AddRule(LayoutRules.Above, Resource.Id.activity_tab_bar);
            txt = FindViewById<TextView>(Resource.Id.day_detail_step_label);
            ((RelativeLayout.LayoutParams)txt.LayoutParameters).AddRule(LayoutRules.Above, Resource.Id.activity_tab_bar);
        }
    }
}

