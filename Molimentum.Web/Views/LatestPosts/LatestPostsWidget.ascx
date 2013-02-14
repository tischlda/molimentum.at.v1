<%@ Control Language="C#" Inherits="ViewUserControl<PagedList<Post>>" %>
<div class="widget">
    <h1>Posts</h1>
    <%  foreach (var postGroup in Model.Items.GroupBy(posts => posts.PublishDate.Value.Year.ToString("0000") + "/" + posts.PublishDate.Value.Month.ToString("00")).OrderByDescending(postGroup => postGroup.Key)) { %>
    <h2><%=postGroup.Key%></h2>
    <ul>
    <% foreach (var post in postGroup.OrderByDescending(p => p.PublishDate)) { %>
    <li>
        <%=Html.ActionLink(post.Title, "Detail", "Posts", new { id = post.ID }, null)%>
    </li>
    <%  } %>
    </ul>
    <%  } %>
</div>