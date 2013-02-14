<%@ Page Language="C#" Inherits="ViewPage<PagedList<Post>>" ContentType="text/plain" %>
<%
foreach (var post in Model.Items) { %>
<%=post.Title %>: [<%=post.ID %>]
<%  } %>