using Pharos.POS.Retailing.Models.ViewModels;
using System;
using System.IO;
using System.Security.AccessControl;
using System.Xml.Serialization;

namespace Pharos.POS.Retailing.Models
{

    public delegate void SettingsLoadHandler(Settings settings);
    public class Settings
    {
        //public Settings()
        //{
        //    Load();
        //}

        [XmlIgnore]
        public bool Enable { get; set; }

        public MachineInformations MachineInformations { get; set; }

        public ServicesConfiguration ServicesConfiguration { get; set; }

        public DevicesSettingsConfiguration DevicesSettingsConfiguration { get; set; }

        public event SettingsLoadHandler SettingsLoadEvent;

        public void Save()
        {
            if (!File.Exists(Global.PosMachineConfigPath))
                File.Create(Global.PosMachineConfigPath);
            File.SetAccessControl(Global.PosMachineConfigPath, new FileSecurity(Global.PosMachineConfigPath, AccessControlSections.Owner));
            File.SetAttributes(Global.PosMachineConfigPath, System.IO.FileAttributes.Normal);

            using (FileStream fs = new FileStream(Global.PosMachineConfigPath, FileMode.Create, FileAccess.ReadWrite))
            {
                XmlSerializer serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(fs, this);
                fs.Close();
                fs.Dispose();
            }
        }
        public void Load()
        {
            try
            {
                if (this.MachineInformations == null)
                    this.MachineInformations = new MachineInformations();
                if (this.ServicesConfiguration == null)
                    this.ServicesConfiguration = new ServicesConfiguration();
                if (this.DevicesSettingsConfiguration == null)
                    this.DevicesSettingsConfiguration = new DevicesSettingsConfiguration();
                if (!File.Exists(Global.PosMachineConfigPath))
                    File.Create(Global.PosMachineConfigPath);
                File.SetAccessControl(Global.PosMachineConfigPath, new FileSecurity(Global.PosMachineConfigPath, AccessControlSections.Owner));
                File.SetAttributes(Global.PosMachineConfigPath, System.IO.FileAttributes.Normal);
                using (FileStream fs = new FileStream(Global.PosMachineConfigPath, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(this.GetType());
                    var result = serializer.Deserialize(fs) as Settings;
                    fs.Close();
                    fs.Dispose();
                    Enable = true;
                    //init

                    this.MachineInformations.Reload(result.MachineInformations);
                    this.ServicesConfiguration.Reload(result.ServicesConfiguration);
                    this.DevicesSettingsConfiguration.Reload(result.DevicesSettingsConfiguration);
                    if (SettingsLoadEvent != null) 
                    {
                        SettingsLoadEvent(this);
                    }
                }
            }
            catch
            {
                Enable = false;
            }
        }
    }
}
