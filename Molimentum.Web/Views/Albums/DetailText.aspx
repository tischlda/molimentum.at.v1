<%@ Page Language="C#" Inherits="ViewPage<Album>" ContentType="text/plain" %>
<%
foreach (var picture in Model.Pictures.OrderByDescending(p => p.PositionDateTime)) { %>
<%: picture.Title %> <%: picture.PictureUri %>
<%  } %>