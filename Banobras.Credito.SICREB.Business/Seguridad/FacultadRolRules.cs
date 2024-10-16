using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Data;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Seguridad;
namespace Banobras.Credito.SICREB.Business.Seguridad
{
    public class FacultadRolCadena
    {
        public int id { get; set; }
        public int idRol { get; set; }
        public int idFacultad { get; set; }
        public string Rol { get; set; }
        public string Facultades { get; set; }
        public Enums.Estado estatus { get; set; }
        //TODO: SOL56792 Bitacora Roles
        public DateTime CreationDate { get; set; }
        //public string Descripcion { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionLogin { get; set; }
    }

    public class FacultadRolRules
    {

        /// <summary>
        /// Obtener las facultades asociadas a los roles
        /// </summary>
        /// <param name="estatus">True = Sólo los activos | False = Todos los registros</param>
        /// <returns></returns>
        public List<FacultadRol> FacultadRoles(bool estatus = true)
        {
            FacultadRolDataAccess frda=new FacultadRolDataAccess();
            //return frda.GetRecords(true);
            return frda.GetRecords(estatus);
        }
       

        public List<FacultadRolCadena> FacultadesRolesCadena()
        {
            RolRules rr = new RolRules();
            List<Rol> Roles= rr.Roles(false);

            FacultadRules fr = new FacultadRules();
            List<Facultad> Facultades = fr.Facultades();

            //FacultadRolRules frr = new FacultadRolRules();
            List<FacultadRol> facultadesRol = this.FacultadRoles(false);

            List<FacultadRolCadena> facultadRolCadena = new List<FacultadRolCadena>();
            var query= from facrol in facultadesRol join r in Roles on facrol.IdRol equals r.Id 
                       select new { 
                           facrol.IdRol,
                           Rol = r.Descripcion,
                           facrol.IdFacultad,
                           facrol.Id,
                           facrol.Estatus
                       };

            List<FacultadRolCadena> frc = (from q in query join f in Facultades on q.IdFacultad equals f.Id 
                                           select new FacultadRolCadena {
                                               idRol= q.IdRol,
                                               Rol= q.Rol,
                                               idFacultad = q.IdFacultad,
                                               id= q.Id,
                                               estatus = q.Estatus,
                                               Facultades = f.Descripcion 
                                           })
                                          .ToList<FacultadRolCadena>();
            
            foreach (FacultadRolCadena tmpfrc in frc)
            {
                int indice = facultadRolCadena.FindIndex(delegate(FacultadRolCadena p) { return p.idRol == tmpfrc.idRol; });
                if (indice >= 0)
                {
                    FacultadRolCadena temporal = facultadRolCadena[indice];
                    temporal.Facultades = string.Concat(facultadRolCadena[indice].Facultades, ",", tmpfrc.Facultades);
                    facultadRolCadena[indice] = temporal;
                }
                else
                {
                    facultadRolCadena.Add(tmpfrc);
                }
            }
            return facultadRolCadena;
        }

        public int InsertarFacultadesRol(FacultadRol facultadRol)
        {
            FacultadRolDataAccess frda = new FacultadRolDataAccess(); 
            return frda.InsertRecord(facultadRol);
        }

        public int EliminarFacultadRol(FacultadRol facultadrol)
        {
            FacultadRolDataAccess frda = new FacultadRolDataAccess();
            return frda.DeleteRecord(facultadrol);
        }

        public int ActualizarFacultadRol(FacultadRol entityCurrent, FacultadRol entityUpdate)
        {
            FacultadRolDataAccess frda = new FacultadRolDataAccess();
            return frda.UpdateRecord(entityCurrent, entityUpdate);
        }


    }
}
