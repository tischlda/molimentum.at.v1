<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" CodeBehind="Detail.aspx.cs" Inherits="Molimentum.Web.Views.Posts.Detail" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="Title" runat="server"><%: Model.Title %></asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="entry">
        <%: Html.DisplayForModel() %>
    </div>
    <div class="entry">
        <a name="Comments"><h2>Kommentare</h2></a>
        <div class="entryList" id="entryListComments">
            <% foreach (var postComment in Model.Comments) Html.RenderPartial("PostCommentDetail", postComment); %>
        </div>
        <div class="entryFooter">
            <%: Html.ActionLink("Kommentieren", "Add", "PostComments", new { postId = Model.ID }, null)%>
        </div>
    </div>
</asp:Content>