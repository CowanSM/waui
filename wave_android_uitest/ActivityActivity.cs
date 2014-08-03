
using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.Support.V4.App;

namespace wave_android_uitest {
	[System.Flags]
	public enum FilterOptions : int {
		NONE = 0x0,
		NewChallenges = 0x1,
		ChallengeResults = 0x2,
		SyncedProgress = 0x4,
		FriendRequests = 0x8,
		ALL = 0xF
	}

	public class OptionListAdapter : ArrayAdapter {
		private LayoutInflater _inflator;
		private List<Item> _items;

		public class Item {
			public struct TxtSize {
				public float Size;
				public Android.Util.ComplexUnitType UnitType;
			}

			public string Text;
			public int LayoutId, TextId, CBoxId, BtnId, CBoxDrawable, BtnDrawable;
			public Action<CheckBox, bool> CBoxChecked;
			public Action<View> BtnClicked;
			public Action<View> ParentClicked;
			public bool Checked, AllCaps;
			public Android.Graphics.Color TextColor;
			public TxtSize TextSize;
			public GravityFlags TextGravity;

			public Item() {
				AllCaps = false;
				TextColor = Android.Graphics.Color.Aqua;
				TextGravity = GravityFlags.NoGravity;
			}
		}

		public class BtnClickListener : Java.Lang.Object, CompoundButton.IOnClickListener {
			Action<View> ClickAction;

			public BtnClickListener(Action<View> clickAction) {
				ClickAction = clickAction;
			}

			#region IOnClickListener implementation
			public void OnClick (View v)
			{
				if (ClickAction != null) {
					ClickAction (v);
				}
			}
			#endregion
		}

		public class CBoxCheckListener : Java.Lang.Object, CompoundButton.IOnCheckedChangeListener {
			Action<CheckBox, bool> CheckAction;

			public CBoxCheckListener(Action<CheckBox, bool> checkAction) {
				CheckAction = checkAction;
			}
			#region IOnCheckedChangeListener implementation

			public void OnCheckedChanged (CompoundButton buttonView, bool isChecked)
			{
				if (CheckAction != null) {
					CheckAction ((CheckBox)buttonView, isChecked);
				}
			}

			#endregion
		}

		public OptionListAdapter(Context context, LayoutInflater inflator)
			: base(context, 0) {
			_items = new List<Item> ();
			_inflator = inflator;
		}

		public void AddItem(Item toAdd) {
			_items.Add (toAdd);
			this.Add ("");	// stupid placeholder to appease the layout gods
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var item = _items [position];

			if (convertView != null) {
				convertView = _inflator.Inflate (item.LayoutId, parent, false);
				return BindData (convertView, position);
			} else {
				var view = _inflator.Inflate (item.LayoutId, parent, false);
				return BindData (view, position);
			}
		}

		private View BindData(View view, int position) {
			var item = _items [position];

			using (var txt = view.FindViewById<TextView> (item.TextId)) {
				if (txt != null) {
					txt.Text = item.Text;
					txt.SetAllCaps (item.AllCaps);
					if (item.TextColor != Android.Graphics.Color.Aqua) {
						txt.SetTextColor (item.TextColor);
					}
					if (item.TextSize.Size > 0) {
						txt.SetTextSize (item.TextSize.UnitType, item.TextSize.Size);
					}
					if (item.TextGravity != GravityFlags.NoGravity) {
						txt.Gravity = item.TextGravity;
					}
				}
			}

			using (var cb = view.FindViewById<CheckBox> (item.CBoxId)) {
				if (cb != null) {
					if (item.CBoxChecked != null) {
						cb.SetOnCheckedChangeListener (new CBoxCheckListener (item.CBoxChecked));
					}

					if (item.CBoxDrawable > 0) {
						cb.SetButtonDrawable (item.CBoxDrawable);
					}

					cb.Checked = item.Checked;
				}
			}

			using (var btn = view.FindViewById<Button> (item.BtnId)) {
				if (btn != null) {
					if (item.BtnClicked != null) {
						btn.SetOnClickListener (new BtnClickListener (item.BtnClicked));
					}

					if (item.BtnDrawable > 0) {
						btn.SetBackgroundResource (item.BtnDrawable);
					}
				}
			}

			if (item.ParentClicked != null) {
				view.SetOnClickListener (new BtnClickListener (item.ParentClicked));
			}

			return view;
		}
	}

