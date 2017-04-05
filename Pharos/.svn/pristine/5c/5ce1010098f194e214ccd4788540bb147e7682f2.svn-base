namespace Pharos.Service.Retailing
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.PharosERPService = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // PharosERPService
            // 
            this.PharosERPService.Account = System.ServiceProcess.ServiceAccount.LocalSystem;

            this.PharosERPService.Password = null;
            this.PharosERPService.Username = null;
            // 
            // serviceInstaller
            // 
            this.serviceInstaller.ServiceName = "Phaors.ERPService";
            this.serviceInstaller.Description = "Phaors ERP服务（促销、积分、充值赠送等服务）";

            this.serviceInstaller.DisplayName = "Phaors.ERPService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.PharosERPService,
            this.serviceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller PharosERPService;
        private System.ServiceProcess.ServiceInstaller serviceInstaller;
    }
}