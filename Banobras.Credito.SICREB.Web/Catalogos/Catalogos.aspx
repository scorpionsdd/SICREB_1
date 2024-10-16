<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Catalogos.aspx.cs" Inherits="Catalogos_Catalogos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <table width="100%" cellpadding="0" cellspacing="0" style="border: 1px solid #696969;">
        <tr>
            <td id="tdRubroIzq" class="titleLeft">
            </td>
            <td id="tdRubroAll" class="titleCenter">
                &nbsp; 
                <asp:Label runat="server" ID="lbltitle" Text="CATÁLOGOS GENERALES" />
            </td>
            <td id="tdRubroDer" class="titleRight">
                &nbsp;
            </td>
        </tr>
        <tr style="width: 100%">
            <td>
                &nbsp;
            </td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HyperLink13" NavigateUrl="~/Catalogos/SEPOMEXpage.aspx" CssClass="listado" Text="SEPOMEX" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td style="height: 30px">
                <asp:HyperLink runat="server" ID="HyperLink7" NavigateUrl="~/Catalogos/Exceptuadospage.aspx" CssClass="listado" Text="Catálogo de Créditos Exceptuados" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HyperLink2" NavigateUrl="~/Catalogos/CreditosClaveObservacionPage.aspx?Persona=PF" CssClass="listado" Text="Catálogo de Créditos con Claves de Observación" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HyperLink12" NavigateUrl="~/Catalogos/Bitacora.aspx" CssClass="listado" Text="Bitácora" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HLink_alertas" NavigateUrl="~/Catalogos/AlertasPage.aspx"  CssClass="listado" Text="Catálogo de Alertas" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HLink_alertasActivas" NavigateUrl="~/Catalogos/ConfiguracionEnviaAlerta.aspx?Persona=PF" CssClass="listado" Text="Configuración Alertas" />
            </td>
            <td>
            </td>
        </tr>
       
        <tr>
            <td>
            </td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HLink_diasInhabiles" NavigateUrl="~/Catalogos/DiasInhabilesPage.aspx" CssClass="listado" Text="Catálogo de Días Inhábiles" />
            </td>
            <td>
            </td>
        </tr>
 
        <tr>
            <td>
            </td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HyperLink33" NavigateUrl="~/Catalogos/ConvertidorActPage.aspx" CssClass="listado" Text="Convertidor Actual" />
            </td>
            <td>
            </td>
        </tr>
       
        <tr>
            <td>
            </td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HyperLink34" NavigateUrl="~/Catalogos/ConvertidorActivosPage.aspx" CssClass="listado" Text="Convertidor Activos" />
            </td>
            <td>
            </td>
        </tr>

        <tr>
            <td></td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HyperLink39" NavigateUrl="~/Catalogos/CreditosTasaVariable.aspx" CssClass="listado" Text="Catálogo de Créditos de Tasa Variable" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>

        <tr>
            <td></td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HyperLink40" NavigateUrl="~/Catalogos/CreditosCompensados.aspx" CssClass="listado" Text="Catálogo de Créditos Compensados" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>

    </table>
    <br />

    <table width="100%" cellpadding="0" cellspacing="0" style="border: 1px solid #696969;">
        <tr>
            <td id="td1" class="titleLeft">
                &nbsp;
            </td>
            <td id="td2" class="titleCenter">
                &nbsp;
                <asp:Label runat="server" ID="Label1" Text="PERSONAS FÍSICAS" />
            </td>
            <td id="td3" class="titleRight">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td style="height: 30px">
                <asp:HyperLink runat="server" ID="HyperLink43" NavigateUrl="~/Catalogos/ClientesPF.aspx" CssClass="listado" Text="Catálogo Intermedio de FS-Clientes" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>

        <tr>
            <td>
            </td>
            <td style="height: 30px">
                <asp:HyperLink runat="server" ID="HyperLink14" NavigateUrl="~/Catalogos/CuentasPage.aspx?Persona=PF" CssClass="listado" Text="Catálogo de Cuentas" />
            </td>
            <td>
            </td>
        </tr>
  
        <tr>
            <td>
            </td>
            <td style="height: 30px">
                <asp:HyperLink runat="server" ID="HyperLink31" NavigateUrl="~/Catalogos/CuentasActPage.aspx?Persona=PF" CssClass="listado" Text="Catálogo de Cuentas Actuales" />
            </td>
            <td>
            </td>
        </tr>
 
        <tr>
            <td>
            </td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HyperLink1" NavigateUrl="~/Catalogos/ClavesObservacionPage.aspx?Persona=PF" CssClass="listado" Text="Catálogo de Claves de Observación" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td style="height: 30px">
                <asp:HyperLink runat="server" ID="HyperLink4" NavigateUrl="~/Catalogos/EstadosPage.aspx?Persona=PF" CssClass="listado" Text="Catálogo de Estados" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HyperLink3" NavigateUrl="~/Catalogos/EstadoCivilPage.aspx" CssClass="listado" Text=" Catálogo de Estado Civil" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td style="height: 30px">
                <asp:HyperLink runat="server" ID="HyperLink5" NavigateUrl="~/Catalogos/FormasPagosPage.aspx" CssClass="listado" Text="Catálogo de Forma de Pago (MOP)" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HyperLink26" NavigateUrl="~/Catalogos/NacionalidadPage.aspx?Persona=f" CssClass="listado" Text="Catálogo de Nacionalidad" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td style="height: 30px">
                <asp:HyperLink runat="server" ID="HyperLink30" NavigateUrl="~/Catalogos/RfcFallecidasPage.aspx" CssClass="listado" Text="Catálogo de Defunción" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HyperLink6" NavigateUrl="~/Catalogos/clavespaispage.aspx?Persona=f" CssClass="listado" Text="Catálogo de País" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td style="height: 30px">
                <asp:HyperLink runat="server" ID="HyperLink24" NavigateUrl="~/Catalogos/AvisoRechazo.aspx?Persona=PF" CssClass="listado" Text="Catálogo de Validaciones" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <br />

    <table width="100%" cellpadding="0" cellspacing="0" style="border: 1px solid #696969;">
        <tr>
            <td id="td4" class="titleLeft">
            </td>
            <td id="td5" class="titleCenter">
                &nbsp;
                <asp:Label runat="server" ID="Label2" Text="PERSONAS MORALES" />
            </td>
            <td id="td6" class="titleRight">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td style="height: 30px">
                <asp:HyperLink runat="server" ID="HyperLink44" NavigateUrl="~/Catalogos/ClientesPM.aspx" CssClass="listado" Text="Catálogo Intermedio de FS-Clientes" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>


        <tr>
            <td>
            </td>
            <td style="height: 30px">
                <asp:HyperLink runat="server" ID="HyperLink21" NavigateUrl="~/Catalogos/ActividadesEconomicasBanxicoPage.aspx" CssClass="listado" Text="Catálogo de Actividad Económica Banxico" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HyperLink38" NavigateUrl="~/Catalogos/TipoAcreditadosPage.aspx" CssClass="listado" Text="Catálogo de Acreditados (PM)" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HyperLink27" NavigateUrl="~/Catalogos/AccionistasPage.aspx" CssClass="listado" Text="Catálogo de Accionistas" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HyperLink35" NavigateUrl="~/Catalogos/AvalesPage.aspx" CssClass="listado" Text="Catálogo de Avales" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HyperLink36" NavigateUrl="~/Catalogos/TipoRelacionesPage.aspx" CssClass="listado" Text="Catálogo de Tipos de Relaciones Acreditado-Accionista" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td style="height: 30px">
                <asp:HyperLink runat="server" ID="HyperLink23" NavigateUrl="~/Catalogos/CalificacionCarteraPage.aspx" CssClass="listado" Text="Catálogo de Calificación de Cartera" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HyperLink18" NavigateUrl="~/Catalogos/ClavesObservacionPage.aspx?Persona=PM" CssClass="listado" Text="Catálogo de Claves de Observación" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td style="height: 30px">
                <asp:HyperLink runat="server" ID="HyperLink20" NavigateUrl="~/Catalogos/CreditosCFiduciariaPage.aspx" CssClass="listado" Text="Catálogo de Créditos en Contabilidad Fiduciaria" /> 
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HyperLink29" NavigateUrl="~/Catalogos/CreditosAuxiliaresPage.aspx" CssClass="listado" Text="Catálogo de Créditos con Auxiliar Diferente en SIC y SICOFIN" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td style="height: 30px">
                <asp:HyperLink runat="server" ID="HyperLink22" NavigateUrl="~/Catalogos/CuentasPage.aspx?Persona=PM" CssClass="listado" Text="Catálogo de Cuentas" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
 
        <tr>
            <td>
            </td>
            <td style="height: 30px">
                <asp:HyperLink runat="server" ID="HyperLink32" NavigateUrl="~/Catalogos/CuentasActPage.aspx?Persona=PM" CssClass="listado" Text="Catálogo de Cuentas Actuales" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HyperLink17" NavigateUrl="~/Catalogos/EstadosPage.aspx?Persona=PM" CssClass="listado" Text="Catálogo de Estados" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td style="height: 30px">
                <asp:HyperLink runat="server" ID="HyperLink9" NavigateUrl="~/Catalogos/frecuenciapagospage.aspx" CssClass="listado" Text="Catálogo de Frecuencias de Pago de Interés" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HyperLink25" NavigateUrl="~/Catalogos/NacionalidadPage.aspx?Persona=m" CssClass="listado" Text="Catálogo de Nacionalidad" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td style="height: 30px">
                <asp:HyperLink runat="server" ID="HyperLink8" NavigateUrl="~/Catalogos/numeropagospage.aspx" CssClass="listado" Text="Catálogo de Número de Pagos" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HyperLink10" NavigateUrl="~/Catalogos/clavespaispage.aspx?Persona=m" CssClass="listado" Text="Catálogo de País" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td style="height: 30px">
                <asp:HyperLink runat="server" ID="HyperLink11" NavigateUrl="~/Catalogos/monedaspage.aspx" CssClass="listado" Text="Catálogo de Tipo de Moneda" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HyperLink16" NavigateUrl="~/Catalogos/TipoCreditosPage.aspx" CssClass="listado" Text="Catálogo de Tipo de Crédito" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td style="height: 30px">
                <asp:HyperLink runat="server" ID="HyperLink28" NavigateUrl="~/Catalogos/BanxicoTiposPage.aspx" CssClass="listado" Text="Catálogo de Tipos de Actividad Económica Banxico" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HyperLink15" NavigateUrl="~/Catalogos/AvisoRechazo.aspx?Persona=PM" CssClass="listado" Text="Catálogo de Validaciones" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
  
        <tr>
            <td></td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HyperLink19" NavigateUrl="~/Catalogos/IdentificadorPage.aspx?Persona=PM" CssClass="listado" Text="Catálogo de Dígito Identificador" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>

 
        <tr>
            <td></td>
            <td class="sombra">
                <asp:HyperLink runat="server" ID="HyperLink37" NavigateUrl="~/Catalogos/BonoCuponCeroPage.aspx" CssClass="listado" Text="Catálogo de Créditos con Bono Cupon Cero" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>

    </table>
    <br />

</asp:Content>
