using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;

namespace wave_android_uitest
{
	public class Reloader : Android.Text.Style.ClickableSpan {
		MainActivity _activity;

		public Reloader(MainActivity activity) {
			_activity = activity;
		}

		public override void OnClick (View widget)
		{
			_activity.RunOnUiThread (_activity.LoadStart);
		}
	}

	public class ActivityListAdapter : ArrayAdapter {
		List<ListItem> _list;
		int _layoutResource;
		LayoutInflater _inflator;

		int _nextId;

		private struct ListItem {
			public int _id, _imageResource;
			public string _text;
			public bool _divider;
			public Android.Text.SpannableString _sequence;
		}

		public ActivityListAdapter(Context context, int layoutId, int textViewId) :
		base (context, layoutId, textViewId){
			_list = new List<ListItem> ();
			_layoutResource = layoutId;
			_inflator = (LayoutInflater)context.GetSystemService (Context.LayoutInflaterService);

			_nextId = 0;
		}

		public void AddItem(int imageResource, string text, bool divider = false) {
			_list.Add (new ListItem () {
				_id = _nextId++,
				_text = text,
				_imageResource = imageResource,
				_divider = divider
			});

			this.Add (text);
		}

		public void AddItem(int imageResource, Android.Text.SpannableString sstring) {
			_list.Add (new ListItem () {
				_id = _nextId++,
				_text = string.Empty,
				_imageResource = imageResource,
				_divider = false,
				_sequence = sstring
			});

			this.Add (sstring);
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			View view;

			if (convertView == null) {
				view = _inflator.Inflate (_layoutResource, parent, false);
			} else {
				view = convertView;
			}

			return this.BindData (view, position);
		}

		private View BindData (View view, int position)
		{
			var item = _list [position];

			if (!item._divider) {
				var image = view.FindViewById<ImageView> (Resource.Id.activity_image);
				image.SetImageResource (item._imageResource);

				var text = view.FindViewById<TextView> (Resource.Id.activity_text);
				if (!string.IsNullOrEmpty (item._text)) {
					text.Text = item._text;
				} else {
					text.TextFormatted = item._sequence;
					text.MovementMethod = new Android.Text.Method.LinkMovementMethod ();
				}
				text.Gravity = GravityFlags.Center;
				text.SetTextColor (Android.Graphics.Color.White);
				var tParams = new RelativeLayout.LayoutParams (RelativeLayout.LayoutParams.FillParent, RelativeLayout.LayoutParams.WrapContent);
				tParams.AddRule (LayoutRules.CenterInParent);
				text.LayoutParameters = tParams;

				var arrow = view.FindViewById<ImageView> (Resource.Id.drop_arrow);
				arrow.SetImageResource (Resource.Drawable.down_arrow);

				view.SetBackgroundColor (Android.Graphics.Color.Black);
			} else {
				var image = view.FindViewById<ImageView> (Resource.Id.activity_image);
//				image.SetImageResource (Resource.Drawable.abc_ab_transparent_light_holo);
				var arrow = view.FindViewById<ImageView> (Resource.Id.drop_arrow);
//				arrow.SetImageResource (Resource.Drawable.abc_ab_transparent_dark_holo);

				var text = view.FindViewById<TextView> (Resource.Id.activity_text);
				text.Text = item._text;
				text.Gravity = GravityFlags.Left;
				text.SetTextColor (Android.Graphics.Color.White);
				var tParams = new RelativeLayout.LayoutParams (RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.WrapContent);
				tParams.AddRule (LayoutRules.AlignParentLeft);
				text.LayoutParameters = tParams;

				view.SetBackgroundColor (Android.Graphics.Color.SlateGray);
			}

			return view;
		}
	}

	public class AsyncUITimerTask : Java.Util.TimerTask {
		private Action RunTask;
		private Activity _activity;

		public AsyncUITimerTask(Activity uiActivity, Action toRun) {
			RunTask = toRun;
			_activity = uiActivity;
		}

		public override void Run ()
		{
			_activity.RunOnUiThread (RunTask);
		}
	}

	public class ClickListener : View.IOnClickListener {
		#region IOnClickListener implementation

		public void OnClick (View v)
		{
			// don't do anything...
		}

		#endregion

		#region IDisposable implementation

		public void Dispose ()
		{
		}

		#endregion

		#region IJavaObject implementation

		public IntPtr Handle {
			get {
				return new IntPtr();
			}
		}

		#endregion


	}

