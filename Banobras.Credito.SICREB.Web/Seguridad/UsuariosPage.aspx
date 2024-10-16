<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="UsuariosPage.aspx.cs" Inherits="Seguridad_UsuariosPage" Theme="Banobras2011" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <style type="text/css">

        .custom_label 
        {
           width:100px;
           text-align: center;
           border-top: 1px solid #96d1f8;
           background: #65a9d7;
           background: -webkit-gradient(linear, left top, left bottom, from(#273138), to(#65a9d7));
           background: -webkit-linear-gradient(top, #273138, #65a9d7);
           background: -moz-linear-gradient(top, #273138, #65a9d7);
           background: -ms-linear-gradient(top, #273138, #65a9d7);
           background: -o-linear-gradient(top, #273138, #65a9d7);
           padding: 7px 14px;
           color: #FFFFFF;
           font-size: 14px;
           font-family: Calibri;
           /*text-decoration: none;*/
           vertical-align: middle;
           font-weight: bold;
        }

    </style>

    <script type="text/javascript">
        
        //Sólo 6 dígitos
        function isNumberKey(evt, value) {
            var limit = 6;
            var charCode = (evt.which) ? evt.which : event.keyCode
            var lengthValue = value.length;

            if ((lengthValue == limit) && (charCode == 8)) {
                value = value.toString().substring(0, txt - 1);
            }

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            else {
                if (lengthValue < limit) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        //Validar dirección de correo
        function ValidateEmail() {
            var control = document.getElementById("<%=txtEmail.ClientID%>");
            var filter = /^([a-zA-Z0-9_.-])+@(([a-zA-Z0-9-])+.)+([a-zA-Z0-9]{2,4})+$/;
            if (!filter.test(control.value)) {
                radalert('Capture una dirección de correo válida.', 400, "Error");
                control.focus;
                return false;
            }
            return true;
        }

        (function (global, undefined) {

            function confirmAspUpdatePanelPostback(button, title, message) {
                function aspUpdatePanelCallbackFn(arg) {
                    if (arg) {
                        __doPostBack(button.name, '');
                    }
                }
                var login = button.getAttribute('data-id');
                radconfirm(message + "<br>" + "Usuario: " + "<strong>" + login + "</strong><br><br>", aspUpdatePanelCallbackFn, 330, 180, null, title);
            }

            global.confirmAspUpdatePanelPostback = confirmAspUpdatePanelPostback;

        })(window);

    </script>

    <%--Botonera inicial--%>
    <table id="Section_Filters" runat="server" cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
		<tr>
			<td>
            </td>
			<td width="100%" align="right">
			    <asp:Button ID="btnAddUser" runat="server" Text="Agregar" CssClass="btnSmall" onclick="btnAddUser_Click" />
				<asp:ImageButton ID="btnExportPDF" ImageUrl ="~/ResourcesSICREB/Images/BNBPF-ICO-PDF.png" runat="server" Width="22px" onclick="btnExportPDF_Click" />
				<asp:ImageButton ID="btnExportExcel" ImageUrl ="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png" runat="server" Width="22px" onclick="btnExportExcel_Click" />
			</td>
			<td></td>
		</tr>
    </table>

    <%--Usuarios--%>
    <div id="Section_UserList" runat="server">

        <%--Título--%>
        <div class="titleCenter" style="text-align:center">
            <span style="padding-top: 5px; padding-left: 8px;">ADMINISTRACIÓN DE USUARIOS</span>            
        </div>

        <%--Listado--%>
	     <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
		    <tr>
			    <td colspan="4" id="td1" width="100%">
                			
	                <telerik:RadGrid runat ="server" ID ="RgdUsuarios" AutoGenerateColumns ="false" AllowSorting ="true" AllowPaging ="true" PageSize="10" 
		                onneeddatasource="RgdUsuarios_NeedDataSource" AllowFilteringByColumn="True" EnableLinqExpressions ="false"  
                        OnItemDataBound="RgdUsuarios_ItemDataBound">
		                <ExportSettings FileName="Administracion Usuarios" IgnorePaging="true" OpenInNewWindow="false" ExportOnlyData="true">				
		                </ExportSettings>
	                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
		                <PagerStyle Mode="NextPrevAndNumeric"  AlwaysVisible="true" 
					                FirstPageToolTip="Primera Página"  
					                PrevPageToolTip="Página Anterior"  
					                NextPageToolTip="Página Siguiente"  
					                LastPageToolTip="Última Página" 
					                PageSizeLabelText="Registros por página: " 
					                PagerTextFormat="{4} Consultando página {0} de {1}, {5} Registros en {1} páginas." />
                        <GroupingSettings CaseSensitive="false" />
		                <MasterTableView Name ="MtvUsuario" NoMasterRecordsText ="No existen Registros" CommandItemDisplay="Top" EditMode="InPlace"  Width="100%" DataKeyNames="Id,Login" >
		                    <CommandItemSettings ShowRefreshButton = "true" ShowAddNewRecordButton="false"  AddNewRecordText ="Agregar Usuario"  RefreshText="Actualizar Datos" />
		                        <Columns>
                                    
					                <Telerik:GridBoundColumn HeaderText="Usuario Id"  HeaderStyle-Width="7%" ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center"
						                UniqueName="Id" DataField="Id" Visible="false"
						                DataType="System.Int32" >
					                </Telerik:GridBoundColumn>

					                <Telerik:GridBoundColumn HeaderText="Login" HeaderStyle-Width="7%" ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center"
						                UniqueName="Login" DataField="Login"
						                DataType="System.String" MaxLength="5">
					                </Telerik:GridBoundColumn>

					                <Telerik:GridBoundColumn HeaderText="Empleado Id" HeaderStyle-Width="7%" ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center"
						                UniqueName="EmployeeNumber" DataField="EmployeeNumber"
						                DataType="System.Int32" MaxLength="10">
					                </Telerik:GridBoundColumn>

                                    <Telerik:GridBoundColumn HeaderText="Nombre" HeaderStyle-Width="15%" ItemStyle-Width="15%" HeaderStyle-HorizontalAlign="Justify"
						                UniqueName="FullName" DataField="FullName"
						                DataType="System.String" MaxLength="100">
					                </Telerik:GridBoundColumn>

                                    <Telerik:GridBoundColumn HeaderText="Correo" HeaderStyle-Width="15%" ItemStyle-Width="15%" HeaderStyle-HorizontalAlign="Justify"
						                UniqueName="Email" DataField="Email"
						                DataType="System.String" MaxLength="100">
					                </Telerik:GridBoundColumn>

						            <Telerik:GridBoundColumn HeaderText="Roles" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center"
						                UniqueName="RolesNameList" DataField="RolesNameList"
						                DataType="System.String" MaxLength="50" >
					                </Telerik:GridBoundColumn>

						            <Telerik:GridBoundColumn HeaderText="Fecha Creación" HeaderStyle-Width="7%" ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center"
						                UniqueName="CreationDate" DataField="CreationDate" AllowFiltering ="false" DataFormatString="{0:dd/MM/yyyy}" Visible="true"
						                DataType="System.String" MaxLength="20" >
					                </Telerik:GridBoundColumn>

						            <Telerik:GridBoundColumn HeaderText="Fecha Edición" HeaderStyle-Width="7%" ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center"
						                UniqueName="TransactionDate" DataField="TransactionDate" AllowFiltering ="false" DataFormatString="{0:dd/MM/yyyy}" Visible="true"
						                DataType="System.String" MaxLength="20" >
					                </Telerik:GridBoundColumn>

						            <Telerik:GridBoundColumn HeaderText="Usuario que editó" HeaderStyle-Width="7%" ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center"
						                UniqueName="TransactionLogin" DataField="TransactionLogin" Visible="true"
						                DataType="System.String" MaxLength="20" >
					                </Telerik:GridBoundColumn>

						            <Telerik:GridBoundColumn HeaderText="Estatus" HeaderStyle-Width="7%" ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center"
						                UniqueName="Estatus" DataField="Estatus"
						                DataType="System.String" MaxLength="10" >
					                </Telerik:GridBoundColumn>

						            <Telerik:GridBoundColumn HeaderText="Sesión" HeaderStyle-Width="7%" ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center"
						                UniqueName="SessionIP" DataField="SessionIP" Visible="false"
						                DataType="System.String" MaxLength="20" >
					                </Telerik:GridBoundColumn>

                                    <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Center" HeaderText="Acciones" AllowFiltering ="false" HeaderStyle-Width="18%" ItemStyle-Width="18%">
                                        <ItemTemplate>                                            
                                            <div style="display: flex; text-align:center; align-items:center;">
                                                <div style="width: 33%;">
                                                    <asp:ImageButton ID="imgbDelete" runat="server" title="Eliminar usuario" ImageUrl="~/App_Themes/Banobras2011/Grid/Delete.gif" data-id='<%# Eval("Login") %>' OnClientClick="confirmAspUpdatePanelPostback(this, 'Eliminación de cuenta', 'Confirma la eliminación de la cuenta?'); return false;" OnClick="btnGridDelete_Click" />
                                                    <asp:ImageButton ID="imgbActivate" runat="server" title="Activar usuario" ImageUrl="~/App_Themes/Banobras2011/Grid/off-button-26.png" data-id='<%# Eval("Login") %>' OnClientClick="confirmAspUpdatePanelPostback(this, 'Activación de cuenta', 'Confirma la activacción de la cuenta?'); return false;" OnClick="btnGridActivate_Click" />
                                                </div>
                                                <div style="width: 33%;">
                                                    <asp:ImageButton ID="imgbEdit" runat="server" title="Editar usuario" ImageUrl="~/App_Themes/Banobras2011/Grid/Edit.gif" OnClick="btnGridEdit_Click" />
                                                </div>
                                                <div style="width: 33%;">
                                                    <asp:ImageButton ID="imgbSession" runat="server" title="Cerrar sesión activa" ImageUrl="~/App_Themes/Banobras2011/Grid/cerrar_sesion_off.png" data-id='<%# Eval("Login") %>' OnClientClick="confirmAspUpdatePanelPostback(this, 'Cierre de sesión', 'Confirma el cierre de sesión?'); return false;" OnClick="btnGridSession_Click" />
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

					            </Columns>
		                </MasterTableView>
	                </telerik:RadGrid> 

	            </td>
            </tr>
	    </table>
    
    </div> 
        
    <br />
    
    <%--SECCION PARA AGREGAR USUARIO--%>
    <div id="Section_User" runat="server">

        <%--Título--%>
        <div class="titleCenter" style="text-align:center">
            <span style="padding-top: 5px; padding-left: 8px;">AGREGAR USUARIO</span>            
        </div>

        <%--Datos--%>
        <table cellpadding="0" cellspacing="0" width="100%" align="center" class="tableinicio">	
            <tr>
                <td style="width:5px;">&nbsp;</td>
                <td style="width:100px;" class="custom_label">
                    <asp:Label ID="LabelUsuario" runat="server" Text="Usuario login:"></asp:Label>
                </td>
                <td style="width:20px;">&nbsp;</td>
                <td style="width:240px; display:flex; padding-top:2px;">                    
                    <div>
                        <asp:TextBox ID="txtUserLogin" runat="server" Height="22px" Enabled="false" ></asp:TextBox>
                    </div>
                    <div style="padding-left:5px;">
                        <asp:ImageButton ID="imgbSearchDA" runat="server" ImageUrl="~/Resources/Images/icon_search_user.png" onclick="btnSearchDA_Click" />
                    </div>                    
                    <asp:TextBox ID="txtUserId" runat="server" Enabled="false" Visible="false"></asp:TextBox>
                </td>
                <td style="width:5px;">&nbsp;</td>
            </tr>
            <%--Empleado Id--%>
            <tr>
                <td>&nbsp;</td>
                <td class="custom_label">
                    <asp:Label ID="lblEmpleadoId" runat="server" Text="Empleado Id"></asp:Label>
                </td>
                <td>&nbsp;</td>
                <td>
                    <asp:TextBox ID="txtEmpleadoId" runat="server" Height="22px" Type="Number" onkeydown="return isNumberKey(event, this.value)"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <%--Nombre--%>
            <tr>
                <td>&nbsp;</td>
                <td class="custom_label">
                    <asp:Label ID="lblName" runat="server" Text="Nombre"></asp:Label>
                </td>
                <td>&nbsp;</td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" Height="22px" Width="400px"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <%--Correo--%>
            <tr>
                <td>&nbsp;</td>
                <td class="custom_label">
                    <asp:Label ID="lblEmail" runat="server" Text="Correo"></asp:Label>
                </td>
                <td>&nbsp;</td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" Width="400px" Height="22px" PlaceHolder="Ingresar dirección de correo." xmlns:asp="#unknown" ></asp:TextBox>
                    <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" xmlns:asp="#unknown"
                       ControlToValidate="txtEmail" ErrorMessage="Correo con formato incorrecto." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                    </asp:RegularExpressionValidator>
                </td>
                <td>&nbsp;</td>
            </tr>
            <%--Roles--%>
            <tr>
                <td></td>
                <td colspan="3">
                    <table style="width: 100%;">
                        <tr style="height:40px;">
                            <td style="width:5%;"></td>
                            <td style="width:40%;">
                                <asp:Label ID="LabelNoAsignadas" runat="server" Text="Roles disponibles:" CssClass="formulario"></asp:Label>
                            </td>
                            <td style="width:10%;"></td>
                            <td style="width:40%;">
                                <asp:Label ID="LabelAsignadas" runat="server" Text="Roles asignados:" CssClass="formulario"></asp:Label>
                            </td>
                            <td style="width:5%;"></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:ListBox ID="listRolesDisponibles" runat="server" Enabled="false" Width="100%" SelectionMode="Multiple" Rows="10"></asp:ListBox>
                            </td>
                            <td style="text-align:center;">
                                <asp:ImageButton ID="btnAddRole" runat="server" ImageUrl="~/Resources/Images/BarsButtons/BNB-SIC-ButtonCtrl_Agregar.png" onclick="btnAddRole_Click" Enabled="false" />
                                <br />
                                <br />
                                <asp:ImageButton ID="btnRemoveRole" runat="server" ImageUrl="~/Resources/Images/BarsButtons/BNB-SIC-ButtonCtrl_Quitar.png" onclick="btnRemoveRole_Click" Enabled="false" />
                            </td>
                            <td>
                                <asp:ListBox ID="listRolesAsignados" runat="server" Enabled="false" Width="100%" SelectionMode="Multiple" Rows="10"></asp:ListBox>
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </td>
                <td></td>
            </tr>
            <%--Espacio--%>
            <tr>
                <td></td>
                <td colspan="3"></td>
                <td></td>
            </tr>
            <%--Botonera--%>
            <tr>    
                <td colspan="4" align="center">
                    <asp:Button ID="btnSaveUser" runat="server" Text="Guardar" Enabled="false" onclick="btnSaveUser_Click" OnClientClick="return ValidateEmail();" CssClass="btnSmall"/>   
                    <asp:Button ID="btnCancelUser" runat="server" Text="Cancelar" Enabled="false" CssClass="btnSmall" onclick="btnCancelUser_Click"/>
                </td>
                <td></td>    
            </tr>     
        </table>

    </div>
    
    
</asp:Content>

