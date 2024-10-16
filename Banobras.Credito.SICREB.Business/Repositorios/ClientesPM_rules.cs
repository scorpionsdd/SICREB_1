using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class ClientesPM_rules
    {

        ClientesPMDataAccess datosClientesPM = null;

        public ClientesPM_rules() //parametro: Enums.Persona pers 
        {
            datosClientesPM = new ClientesPMDataAccess(); 
        }

        public List<ClientesPM> GetRecords() 
        {
            return datosClientesPM.GetRecords();
        }
        public int Update(ClientesPM entityOld, ClientesPM entityNew)
        {
            return datosClientesPM.UpdateRecord(entityOld, entityNew);
        }
        public int Insert(ClientesPM entityToInsert)
        {

            return datosClientesPM.InsertRecord(entityToInsert);

        }

        public int Delete(string entityToDelete)
        {

            return datosClientesPM.DeleteRecord(entityToDelete);

        }


        public int DeleteAll()
        {

            return datosClientesPM.DeleteAllRecords();

        }
    }
}
