<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<ul id="menu">
    <li><%= Html.ActionLink("Home", "Index", "Posts")%></li>
    <li><%= Html.ActionLink("Fotos", "Index", "Albums")%></li>
    <li><%= Html.ActionLink("Gästebuch", "Index", "PostComments")%></li>
    <li><%= Html.ActionLink("Das Boot", "Detail", "Posts", new { id = "eb0e1b10-36b8-4de9-a303-98c49c730bda" }, null)%></li>
    <li><%= Html.ActionLink("Der Plan", "Detail", "Posts", new { id = "8e2ab327-9d97-41de-986d-71ed59e62dd7" }, null)%></li>
    <li><%= Html.ActionLink("Reiseverlauf", "Map", "PositionReports")%></li>
    <li><%= Html.ActionLink("Shop", "Index", "Shop")%></li>
    <li><a href="http://feeds2.feedburner.com/molimentum"><img id="Img1" runat="server" src="~/Content/images/rss.png" alt="Subscribe" border="0" align="bottom"/></a></li>
</ul>
