using System;
using System.Linq;
using System.Windows;
using System.Diagnostics;
using Microsoft.Phone.Scheduler;
using CMcG.CommonwealthBank.Data;
using CMcG.CommonwealthBank.Logic;

namespace CMcG.CommonwealthBank.Agent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        static volatile bool m_classInitialized;

        public ScheduledAgent()
        {
            if (m_classInitialized)
                return;

            m_classInitialized = true;
            
            Deployment.Current.Dispatcher.BeginInvoke(
                () => Application.Current.UnhandledException += GlobalUnhandledExceptionHandler);
        }

        void GlobalUnhandledExceptionHandler(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
                Debugger.Break();

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

            e.Handled = true;
            NotifyComplete();
        }

        protected override void OnInvoke(ScheduledTask task)
        {
            if (!new Updater().Update(NotifyComplete))
                NotifyComplete();
        }
    }
}