<%@ Control Language="C#" Inherits="ViewUserControl<PagedList<AlbumSummary>>" %>
<div class="widget">
    <h1>Letzte Fotos</h1>
    <% foreach (var album in Model.Items.Take(1)) { %>
    <h2><a href="<%=Url.Action("Detail", "Albums",  new { id = album.ID }) %>"><%=album.Title %></a></h2>
    <p><a href="<%=Url.Action("Detail", "Albums", new { id = album.ID }) %>"><img src="<%=album.ThumbnailUri %>" /></a></p>
    <%  } %>
</div>