using Banobras.Credito.SICREB.Business.Repositorios;
using Banobras.Credito.SICREB.Data.Transaccionales;
using Banobras.Credito.SICREB.Entities.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class ConfiguracionEnviaAlerta : System.Web.UI.Page
{
  
    public const String catalog = "Configuración Alertas";  

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {           

            if (Session["Facultades"] != null)
            {                           
                if (!this.Page.IsPostBack)
                {
                    Lllena_drplist();
                    int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GuardarActividad(4444, idUs, "El Usuario " + Session["nombreUser"] + " Ingresó a Catálogo " + catalog);
                }
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");
            }
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), "Load :" + ex.Message.ToString().Replace(Environment.NewLine, "<br>"));
        }     
    }

    protected void btn_aceptarConfigAlerta_Click(object sender, EventArgs e)
    {
        try
        {
            this.ActualizarListaEmails();
            //CheckBox chkCell;
            //bool chkResult;
            StringBuilder MyStringBuilder = new StringBuilder();
            int auxID = Parser.ToNumber(drplist_Alerta.SelectedValue.ToString());
            string auxEmail = "";
            //foreach (GridDataItem row in RgdCorreos.Items)
            //{

            //    chkCell = (CheckBox)row["SELECCION"].FindControl("chk");
            //    chkResult = (bool)chkCell.Checked;


            //    if (chkResult)
            //    {
            //        auxEmail = row["EMAIL"].Text.Trim() + ";";
            //        //auxEmail = row.Cells["EMAIL"].FindControl("EMAIL").ToString() + ";";
            //        MyStringBuilder.Append(auxEmail);
            //    }

            //}

            if (Session["lstEmails"] != null)
            {
                foreach (String sMail in (List<String>)Session["lstEmails"])
                {
                    auxEmail = sMail + ";";
                    MyStringBuilder.Append(auxEmail);
                }
            }

            ConfiguracionTable obj = new ConfiguracionTable();
            string strMsgError = string.Empty;
            obj.addTableConfig(auxID, new StringBuilder(MyStringBuilder.ToString().Substring(0, MyStringBuilder.ToString().Length - 1)), ref strMsgError);
            if (strMsgError.Equals(string.Empty))
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Datos Ingresados Correctamente");
            else
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), strMsgError);

        }
        catch ( Exception ex)
        {
            Mensajes.ShowError(Page, this.GetType(), "btn_aceptarConfigAlerta_Click : " + ex.Message.ToString().Replace(Environment.NewLine, "<br>"));

        }
    }

    public void Lllena_drplist()
    {
        try
        {
            AlertasRules mr = new AlertasRules();
            drplist_Alerta.DataSource = mr.Alertas();
            drplist_Alerta.DataTextField = "TITULOALERTA";
            drplist_Alerta.DataValueField = "ID";
            drplist_Alerta.DataBind();
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(Page, this.GetType(), "Lllena_drplist : " + ex.Message.ToString().Replace(Environment.NewLine, "<br>"));
        }
    }


    protected void RgdCorreos_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        try
        {
            string Dominio = ConfigurationManager.AppSettings["RutaActiveDirectory"].ToString();
            ConfiguracionTable obj = new ConfiguracionTable();

            DataTable tblEmails = obj.CrearTabla();
            //tblEmails.Rows.Add("prueba", "prueba@");
            //RgdCorreos.DataSource = tblEmails;
            RgdCorreos.DataSource = obj.get_Email(Dominio);
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(Page, this.GetType(), "RgdCorreos_NeedDataSource : " + ex.Message.ToString().Replace(Environment.NewLine, "<br>"));          
        }
    }

    /// <summary>
    /// Agregando para manejar el paginador del RadGrid
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void RgdCorreosPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        try
        {
            this.ActualizarListaEmails();

            this.RgdCorreos.CurrentPageIndex = e.NewPageIndex;
            if ((DataTable)Session["SourceEmail"] != null)
            {
                RgdCorreos.DataSource = (DataTable)Session["SourceEmail"];
            }
            else
            {
                //Ya no hay sesión, direccionar a otra página o volver a cargar la lista
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Función para actualizar la lista de correos dependiendo de si selecciona o no un registro
    /// </summary>
    public void ActualizarListaEmails()
    {
        try
        {

        List<String> lstEmails = new List<String>();

        if (Session["lstEmails"] != null)
        {
            lstEmails = (List<String>)Session["lstEmails"];
        }

        foreach (GridDataItem row in RgdCorreos.Items)
        {
            string sEmail = row["EMAIL"].Text.Trim();

            CheckBox ckb = (CheckBox)row.FindControl("chk");

            //Si hay mas de un elemento en la lista, compararla si no, agregar el elemento
            if (lstEmails != null && lstEmails.Count() > 0)
            {
                String sEmailLista = (from mail in lstEmails
                                      where mail.Equals(sEmail)
                                      select mail).SingleOrDefault();

                if (sEmailLista != null)//Quiere decir que está en la lista
                {
                    if (sEmailLista != null)
                    {
                        if (ckb.Checked == false) //Ya no la tiene
                        {
                            //Eliminarla de la lista
                            lstEmails.Remove(sEmailLista);
                        }
                    }

                }
                else
                {
                    if (ckb.Checked == true) //La está agregando
                    {
                        //Agregarla a la lista
                        lstEmails.Add(sEmail);
                    }
                }
            }
            else
            {
                if (ckb.Checked == true) //La está agregando
                {
                    lstEmails.Add(sEmail);
                }
            }        
        }

        if (lstEmails.Count() > 0)
        {
            Session["lstEmails"] = lstEmails;
        }
        else
        {
            Session["lstEmails"] = null;
        }

        }
        catch (Exception ex)
        {
            Mensajes.ShowError(Page, this.GetType(), "ActualizarListaEmails : "+ ex.Message);           
        }
    }

    /// <summary>
    /// Función para que marque los correos que fueron seleccionados previamente
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RgdCorreos_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)// es una row
        {
            if (Session["lstEmails"] != null)
            {
                List<String> lstMails = (List<String>)Session["lstEmails"]; //Lista de todos los correos
                GridDataItem item = (GridDataItem)e.Item; //item es la row
                string Mail = item["EMAIL"].Text.ToString(); //Debe de ser el correo

                String MailMarcado = (from res in lstMails
                                      where res.Equals(Mail)
                                      select res).SingleOrDefault(); //Busco si existe ese Mail en la lista

                if (MailMarcado != null)
                {
                    CheckBox ckb = (CheckBox)item.FindControl("chk");
                    ckb.Checked = true; //Si este Mail está en la lista, marco el checkBox

                }

            }
        }
    }
    
}