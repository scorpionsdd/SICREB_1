<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 	CodeFile="PersonasMoralesGL.aspx.cs" Inherits="PersonasMoralesGL" Theme="Banobras2011" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content runat="Server" ID="Content1" ContentPlaceHolderID="HeadContent" >
</asp:Content>

<asp:Content runat="Server" ID="Content2" ContentPlaceHolderID="MainContent" >
    
    <telerik:RadAjaxManager runat="Server" ID="RAManager" EnableViewState="true">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rgArchivoGL">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgArchivoGL" />
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
        
	<div class="divCard">
		<div class="divHeader">&nbsp;PROCESO DE GRUPOS y LINEAS DE CREDITO</div>
		<table cellpadding="0px" cellspacing="0px" class="resumenProceso">
			<tr>
				<td>
                    &nbsp; Fecha de último proceso:
				</td>
				<td>
					<asp:Label runat="server" ID="lblFechaArchivo" />
				</td>
				<td>
					&nbsp; Errores detectados:
				</td>
				<td>
					<asp:Label runat="server" ID="lblErrores" />
				</td>
				<td>
					&nbsp; Datos correctos:
				</td>
				<td>
					<asp:Label runat="server" ID="lblCorrectos" />
				</td>
			</tr>
			<tr>
				<td>
					&nbsp; Datos procesados:
				</td>
				<td>
					<asp:Label runat="server" ID="lblProcesados" />
				</td>
				<td>
					&nbsp; Warnings:
				</td>
				<td>
					<asp:Label runat="server" ID="lblWarnings" />
				</td>
				<td>
				</td>
				<td>
				</td>
			</tr>
		</table>
	</div>


        <!-- Grid Archivo PM-->
		<div class="divCard" >
			<div class="divHeader">&nbsp;INFORMACIÓN PROCESADA</div>
			<div class="divExporters">
                <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
                   <tr>
                     <td width="100%" align="right">
                       <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-PDF.png" onclick="imgExportArchivoPDF_Click" />
                        &nbsp;
				       <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png" onclick="imgExportArchivoXLS_Click" />   
                     </td>
                   </tr>
                </table>  		
			</div>
			
			<div class="divGrid">
				
                <telerik:RadGrid runat="server" ID="rgArchivoGL" AllowPaging="True" AllowSorting="True" OnItemCommand="grids_ItemCommand" GridLines="None" Skin="Banobras2011"
					             AutoGenerateColumns="False" OnNeedDataSource="rgArchivoGL_NeedDataSource" AllowFilteringByColumn="true" EnableEmbeddedSkins="false" >
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
                    <MasterTableView ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true" DataKeyNames="Id" PageSize="10" HierarchyLoadMode="ServerOnDemand" Width="2500px" >
                        <DetailTables>
                            
                            <telerik:GridTableView Name="Credito" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="false" AllowFilteringByColumn="false" Width="100%"   >
                                <ParentTableRelation>
                                    <telerik:GridRelationFields DetailKeyField="EM_ID" MasterKeyField="Id" />
                                </ParentTableRelation>
                                <Columns>
                                    <telerik:GridBoundColumn HeaderText="RFC"                            DataField="CR_00" />
                                    <telerik:GridBoundColumn HeaderText="# Contrato"                     DataField="CR_02" />
                                    <telerik:GridBoundColumn HeaderText="Contrato Anterior"              DataField="CR_03" />
                                    <telerik:GridBoundColumn HeaderText="Apertura"                       DataField="CR_04"             UniqueName="Apertura" />
                                    <telerik:GridBoundColumn HeaderText="Plazo"                          DataField="CR_05" />
                                    <telerik:GridBoundColumn HeaderText="Tipo"                           DataField="CR_06" />
                                    <telerik:GridBoundColumn HeaderText="Saldo Inicial"                  DataField="CR_07"             UniqueName="SaldoInicial" />
                                    <telerik:GridBoundColumn HeaderText="Moneda"                         DataField="CR_08" />
                                    <telerik:GridBoundColumn HeaderText="# Pagos"                        DataField="CR_09" />
                                    <telerik:GridBoundColumn HeaderText="Frecuencia"                     DataField="CR_10" />
                                    <telerik:GridBoundColumn HeaderText="Importe"                        DataField="CR_11"             UniqueName="ImportePago" />
                                    <telerik:GridBoundColumn HeaderText="Último Pago"                    DataField="CR_12"             UniqueName="UltimoPago" />
                                    <telerik:GridBoundColumn HeaderText="Reestructura"                   DataField="CR_13" />
                                    <telerik:GridBoundColumn HeaderText="Pago"                           DataField="CR_14" />
                                    <telerik:GridBoundColumn HeaderText="Liquidación"                    DataField="CR_15"             UniqueName="Liquidacion" />
                                    <telerik:GridBoundColumn HeaderText="Quita"                          DataField="CR_16" />
                                    <telerik:GridBoundColumn HeaderText="Dación"                         DataField="CR_17" />
                                    <telerik:GridBoundColumn HeaderText="Quebranto"                      DataField="CR_18" />
                                    <telerik:GridBoundColumn HeaderText="Monto"                          DataField="MontoPagar"        DataFormatString="{0:C}" />
                                    <telerik:GridBoundColumn HeaderText="Monto Vencido"                  DataField="MontoPagarVencido" DataFormatString="{0:C}" />
                                    <telerik:GridBoundColumn HeaderText="Intereses"                      DataField="Intereses"         DataFormatString="{0:C}" />
                                    <telerik:GridBoundColumn HeaderText="Dias Vencido"                   DataField="DiasVencido" />
                                    <telerik:GridBoundColumn HeaderText="Fecha de Primer Incumplimiento" DataField="CR_21" />
                                    <telerik:GridBoundColumn HeaderText="Fecha de Ingreso a Cartera Vencida" DataField="CR_24" />
                                    <telerik:GridBoundColumn HeaderText="Saldo Insoluto"                 DataField="CR_22"             UniqueName="SaldoInsoluto" />
                                    <telerik:GridBoundColumn HeaderText="Clave Observacion"              DataField="CR_19" />
                                </Columns>
                                <EditFormSettings>
                                    <EditColumn InsertImageUrl="Update.gif" UpdateImageUrl="Update.gif" EditImageUrl="Edit.gif" CancelImageUrl="Cancel.gif" />
                                </EditFormSettings>                  
                            </telerik:GridTableView>

                            <telerik:GridTableView Name="Accionistas" DataKeyNames="ParentId" AutoGenerateColumns="false" AllowPaging="false" AllowFilteringByColumn="false" Width="100%" >
                                <ParentTableRelation>
                                   <telerik:GridRelationFields  DetailKeyField="AuxId" MasterKeyField="Id" />
                                </ParentTableRelation>
                                <Columns>
                                    <telerik:GridBoundColumn HeaderText="RFC Accionista"         DataField="AC_00" />
                                    <telerik:GridBoundColumn HeaderText="Compañía Accionista"    DataField="AC_03" />
                                    <telerik:GridBoundColumn HeaderText="Nombre Accionista"      DataField="AC_04" />
                                    <telerik:GridBoundColumn HeaderText="Apellido Paterno"       DataField="AC_06" />
                                    <telerik:GridBoundColumn HeaderText="Apellido Materno"       DataField="AC_07" />
                                    <telerik:GridBoundColumn HeaderText="Porcentaje"             DataField="AC_08" />
                                    <telerik:GridBoundColumn HeaderText="Direccion"              DataField="AC_09" />
                                    <telerik:GridBoundColumn HeaderText="Colonia o Poblacion"    DataField="AC_11" />
                                    <telerik:GridBoundColumn HeaderText="Delegacion o Municipio" DataField="AC_12" />
                                    <telerik:GridBoundColumn HeaderText="Ciudad"                 DataField="AC_13" />
                                    <telerik:GridBoundColumn HeaderText="Estado (Mexico)"        DataField="AC_14" />
                                    <telerik:GridBoundColumn HeaderText="Codigo Postal"          DataField="AC_15" />
                                </Columns>
                                <EditFormSettings>
                                    <EditColumn InsertImageUrl="Update.gif" UpdateImageUrl="Update.gif" EditImageUrl="Edit.gif" CancelImageUrl="Cancel.gif" />
                                </EditFormSettings>
                            </telerik:GridTableView>

                            <telerik:GridTableView Name="Avales" DataKeyNames="ParentId" AutoGenerateColumns="false" AllowPaging="false" AllowFilteringByColumn="false" Width="100%" >
                                <ParentTableRelation>
                                   <telerik:GridRelationFields  DetailKeyField="AuxId" MasterKeyField="Id" />
                                </ParentTableRelation>
                                <Columns>
                                    <telerik:GridBoundColumn HeaderText="No. Credito"            DataField="AV_21" />
                                    <telerik:GridBoundColumn HeaderText="RFC Aval"               DataField="AV_00" />
                                    <telerik:GridBoundColumn HeaderText="Compañía Aval"          DataField="AV_03" />
                                    <telerik:GridBoundColumn HeaderText="Nombre Aval"            DataField="AV_04" />
                                    <telerik:GridBoundColumn HeaderText="Apellido Paterno"       DataField="AV_06" />
                                    <telerik:GridBoundColumn HeaderText="Apellido Materno"       DataField="AV_07" />
                                    <telerik:GridBoundColumn HeaderText="Direccion"              DataField="AV_08" />
                                    <telerik:GridBoundColumn HeaderText="Colonia o Poblacion"    DataField="AV_10" />
                                    <telerik:GridBoundColumn HeaderText="Delegacion o Municipio" DataField="AV_11" />
                                    <telerik:GridBoundColumn HeaderText="Ciudad"                 DataField="AV_12" />
                                    <telerik:GridBoundColumn HeaderText="Estado (Mexico)"        DataField="AV_13" />
                                    <telerik:GridBoundColumn HeaderText="Codigo Postal"          DataField="AV_14" />
                                </Columns>
                                <EditFormSettings>
                                    <EditColumn InsertImageUrl="Update.gif" UpdateImageUrl="Update.gif" EditImageUrl="Edit.gif" CancelImageUrl="Cancel.gif" />
                                </EditFormSettings>
                            </telerik:GridTableView>

                        </DetailTables>

                        <ExpandCollapseColumn Visible="True">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn HeaderText="RFC"                 DataField="EM_00" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" UniqueName="EM_00" DataType="System.String"/>
                            <telerik:GridBoundColumn HeaderText="CURP"                DataField="EM_01" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" />
                            <telerik:GridBoundColumn HeaderText="Compañía"            DataField="EM_03" HeaderStyle-Width="300px" ItemStyle-Width="300px" FilterControlWidth="290px" />
                            <telerik:GridBoundColumn HeaderText="Nacionalidad"        DataField="EM_08" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" />
                            <telerik:GridBoundColumn HeaderText="Calificación"        DataField="EM_27" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" />
                            <telerik:GridBoundColumn HeaderText="Banxico"             DataField="EM_10" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" />
                            <telerik:GridBoundColumn HeaderText="Dirección"           DataField="EM_13" HeaderStyle-Width="300px" ItemStyle-Width="300px" FilterControlWidth="290px" />
                            <telerik:GridBoundColumn HeaderText="Colonia"             DataField="EM_15" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" />
                            <telerik:GridBoundColumn HeaderText="Municipio"           DataField="EM_16" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" />
                            <telerik:GridBoundColumn HeaderText="Ciudad"              DataField="EM_17" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" />
                            <telerik:GridBoundColumn HeaderText="Estado"              DataField="EM_18" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" />
                            <telerik:GridBoundColumn HeaderText="Código Postal"       DataField="EM_19" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" />
                            <telerik:GridBoundColumn HeaderText="Tipo Cliente"        DataField="EM_23" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" />
                            <telerik:GridBoundColumn HeaderText="País de domicilio"   DataField="EM_25" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" />
                            <telerik:GridBoundColumn HeaderText="Clave Consolidación" DataField="EM_26" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" />
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
        
                
        <!-- Grid errores-->  
		<div class="divCard">
		    <div class="divHeader">
			    &nbsp;ERRORES
            </div>
			<div class="divExporters">
                <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
                    <tr>
                        <td width="100%" align="right">
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-PDF.png" onclick="imgExportErrorPDF_Click" />
                            &nbsp;
				            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png" onclick="imgExportErrorXLS_Click" />   
                        </td>
                    </tr>
                </table>  			
			</div>
			<div class="divGrid">
					
			    <telerik:RadGrid ID="rgErrores" runat="server" AllowPaging="True" Width="968px" OnItemCommand="grids_ItemCommand" AllowSorting="True" GridLines="None" AutoGenerateColumns="False"  
                    OnNeedDataSource="rgErrores_NeedDataSource" AllowFilteringByColumn="true" EnableEmbeddedSkins="false" Skin="Banobras2011">				  
					<ExportSettings ExportOnlyData="True" FileName="PM_Errores" IgnorePaging="True">
						<Pdf PageWidth="1000px" Title="Errores (GL)" />
					</ExportSettings>
					<PagerStyle Mode="NextPrevAndNumeric"  AlwaysVisible="true" 
					    FirstPageToolTip="Primera Página"  
						PrevPageToolTip="Página Anterior"  
						NextPageToolTip="Página Siguiente"  
						LastPageToolTip="Última Página" 
						PageSizeLabelText="Registros por página: " 
						PagerTextFormat="{4} Consultando página {0} de {1}, {5} Registros en {1} páginas." />
                    <GroupingSettings CaseSensitive="false" />
					<FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
					<MasterTableView ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true" DataKeyNames="id" PageSize="10" HierarchyLoadMode="ServerOnDemand" Width="2000px">
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
							<telerik:GridBoundColumn HeaderText="Capital Vigente" DataField="Capital_Vigente" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  DataFormatString="${0:N2}" />
							<telerik:GridBoundColumn HeaderText="Capital Vencido" DataField="Capital_Vencido" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  DataFormatString="${0:N2}" />
							<telerik:GridBoundColumn HeaderText="Interés Vigente" DataField="Interes_Vigente" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  DataFormatString="${0:N2}" />
							<telerik:GridBoundColumn HeaderText="Interés Vencido" DataField="Interes_Vencido" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  DataFormatString="${0:N2}" />
							<telerik:GridBoundColumn HeaderText="Total"           DataField="total"           HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  DataFormatString="${0:N2}" />
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
		    <div class="divHeader">&nbsp;WARNINGS</div>
		    <div class="divExporters">
                <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
                    <tr>
                        <td width="100%" align="right">
                            <asp:ImageButton ID="imgExportWarningPDF" runat="server" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-PDF.png" onclick="imgExportWarningPDF_Click" />
                            &nbsp;
				            <asp:ImageButton ID="imgExportWarningXLS" runat="server"  ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png" onclick="imgExportWarningXLS_Click" />    
                        </td>
                    </tr>
                </table>                           
	        </div>
            <div class="divGrid">
					
	            <telerik:RadGrid ID="rgWarnings" runat="server" AllowPaging="True" Width="968px" OnItemCommand="grids_ItemCommand" AllowSorting="True" GridLines="None" AutoGenerateColumns="False"  
                    OnNeedDataSource="rgWarnings_NeedDataSource" AllowFilteringByColumn="true" EnableEmbeddedSkins="false" Skin="Banobras2011">
				  
			        <ExportSettings ExportOnlyData="True" FileName="PM_Warnings" IgnorePaging="True">
				        <Pdf PageWidth="1000px" Title="Warnings (GL)" />
				    </ExportSettings>
				    <PagerStyle Mode="NextPrevAndNumeric"  AlwaysVisible="true" 
				        FirstPageToolTip="Primera Página"  
					    PrevPageToolTip="Página Anterior"  
					    NextPageToolTip="Página Siguiente"  
					    LastPageToolTip="Última Página" 
					    PageSizeLabelText="Registros por página: " 
					    PagerTextFormat="{4} Consultando página {0} de {1}, {5} Registros en {1} páginas." />
                    <GroupingSettings CaseSensitive="false" />
				    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
				    <MasterTableView ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true" DataKeyNames="id" PageSize="10" HierarchyLoadMode="ServerOnDemand" Width="2000px" >
						<ExpandCollapseColumn Visible="True" />
						<Columns>
							<telerik:GridBoundColumn HeaderText="RFC"             DataField="rfc"             HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" />
							<telerik:GridBoundColumn HeaderText="Nombre"          DataField="nombre"          HeaderStyle-Width="350px" ItemStyle-Width="350px" FilterControlWidth="340px" />
							<telerik:GridBoundColumn HeaderText="Crédito"         DataField="credito"         HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" />
							<telerik:GridBoundColumn HeaderText="Etiqueta"        DataField="etiqueta"        HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" />
							<telerik:GridBoundColumn HeaderText="Campo"           DataField="campo"           HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" />
							<telerik:GridBoundColumn HeaderText="Dato"            DataField="dato"            HeaderStyle-Width="200px" ItemStyle-Width="200px" FilterControlWidth="190px" />
							<telerik:GridBoundColumn HeaderText="Código Error"    DataField="codigo_error"    HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" />
							<telerik:GridBoundColumn HeaderText="Mensaje"         DataField="mensaje"         HeaderStyle-Width="350px" ItemStyle-Width="350px" FilterControlWidth="340px" />
							<telerik:GridBoundColumn HeaderText="Capital Vigente" DataField="Capital_Vigente" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="${0:N2}"/>
							<telerik:GridBoundColumn HeaderText="Capital Vencido" DataField="Capital_Vencido" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="${0:N2}"/>
							<telerik:GridBoundColumn HeaderText="Interés Vigente" DataField="Interes_Vigente" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="${0:N2}"/>
							<telerik:GridBoundColumn HeaderText="Interés Vencido" DataField="Interes_Vencido" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="${0:N2}"/>
							<telerik:GridBoundColumn HeaderText="Total"           DataField="total"           HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="${0:N2}"/>
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


	<%--</telerik:RadAjaxPanel>--%>
</asp:Content>
