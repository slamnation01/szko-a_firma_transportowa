<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/_WebFormsMasterPage.master" AutoEventWireup="true" CodeBehind="AddRoutes.aspx.cs" Inherits="FirmaTransportowa.Views.Aspx.AddRoutes" %>


<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="container">
        <asp:UpdatePanel ID="updateEdit" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="hfId" runat="server" />

                <div>
                    <div style="margin-top: 30px; margin-bottom: 30px">
                        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_OnClick" UseSubmitBehavior="false" Text="Zapisz" class="btn btn-primary" />
                        <asp:Button ID="btnSaveLeave" runat="server" OnClick="btnSaveLeave_OnClick" UseSubmitBehavior="false" Text="Zapisz i wyjdź" class="btn btn-success" />
                        <asp:Button ID="btnBack" runat="server" OnClick="btnBack_OnClick" UseSubmitBehavior="false" Text="Powrót" class="btn btn-default" />
                    </div>

                    <div class="input-group">
                        <span class="input-group-addon" id="basic-addon1">Nazwa:</span>
                        <asp:TextBox ID="txtRouteName" runat="server" class="form-control" aria-describedby="basic-addon1"></asp:TextBox>
                    </div>

                    <div class="input-group">
                        <span class="input-group-addon" id="basic-addon2">Cena:</span>
                        <asp:TextBox ID="txtRoutePrice" runat="server" AutoPostBack="True" class="form-control" aria-describedby="basic-addon2"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate="txtRoutePrice" runat="server" ErrorMessage="Podaj poprawna cenę (część dziesiętną oddziel przecinkiem)" ValidationExpression="[0-9]?[0-9](\,[0-9][0-9]?)?"></asp:RegularExpressionValidator>
                    </div>

                    <div class="input-group">
                        <span class="input-group-addon" id="basic-addon3">Data odjazdu:</span>
                        <asp:TextBox ID="textRouteDate" runat="server" class="form-control" aria-describedby="basic-addon3"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ControlToValidate="textRouteDate" runat="server" ErrorMessage="Podaj datę w formacie dd-mm-yyy H:mm" ValidationExpression="^([1-9]|([012][0-9])|(3[01]))-([0]{0,1}[1-9]|1[012])-\d\d\d\d [012]{0,1}[0-9]:[0-6][0-9]$"></asp:RegularExpressionValidator>
                    </div>
                </div>

                <div style="margin-top: 20px">
                    <h4>Lista przystanków:</h4>

                    <asp:Repeater ID="rptStopsList" runat="server">
                        <HeaderTemplate>
                            <table class="table table-hover">
                                <thead class="thead-default">
                                    <tr>
                                        <th>Przystanek startowy</th>
                                        <th>Przystanek końcowy</th>
                                        <th>Cena</th>
                                        <th></th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tbody>
                                <tr>
                                    <asp:HiddenField ID="hfRptStopId" runat="server" Value='<%# Eval("Id") %>' />
                                    <td>
                                        <asp:TextBox runat="server" ID="rptStartStop" Text='<%# Eval("FirstStop") %>' OnTextChanged="rptConsign_textChng" /></td>
                                    <td>
                                        <asp:TextBox runat="server" ID="rptEndStop" Text='<%# Eval("LastStop") %>' OnTextChanged="rptConsign_textChng" /></td>
                                    <td>
                                        <asp:TextBox runat="server" ID="rptStopPrice" Text='<%# Eval("Price") %>' />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="rptStopPrice" runat="server" ErrorMessage="Podaj poprawna cene" ValidationExpression="^\d+(\,\d\d)?$"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                        <asp:Button runat="server" ID="btnDeleteConsign" OnClick="btnDeleteConsign_OnClick" UseSubmitBehavior="false" CommandArgument='<%# Eval("ID").ToString() %>' Text="Usun" class="btn btn-xs btn-danger" /></td>
                                </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:Repeater>
                    <tr>
                        <asp:HiddenField ID="hfConsignId" runat="server" Value='<%#Eval("ID") %>' />

                        <td>
                            <asp:TextBox ID="txtStartStop" runat="server" Text="" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtEndStop" runat="server" Text="" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtStopPrice" runat="server" Text="" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtStopPrice" runat="server" ErrorMessage="Dozwolone tylko liczby" ValidationExpression="^\d+(\,\d+)?$"></asp:RegularExpressionValidator>
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnCreateConsign" OnClick="btnCreateConsign_OnClick" UseSubmitBehavior="false" Text="Dodaj" class="btn btn-xs btn-primary" /></td>
                    </tr>

                    </tbody>
                        </table>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

