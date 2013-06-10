using System;
using System.Linq;
using System.Windows.Threading;

namespace CMcG.Bankr.Logic
{
    public class AppStatus : Data.NotifyBase
    {
        public bool   AutoRemove { get; set; }
        public bool   IsBusy     { get; set; }
        public string Action     { get; set; }

        public void FireChanged()
        {
            FirePropertyChanged(() => IsBusy, () => Action);
        }

        public void SetAction(string action, bool isState = false)
        {
            IsBusy = !isState && action != null;
            Action = action;
            FireChanged();
            if (!isState || !AutoRemove)
                return;

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            timer.Tick += (s, e) =>
            {
                Action = null;
                FireChanged();
                timer.Stop();
            };
            timer.Start();
        }
    }
}
