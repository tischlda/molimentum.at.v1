<%@ Control Language="C#" Inherits="ViewUserControl<IEnumerable<PostCategory>>" %>
<div class="widget">
    <h1>Kategorien</h1>
    <ul class="linkList">
<%  foreach (var postCategory in Model) { %>
        <li><%=Html.ActionLink(postCategory.Title, "DetailByTitle", "PostCategories", new { title = postCategory.Title}, null) %></li>
<%  } %>
    </ul>
</div>