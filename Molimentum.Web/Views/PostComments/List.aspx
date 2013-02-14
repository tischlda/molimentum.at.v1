<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<PagedList<PostComment>>" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="entry">
        <h1>Gästebuch</h1>
        <div class="entryFooter">
            <%: Html.ActionLink("Kommentieren", "Add", "PostComments", null, null)%>
        </div>
    </div>

    <div class="entry">
        <a name="Comments"><h2>Kommentare</h2></a>
        <div class="entryList" id="postCommentList">
        <%
            var firstEntry = true;

            foreach (var postComment in Model.Items) { %>
            <% Html.RenderPartial("PostCommentDetail", postComment); %>
        <%  } %>
        </div>
    </div>
    <div class="entryListFooter">
        <% Html.RenderPartial("PagerControl", Model); %>
    </div>
    
</asp:Content>