						/*
	public class OptionCheckBoxAdapter : ArrayAdapter {
		private int _layout;
		private LayoutInflater _inflater;
		private List<Item> _items;

		private struct Item {
			public string text;
			public bool divider;
			public Action<bool> ClickEvent;
		}

		public OptionCheckBoxAdapter(Context context, int layoutRes, LayoutInflater inflater) :
		base (context, 0) {
			_layout = layoutRes;
			_inflater = inflater;
			_items = new List<Item> ();

		}

		public void AddOption(string text, Action<bool> clickEvent, bool divider = false) {
			var item = new Item () {
				text = text,
				divider = divider,
				ClickEvent = clickEvent
			};

			_items.Add (item);

			this.Add ("");
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			View view;
			if (convertView != null) {
				view = convertView;
			} else {
				view = _inflater.Inflate (_layout, parent, false);
			}

			return BindData (view, position);
		}

		private View BindData (View view, int position) {
			var item = _items [position];
			var cb = view.FindViewById<CheckBox> (Resource.Id.option_cb);
			cb.Focusable = false;
			cb.FocusableInTouchMode = false;

			if (item.divider) {
				cb.SetButtonDrawable (Resource.Drawable.transparent);
				cb.SetOnCheckedChangeListener (null);
			} else {
				cb.SetButtonDrawable (Android.Resource.Drawable.CheckboxOffBackground);
				// handle clicking the checkbox
				cb.SetOnCheckedChangeListener (new CheckChangeListener (item.ClickEvent));
			}

			cb.SetText (item.text, TextView.BufferType.Normal);

			return view;
		}
	}
	*/

	// class to start an activity to show a person
	public class PersonSpan : Android.Text.Style.ClickableSpan {

		#region implemented abstract members of ClickableSpan

		public override void OnClick (View widget)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}

	[Activity (Label = "ActivityActivity")]			
	public class ActivityActivity : FragmentActivity {
		private ListView _activityList, _groupList, _optionList;
		private FilterOptions _filterOptions;

		protected override void OnCreate (Bundle bundle) {
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.activity);

			_filterOptions = FilterOptions.ALL;

			// create the list views for the activity
			ViewPager.LayoutParams vParams = new ViewPager.LayoutParams ();
			_activityList = new ListView (this);
			_activityList.LayoutParameters = vParams;
			_groupList = new ListView (this);
			_groupList.LayoutParameters = vParams;
			_optionList = new ListView (this);
			_optionList.LayoutParameters = vParams;

			ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

			var pager = FindViewById<ViewPager> (Resource.Id.activity_pager);
			var adaptor = new GenericFragmentPagerAdaptor(SupportFragmentManager);

			adaptor.AddFragmentView ((i, v, b) => {
				return _groupList;
			});

			adaptor.AddFragmentView ((i, v, b) => {
				var listAdaptor = new ActivityListAdapter (this, Resource.Layout.activity_list_item, Resource.Id.activity_text);
				_activityList.Adapter = listAdaptor;

				/* Populate activity list here... */
				Android.Text.SpannableString span = new Android.Text.SpannableString ("This is a sample spannable string");
				span.SetSpan (new PersonSpan(), 10, 16, Android.Text.SpanTypes.InclusiveInclusive);
				span.SetSpan (new PersonSpan(), 27, 33, Android.Text.SpanTypes.InclusiveInclusive);
				int n = 0;
				listAdaptor.AddItem (Resource.Drawable.excl_triangle, span);
				listAdaptor.AddItem (Resource.Drawable.excl_triangle, "This is sample text " + n++);
				listAdaptor.AddItem (Resource.Drawable.excl_triangle, "This is sample text " + n++);

				listAdaptor.AddItem (Resource.Drawable.red_check, "This is sample text " + n++);
				listAdaptor.AddItem (Resource.Drawable.red_check, "This is sample text " + n++);
				listAdaptor.AddItem (Resource.Drawable.red_check, "This is sample text " + n++);

				listAdaptor.AddItem (0, "At Somepoint", true);

				listAdaptor.AddItem (Resource.Drawable.green_check, "This is sample text " + n++);
				listAdaptor.AddItem (Resource.Drawable.green_check, "This is sample text " + n++);
				listAdaptor.AddItem (Resource.Drawable.green_check, "This is sample text " + n++);

				listAdaptor.AddItem (Resource.Drawable.person_outline, "This is sample text " + n++);
				listAdaptor.AddItem (Resource.Drawable.person_outline, "This is sample text " + n++);
				listAdaptor.AddItem (Resource.Drawable.person_outline, "This is sample text " + n++);
				/* * * * * * * * * * * * */

				return _activityList;
			});

