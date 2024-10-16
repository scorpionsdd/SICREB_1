using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Data.Seguridad
{

    public class UsuariosDataAccess:CatalogoBase<Usuario,string>
    {

        public override string IdField { get { return "IdUsuario"; } }
        public override string ClaveField { get { return "pLogin"; } }
        public override String ErrorGet { get { return " 1030 - Error al cargar los datos de la base de datos en catálogo Usuarios."; } }
        public override String ErrorInsert { get { return "1031 - Error al insertar nuevo registro en catálogo Usuarios."; } }
        public override String ErrorUpdate { get { return "1032 - Error al actualizar registro en catálogo Usuarios."; } }
        public override String ErrorDelete { get { return "1033 - Error al borrar el registro en catálogo Usuarios."; } }

        public override Usuario GetEntity(IDataReader reader, out bool activo)
        {
            int id = Parser.ToNumber(reader["id"]);
            string login = reader["login"].ToString();
            Enums.Estado estatus = Util.GetEstado(Parser.ToChar(reader["ESTATUS"]));
            //TODO: SOL54125 Bitacora
            string email = reader["Correo"].ToString();
            DateTime creationDate = Parser.ToDateTime(reader["Fecha_Registro"]);
            int employeeNumber = Parser.ToNumber(reader["Empleado_Id"]);
            string name = reader["Nombre"].ToString();
            DateTime sessionDate = Parser.ToDateTime(reader["Fecha_Ultima_Sesion"]);
            string sesssonIP = reader["Sesion_IP"].ToString();
            DateTime transactionDate = Parser.ToDateTime(reader["Fecha_Actualizacion"]);
            string transactionUserLogin = reader["Login_Actualizacion"].ToString();

            Usuario u = new Usuario { 
                Id = id, 
                Login = login, 
                Estatus = estatus, 
                Email = email, 
                CreationDate = creationDate, 
                EmployeeNumber = employeeNumber,
                FullName = name, 
                SessionDate = sessionDate, 
                SessionIP = sesssonIP, 
                TransactionDate = transactionDate,
                TransactionLogin = transactionUserLogin 
            };

            activo = (u.Estatus == Enums.Estado.Activo);

            return u;
        }

        public override string StoredProcedure
        {
            get { return "Seguridad.SP_GetUsuarios"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "Seguridad.F_DelUsuario"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "seguridad.F_InsUsuarios"; }
            
        }

        public override string StoredProcedureUpdate
        {
            get { return "Seguridad.F_UpdUsuario"; }
        }

        public override void SetEntity(DbCommand cmd, Usuario entityOld, Usuario entityNew)
        {
            DB.AddInParameter(cmd, "pId", DbType.Int32, Parser.ToNumber(entityNew.Id));
            DB.AddInParameter(cmd, "pLogin", DbType.String, entityNew.Login);
            DB.AddInParameter(cmd, "pEstatus", DbType.String, Util.SetEstado(entityNew.Estatus.ToString()));
            //TODO: SOL54125 Bitacora
            DB.AddInParameter(cmd, "pFechaRegistro", DbType.DateTime, entityNew.CreationDate);
            DB.AddInParameter(cmd, "pCorreo", DbType.String, entityNew.Email);
            DB.AddInParameter(cmd, "pEmpleadoId", DbType.Int32, entityNew.EmployeeNumber);
            DB.AddInParameter(cmd, "pNombre", DbType.String, entityNew.FullName);
            DB.AddInParameter(cmd, "pFechaUltimaSesion", DbType.DateTime, entityNew.SessionDate);
            DB.AddInParameter(cmd, "pSesionIp", DbType.String, entityNew.SessionIP);
            DB.AddInParameter(cmd, "pFechaActualizacion", DbType.DateTime, entityNew.TransactionDate);
            DB.AddInParameter(cmd, "pLoginActualizacion", DbType.String, entityNew.TransactionLogin);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, Usuario entityToDelete)
        {
            DB.AddInParameter(cmd, "idusuario", DbType.Int32, Parser.ToNumber(entityToDelete.Id));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, Usuario entityToInsert)
        {
            DB.AddInParameter(cmd, "pLogin", DbType.String, entityToInsert.Login);
            //TODO: SOL54125 Bitacora
            DB.AddInParameter(cmd, "pFechaRegistro", DbType.DateTime, entityToInsert.CreationDate);
            DB.AddInParameter(cmd, "pCorreo", DbType.String, entityToInsert.Email);
            DB.AddInParameter(cmd, "pEmpleadoId", DbType.Int32, entityToInsert.EmployeeNumber);
            DB.AddInParameter(cmd, "pNombre", DbType.String, entityToInsert.FullName);
            DB.AddInParameter(cmd, "pFechaUltimaSesion", DbType.DateTime, entityToInsert.SessionDate);
            DB.AddInParameter(cmd, "pSesionIp", DbType.String, entityToInsert.SessionIP);
            DB.AddInParameter(cmd, "pFechaActualizacion", DbType.DateTime, entityToInsert.TransactionDate);
            DB.AddInParameter(cmd, "pLoginActualizacion", DbType.String, entityToInsert.TransactionLogin);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
