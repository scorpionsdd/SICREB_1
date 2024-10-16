using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class ClientesPF_rules
    {

        ClientesPFDataAccess datosClientesPF = null;

        public ClientesPF_rules() //parametro: Enums.Persona pers 
        {
            datosClientesPF = new ClientesPFDataAccess();
        }

        public List<ClientesPF> GetRecords() 
        {
            return datosClientesPF.GetRecords();
        }
        public int Update(ClientesPF entityOld, ClientesPF entityNew)
        {

            return datosClientesPF.UpdateRecord(entityOld, entityNew);
 
        }
        public int Insert(ClientesPF entityToInsert)
        {

            return datosClientesPF.InsertRecord(entityToInsert);
           
        }

        public int Delete(string entityToDelete)
        {

            return datosClientesPF.DeleteRecord(entityToDelete);
        
        }


        public int DeleteAll()
        {

            return datosClientesPF.DeleteAllRecords();

        }


    }
}
