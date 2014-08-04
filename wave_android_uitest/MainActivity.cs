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
			public int _id, _imageResourceLeft, _imageResourceRight;
			public string _topText, _bottomText, _timeText;
			public bool _divider;
			public Android.Text.SpannableString _topSequence;
		}

		public ActivityListAdapter(Context context, int layoutId, int textViewId) :
		base (context, layoutId, textViewId){
			_list = new List<ListItem> ();
			_layoutResource = layoutId;
			_inflator = (LayoutInflater)context.GetSystemService (Context.LayoutInflaterService);

			_nextId = 0;
		}

		public void AddItem(int imageResourceLeft, int imageResourceRight, string topText, string bottomText, string time, bool divider = false) {
			_list.Add (new ListItem () {
				_id = _nextId++,
				_topText = topText,
				_bottomText = bottomText,
				_timeText = time,
				_imageResourceLeft = imageResourceLeft,
				_imageResourceRight = imageResourceRight,
				_divider = divider
			});

			this.Add (topText);
		}

		public void AddItem(int imageResourceLeft, int imageResRight, Android.Text.SpannableString topString, string bottomString, string time) {
			_list.Add (new ListItem () {
				_id = _nextId++,
				_topText = string.Empty,
				_bottomText = bottomString,
				_timeText = time,
				_imageResourceLeft = imageResourceLeft,
				_imageResourceRight = imageResRight,
				_divider = false,
				_topSequence = topString
			});

			this.Add (topString);
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

			var image = view.FindViewById<ImageView> (Resource.Id.activity_limage);
			image.SetImageResource (item._imageResourceLeft);

			var text1 = view.FindViewById<TextView> (Resource.Id.activity_text1);
			if (!string.IsNullOrEmpty (item._topText)) {
				text1.Text = item._topText;
			} else {
				text1.SetLinkTextColor (Android.Graphics.Color.White);
				text1.TextFormatted = item._topSequence;
				text1.MovementMethod = new Android.Text.Method.LinkMovementMethod ();
			}
			text1.Gravity = GravityFlags.Left;

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
			var intent = new Intent (this, typeof(ActivityActivity));
			StartActivity (intent);
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


