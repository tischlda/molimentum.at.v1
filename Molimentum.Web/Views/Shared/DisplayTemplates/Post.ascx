<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Molimentum.Model.Post>" %>
<h1><%: Html.ActionLink(Model.Title, "Detail", new { id = Model.ID }) %></h1>
<div class="entryBody">
<% if (Model.DateFrom != null && Model.DateTo != null) { %>
    <h3><%: Model.DateFrom.Value.ToShortDateString() %> - <%: Model.DateTo.Value.ToShortDateString() %></h3>
    <script type="text/javascript">
        $(document).bind('googleLoaded', function () {
            loadPostMap('<%: Model.ID %>', '<%= Url.Encode(Model.DateFrom.Value.ToString("d", CultureInfo.InvariantCulture)) %>', '<%= Url.Encode(Model.DateTo.Value.AddDays(1).ToString("d", CultureInfo.InvariantCulture)) %>');
        });
    </script>
    <div class="postMap" id="postMap<%: Model.ID %>"></div>
    <% } %>
    <%: Html.DisplayTextFor(m => m.Body) %>
</div>
<div class="entryBody">
    <span class="data">
        Kategorie: <%: Html.ActionLink(Model.Category.Title, "DetailByTitle", "PostCategories", new { title = Model.Category.Title}, null) %>
    <% if(Model.Tags.Count > 0) { %>
        | Tags: <% foreach(var tag in Model.Tags) { if(!String.IsNullOrEmpty(tag)) { %><%: Html.ActionLink(tag, "Query", "Posts", new { tag = tag }, null)%> <% } } %>
    <% } %>
    </span>
</div>
<div class="entryFooter">
    <span class="data">
        <%: Html.DisplayFor(m => m.PublishDate) %> |
        <a href="<%: Url.Action("Detail", "Posts", "Comments", new { id = Model.ID })%>"><%: Model.Comments.Count%> Kommentare</a>
    </span>
    <span class="links">
    <% if ((bool)ViewData["Editable"])
        { %>
        |
        <%: Html.ActionLink("Edit", "Edit", new { id = Model.ID })%>
        |
        <%: Html.ActionLink("Delete", "Delete", new { id = Model.ID })%>
    <% } %>
    | <%: Html.ActionLink("Kommentieren", "Add", "PostComments", new { postId = Model.ID }, null)%>
    | <a href="<%: Url.Action("Print", "Posts", new { id = Model.ID })%>" target="_blank">Drucken</a>
    </span>
</div>
