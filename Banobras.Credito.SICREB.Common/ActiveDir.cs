using System;
using System.Configuration;
using System.DirectoryServices;
using Banobras.Credito.SICREB.Entities;
using System.Collections.Generic;

namespace Banobras.Credito.SICREB.Common
{

    /// <summary>
    /// Clase que administra los acceso y la manipulación del Active Directory.
    /// </summary>
    public class ActiveDir
    {
        string path;
        string UserName;
        string nombreCompleto;
        private string eMail;        
        private string dominio;
        private string password;
        private string domainAndUsername;
        private static string _activeDirectoryUserId;
        private static string _activeDirectoryPassword;
        private static DirectoryEntry _entryDA;

        static ActiveDir()
        {
            var environmentVariableValues = Util.GetEnvironmentVariableValues();
            _activeDirectoryUserId = environmentVariableValues.ActiveDirectory.UserId; //ConfigurationManager.AppSettings["UsuarioDA"]
            _activeDirectoryPassword = environmentVariableValues.ActiveDirectory.Password; //ConfigurationManager.AppSettings["ContrasenaDA"]
            _entryDA = new DirectoryEntry(ConfigurationManager.AppSettings["RutaActiveDirectory"].ToString(), _activeDirectoryUserId, _activeDirectoryPassword);
        }

