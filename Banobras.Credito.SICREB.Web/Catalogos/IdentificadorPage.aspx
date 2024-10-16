<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="IdentificadorPage.aspx.cs" Inherits="IdentificadorPage" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<script type="text/javascript">
    function msj() {

        var chk = document.getElementById('MainContent_RgdIdentificador_ctl00_ctl02_ctl02_ChkTodo');
        var total = document.getElementById('tot');
          var chks = getElementsByClassNameIE("chks");
        if (chk.checked) {
            var answer = confirm("Esta seguro que desea eliminar " + total.innerHTML + " Registros de Esta Tabla");
            if (answer) {
                return true;
            }
            else {
              
                return false;
            }
        }else{
          var num = 0;
          for (i = 0, j = 0; i < chks.length; i++) {
              chk1 = chks[i].getElementsByTagName('input')[0];
                    if (chk1.checked) num++;
                }
                
                
                if (num == 0) {
                    alert("Debe seleccionar al menos un registro para eliminar");
                    return false;
                }
                else {

                    var answer = confirm("Esta seguro que desea eliminar " + num + " Registros de Esta Tabla");
                    if (answer) {
                        return true;
                    }
                    else {
                        return false;
                    }
                
                }

            }
        return false;

    }

     //JAGH
    function OnlyNumbers(event)
     {
         var e = event || evt; // for trans-browser compatibility
         var charCode = e.which || e.keyCode;

         if (charCode > 31 && (charCode < 48 || charCode > 57))
             return false;

         return true;
    }

    //JAGH regresa ceros al inicio del numerico en Pattern
    function AddZeros(sender, eventArgs) 
    {
        var senderVal = sender.get_textBoxValue();
        var iContador = 0;
        var PPattern;
        
        //recorre para saber cuantas posiciones ingresara
        for(i=0; i<senderVal.length; i++)
        {
            if (senderVal.charAt(i) == "0") iContador++;
            else
                break;
            
        }
        
        //ingresa ceros 
        switch(iContador)
        {
            case 1:  PPattern = "0n";  break;
            case 2:  PPattern = "00n";  break;
            case 3:  PPattern = "000n";  break;
            case 4:  PPattern = "0000n";  break;
            case 5:  PPattern = "00000n";  break;
            case 6:  PPattern = "000000n";  break;
            case 7:  PPattern = "0000000n";  break;
            case 8:  PPattern = "00000000n";  break;
            case 9:  PPattern = "000000000n";  break;
            default: PPattern = ""; break;

        }
        
        sender.get_numberFormat().PositivePattern = PPattern;
    }