			adaptor.AddFragmentView ((i, v, b) => {
				// filter options
				var listAdapter = new OptionListAdapter(this.BaseContext, LayoutInflater);
				listAdapter.AddItem(new OptionListAdapter.Item() {
					Text = "Filter Options",
					TextColor = Android.Graphics.Color.DimGray,
					AllCaps = true,
					TextGravity = GravityFlags.Bottom,
					TextSize = new OptionListAdapter.Item.TxtSize() {
						Size = 12f,
						UnitType = Android.Util.ComplexUnitType.Dip
					},
					TextId = Resource.Id.option_text,
					CBoxId = Resource.Id.option_cb,
					LayoutId = Resource.Layout.activity_option_item,
					CBoxDrawable = Resource.Drawable.transparent
				});

				var item =new OptionListAdapter.Item() {
					Text = "New Challenges",
					TextId = Resource.Id.option_text,
					CBoxId = Resource.Id.option_cb,
					BtnId = Resource.Id.option_cb,
					LayoutId = Resource.Layout.activity_option_item,
					Checked = (_filterOptions & FilterOptions.NewChallenges) == FilterOptions.NewChallenges
				};
				item.CBoxChecked = (cb, checkd) => {
					cb.Checked = item.Checked;
				};
				item.ParentClicked = (v2) => {
					var cb = v2.FindViewById<CheckBox>(Resource.Id.option_cb);
					_filterOptions ^= FilterOptions.NewChallenges;
					item.Checked = (_filterOptions & FilterOptions.NewChallenges) == FilterOptions.NewChallenges;
					cb.Checked = item.Checked;
				};
				item.BtnClicked = (v2) => {
					var cb = (CheckBox)v2;
					_filterOptions ^= FilterOptions.NewChallenges;
					item.Checked = (_filterOptions & FilterOptions.NewChallenges) == FilterOptions.NewChallenges;
					cb.Checked = item.Checked;
				};
				listAdapter.AddItem(item);

				item =new OptionListAdapter.Item() {
					Text = "Challenge Results",
					TextId = Resource.Id.option_text,
					CBoxId = Resource.Id.option_cb,
					BtnId = Resource.Id.option_cb,
					LayoutId = Resource.Layout.activity_option_item,
					Checked = (_filterOptions & FilterOptions.ChallengeResults) == FilterOptions.ChallengeResults
				};
				item.CBoxChecked = (cb, checkd) => {
					cb.Checked = item.Checked;
				};
				item.ParentClicked = (v2) => {
					var cb = v2.FindViewById<CheckBox>(Resource.Id.option_cb);
					_filterOptions ^= FilterOptions.ChallengeResults;
					item.Checked = (_filterOptions & FilterOptions.ChallengeResults) == FilterOptions.ChallengeResults;
					cb.Checked = item.Checked;
				};
				item.BtnClicked = (v2) => {
					var cb = (CheckBox)v2;
					_filterOptions ^= FilterOptions.ChallengeResults;
					item.Checked = (_filterOptions & FilterOptions.ChallengeResults) == FilterOptions.ChallengeResults;
					cb.Checked = item.Checked;
				};
				listAdapter.AddItem(item);

				item =new OptionListAdapter.Item() {
					Text = "Synced Progress",
					TextId = Resource.Id.option_text,
					CBoxId = Resource.Id.option_cb,
					BtnId = Resource.Id.option_cb,
					LayoutId = Resource.Layout.activity_option_item,
					Checked = (_filterOptions & FilterOptions.SyncedProgress) == FilterOptions.SyncedProgress
				};
				item.CBoxChecked = (cb, checkd) => {
					cb.Checked = item.Checked;
				};
				item.ParentClicked = (v2) => {
					var cb = v2.FindViewById<CheckBox>(Resource.Id.option_cb);
					_filterOptions ^= FilterOptions.SyncedProgress;
					item.Checked = (_filterOptions & FilterOptions.SyncedProgress) == FilterOptions.SyncedProgress;
					cb.Checked = item.Checked;
				};
				item.BtnClicked = (v2) => {
					var cb = (CheckBox)v2;
					_filterOptions ^= FilterOptions.SyncedProgress;
					item.Checked = (_filterOptions & FilterOptions.SyncedProgress) == FilterOptions.SyncedProgress;
					cb.Checked = item.Checked;
				};
				listAdapter.AddItem(item);

				item =new OptionListAdapter.Item() {
					Text = "Friend Requests",
					TextId = Resource.Id.option_text,
					CBoxId = Resource.Id.option_cb,
					BtnId = Resource.Id.option_cb,
					LayoutId = Resource.Layout.activity_option_item,
					Checked = (_filterOptions & FilterOptions.FriendRequests) == FilterOptions.FriendRequests
				};
				item.CBoxChecked = (cb, checkd) => {
					cb.Checked = item.Checked;
				};
				item.ParentClicked = (v2) => {
					var cb = v2.FindViewById<CheckBox>(Resource.Id.option_cb);
					_filterOptions ^= FilterOptions.FriendRequests;
					item.Checked = (_filterOptions & FilterOptions.FriendRequests) == FilterOptions.FriendRequests;
					cb.Checked = item.Checked;
				};
				item.BtnClicked = (v2) => {
					var cb = (CheckBox)v2;
					_filterOptions ^= FilterOptions.FriendRequests;
					item.Checked = (_filterOptions & FilterOptions.FriendRequests) == FilterOptions.FriendRequests;
					cb.Checked = item.Checked;
				};
				listAdapter.AddItem(item);

				_optionList.Adapter = listAdapter;

				return _optionList;
			});

