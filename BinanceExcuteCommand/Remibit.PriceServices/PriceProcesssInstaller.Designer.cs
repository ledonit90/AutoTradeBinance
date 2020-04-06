namespace Remibit.PriceServices
{
    partial class PriceProcesssInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.processIntaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // processIntaller
            // 
            this.processIntaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.processIntaller.Password = null;
            this.processIntaller.Username = null;
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller.Description = "Remibit.PriceSerivces - ServiceStack Windows Service.";
            this.serviceInstaller.DisplayName = "Remibit.PriceSerivces";
            this.serviceInstaller.ServiceName = "Remibit.PriceSerivces";
            this.serviceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.processIntaller,
            this.serviceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller processIntaller;
        private System.ServiceProcess.ServiceInstaller serviceInstaller;
    }
}