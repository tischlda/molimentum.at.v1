<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<EditPostCommentData>" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="Title" runat="server">Kommentar</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
<% using (Html.BeginForm("Save", "PostComments", new { id = Model.PostComment.ID, postId = Model.PostComment.Post != null ? Model.PostComment.Post.ID : null, editMode = Model.EditMode })) {%>
    <%: Html.EditorFor(m => m.PostComment) %>
    <fieldset>
        <div class="editor-label">
            Name unseres Schiffes:
        </div>
        <div class="editor-field">
            <%: Html.TextBox("shipName", "")%>
        </div>
        <%: Html.ValidationSummary() %>
        <p>
            <input type="submit" value="Absenden" />
<% if (Model.PostComment.Post != null) { %>
            <%=Html.ActionLink("Zurück", "Detail", "Posts", new { id = Model.PostComment.Post.ID }, null)%>
<% } else { %>
            <%=Html.ActionLink("Zurück", "Index")%>
<% } %>
        </p>
    </fieldset>
<% } %>
</asp:Content>