			pager.Adapter = adaptor;
			pager.SetOnPageChangeListener (new GenericPageListener (ActionBar));
			pager.OffscreenPageLimit = 3;

			ActionBar.AddTab (pager.GetViewPageTab (ActionBar, "Groups"));
			ActionBar.AddTab (pager.GetViewPageTab (ActionBar, "Activity"));
			ActionBar.AddTab (pager.GetViewPageTab (ActionBar, "Options"));

			ActionBar.SelectTab (ActionBar.GetTabAt (1));

			/*
			 * 
			root.RemoveAllViews ();

			var vActivity = LayoutInflater.Inflate (Resource.Layout.activity, null);

			root.AddView (vActivity);

//			var adaptor = new ArrayAdapter<LinearLayout> (this, Resource.Layout.tab1, Resource.Id.title);
			var adaptor = new ActivityListAdapter (this, Resource.Layout.activity_list_item, Resource.Id.activity_text);
			var lview = root.FindViewById<ListView> (Resource.Id.actvity_list);

			Android.Text.SpannableString span = new Android.Text.SpannableString ("This is a sample spannable string");
			span.SetSpan (new Reloader (this), 10, 16, Android.Text.SpanTypes.InclusiveInclusive);
			span.SetSpan (new Reloader (this), 27, 33, Android.Text.SpanTypes.InclusiveInclusive);

			lview.Adapter = adaptor;

			int n = 0;
			adaptor.AddItem (Resource.Drawable.excl_triangle, span);
			adaptor.AddItem (Resource.Drawable.excl_triangle, "This is sample text " + n++);
			adaptor.AddItem (Resource.Drawable.excl_triangle, "This is sample text " + n++);

			adaptor.AddItem (Resource.Drawable.red_check, "This is sample text " + n++);
			adaptor.AddItem (Resource.Drawable.red_check, "This is sample text " + n++);
			adaptor.AddItem (Resource.Drawable.red_check, "This is sample text " + n++);

			adaptor.AddItem (0, "At Somepoint", true);

			adaptor.AddItem (Resource.Drawable.green_check, "This is sample text " + n++);
			adaptor.AddItem (Resource.Drawable.green_check, "This is sample text " + n++);
			adaptor.AddItem (Resource.Drawable.green_check, "This is sample text " + n++);

			adaptor.AddItem (Resource.Drawable.person_outline, "This is sample text " + n++);
			adaptor.AddItem (Resource.Drawable.person_outline, "This is sample text " + n++);
			adaptor.AddItem (Resource.Drawable.person_outline, "This is sample text " + n++);
			ActionBar.Show ();
			*/
		}
	}
}

