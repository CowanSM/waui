using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.App;

namespace wave_android_uitest
{
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
			var task = new AsyncUITimerTask (this, LoadStart);
//			var task = new AsyncUITimerTask (this, LoadActivity);

			timer.Schedule (task, 5000);
		}

		private void LoadActivity() {
			root.RemoveAllViews ();

			var vActivity = LayoutInflater.Inflate (Resource.Layout.activity, null);

			root.AddView (vActivity);

			ActionBar.Show ();
		}

		private void LoadStart() {
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


