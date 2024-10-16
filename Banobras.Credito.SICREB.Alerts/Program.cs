using System.ServiceProcess;

namespace Banobras.Credito.SICREB.Alerts
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new SICREBAlerts() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
