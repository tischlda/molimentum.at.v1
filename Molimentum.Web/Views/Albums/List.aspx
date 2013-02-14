<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<PagedList<AlbumSummary>>" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="Title" runat="server">Fotos</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="entry">
        <h1>Fotos</h1>
        <div class="entryBody">
            <p>Hinweis: In diesem Teil der Website gibt es nur dann Neues, wenn wir Zugang zu "echtem" Internet, z.B. über einen offenen WiFi Access Point
            oder in einem Internet-Café, haben.</p>
        </div>
    </div>
    <div class="entryList">
        <%  foreach (var album in Model.Items) { %>
        <div class="entry">
            <h1><a href="<%=Url.Action("Detail", new { id = album.ID }) %>"><%=album.Title %></a></h1>
            <div class="entryBody">
                <a href="<%=Url.Action("Detail", new { id = album.ID }) %>"><img src="<%=album.ThumbnailUri %>" /></a>
                <%=album.Description %>
             </div>
             <div class="entryFooter"><%=album.PositionDateTime %></div>
        </div>
        <%  } %>
    </div>
    <div class="entryListFooter">
        <% Html.RenderPartial("PagerControl", Model); %>
    </div>
</asp:Content>
