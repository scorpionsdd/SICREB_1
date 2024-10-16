using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

public partial class Admin_ViewSystemEventLog : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        EventLog aLog = new EventLog();
        aLog.Log = "System";
        aLog.MachineName = ".";

        grdLog.DataSource = aLog.Entries;
        grdLog.DataBind();

    }
}