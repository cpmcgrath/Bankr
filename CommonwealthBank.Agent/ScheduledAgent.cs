using System;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Scheduler;
using System.Diagnostics;
using Microsoft.Phone.Shell;

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