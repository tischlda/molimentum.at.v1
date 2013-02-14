<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<PagedList<PositionReport>>" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="Title" runat="server">Positionen</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="entryList">
    <%=Html.ActionLink("Add", "Add") %>
    <%  foreach (var positionReport in Model.Items)
        { %>
        <div class="positionReport">
            <h3><%=positionReport.PositionDateTime %>: <%=positionReport.Comment %></h3>
            <div class="entryBody"><%=positionReport.Position.Latitude%> <%=positionReport.Position.Longitude%></div>
            <div class="entryFooter">
                <span class="links">
                    <%=Html.ActionLink("Edit", "Edit", new { id = positionReport.ID })%>
                    |
                    <%=Html.ActionLink("Delete", "Delete", new { id = positionReport.ID })%>
                </span>
            </div>
        </div>
    <%  } %>
    </div>
    <div class="entryListFooter">
        <% Html.RenderPartial("PagerControl", Model); %>
    </div>
</asp:Content>