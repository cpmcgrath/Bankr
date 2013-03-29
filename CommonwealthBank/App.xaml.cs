using System;
using System.Windows;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using CMcG.CommonwealthBank.Data;
using CMcG.CommonwealthBank.Logic;
using Microsoft.Phone.Scheduler;
using CMcG.CommonwealthBank.ViewModels.Options;

namespace CMcG.CommonwealthBank
{
    public partial class App : Application
    {
        public PhoneApplicationFrame RootFrame { get; private set; }

        public new static App Current
        {
            get { return (App)Application.Current; }
        }

        public Security  Security      { get; private set; }
        public AppStatus Status        { get; private set; }
        public bool      WasTombstoned { get; private set; }

        public App()
        {
            InitializeComponent();
            InitializePhoneApplication();
            Status   = new AppStatus { AutoRemove = true };
            Security = new Security();
            Security.UpdatePermission<OptionsViewModel>(true);

            if (System.Diagnostics.Debugger.IsAttached)
                ShowGraphicsProfiling();

            StartBackgroundAgent();
        }

        void ShowGraphicsProfiling()
        {
            Current.Host.Settings.EnableFrameRateCounter = true;
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;

            // Show the areas of the app that are being redrawn in each frame.
            //Current.Host.Settings.EnableRedrawRegions = true;

            // Shows areas of a page that are handed off to GPU with a colored overlay.
            //Current.Host.Settings.EnableCacheVisualization = true;

        }

        void OnInitialLaunch(object sender, LaunchingEventArgs e)
        {
            WasTombstoned = true;
        }

        void OnReactivated(object sender, ActivatedEventArgs e)
        {
            WasTombstoned = !e.IsApplicationInstancePreserved;
        }

        void OnDeactivated(object sender, DeactivatedEventArgs e) { }
        void OnClosing    (object sender, ClosingEventArgs e)     { }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Break();
        }

        void GlobalUnhandledExceptionHandler(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Break();

            var error = new Error
            {
                Time     = DateTime.Now,
                Message  = e.ExceptionObject.Message,
                Extended = e.ExceptionObject.ToVerboseString()
            };

            using (var store = new DataStoreContext())
            {
                store.Errors.InsertOnSubmit(error);
                store.SubmitChanges();
            }

            Status.SetAction("Unknown error occurred", true);
            e.Handled = true;
        }

        void StartBackgroundAgent()
        {
            var taskName = "CMcG.CommonwealthBank.Agent";

            if (ScheduledActionService.Find(taskName) is PeriodicTask)
                ScheduledActionService.Remove(taskName);

            ScheduledActionService.Add(new PeriodicTask(taskName)
            {
                Description = "Checks for new transactions"
            });

            //ScheduledActionService.LaunchForTest(taskName,
            //    TimeSpan.FromSeconds(15));
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += OnNavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}