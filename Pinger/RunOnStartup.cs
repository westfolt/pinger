using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Application = System.Windows.Forms.Application;

namespace Pinger
{
    class RunOnStartup
    {
        private RegistryKey rgKey;

        public RunOnStartup()
        {
            rgKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\" +
                                                    "CurrentVersion\\Run", true);
        }

        public bool IsStartup()
        {
            if (rgKey.GetValue("Pinger") == null)
            {
                //the value doesn't exist, app doesn't run on startup
                return false;
            }
            return true;
        }

        public void SetStartup()
        {
            if (!IsStartup())
            {
                rgKey.SetValue("Pinger",Application.ExecutablePath);
            }
        }

        public void RemoveStartup()
        {
            if (IsStartup())
            {
                rgKey.DeleteValue("Pinger",false);
            }
        }
    }
}
