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
        private readonly ServiceProcessor registerTask;
        public PriceServices(AppHostHttpListenerBase appHost, string listeningOn)
        {
            registerTask = new ServiceProcessor();
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            this.appHost.Start(listeningOn);
            registerTask.getRateRemitano();
        }

        protected override void OnStop()
        {
            this.appHost.Stop();
        }
    }
}