	[Activity (Label = "wave_android_uitest", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : FragmentActivity
	{
		//int count = 1;

		private View tView;
		private View splash;
		private LinearLayout root;

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.activity_actions, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			ActionBar.Title = "";
			ActionBar.Hide ();

			tView = LayoutInflater.Inflate (Resource.Layout.test_layout, null);
			var lSplash = LayoutInflater.Inflate (Resource.Layout.splash, null);

			root = lSplash.FindViewById<LinearLayout> (Resource.Id.root);
			splash = lSplash.FindViewById<View> (Resource.Id.splash);

			// Set our view from the "main" layout resource
			SetContentView (lSplash);

			Java.Util.Timer timer = new Java.Util.Timer ();
//			var task = new AsyncUITimerTask (this, LoadStart);
			var task = new AsyncUITimerTask (this, LoadActivity);

			timer.Schedule (task, 5000);
		}

		private void LoadActivity() {
			root.RemoveAllViews ();

			var vActivity = LayoutInflater.Inflate (Resource.Layout.activity, null);

			root.AddView (vActivity);

//			var adaptor = new ArrayAdapter<LinearLayout> (this, Resource.Layout.tab1, Resource.Id.title);
			var adaptor = new ActivityListAdapter (this, Resource.Layout.activity_list_item, Resource.Id.activity_text);
			var lview = root.FindViewById<ListView> (Resource.Id.listView1);

			Android.Text.SpannableString span = new Android.Text.SpannableString ("This is a sample spannable string");
			span.SetSpan (new Reloader (this), 10, 15, Android.Text.SpanTypes.InclusiveInclusive);

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
		}

		public void LoadStart() {
//			root.RemoveView(splash);

			root.RemoveAllViews ();

			root.AddView(tView);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;

			var pager = FindViewById<ViewPager>(Resource.Id.pager);
			var adaptor = new GenericFragmentPagerAdaptor(SupportFragmentManager);

			adaptor.AddFragmentView ((i, v, b) => {
				var view = i.Inflate(Resource.Layout.tab1, v, false);
				var title = view.FindViewById<TextView>(Resource.Id.title);
				title.Text = "Lorem";
				var body = view.FindViewById<TextView>(Resource.Id.body);
				body.Text = "Lorem ipsum dolor sit amet, consectetur adipisicing elit";
				return view;
			});

			adaptor.AddFragmentView ((i, v, b) => {
				var view = i.Inflate(Resource.Layout.tab1, v, false);
				var title = view.FindViewById<TextView>(Resource.Id.title);
				title.Text = "Neque";
				var body = view.FindViewById<TextView>(Resource.Id.body);
				body.Text = "Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet.";
				return view;
			});

			adaptor.AddFragmentView ((i, v, b) => {
				var view = i.Inflate(Resource.Layout.tab1, v, false);
				var title = view.FindViewById<TextView>(Resource.Id.title);
				title.Text = "Itaque";
				var body = view.FindViewById<TextView>(Resource.Id.body);
				body.Text = "Itaque earum rerum hic tenetur a sapiente delectus, ut aut reiciendis voluptatibus.";
				return view;
			});

			var radioGroup = FindViewById<RadioGroup> (Resource.Id.rg1);

			radioGroup.SetOnClickListener(new ClickListener());

			pager.Adapter = adaptor;
			pager.SetOnPageChangeListener (new ViewPageListenerForActionBar (ActionBar, radioGroup));

			// setup buttons
			var lbtn = FindViewById<Button>(Resource.Id.login);
			var sbtn = FindViewById<Button>(Resource.Id.signup);

			lbtn.Click += (sender, e) => {
				// go to login page
				RunOnUiThread(() => {
					root.RemoveAllViews();

					var login = LayoutInflater.Inflate(Resource.Layout.login, null);

					root.AddView(login);

					var facebook = login.FindViewById<Button>(Resource.Id.login_facebook);
					facebook.Click += (sender2, e2) => {
						// go back to main view
						RunOnUiThread(LoadStart);
					};

					var cont = login.FindViewById<Button>(Resource.Id.login_btn);
					cont.Click += (sender2, e2) => {
						// go to activity
						RunOnUiThread(LoadActivity);
					};
				});
			};

			sbtn.Click += (sender, e) => {
				// go to sign-up page
				RunOnUiThread(() => {
					root.RemoveAllViews();

					var signup = LayoutInflater.Inflate(Resource.Layout.signup, null);

					root.AddView(signup);
					// go back, for now
					var facebook = signup.FindViewById<Button>(Resource.Id.sign_facebook);
					facebook.Click += (sender2, e2) => {
						RunOnUiThread(LoadStart);
					};

					// go to activity
					var cont = signup.FindViewById<Button>(Resource.Id.sign_btn);
					cont.Click += (sender2, e2) => {
						RunOnUiThread(LoadActivity);
					};
				});
			};
		}
	}
}