</script>
<table cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
		<tr>
			<td></td>
			<td width="100%" align="right">
				<asp:ImageButton ID="btnExportPDF" 
					ImageUrl ="~/ResourcesSICREB/Images/BNBPF-ICO-PDF.png" runat="server" Width="22px" 
					onclick="btnExportPDF_Click1" />
				<asp:ImageButton ID="ImageButton1" 
					ImageUrl ="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png" runat="server" Width="22px" 
					onclick="ImageButton1_Click" />
			</td>
			<td></td>
		</tr>
		<tr>
			<td id="tdRubroIzq" class="titleLeft">&nbsp;</td>
			<td id="tdRubroAll" class="titleCenter">&nbsp;<asp:Label runat="server" ID="lblTitle" Text="DIGITO IDENTIFICADOR"></asp:Label></td>
			<td id="tdRubroDer" class="titleRight">&nbsp;</td>
		</tr>
	</table>
	 <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center" style="table-layout:fixed;">
		
		<tr>
			<td colspan="3" id="td1" width="100%">
            <div class="textEntry">
                 Agregar Registros 
               <asp:FileUpload ID="file_txt_layout" runat="server" />
                <asp:Button ID="btn_cargar" runat="server" Text="Cargar" 
                     onclick="btn_cargar_Click" />
                      <span style="left:575px;    width:90px; position:relative" >
                        <asp:Button ID="btn_eliminar" runat="server" Text="Eliminar" onclick="btn_eliminar_Click" OnClientClick="return msj();"/>
                    </span>   
            </div>
	<telerik:RadGrid runat ="server" ID ="RgdIdentificador" AutoGenerateColumns ="false" OnItemCommand="grids_ItemCommand"
		AllowSorting ="true" AllowPaging ="true" PageSize="10" 
		onneeddatasource="RgdIdentificador_NeedDataSource" 
		AllowFilteringByColumn="True" oneditcommand="RgdIdentificador_EditCommand" 
		onupdatecommand="RgdIdentificador_UpdateCommand" 
		oncolumncreated="RgdIdentificador_ColumnCreated"
		oninsertcommand="RgdIdentificador_InsertCommand" 
					onitemdatabound="RgdIdentificador_ItemDataBound" 
					onitemcreated="RgdIdentificador_ItemCreated" >
	<FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
					<PagerStyle Mode="NextPrevAndNumeric"  AlwaysVisible="true" 
							FirstPageToolTip="Primera Página"  
							PrevPageToolTip="Página Anterior"  
							NextPageToolTip="Página Siguiente"  
							LastPageToolTip="Última Página" 
							PageSizeLabelText="Registros por página: " 
							PagerTextFormat="{4} Consultando página {0} de {1},  <span id='tot'> {5}</span>  Registros en {1} páginas." /><GroupingSettings CaseSensitive="false" />
		<MasterTableView Name ="MtvEstados" DataKeyNames="Id"  NoMasterRecordsText ="No existen Registros" CommandItemDisplay="Top" EditMode="InPlace"  Width="100%" >
		<CommandItemSettings ShowRefreshButton = "true"  AddNewRecordText ="Agregar Identificador"  RefreshText="Actualizar Datos" />
		<Columns>
							<Telerik:GridEditCommandColumn  ButtonType="ImageButton"  UniqueName="EditCommandColumn"
								EditImageUrl = "../App_Themes/Banobras2011/Grid/Edit.gif" EditText="Editar" HeaderText="Editar"
								InsertImageUrl ="../App_Themes/Banobras2011/Grid/Agregar.png" InsertText="Insertar"
								UpdateImageUrl="../App_Themes/Banobras2011/Grid/Aceptar.png" UpdateText="Actualizar"
								CancelImageUrl="../App_Themes/Banobras2011/Grid/Cancelar.png" CancelText="Cancelar">
							</Telerik:GridEditCommandColumn> 
	
								<Telerik:GridBoundColumn HeaderText="Id" HeaderStyle-HorizontalAlign="Justify"
								UniqueName="Id" DataField="Id"  Visible ="false"
								DataType="System.Int32" MaxLength="12" >
							</Telerik:GridBoundColumn>
													
							 <Telerik:GridBoundColumn HeaderText="RFC" HeaderStyle-HorizontalAlign="Justify"
								UniqueName="Rfc" DataField="Rfc" 
								DataType="System.String  " MaxLength="25" >
							</Telerik:GridBoundColumn>

							<Telerik:GridNumericColumn HeaderText="No Credito" HeaderStyle-HorizontalAlign="Justify"
								UniqueName="credito" DataField="Credito"  
								DataType="System.String" MaxLength="25" >
							</Telerik:GridNumericColumn>                           

							<Telerik:GridBoundColumn HeaderText="Digito Id" HeaderStyle-HorizontalAlign="Justify"
								UniqueName="DigitoIdentificador" DataField="DigitoIdentificador" 
								DataType="System.String"  MaxLength="25">
							</Telerik:GridBoundColumn>
							
							<Telerik:GridBoundColumn HeaderText="Nombre" HeaderStyle-HorizontalAlign="Justify"
								UniqueName="Nombre" DataField="Nombre" 
								DataType="System.String  " MaxLength="255" >
							</Telerik:GridBoundColumn>

							<Telerik:GridBoundColumn HeaderText="" HeaderStyle-HorizontalAlign="Justify"
								UniqueName="Persona" DataField="Persona" 
								DataType="System.String  " MaxLength="1" Visible ="false">
							</Telerik:GridBoundColumn>

							<telerik:GridTemplateColumn HeaderText="Persona" Visible="false" UniqueName="TipoPersonaTemp" >								
								<ItemTemplate>
									<telerik:RadComboBox runat ="server" ID = "ComboPersona">
									<Items> 
									<telerik:RadComboBoxItem Text ="Fisica" Value = "F" />
									<telerik:RadComboBoxItem Text ="Moral" Value = "M" />
									</Items>
									</telerik:RadComboBox>
								</ItemTemplate>
							</telerik:GridTemplateColumn>

							<Telerik:GridBoundColumn HeaderText="Estatus" HeaderStyle-HorizontalAlign="Justify" AllowFiltering="false" 
								UniqueName="Estatus" DataField="Estatus" 
								DataType="System.String  " MaxLength="1" Visible ="false">
							</Telerik:GridBoundColumn>

							<telerik:GridTemplateColumn HeaderText="Estatus" UniqueName="EstatusTemp" AllowFiltering="false" SortExpression = "EstatusTemp" Visible ="false">
								
								<ItemTemplate>
									<telerik:RadComboBox runat ="server" ID = "ComboEstatus">
									<Items> 
									<telerik:RadComboBoxItem Text ="Activo" Value = "1" />
									<telerik:RadComboBoxItem Text ="Inactivo" Value = "2" />
									</Items>
									</telerik:RadComboBox>
								</ItemTemplate>
							</telerik:GridTemplateColumn>


						
            <Telerik:GridTemplateColumn HeaderText="" AllowFiltering="false"> 
           <HeaderTemplate>
           <asp:CheckBox ID="ChkTodo" text="TODOS" AutoPostBack="true" OnCheckedChanged="ChkTodo_CheckedChanged" runat="server"  />
           </HeaderTemplate>
           <ItemTemplate>  
           <asp:CheckBox ID="chk" runat="server" CssClass="chks"/>             
           </ItemTemplate>
           <EditItemTemplate>&nbsp;</EditItemTemplate> 
           </Telerik:GridTemplateColumn>
			</Columns>
            </MasterTableView>

					
			<ClientSettings>
				<Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="True">
				</Scrolling>
			</ClientSettings>

	</telerik:RadGrid> 
			</td>
		</tr>
	</table>
</asp:Content>

