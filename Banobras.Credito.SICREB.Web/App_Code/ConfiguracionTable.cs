using Banobras.Credito.SICREB.Common;
using System;
using System.Data;
using System.Data.Common;
using System.DirectoryServices;
using System.Linq;
using System.Text;

namespace Banobras.Credito.SICREB.Data.Transaccionales
{
    public class ConfiguracionTable
    {

        public ConfiguracionTable() { }

        public DataTable CrearTabla()
        {
            DataTable tblEmails = new DataTable();
            tblEmails.Columns.Add(new DataColumn("USUARIO", typeof(string)));
            tblEmails.Columns.Add(new DataColumn("EMAIL", typeof(string)));
           
            return tblEmails;
        }

        public DataTable get_Email(string LDAPPATH)
        {
            DataTable tblEmails = CrearTabla();//, username, strPassDA
            //DirectoryEntry entry = new DirectoryEntry(LDAPPATH, "sicrebds", "B@n0bras2013");
            var environmentVariableValues = Util.GetEnvironmentVariableValues();
            var valuesAD = environmentVariableValues.Emails.FirstOrDefault(x => x.Scheme == "ActiveDirectory");
            DirectoryEntry entry = new DirectoryEntry(LDAPPATH, valuesAD.UserId, valuesAD.Password);
            DirectorySearcher dSearch = new DirectorySearcher(entry);
            dSearch.Filter = "(&(objectClass=user))";
            foreach (SearchResult sResultSet in dSearch.FindAll())
            {
                string strEmail = GetProperty(sResultSet, "userprincipalname").Trim();
                if (!strEmail.Equals(string.Empty))
                {
                    string strNombre = GetProperty(sResultSet, "givenname").Trim();
                    string strApellidos = GetProperty(sResultSet, "sn").Trim();
                    tblEmails.Rows.Add(strNombre + " " + strApellidos, strEmail);
                }
            }
            return tblEmails;
        }

        public string GetProperty(SearchResult searchResult, string PropertyName)
        {
            if (searchResult.Properties.Contains(PropertyName))
            {
                return searchResult.Properties[PropertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public void addTableConfig(int auxID, StringBuilder MyStringBuilder, ref string strMsgError)
        {
            try
            {
                DbParameter[] parametros = new DbParameter[2];
                parametros[0] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[0].ParameterName = "IdAlerta"; //param[ipar].ParameterName;
                parametros[0].DbType = DbType.Int32; //param[ipar].DbType;
                parametros[0].Direction = ParameterDirection.Input; //param[ipar].Direction;
                parametros[0].Value = auxID;

                parametros[1] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[1].ParameterName = "DESTINATARIO"; //param[ipar].ParameterName;
                parametros[1].DbType = DbType.String; //param[ipar].DbType;
                parametros[1].Direction = ParameterDirection.Input; //param[ipar].Direction;
                parametros[1].Value = MyStringBuilder.ToString();

                cls_accesoData accesoData = new cls_accesoData();
                accesoData.fn_getResultadoSTORE_Command("SP_INSERTCONFIGURACIONALERTA", parametros);
            }
            catch (Exception ex)
            {
                strMsgError = ex.Message;
            }
        }
    
    }
}