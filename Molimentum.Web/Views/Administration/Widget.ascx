<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div class="widget">
<h1>Administration</h1>
    <ul class="linkList">
        <li><%=Html.ActionLink("Add post", "Add", "Posts") %></li>
        <li><%=Html.ActionLink("Add article", "Add", "Articles") %></li>
        <li><%=Html.ActionLink("Add position report", "Add", "PositionReports") %></li>
    </ul>
</div>