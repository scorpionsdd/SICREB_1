using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections;
				

public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

     

        StreamReader objReader = new StreamReader(Server.MapPath("~")+"/Logs/log.txt");
        string sLine = "";
        string sCadena = string.Empty;

        while (sLine != null)
        {
            sLine = objReader.ReadLine();
            if (sLine != null)
                sCadena = sCadena + sLine ;
        }
        objReader.Close();

        tbCadena.Text = sCadena;

    }
}