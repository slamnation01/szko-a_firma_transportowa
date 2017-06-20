<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/_WebFormsMasterPage.master" AutoEventWireup="true" CodeBehind="RoutesList.aspx.cs" Inherits="FirmaTransportowa.Areas.BusRoutes.RoutesList" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="container">
        <div style="margin-top: 30px">
            <asp:Button ID="btnCreate1" runat="server" OnClick="BtnCreate1_Click" Text="Dodaj" class="btn btn-primary"></asp:Button>
        </div>

        <asp:UpdatePanel ID="mainData" runat="server">
            <ContentTemplate>
                <div>
                    <asp:Repeater ID="rptRoutesList" runat="server">
                        <HeaderTemplate>
                            <table class="table table-hover">
                                <thead class="thead-default">
                                    <tr>
                                        <th>Nazwa</th>
                                        <th>Cena</th>
                                        <th>Data odjazdu</th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tbody>
                                <tr>
                                    <td><a href="AddRoutes.aspx?routeID=<%# Eval("Id")%> ">
                                        <asp:Label runat="server" ID="nameLabel" Text='<%# Eval("Name") %>' /></a></td>
                                    <td>
                                        <asp:Label runat="server" ID="priceLabel" Text='<%# Eval("Price") %>' /></td>
                                    <td>
                                        <asp:Label runat="server" ID="dateLabel" Text='<%# Eval("DepartDate") %>' /></td>
                                </tr>
                            </tbody>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>

                    <asp:Repeater ID="rptPages" runat="server">
                        <HeaderTemplate>
                            <table border="0">
                                <tr class="text">
                                    <td><b>Page:</b>&nbsp;</td>
                                    <td>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnPage" CommandName="Page" CommandArgument="<%# Container.DataItem %>" CssClass="text" runat="server"><%# Container.DataItem %> </asp:LinkButton>&nbsp;
                        </ItemTemplate>
                        <FooterTemplate>
                            </td>
                              </tr>
                              </table>
                        </FooterTemplate>
                    </asp:Repeater>

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