        /// <summary>
        /// Obtener dirección de correo de un usuario en el DA através de su Login
        /// </summary>
        /// <param name="userName">Login de usuario en DA</param>
        /// <returns></returns>
        public static string GetEmail(string userName)
        {
            string domainName = Environment.UserDomainName;

            //Set the correct format for the AD query and filter
            //string rootQuery = string.Format(@"{0}/{1}", WebConfig.RutaActiveDirectory, WebConfig.DCActiveDirectory);
            string searchFilter = string.Format(@"(&(samAccountName={0})(objectCategory=person)(objectClass=user))", userName);

            SearchResult result = null;
            try
            {
                using (DirectoryEntry root = _entryDA)
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(root))
                    {
                        searcher.Filter = searchFilter;
                        SearchResultCollection results = searcher.FindAll();

                        result = (results.Count != 0) ? results[0] : null;

                        if (result != null)
                        {
                            return result.Properties["Mail"][0].ToString();
                        }
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        /// Verificar la existencia de credenciales de usuario en DA y retornar nombre y correo
        /// </summary>
        /// <param name="strUserDA">Login en DA</param>
        /// <param name="strPassDA">Contraseña en DA</param>
        /// <param name="nombre">Nombre de retorno</param>
        /// <param name="mail">Correo de retorno</param>
        /// <returns></returns>
        public static bool AutenticaUsuario(string strUserDA, string strPassDA, ref string nombre, ref string mail)
        {
            bool bOK = false;

            try
            {
                string path = ConfigurationManager.AppSettings["RutaActiveDirectory"].ToString();
                string Dominio = ConfigurationManager.AppSettings["NombreDominio"].ToString();
                string username = Dominio.Length > 0 ? string.Format(@"{0}\{1}", Dominio, strUserDA) : strUserDA;
                DirectoryEntry entry = null;
                DirectorySearcher search = null;

                if (username.Length > 0)
                {
                    entry = new DirectoryEntry(path, username, strPassDA);
                    Object obj = entry.NativeObject;

                    search = new DirectorySearcher(entry);
                    search.Filter = "(SAMAccountName=" + strUserDA + ")";
                    search.PropertiesToLoad.Add("givenname");
                    search.PropertiesToLoad.Add("userPrincipalName");
                    SearchResult result = search.FindOne();
                    if (result != null)
                    {
                        nombre = result.Path.Replace("\\","");
                        nombre = nombre.Substring(nombre.IndexOf("CN=") + 3, nombre.Length - nombre.IndexOf("CN=")-3);
                        nombre = nombre.Substring(0, nombre.IndexOf("=") - 3);
                        bOK = true;

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                return bOK;
            }

            return bOK;
        }

        /// <summary>
        /// Verificar la existencia de credenciales de usuario en DA
        /// </summary>
        /// <param name="nomUsu">Login en DA</param>
        /// <param name="pass">Password en DA</param>
        /// <returns></returns>
        public bool IsAuthenticated(string nomUsu, string pass)
        {
            this.password = pass;
            this.dominio = ConfigurationManager.AppSettings["NombreDominio"].ToString();
            this.domainAndUsername = dominio.Length == 0 ? string.Empty : string.Format("{0}{1}{2}", dominio, @"\", nomUsu);

            this.path = ConfigurationManager.AppSettings["RutaActiveDirectory"].ToString();
            DirectoryEntry entry = null;
            if (dominio.Length > 0)
            {
                entry = new DirectoryEntry(this.path, this.domainAndUsername, this.password);
            }
            try
            {
                if (ActiveDir.SetAdmin(new DirectorySearcher(entry), nomUsu, pass, path, domainAndUsername) == "")
                {
                    return false;
                }
                else
                {
                    nombreCompleto = domainAndUsername;
                    this.UserName = nomUsu;
                }
                
                if (dominio == "login")
                {
                    domainAndUsername = "";
                }

                return true;  
            }
            catch (Exception ex)
            {
               
                return false;
            }
        }

        public static string SetAdmin(DirectorySearcher search, string nomUsu, string pass, string path, string domainAndUsername)
        {
            string NombreCompleto;

            search.Filter = "(SAMAccountName=" + nomUsu + ")";
            search.PropertiesToLoad.Add("cn");
            search.PropertiesToLoad.Add("givenname");
            search.PropertiesToLoad.Add("sn");
            search.PropertiesToLoad.Add("mail");
            search.PropertiesToLoad.Add("userPrincipalName");
            search.PropertiesToLoad.Add("memberOf");

            SearchResult result = search.FindOne();
            if (result == null)
            {
                return "";
            }
            else
            {

                path = result.Path;


                NombreCompleto = string.Empty;
                if (result.Properties["givenname"] != null && result.Properties["givenname"].Count > 0)
                {
                    NombreCompleto += result.Properties["givenname"][0].ToString();
                }
                if (result.Properties["sn"] != null && result.Properties["sn"].Count > 0)
                {
                    NombreCompleto += string.Format(" {0}", result.Properties["sn"][0]);
                }
                if (NombreCompleto.Length == 0 && result.Properties["displayName"] != null && result.Properties["displayName"].Count > 0)
                {
                    NombreCompleto = result.Properties["displayName"][0].ToString();
                }
                if (NombreCompleto.Length == 0)
                {
                    NombreCompleto = domainAndUsername;
                }

            }
            return NombreCompleto;
        }

        /// <summary>
        /// Verificar la existencia de un usuario a través de su Login
        /// </summary>
        /// <param name="login">Login en DA</param>
        /// <returns></returns>
        public static bool BuscarUsuario(string login)
        {
            try
            {
                DirectoryEntry des = _entryDA;
                DirectorySearcher buscador = new DirectorySearcher(des);
                buscador.Filter = string.Format("(SAMAccountName={0})", login);
                SearchResult sr = buscador.FindOne();
                if (sr != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        
        /// <summary>
        /// Obtener el nombre de un usuario en DA através de su Login
        /// </summary>
        /// <param name="login">Login en DA</param>
        /// <returns></returns>
        public static string ObtenerNombre(string login)
        {
            try
            {
                string searchFilter = string.Format(@"(&(samAccountName={0})(objectCategory=person)(objectClass=user))", login);
                SearchResult result;
                string nombre;
                using (DirectoryEntry root = _entryDA)
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(root))
                    {
                        searcher.Filter = searchFilter;
                        SearchResultCollection results = searcher.FindAll();

                        result = (results.Count != 0) ? results[0] : null;

                        if (result != null)
                        {
                            nombre = result.Path.Replace("\\", "");
                            nombre = nombre.Substring(nombre.IndexOf("CN=") + 3, nombre.Length - nombre.IndexOf("CN=") - 3);
                            nombre = nombre.Substring(0, nombre.IndexOf("=") - 3);
                            return nombre;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
            return string.Empty;
        }
        


        #region New Methods ...

        //TODO: SOL54125 Bitacora

        public enum ADProperties
        {
            distinguishedName,
            displayName,
            telephoneNumber,
            samAccountName,
            manager,
            title,
            department,
            givenName,
            sn,
            Initials,
            mail
        }
        
        /// <summary>
        /// Obtener información del Directorio Activo para un usuario.
        /// </summary>
        /// <param name="userId">No. Empleado (Initials)</param>
        /// <returns></returns>
        public static ActiveDirectoryData GetUserDataById(int userId)
        {
            ActiveDirectoryData objSearchResult = new ActiveDirectoryData();

            try
            {
                DirectorySearcher search = new DirectorySearcher(_entryDA);
                SearchResultCollection resultCollection;

                LoadProperties(ref search);

                search.Filter = "(" + ADProperties.Initials + "=" + userId + ")";
                resultCollection = search.FindAll();

                if (resultCollection != null)
                {
                    if (resultCollection.Count == 1)
                    {
                        MapToObject(resultCollection[0], ref objSearchResult);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objSearchResult;
        }

        /// <summary>
        /// Obtener información del Directorio Activo para un usuario.
        /// </summary>
        /// <param name="userName">Login de usuario</param>
        /// <returns></returns>
        public static ActiveDirectoryData GetUserDataByUserName(string userName)
        {
            ActiveDirectoryData objSearchResult = new ActiveDirectoryData();

            try
            {
                DirectorySearcher search = new DirectorySearcher(_entryDA);
                SearchResultCollection resultCollection;

                LoadProperties(ref search);

                search.Filter = "(" + ADProperties.samAccountName + "=" + userName + ")";
                resultCollection = search.FindAll();

                if (resultCollection != null)
                {
                    if (resultCollection.Count == 1)
                    {
                        MapToObject(resultCollection[0], ref objSearchResult);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objSearchResult;
        }


        /// <summary>
        /// Asignar los resultados del directorio activo con cada propiedad de la clase ActiveDirectoryData
        /// </summary>
        /// <param name="result"></param>
        /// <param name="objUser"></param>
        private static void MapToObject(SearchResult result, ref ActiveDirectoryData objSearchResult)
        {
            if (result.Properties["title"].Count > 0)
                objSearchResult.Title = result.Properties["title"][0].ToString();

            if (result.Properties["distinguishedName"].Count > 0)
                objSearchResult.DistinguishedName = result.Properties["distinguishedName"][0].ToString();

            if (result.Properties["displayName"].Count > 0)
                objSearchResult.DisplayName = result.Properties["displayname"][0].ToString();

            if (result.Properties["telephoneNumber"].Count > 0)
                objSearchResult.TelephoneNumber = result.Properties["telephoneNumber"][0].ToString();

            if (result.Properties["samAccountName"].Count > 0)
                objSearchResult.SamAccountName = result.Properties["samAccountName"][0].ToString();

            if (result.Properties["department"].Count > 0)
                objSearchResult.Department = result.Properties["department"][0].ToString();

            if (result.Properties["givenName"].Count > 0)
                objSearchResult.FirstName = result.Properties["givenName"][0].ToString();

            if (result.Properties["sn"].Count > 0)
                objSearchResult.LastName = result.Properties["sn"][0].ToString();

            if (result.Properties["Initials"].Count > 0)
                objSearchResult.Initials = result.Properties["Initials"][0].ToString();

            if (result.Properties["mail"].Count > 0)
                objSearchResult.Mail = result.Properties["mail"][0].ToString();
        }


        /// <summary>
        /// Carga las propiedades para recuperarlas en un resultado de búsqueda.
        /// </summary>
        /// <param name="search"></param>
        private static void LoadProperties(ref DirectorySearcher search)
        {
            search.PropertiesToLoad.Add("title");
            search.PropertiesToLoad.Add("distinguishedName");
            search.PropertiesToLoad.Add("samAccountName");
            search.PropertiesToLoad.Add("displayName");
            search.PropertiesToLoad.Add("telephoneNumber");
            search.PropertiesToLoad.Add("manager");
            search.PropertiesToLoad.Add("department");
            search.PropertiesToLoad.Add("givenName");
            search.PropertiesToLoad.Add("sn");
            search.PropertiesToLoad.Add("Initials");
            search.PropertiesToLoad.Add("mail");
        }


        #endregion


    
    }
}
