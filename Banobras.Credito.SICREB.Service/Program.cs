using System;
using System.ServiceProcess;

namespace Banobras.Credito.SICREB.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            SICREBService nuevoServicio = new SICREBService();
            nuevoServicio.depurando_servicio();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new SICREBService() 
			};
            ServiceBase.Run(ServicesToRun);
#endif

        }
    }
}
