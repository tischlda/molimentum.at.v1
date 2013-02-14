<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<PagedList<PostCategory>>" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="Title" runat="server">Kategorien</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="entry">
        <h1>Kategorien</h1>
        <div class="entryBody">
            <ul class="linkList">
    <%  foreach (var postCategory in Model.Items)
        { %>
                <li><%=Html.ActionLink(postCategory.Title, "Detail", new { id = postCategory.ID }) %></li>
    <%  } %>
            </ul>
        </div>
    </div>
    <div class="entryListFooter">
        <% Html.RenderPartial("PagerControl", Model); %>
    </div>
</asp:Content>