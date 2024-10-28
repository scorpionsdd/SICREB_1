using System;

public partial class Error : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string error = this.Request.QueryString["Error"];
        if (error != null)
        {
            this.lblError.Text = error;
            Console.WriteLine(error);
        }
    }
}