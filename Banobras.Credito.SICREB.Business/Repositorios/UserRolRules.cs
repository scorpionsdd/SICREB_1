using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class UserRolRules
    {
        CruceUsuarioRolDataAccess datosUsuario = null;

        public UserRolRules(string pLogin)
        {

            datosUsuario = new CruceUsuarioRolDataAccess(pLogin);

        }

        public List<CruceUsuarioRol> GetRecords(bool soloActivos)
        {

            return datosUsuario.GetRecords(soloActivos);

        }
        public int Update(CruceUsuarioRol entityOld, CruceUsuarioRol entityNew)
        {

            return datosUsuario.UpdateRecord(entityOld, entityNew);
 
        }
        public int Insert(CruceUsuarioRol entityToInsert)
        {

            return datosUsuario.InsertRecord(entityToInsert);
           
        }

        public int Delete(CruceUsuarioRol entityToDelete)
        {

            return datosUsuario.DeleteRecord(entityToDelete);
        
        }

    }
}
