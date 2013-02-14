<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Molimentum.Model.PostComment>" %>
<a name="comment:<%: Model.ID %>"><h3><%: Html.DisplayFor(m => m.Title)%></h3></a>
<div class="entryBody"><%=System.Net.WebUtility.HtmlEncode(Model.Body).Replace("\n", "<br>")%></div>
<div class="entryFooter">
    <span class="data"><% if(!String.IsNullOrEmpty(Model.Website)) { %><a href="<%: Html.Encode(Model.Website) %>"><% } %><%: Html.DisplayFor(m => m.Author)%><% if(!String.IsNullOrEmpty(Model.Website)) { %> <img src="/content/images/external_link_icon.gif" class="inline" alt="offsite link" /></a><% } %>, <%: Html.DisplayFor(m => m.PublishDate)%></span>
    <% if(Model.Post != null) { %>
    <span class="data"><i>(Kommentar zu: <%: Html.ActionLink(Model.Post.Title, "Detail", "Posts", new { id = Model.Post.ID }, null) %>)</i></span>
    <% } %>
    <span class="links">
    | <%: Html.ActionLink("Antworten", "Reply", "PostComments", new { id = Model.ID }, null)%>
    <% if ((bool)ViewData["Editable"])
        { %>
        |
        <%: Html.ActionLink("Edit", "Edit", "PostComments", new { id = Model.ID }, null)%>
        |
        <%: Html.ActionLink("Delete", "Delete", "PostComments", new { id = Model.ID }, null)%>
    <% } %>
    </span>
</div>