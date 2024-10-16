using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Banobras.Credito.SICREB.Data.Transaccionales;

namespace runprocess
{
    public partial class SystemAlerts : System.Web.UI.Page
    {
        protected void btnAlerts_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblMsg.Text = string.Empty;
                ExecSystemAlerts exeSA = new ExecSystemAlerts(DateTime.Now);
                String strMsg = exeSA.ejecutaSystemAlerts();
                this.lblMsg.Text = strMsg;

                if (strMsg.Contains("Error") || strMsg.Contains("error"))
                    this.lblMsg.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.ToString()); 
            }
        }        
    }
}