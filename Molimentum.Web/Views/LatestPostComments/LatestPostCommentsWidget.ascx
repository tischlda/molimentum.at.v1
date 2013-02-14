<%@ Control Language="C#" Inherits="ViewUserControl<PagedList<PostComment>>" %>
<div class="widget">
    <h1>Kommentare</h1>
    <%  foreach (var postCommentGroup in Model.Items.GroupBy(postComments => postComments.PublishDate.Value.Year.ToString("0000") + "/" + postComments.PublishDate.Value.Month.ToString("00") + "/" + postComments.PublishDate.Value.Day.ToString("00")).OrderByDescending(postCommentGroup => postCommentGroup.Key))
        { %>
    <h2><%: postCommentGroup.Key%></h2>
    <ul class="linkList">
    <% foreach (var postComment in postCommentGroup.OrderByDescending(p => p.PublishDate)) { %>
        <li>
        <% if(postComment.Post != null) { %>
            <a href="<%: Url.Action("Detail", "Posts", "comment:" + postComment.ID, new { id = postComment.Post.ID })%>"><%: postComment.Title%></a> (<%: postComment.Author %>)
        <% } else { %>
            <a href="<%: Url.Action("Index", "PostComments", "comment:" + postComment.ID, (object)null)%>"><%: postComment.Title%></a> (<%: postComment.Author %>)
        <% } %>
        </li>
    <%  } %>
    </ul>
    <%  } %>

</div>