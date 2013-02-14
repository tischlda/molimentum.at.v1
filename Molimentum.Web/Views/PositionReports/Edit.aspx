<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<EditPositionReportData>" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="Title" runat="server">Positionen - <%=Model.PositionReport.Comment %></asp:Content>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <%=Html.ValidationSummary() %>
    <%  using (Html.BeginForm("Save", "PositionReports", new { id = Model.PositionReport.ID, editMode = Model.EditMode }))
        {%>
    <table class="form">
        <tr>
            <td>
                Position Date:
            </td>
            <td>
                <%= Html.TextBox("positionDateTime", Model.PositionReport.PositionDateTime)%>
            </td>
        </tr>
        <tr>
            <td>
                Comment:
            </td>
            <td>
                <%= Html.TextBox("comment", Model.PositionReport.Comment, new { alt = "comment" })%>
            </td>
        </tr>
        <tr>
            <td>
                Latitude:
            </td>
            <td>
                <%= Html.TextBox("latitude", Model.PositionReport.Position.Latitude, new { alt = "latitude" })%>
            </td>
        </tr>
        <tr>
            <td>
                Longitude:
            </td>
            <td>
                <%= Html.TextBox("longitude", Model.PositionReport.Position.Longitude, new { alt = "longitude" })%>
            </td>
        </tr>
    </table>
    <%: Html.ValidationSummary() %>
    <input type="submit" value="Save" />
    <% if (Model.EditMode == EditMode.Edit)
       { %>
    <%=Html.ActionLink("Cancel", "Detail", new { id = Model.PositionReport.ID })%>
    <% } else { %>
    <%=Html.ActionLink("Cancel", "List")%>
    <% } %>
    <% } %>
</asp:Content>