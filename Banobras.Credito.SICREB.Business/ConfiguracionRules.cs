using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business
{
    public class ConfiguracionRules
    {
        ConfiguracionDataAccess config = null;
        public ConfiguracionRules()
        {
            config = new ConfiguracionDataAccess(); 
        }
        public bool GuardarConfiguracion(int pSicofin, int pSic, int pCalif, int idUsuario)
        {
            bool resp;
            return resp = ConfiguracionDataAccess.insertConfiguracion(pSicofin,pSic, pCalif ,idUsuario);
        }

        public Configuracion GetRecords(string palabra) // parametro: bool soloActivos
        {
            return config.ObtenerConfiguracion(palabra);
        }      

    }
}
