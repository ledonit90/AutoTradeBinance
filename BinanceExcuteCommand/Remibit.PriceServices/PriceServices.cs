using ServiceStack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Remibit.PriceServices
{
    public partial class PriceServices : ServiceBase
    {
        private readonly AppHostHttpListenerBase appHost;
        private readonly string listeningOn;
        public PriceServices(AppHostHttpListenerBase appHost, string listeningOn)
        {
            this.appHost = appHost;
            this.listeningOn = listeningOn;

            this.appHost.Init();
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            this.appHost.Start(listeningOn);
        }

        protected override void OnStop()
        {
            this.appHost.Stop();
        }
    }
}
