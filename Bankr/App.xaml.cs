using System;
using System.Linq;
using System.Windows;
using CMcG.Bankr.Data;
using CMcG.Bankr.Logic;
using System.Reflection;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Scheduler;
using System.Collections.Generic;

namespace CMcG.Bankr
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

        public static IEnumerable<Type> GetViewModelTypes(bool defaultPathOnly = true, bool includeLogin = false)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            return types.Where(x => x.Name.EndsWith("ViewModel"))
                        .Where(x => !defaultPathOnly || x.Namespace.Contains("ViewModels"))
                        .Where(x => includeLogin     || !x.Name.Contains("Login"));
        }

        public App()
        {
            InitializeComponent();
            Status   = new AppStatus { AutoRemove = true };
            Security = new Bankr.Security();
            Security.LoadFromDatabase();

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
            var oldTaskName = "CMcG.CommonwealthBank.Agent";
            var taskName = "CMcG.Bankr.Agent";

            if (ScheduledActionService.Find(taskName) is PeriodicTask)
                ScheduledActionService.Remove(taskName);

            if (ScheduledActionService.Find(oldTaskName) is PeriodicTask)
                ScheduledActionService.Remove(oldTaskName);

            ScheduledActionService.Add(new PeriodicTask(taskName)
            {
                Description = "Checks for new transactions"
            });

            //ScheduledActionService.LaunchForTest(taskName,
            //    TimeSpan.FromSeconds(15));
        }
    }
}