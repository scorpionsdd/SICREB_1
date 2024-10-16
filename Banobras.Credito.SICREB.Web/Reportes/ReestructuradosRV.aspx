<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="ReestructuradosRV.aspx.cs" Inherits="ReestructuradosRV" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

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
			<td id="tdRubroAll" class="titleCenter">&nbsp;<asp:Label runat="server" ID="lblTitle" Text="CRÉDITOS REESTRUCTURADOS SIN CLAVE DE OBSERVACIÓN RV"></asp:Label></td>
			<td id="tdRubroDer" class="titleRight">&nbsp;</td>
		</tr>
	</table>
	 <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
		
		<tr>
			<td colspan="3" id="td1" width="100%">
            
	<telerik:RadGrid runat ="server" ID ="RgdCredCveObs" AutoGenerateColumns ="false"
		AllowSorting ="true" AllowPaging ="true" PageSize="10" 
		onneeddatasource="RgdCredCveObs_NeedDataSource" 
		AllowFilteringByColumn="True" oneditcommand="RgdCredCveObs_EditCommand" 
		onupdatecommand="RgdCredCveObs_UpdateCommand" 
		oncolumncreated="RgdCredCveObs_ColumnCreated" 
		ondeletecommand="RgdCredCveObs_DeleteCommand" 
		 onitemdatabound="RgdCredCveObs_ItemDataBound" 
					onitemcreated="RgdCredCveObs_ItemCreated" >
                     <ExportSettings FileName="ReestructuradosRV" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="Reporte Reestructurados RV" />
                    </ExportSettings>
	<FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
					<PagerStyle Mode="NextPrevAndNumeric"  AlwaysVisible="true" 
							FirstPageToolTip="Primera Página"  
							PrevPageToolTip="Página Anterior"  
							NextPageToolTip="Página Siguiente"  
							LastPageToolTip="Última Página" 
							PageSizeLabelText="Registros por página: " 
							PagerTextFormat="{4} Consultando página {0} de {1}, <span id='tot'> {5}</span> Registros en {1} páginas." />
                            <GroupingSettings CaseSensitive="false" />
		<MasterTableView Name ="MtvCredCveObs" DataKeyNames="Id" NoMasterRecordsText ="No existen Registros" CommandItemDisplay="Top" EditMode="InPlace"  Width="100%" >
		<CommandItemSettings ShowRefreshButton = "true"  RefreshText="Actualizar Datos" />
		<Columns>
							<Telerik:GridEditCommandColumn HeaderStyle-Width="10%" Visible="false" ItemStyle-Width="10%" FooterStyle-Width="10%" ButtonType="ImageButton"  UniqueName="EditCommandColumn"
								EditImageUrl = "../App_Themes/Banobras2011/Grid/Edit.gif" EditText="Editar" HeaderText="Editar" 
								InsertImageUrl ="../App_Themes/Banobras2011/Grid/Agregar.png" InsertText="Insertar"
								UpdateImageUrl="../App_Themes/Banobras2011/Grid/Aceptar.png" UpdateText="Actualizar"
								CancelImageUrl="../App_Themes/Banobras2011/Grid/Cancelar.png" CancelText="Cancelar">
							</Telerik:GridEditCommandColumn > 
	
								<Telerik:GridBoundColumn HeaderText="Id" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Justify"
								UniqueName="Id" DataField="Id" 
								DataType="System.Int32" MaxLength="10" Visible ="false" >
							</Telerik:GridBoundColumn>
													
							 <Telerik:GridBoundColumn HeaderText="Crédito" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Justify"
								UniqueName="Credito" DataField="CREDITO" 
								DataType="System.String  " MaxLength="70" >
							</Telerik:GridBoundColumn>

                             <Telerik:GridBoundColumn HeaderText="RFC" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Justify"
								UniqueName="RFC" DataField="RFC" 
								DataType="System.String  " MaxLength="70" >
							</Telerik:GridBoundColumn>

                             <Telerik:GridBoundColumn HeaderText="Nombre" HeaderStyle-Width="30%" ItemStyle-Width="30%" HeaderStyle-HorizontalAlign="Justify"
								UniqueName="Nombre" DataField="Nombre" 
								DataType="System.String  " MaxLength="70" >
							</Telerik:GridBoundColumn>

							<Telerik:GridBoundColumn HeaderText="Clave de Observación" HeaderStyle-Width="20%" ItemStyle-Width="20%" HeaderStyle-HorizontalAlign="Justify"
								UniqueName="IdCvesObservacion" DataField="CveExterna" 
								DataType="System.String" MaxLength="10" Visible ="false" >
							</Telerik:GridBoundColumn>

							
							<telerik:GridTemplateColumn HeaderText="Clave de Observación" UniqueName="CveObservacion" AllowFiltering="true" SortExpression = "CveObservacion" 
							DataType="System.String" Visible = "false" >
								<HeaderStyle Width="30%" />
								<ItemTemplate>
									<telerik:RadComboBox runat ="server" ID = "ComboObservacion">
									</telerik:RadComboBox>
								</ItemTemplate>
							</telerik:GridTemplateColumn>


							<telerik:GridTemplateColumn HeaderText="Estatus" UniqueName="EstatusTemp" AllowFiltering="false" SortExpression = "EstatusTemp" Visible ="false">
								<HeaderStyle Width="30%" />
								<ItemTemplate>
									<telerik:RadComboBox runat ="server" ID = "ComboEstatus">
									<Items> 
									<telerik:RadComboBoxItem Text ="Activo" Value = "0" />
									<telerik:RadComboBoxItem Text ="Inactivo" Value = "1" />
									</Items>
									</telerik:RadComboBox>
								</ItemTemplate>
							</telerik:GridTemplateColumn>
							
							<Telerik:GridBoundColumn HeaderText="Estatus" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Justify"
								UniqueName="Estatus" DataField="Estatus" 
								DataType="System.String  " MaxLength="10" Visible ="false" >
							</Telerik:GridBoundColumn>

							  <Telerik:GridTemplateColumn HeaderText="" Visible="false"> 
           <HeaderTemplate>
           <asp:CheckBox ID="ChkTodo" text="TODOS" AutoPostBack="true" OnCheckedChanged="ChkTodo_CheckedChanged" runat="server"  />
           </HeaderTemplate>
           <ItemTemplate>  
           <asp:CheckBox ID="chk" runat="server" CssClass="chks"/>               
           </ItemTemplate> 
           </Telerik:GridTemplateColumn>
						</Columns>
                         </MasterTableView>

	</telerik:RadGrid> 
			</td>
		</tr>
	</table>
</asp:Content>

