using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Entities;
using System.Data.Common;
using Banobras.Credito.SICREB.Business;

public partial class Catalogos_ActivarDesactivarIFRS9 : System.Web.UI.Page
{

    private const string AGREGA = "PACCONFIG.SP_INSERT_CONFIG";
    ConfiguracionRules configuracionReglas = new ConfiguracionRules();

    protected void Page_Load(object sender, EventArgs e)
    {
        int idUsuario;
        try
        {
            if (!IsPostBack)
            {
                idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());

                Configuracion sicofin = configuracionReglas.GetRecords("SICOFIN");
                Configuracion sic = configuracionReglas.GetRecords("SIC");
                Configuracion calificacion = configuracionReglas.GetRecords("CALIFICACIONES");

                
                if (sicofin.Catsicofin == 1)
                {
                    rbSicofin.SelectedValue = "CT_SICO";
                }else if(sicofin.Catsicofin == 3)
                {
                    //rbSicofin.SelectedValue="CT_2021";
                }
                else
                {
                    rbSicofin.SelectedValue = "CN_SICO";
                }

                //PSL 02/02/2022 en productivo solo existen cuentas IFRS9. No hay Switch
                /*if (sic.Catsic == 1)
                {
                    rbSic.SelectedValue = "CT_SIC";
                }
                else
                {
                    rbSic.SelectedValue = "CN_SIC";
                }

                if (calificacion.Calificacion == 1)
                {
                    rbCalif.SelectedValue = "CT_PCC";
                }
                else
                {
                    rbCalif.SelectedValue = "CN_PCC";
                }*/
                
                if (Session["Facultades"] != null)
                {

                }
                else
                {
                    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario Válido", "~/Login.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }

    protected void rbSICOFIN_SelectedIndexChanged(object sender, EventArgs e)
    {

        int idUsuario;
        idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());
        if (rbSicofin.SelectedValue.ToString() == "CT_SICO")
        {
            try
            {
                if (configuracionReglas.GuardarConfiguracion(1, 0, 0, idUsuario))
                {

                    Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se guardó exitosamente");
                }
                else
                {
                    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue guardado");
                }
            }
            catch (Exception ex)
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue guardado");

            }


        }
        else if (rbSicofin.SelectedValue.ToString() == "CT_2021")
        {

            //if (configuracionReglas.GuardarConfiguracion(3, 0, 0, idUsuario))
            //{
            //    Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se guardó exitosamente");
            //}
            //else
            //{
            //    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue guardado");
            //}
        }
        else
        {

            if (configuracionReglas.GuardarConfiguracion(2, 0, 0, idUsuario))
            {
                Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se guardó exitosamente");
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue guardado");
            }
        }


    }

    protected void rbSIC_SelectedIndexChanged(object sender, EventArgs e)
    {

        int idUsuario;
        idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());

        if (rbSic.SelectedValue.ToString() == "CT_SIC")
        {
            try
            {
                if (configuracionReglas.GuardarConfiguracion(0, 1 , 0, idUsuario))
                {

                    Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se guardó exitosamente");
                }
                else
                {
                    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue guardado");
                }
            }
            catch (Exception ex)
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue guardado");

            }


        }
        else
        {
            try
            {
                if (configuracionReglas.GuardarConfiguracion(0, 2, 0, idUsuario))
                {

                    Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se guardó exitosamente");
                }
                else
                {
                    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue guardado");
                }
            }
            catch (Exception ex)
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue guardado");

            }

        }

    }

    protected void rbCalif_SelectedIndexChanged(object sender, EventArgs e)
    {

        int idUsuario;
        idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());

        if (rbCalif.SelectedValue.ToString() == "CT_PCC")
        {

            try
            {
                if (configuracionReglas.GuardarConfiguracion(0, 0, 1, idUsuario))
                {

                    Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se guardó exitosamente");
                }
                else
                {
                    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue guardado");
                }
            }
            catch (Exception ex)
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue guardado");

            }

        }
        else
        {

            try
            {
                if (configuracionReglas.GuardarConfiguracion(0, 0, 2, idUsuario))
                {

                    Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se guardó exitosamente");
                }
                else
                {
                    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue guardado");
                }
            }
            catch (Exception ex)
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue guardado");

            }

        }

    }


}