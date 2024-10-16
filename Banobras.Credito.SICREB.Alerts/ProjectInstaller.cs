using System;
using System.ComponentModel;
using System.Configuration.Install;

namespace Banobras.Credito.SICREB.Alerts
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}
