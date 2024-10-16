<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="PersonaFisica.aspx.cs" Inherits="Detalles_PersonaFisica" Theme="Banobras2011" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <!-- JAGH Manager 21/12/12-->
        <telerik:RadAjaxManager ID="RAManager" runat="server" EnableViewState="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rgArchivoFisicas">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgArchivoFisicas" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rgErrores">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgErrores" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rgWarnings">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgWarnings" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <!-- Etiquetas informativas-->
        <div class="divCard">
			<div class="divHeader">
				&nbsp;PROCESO DE PERSONAS FÍSICAS</div>
			<table cellpadding="0px" cellspacing="0px" class="resumenProceso">
				<tr>
					<td>
						&nbsp; Fecha de último proceso:
					</td>
					<td>
						<asp:Label ID="lblFechaArchivo" runat="server"></asp:Label>
					</td>
					<td>
						&nbsp; Errores detectados:
					</td>
					<td>
						<asp:Label ID="lblErrores" runat="server"></asp:Label>
					</td>
					<td>
						&nbsp; Datos correctos:
					</td>
					<td>
						<asp:Label ID="lblCorrectos" runat="server"></asp:Label>
					</td>
				</tr>
				<tr>
					<td>
						&nbsp; Datos procesados:
					</td>
					<td>
						<asp:Label ID="lblProcesados" runat="server"></asp:Label>
					</td>
					<td>
						&nbsp; Warnings:
					</td>
					<td>
						<asp:Label ID="lblWarnings" runat="server"></asp:Label>
					</td>
					<td>
					</td>
					<td>
					</td>
				</tr>
			</table>
		</div>

        <!-- Grid PF-->
		<div class="divCard" >
			<div class="divHeader">
				&nbsp;INFORMACIÓN PROCESADA</div>
			<div class="divExporters">
               <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
                   <tr>
                     <td width="100%" align="right">
                       <asp:ImageButton ID="imgExportArchivoPDF" runat="server" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-PDF.png" onclick="imgExportArchivoPDF_Click" style="height: 22px" />
                        &nbsp;
				      <asp:ImageButton ID="imgExportArchivoXLS" runat="server"  ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png" onclick="imgExportArchivoXLS_Click" />
                     </td>
                   </tr>
                </table>
			</div>			
			<div class="divGrid">
				<telerik:RadGrid ID="rgArchivoFisicas" runat="server" AllowPaging="True" AllowSorting="True"  OnItemCommand="grids_ItemCommand" 
					GridLines="None" AutoGenerateColumns="False"
					OnNeedDataSource="rgArchivoFisicas_NeedDataSource" OnItemDataBound="rgArchivoFisicas_ItemDataBound" AllowFilteringByColumn="true" EnableEmbeddedSkins="false" Skin="Banobras2011">
					<PagerStyle Mode="NextPrevAndNumeric"  AlwaysVisible="true" 
							FirstPageToolTip="Primera Página"  
							PrevPageToolTip="Página Anterior"  
							NextPageToolTip="Página Siguiente"  
							LastPageToolTip="Última Página" 
							PageSizeLabelText="Registros por página: " 
							PagerTextFormat="{4} Consultando página {0} de {1}, {5} Registros en {1} páginas." />
                    <GroupingSettings CaseSensitive="false" />
		
					<ExportSettings FileName="PM_InfoProcesada" IgnorePaging="True" ExportOnlyData="true">
						<Pdf PageWidth="3000px" Title="Información Procesada (PM)" />
					</ExportSettings>
					<MasterTableView ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true" DataKeyNames="Id" PageSize="10" HierarchyLoadMode="ServerOnDemand" Name="Nombre" Width="2500px" >
						<DetailTables>
							
                            <telerik:GridTableView AutoGenerateColumns="false" DataKeyNames="Id" Width="100%" AllowPaging="false" AllowFilteringByColumn="false" Name="Direccion">
								<ParentTableRelation>
									<telerik:GridRelationFields DetailKeyField="PN_ID" MasterKeyField="Id" />
								</ParentTableRelation>
								<Columns>
									<telerik:GridBoundColumn HeaderText="Dirección"              DataField="PA_PA" />
                                    <telerik:GridBoundColumn HeaderText="Colonia o Poblacion"    DataField="PA_01" />
									<telerik:GridBoundColumn HeaderText="Delegación o Municipio" DataField="PA_02" />
									<telerik:GridBoundColumn HeaderText="Ciudad"                 DataField="PA_03" />
									<telerik:GridBoundColumn HeaderText="Estado"                 DataField="PA_04" />
									<telerik:GridBoundColumn HeaderText="Código Postal"          DataField="PA_05" />
									<telerik:GridBoundColumn HeaderText="Fecha Residencia"       DataField="PA_06" UniqueName="FechaResidencia"/>
									<telerik:GridBoundColumn HeaderText="Teléfono"               DataField="PA_07" />
									<telerik:GridBoundColumn HeaderText="Extensión"              DataField="PA_08" />
									<telerik:GridBoundColumn HeaderText="Fax"                    DataField="PA_09" />
									<telerik:GridBoundColumn HeaderText="Tipo Domicilio"         DataField="PA_10" />
									<telerik:GridBoundColumn HeaderText="Especial"               DataField="PA_11" />									
								</Columns>
								<EditFormSettings>
									<EditColumn InsertImageUrl="../App_Themes/Banobras2011/Grid/Agregar.png" UpdateImageUrl="../App_Themes/Banobras2011/Grid/Aceptar.png" EditImageUrl="../App_Themes/Banobras2011/Grid/Edit.gif" CancelImageUrl="../App_Themes/Banobras2011/Grid/Cancelar.png" />
								</EditFormSettings>
								<PagerStyle AlwaysVisible="True" />
							</telerik:GridTableView>

							<telerik:GridTableView AutoGenerateColumns="false" DataKeyNames="Id" Width="100%"  AllowPaging="false" AllowFilteringByColumn="false">
								<ParentTableRelation>
									<telerik:GridRelationFields DetailKeyField="PN_ID" MasterKeyField="Id" />
								</ParentTableRelation>
								<Columns>
									<telerik:GridBoundColumn HeaderText="Razón Social"            DataField="PE_PE" />
									<telerik:GridBoundColumn HeaderText="Dirección"               DataField="DireccionCompleta" />
									<telerik:GridBoundColumn HeaderText="Delegacion/Municipio"    DataField="PE_03" />
									<telerik:GridBoundColumn HeaderText="Ciudad"                  DataField="PE_04" />
									<telerik:GridBoundColumn HeaderText="Estado"                  DataField="PE_05" />
									<telerik:GridBoundColumn HeaderText="CP"                      DataField="PE_06" />
									<telerik:GridBoundColumn HeaderText="Teléfono"                DataField="PE_07" />
									<telerik:GridBoundColumn HeaderText="Extensión"               DataField="PE_08" />
									<telerik:GridBoundColumn HeaderText="Fax"                     DataField="PE_09" />
									<telerik:GridBoundColumn HeaderText="Teléfono"                DataField="PE_07" />
									<telerik:GridBoundColumn HeaderText="Cargo"                   DataField="PE_10" />
									<telerik:GridBoundColumn HeaderText="Fecha con."              DataField="PE_11" />
									<telerik:GridBoundColumn HeaderText="Moneda"                  DataField="PE_12" />
									<telerik:GridBoundColumn HeaderText="Salario"                 DataField="PE_13" />
									<telerik:GridBoundColumn HeaderText="Base Salarial"           DataField="PE_14" />
									<telerik:GridBoundColumn HeaderText="No. Empleados"           DataField="PE_15" />
									<telerik:GridBoundColumn HeaderText="Fecha último dia empleo" DataField="PE_16" />
									<telerik:GridBoundColumn HeaderText="Fecha verificación"      DataField="PE_17" />
								</Columns>
								<EditFormSettings>
									<EditColumn InsertImageUrl="Update.gif" UpdateImageUrl="Update.gif" EditImageUrl="Edit.gif" CancelImageUrl="Cancel.gif" />
								</EditFormSettings>
								<PagerStyle Mode="NextPrevAndNumeric"  AlwaysVisible="true" 
							                FirstPageToolTip="Primera Página"  
							                PrevPageToolTip="Página Anterior"  
							                NextPageToolTip="Página Siguiente"  
							                LastPageToolTip="Última Página" 
							                PageSizeLabelText="Registros por página: " 
							                PagerTextFormat="{4} Consultando página {0} de {1}, {5} Registros en {1} páginas." />
							</telerik:GridTableView>

							<telerik:GridTableView AutoGenerateColumns="false" DataKeyNames="Id" Width="100%" AllowPaging="false" AllowFilteringByColumn="false" name="Creditos" >
								<ParentTableRelation>
									<telerik:GridRelationFields DetailKeyField="PN_ID" MasterKeyField="Id" />
								</ParentTableRelation>
								<Columns>
									<telerik:GridBoundColumn HeaderText="No.cuenta"                      DataField="TL_04" />
                                    <telerik:GridBoundColumn HeaderText="Responsabilidad"                DataField="TL_05" />
									<telerik:GridBoundColumn HeaderText="Tipo cuenta"                    DataField="TL_06" />
									<telerik:GridBoundColumn HeaderText="Tipo contrato"                  DataField="TL_07" UniqueName="TipoContrato" />
                                    <telerik:GridBoundColumn HeaderText="Clave Unidad Monetaria"         DataField="TL_08" />
									<telerik:GridBoundColumn HeaderText="No.pagos"                       DataField="TL_10" />
									<telerik:GridBoundColumn HeaderText="Frec. pagos"                    DataField="TL_11" />
									<telerik:GridBoundColumn HeaderText="Monto"                          DataField="TL_12" UniqueName="Monto" />
									<telerik:GridBoundColumn HeaderText="Fecha apertura"                 DataField="TL_13" UniqueName="FechaApertura"/>
									<telerik:GridBoundColumn HeaderText="Fecha último pago"              DataField="TL_14" UniqueName="FechaUltimoPago"/>
									<telerik:GridBoundColumn HeaderText="Fecha última compra"            DataField="TL_15" UniqueName="FechaUltimaCompra" />
                                    <telerik:GridBoundColumn HeaderText="Fecha de Cierre"                DataField="TL_16" UniqueName="FechaCierre" />
                                    <telerik:GridBoundColumn HeaderText="Fecha Reporte"                  DataField="TL_17" />
									<telerik:GridBoundColumn HeaderText="Garantía"                       DataField="TL_20" UniqueName="Garantia"/>
									<telerik:GridBoundColumn HeaderText="Crédito máximo"                 DataField="TL_21" UniqueName="CreditoMaximo"/>
									<telerik:GridBoundColumn HeaderText="Saldo actual"                   DataField="TL_22" UniqueName="SaldoActual"/>
									<telerik:GridBoundColumn HeaderText="Saldo vencido"                  DataField="TL_24" UniqueName="SaldoVencido"/>
									<telerik:GridBoundColumn HeaderText="Pagos vencidos"                 DataField="TL_25" />
									<telerik:GridBoundColumn HeaderText="Forma pago actual"              DataField="TL_26" />
                                    <telerik:GridBoundColumn HeaderText="Fecha de Primer Incumplimiento" DataField="TL_43" />
                                    <telerik:GridBoundColumn HeaderText="Saldo Insoluto"                 DataField="TL_22" UniqueName="SaldoInsoluto" />
                                    <telerik:GridBoundColumn HeaderText="Clave de Observacion"           DataField="TL_30" />
                                    <telerik:GridBoundColumn HeaderText="Fecha de Ingreso a Cartera Vencida"  DataField="TL_46" />
                                    <telerik:GridBoundColumn HeaderText="Monto Inereses"                 DataField="TL_47" UniqueName="MontoIntereses" />
                                    <telerik:GridBoundColumn HeaderText="Forma de Pago Int."             DataField="TL_48" />
                                    <telerik:GridBoundColumn HeaderText="Días de vencido Int."           DataField="TL_49" />
                                    <%--//TODO: SOL53051 => Campos telefono y mail obligatorios--%>
                                    <telerik:GridBoundColumn HeaderText="Correo"                         DataField="TL_52" />

								</Columns>
								<EditFormSettings>
									<EditColumn InsertImageUrl="Update.gif" UpdateImageUrl="Update.gif" EditImageUrl="Edit.gif" CancelImageUrl="Cancel.gif" />
								</EditFormSettings>
							</telerik:GridTableView>

						</DetailTables>
						<ExpandCollapseColumn Visible="True" />
						<Columns>
							<telerik:GridBoundColumn HeaderText="Nombre"                        DataField="NombreCompleto" HeaderStyle-Width="300px" ItemStyle-Width="300px" FilterControlWidth="290px" />
							<telerik:GridBoundColumn HeaderText="Fecha Nacimiento"              DataField="PN_04"          HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  UniqueName="FechaNacimiento"/>                            
							<telerik:GridBoundColumn HeaderText="RFC"                           DataField="PN_05"          HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  />
							<telerik:GridBoundColumn HeaderText="Nacionalidad"                  DataField="PN_08"          HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  />
							<telerik:GridBoundColumn HeaderText="Tipo Residencia"               DataField="PN_09"          HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  />
							<telerik:GridBoundColumn HeaderText="Estado Civil"                  DataField="PN_11"          HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  />
							<telerik:GridBoundColumn HeaderText="Sexo"                          DataField="PN_12"          HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  />
							<telerik:GridBoundColumn HeaderText="Numero de Registro Electoral"  DataField="PN_14"          HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  />
							<telerik:GridBoundColumn HeaderText="Clave de Identificacion Unica" DataField="PN_15"          HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  />
							<telerik:GridBoundColumn HeaderText="Clave dePaís"                  DataField="PN_16"          HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  />			
                            <telerik:GridBoundColumn HeaderText="Fecha de Defunción"            DataField="PN_20"          HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  UniqueName="FechaDefuncion"/>				
						</Columns>
						<EditFormSettings>
							<EditColumn InsertImageUrl="Update.gif" UpdateImageUrl="Update.gif" EditImageUrl="Edit.gif" CancelImageUrl="Cancel.gif" />
						</EditFormSettings>
					</MasterTableView>
					<FilterMenu EnableEmbeddedSkins="False" />
					<HeaderContextMenu EnableEmbeddedSkins="False" />
				</telerik:RadGrid>
			</div>
			
		</div>


        <!-- Grid Errores-->
		<div class="divCard">
	        <div class="divHeader">
				&nbsp;ERRORES
            </div>
            <div class="divExporters">
                <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
                    <tr>
                        <td width="100%" align="right">
                            <asp:ImageButton ID="imgExportErrorPDF" runat="server" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-PDF.png" onclick="imgExportErrorPDF_Click" />
                            &nbsp;
                            <asp:ImageButton ID="imgExportErrorXLS" runat="server"  ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png" onclick="imgExportErrorXLS_Click" />
                        </td>
                    </tr>
                </table>			
            </div>
			<div class="divGrid">
					
				<telerik:RadGrid ID="rgErrores" runat="server" AllowPaging="True" Width="968px"  OnItemCommand="grids_ItemCommand" 
					AllowSorting="True" GridLines="None" AutoGenerateColumns="False"  OnNeedDataSource="rgErrores_NeedDataSource" AllowFilteringByColumn="true" EnableEmbeddedSkins="false" Skin="Banobras2011">
				  
					<ExportSettings ExportOnlyData="True" FileName="PF_Errores" IgnorePaging="True">
						<Pdf PageWidth="1000px" Title="Errores (PF)" />
					</ExportSettings>
					<PagerStyle Mode="NextPrevAndNumeric"  AlwaysVisible="true" 
							    FirstPageToolTip="Primera Página"  
							    PrevPageToolTip="Página Anterior"  
							    NextPageToolTip="Página Siguiente"  
							    LastPageToolTip="Última Página" 
							    PageSizeLabelText="Registros por página: " 
							    PagerTextFormat="{4} Consultando página {0} de {1}, {5} Registros en {1} páginas." /><GroupingSettings CaseSensitive="false" />
					<FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
					<MasterTableView ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true" DataKeyNames="id" PageSize="10" HierarchyLoadMode="ServerOnDemand" Width="2500px" >
						<ExpandCollapseColumn Visible="True" />
						<Columns>
							<telerik:GridBoundColumn HeaderText="RFC"             DataField="rfc"             HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  />
							<telerik:GridBoundColumn HeaderText="Nombre"          DataField="nombre"          HeaderStyle-Width="350px" ItemStyle-Width="350px" FilterControlWidth="340px" />
							<telerik:GridBoundColumn HeaderText="Crédito"         DataField="credito"         HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  />
							<telerik:GridBoundColumn HeaderText="Etiqueta"        DataField="etiqueta"        HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  />
							<telerik:GridBoundColumn HeaderText="Campo"           DataField="campo"           HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  />
							<telerik:GridBoundColumn HeaderText="Dato"            DataField="dato"            HeaderStyle-Width="200px" ItemStyle-Width="200px" FilterControlWidth="190px" />
							<telerik:GridBoundColumn HeaderText="Código Error"    DataField="codigo_error"    HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  />
							<telerik:GridBoundColumn HeaderText="Mensaje"         DataField="mensaje"         HeaderStyle-Width="350px" ItemStyle-Width="350px" FilterControlWidth="340px" />
							<telerik:GridBoundColumn HeaderText="Capital Vigente" DataField="Capital_Vigente" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  DataFormatString="${0:N2}"/>
							<telerik:GridBoundColumn HeaderText="Capital Vencido" DataField="Capital_Vencido" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  DataFormatString="${0:N2}"/>
							<telerik:GridBoundColumn HeaderText="Interés Vigente" DataField="Interes_Vigente" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  DataFormatString="${0:N2}"/>
							<telerik:GridBoundColumn HeaderText="Interés Vencido" DataField="Interes_Vencido" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  DataFormatString="${0:N2}"/>
							<telerik:GridBoundColumn HeaderText="Total"           DataField="total"           HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  DataFormatString="${0:N2}"/>
						</Columns>
						<EditFormSettings>
							<EditColumn InsertImageUrl="Update.gif" UpdateImageUrl="Update.gif" EditImageUrl="Edit.gif" CancelImageUrl="Cancel.gif" />
						</EditFormSettings>

					</MasterTableView>
					<FilterMenu EnableEmbeddedSkins="False" />
					<HeaderContextMenu EnableEmbeddedSkins="False" />
				</telerik:RadGrid>
			</div>
		</div>


        <!-- Grid Warnings-->
        <div class="divCard">
			<div class="divHeader">
				&nbsp;WARNINGS
            </div>
			<div class="divExporters">
                <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
                    <tr>
                        <td width="100%" align="right">
                            <asp:ImageButton ID="imgExportWarningPDF" runat="server" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-PDF.png" onclick="imgExportWarningPDF_Click" />
                            &nbsp;
				            <asp:ImageButton ID="imgExportWarningXLS" runat="server" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png" onclick="imgExportWarningXLS_Click" />
                        </td>
                    </tr>
                </table>				
	        </div>
            <div class="divGrid">
					
				<telerik:RadGrid ID="rgWarnings" runat="server" AllowPaging="True" Width="968px"  OnItemCommand="grids_ItemCommand" 
					AllowSorting="True" GridLines="None" AutoGenerateColumns="False"  OnNeedDataSource="rgWarnings_NeedDataSource" AllowFilteringByColumn="true" EnableEmbeddedSkins="false" Skin="Banobras2011">
				  
					<ExportSettings ExportOnlyData="True" FileName="PF_Warnings" IgnorePaging="True">
						<Pdf PageWidth="1000px" Title="Errores (PF)" />
					</ExportSettings>
					<PagerStyle Mode="NextPrevAndNumeric"  AlwaysVisible="true" 
							    FirstPageToolTip="Primera Página"  
							    PrevPageToolTip="Página Anterior"  
							    NextPageToolTip="Página Siguiente"  
							    LastPageToolTip="Última Página" 
							    PageSizeLabelText="Registros por página: " 
							    PagerTextFormat="{4} Consultando página {0} de {1}, {5} Registros en {1} páginas."  /><GroupingSettings CaseSensitive="false" />
					<FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
					<MasterTableView ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true" DataKeyNames="id" PageSize="10" HierarchyLoadMode="ServerOnDemand" Width="2500px" >
						<ExpandCollapseColumn Visible="True" />
						<Columns>
							<telerik:GridBoundColumn HeaderText="RFC"             DataField="rfc"             HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  />
							<telerik:GridBoundColumn HeaderText="Nombre"          DataField="nombre"          HeaderStyle-Width="350px" ItemStyle-Width="350px" FilterControlWidth="340px" />
							<telerik:GridBoundColumn HeaderText="Crédito"         DataField="credito"         HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  />
							<telerik:GridBoundColumn HeaderText="Etiqueta"        DataField="etiqueta"        HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  />
							<telerik:GridBoundColumn HeaderText="Campo"           DataField="campo"           HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  />
							<telerik:GridBoundColumn HeaderText="Dato"            DataField="dato"            HeaderStyle-Width="200px" ItemStyle-Width="200px" FilterControlWidth="190px" />
							<telerik:GridBoundColumn HeaderText="Código Error"    DataField="codigo_error"    HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  />
							<telerik:GridBoundColumn HeaderText="Mensaje"         DataField="mensaje"         HeaderStyle-Width="350px" ItemStyle-Width="350px" FilterControlWidth="340px" />
							<telerik:GridBoundColumn HeaderText="Capital Vigente" DataField="Capital_Vigente" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  DataFormatString="${0:N2}"/>
							<telerik:GridBoundColumn HeaderText="Capital Vencido" DataField="Capital_Vencido" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  DataFormatString="${0:N2}"/>
							<telerik:GridBoundColumn HeaderText="Interés Vigente" DataField="Interes_Vigente" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  DataFormatString="${0:N2}"/>
							<telerik:GridBoundColumn HeaderText="Interés Vencido" DataField="Interes_Vencido" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  DataFormatString="${0:N2}"/>
							<telerik:GridBoundColumn HeaderText="Total"           DataField="total"           HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  DataFormatString="${0:N2}"/>
						</Columns>
						<EditFormSettings>
							<EditColumn InsertImageUrl="Update.gif" UpdateImageUrl="Update.gif" EditImageUrl="Edit.gif" CancelImageUrl="Cancel.gif" />
						</EditFormSettings>

					</MasterTableView>
					<FilterMenu EnableEmbeddedSkins="False" />
					<HeaderContextMenu EnableEmbeddedSkins="False" />
				</telerik:RadGrid>
			</div>
        </div>

</asp:Content>

