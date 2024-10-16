using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SessionVariables
/// </summary>
public class SessionVariables
{
	public SessionVariables()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// Lista de facultades asociadas al usuario que inicia sesión
    /// </summary>
    public string Faculties { get; set; }

    /// <summary>
    /// Ip desde dónde se inicia la sesión
    /// </summary>
    public string SessionIP { get; set; }

    /// <summary>
    /// Número de empleado de nómina o directorio activo del usuario que inicia sesión
    /// </summary>
    public string SessionUserEmployeeId { get; set; }

    /// <summary>
    /// Identificador de usuario que inicia sesión
    /// </summary>
    public string SessionUserId { get; set; }

    /// <summary>
    /// Nombre completo del usuario que inicia sesión
    /// </summary>
    public string SessionUserFullName { get; set; }

    /// <summary>
    /// Login del usuario que inicia sesión
    /// </summary>
    public string SessionUserLogin { get; set; }

    //public int MyProperty { get; set; }
    //public int MyProperty { get; set; }
    //public int MyProperty { get; set; }



}