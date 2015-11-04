using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Timers;

namespace Framework.DistributedService
{
    public abstract class DistributedHostManagerBase
    {
        private List<IDistributedHost> _hostList = new List<IDistributedHost>();
        private readonly Timer _serviceStatusReportTimer = new Timer(1000);

        protected DistributedHostManagerBase()
        {
            _serviceStatusReportTimer.Elapsed += ServiceStatusReportTimer_Elapsed;
        }
        
        public void OpenAllService()
        {
            if (_hostList.Count > 0 == false)
            {
                return;
            }

            foreach (var host in _hostList)
            {
                host.Open();
            }

            ReportStatus();
            _serviceStatusReportTimer.Enabled = true;
        }

        /// <summary>
        /// 子类实现：用于加载Host实例到管理容器中来
        /// </summary>
        public abstract void Initialize();

        #region Protected Method
        protected void AddHost(IDistributedHost host)
        {
            _hostList.Add(host);
        }

        protected void SetStatusReportInterval(int interval)
        {
            _serviceStatusReportTimer.Interval = interval;
        }
        #endregion

        #region Pirvate Method
        private void ServiceStatusReportTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _serviceStatusReportTimer.Enabled = false;
            ReportStatus();
            _serviceStatusReportTimer.Enabled = true;
        }

        private void ReportStatus()
        {
            if(_hostList.Count > 0 == false)
            {
                return;
            }

            var items = new List<HostStatusReportEventArgsItem>();
            foreach (var host in _hostList)
            {
                HostStatusReportEventArgsItem item = new HostStatusReportEventArgsItem
                {
                    Time = DateTime.Now,
                    HostName = host.Name,
                    HostStatus = host.State.ToString()
                };
                items.Add(item);
            }

            HostStatusReportEventArgs e = new HostStatusReportEventArgs(items);
            RaiseHostStatusReportEvent(e);
        }
        #endregion

        #region Event
        public event EventHandler<HostStatusReportEventArgs> HostStatusReportEvent;
        private void RaiseHostStatusReportEvent(HostStatusReportEventArgs e)
        {
            var handler = HostStatusReportEvent;
            if (handler != null)
                handler(this, e);
        }
        #endregion
    }

    #region EventArgs
    public class HostStatusReportEventArgs : EventArgs
    {
        private List<HostStatusReportEventArgsItem> _items;

        public HostStatusReportEventArgs(IEnumerable<HostStatusReportEventArgsItem> items)
        {
            _items = items.ToList();
        }

        public IEnumerable<HostStatusReportEventArgsItem> Items
        {
            get { return _items; }
        }
    }

    public class HostStatusReportEventArgsItem
    {
        public DateTime Time { get; set; }

        public string HostName { get; set; }

        public string HostStatus { get; set; }
    }
    #endregion
}
