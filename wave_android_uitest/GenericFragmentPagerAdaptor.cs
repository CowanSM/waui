using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Support.V4.View;
using Android.Support.V4.App;

namespace wave_android_uitest
{
	public class GenericFragmentPagerAdaptor : FragmentPagerAdapter
	{
		private List<Android.Support.V4.App.Fragment> _fragmentList = new List<Android.Support.V4.App.Fragment>();
		public GenericFragmentPagerAdaptor(Android.Support.V4.App.FragmentManager fm)
			: base(fm) {}

		public override int Count
		{
			get { return _fragmentList.Count; }
		}

		public override Android.Support.V4.App.Fragment GetItem(int position)
		{
			return _fragmentList[position];
		}

		public void AddFragment(GenericViewPagerFragment fragment)
		{
			_fragmentList.Add(fragment);
		}

		public void AddFragmentView(Func<LayoutInflater, ViewGroup, Bundle, View> view)
		{
			_fragmentList.Add(new GenericViewPagerFragment(view));
		}
	}
	public class ViewPageListenerForActionBar : ViewPager.SimpleOnPageChangeListener
	{
		private ActionBar _bar;
		private Android.Widget.RadioGroup _rGroup;
		private List<int> _rButtonIds;
		public ViewPageListenerForActionBar(ActionBar bar, Android.Widget.RadioGroup group)
		{
			_bar = bar;
			_rGroup = group;

			_rButtonIds = new List<int> (_rGroup.ChildCount);

			for (int i = 0; i < _rGroup.ChildCount; i++) {
				var child = _rGroup.GetChildAt (i);
				_rButtonIds.Add (child.Id);
			}
		}
		public override void OnPageSelected(int position)
		{
			if (_bar.NavigationMode == ActionBarNavigationMode.Tabs) {
				_bar.SetSelectedNavigationItem (position);
			}

			if (_rGroup != null && _rButtonIds.Count > position) {
				_rGroup.Check (_rButtonIds [position]);
			}
		}
	}
	public static class ViewPagerExtensions
	{
		public static ActionBar.Tab GetViewPageTab(this ViewPager viewPager, ActionBar actionBar, string name)
		{
			var tab = actionBar.NewTab();
			tab.SetText(name);
			tab.TabSelected += (o, e) =>
			{
				viewPager.SetCurrentItem(actionBar.SelectedNavigationIndex, false);
			};
			return tab;
		}
	}

}