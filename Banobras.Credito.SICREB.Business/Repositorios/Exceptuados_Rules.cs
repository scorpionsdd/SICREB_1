using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class Exceptuados_Rules
    {
        ExceptuadosDataAccess datosExceptuados = null;

        public Exceptuados_Rules()
        {

            datosExceptuados = new ExceptuadosDataAccess();

        }

        public List<Exceptuado> GetRecords(bool soloActivos)
        {

            return datosExceptuados.GetRecords(soloActivos);

        }
        public int Update(Exceptuado entityOld, Exceptuado entityNew )
        {

            return datosExceptuados.UpdateRecord(entityOld, entityNew);
 
        }
        public int Insert(Exceptuado entityToInsert)
        {

            return datosExceptuados.InsertRecord(entityToInsert);
           
        }

        public int Delete(Exceptuado entityToDelete)
        {

            return datosExceptuados.DeleteRecord(entityToDelete);
        
        }

    }
}
