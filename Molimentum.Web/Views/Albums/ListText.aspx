<%@ Page Language="C#" Inherits="ViewPage<PagedList<AlbumSummary>>" ContentType="text/plain" %>
<%
foreach (var album in Model.Items) { %>
<%: album.Title %> {<%: album.ID %>}
<%  } %>