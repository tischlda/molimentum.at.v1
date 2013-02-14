<%@ Page Language="C#" Inherits="ViewPage<PagedList<PostComment>>" ContentType="text/plain" %>
<%
foreach (var postComment in Model.Items)
{ %>
<% if(postComment.Post != null) {%><%=postComment.Post.Title %>: <%} %><%=postComment.Title %> [<% if(postComment.Post != null) {%><%=postComment.Post.ID %><%} %>]

<%=postComment.Body %>

<%=postComment.Author%> <%=postComment.Email ?? ""%> <%=postComment.Website ?? ""%>
<%=postComment.PublishDate%>

--
<%  } %>