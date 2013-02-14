<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<EditPostData>" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="Title" runat="server"><%=Model.Post.Title %></asp:Content>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <%=Html.ValidationSummary() %>
    <%  using (Html.BeginForm("Save", "Posts", new { id = Model.Post.ID, editMode = Model.EditMode }))
        {%>
    <table class="form">
        <tr>
            <td>
                Category:
            </td>
            <td>
                <%= Html.DropDownList("categoryId", new SelectList(Model.PostCategories, "ID", "Title", Model.Post.Category != null ? Model.Post.Category.ID : ""))%>
                <%= Html.TextBox("newCategory", "", new { cols = 30 }) %>
            </td>
        </tr>
        <tr>
            <td>
                Title:
            </td>
            <td>
                <%= Html.TextBox("title", Model.Post.Title)%>
            </td>
        </tr>
        <tr>
            <td>
                Body:
            </td>
            <td>
                <%= Html.TextArea("body", Model.Post.Body, new { cols = 80, rows = 20 })%>
            </td>
        </tr>
        <tr>
            <td>
                Published:
            </td>
            <td>
                <%= Html.CheckBox("isPublished", Model.Post.IsPublished)%>
            </td>
        </tr>
        <tr>
            <td>
                Latitude:
            </td>
            <td>
                <%= Html.EditorFor(m => m.Post.Position.Latitude, null, "latitude")%>
            </td>
        </tr>
        <tr>
            <td>
                Longitude:
            </td>
            <td>
                <%= Html.EditorFor(m => m.Post.Position.Longitude, null, "longitude")%>
            </td>
        </tr>
        <tr>
            <td>
                Publishing Date:
            </td>
            <td>
                <%= Html.EditorFor(m => m.Post.PublishDate, null, "publishDate")%>
            </td>
        </tr>
        <tr>
            <td>
                Date From:
            </td>
            <td>
                <%= Html.EditorFor(m => m.Post.DateFrom, null, "dateFrom")%>
            </td>
        </tr>
        <tr>
            <td>
                Date To:
            </td>
            <td>
                <%= Html.EditorFor(m => m.Post.DateTo, null, "dateTo")%>
            </td>
        </tr>
        <tr>
            <td>
                Tags:
            </td>
            <td>
                <%= Html.TextBox("tags", Model.Post.Tags.Concat(";"))%>
            </td>
        </tr>
    </table>
    <%: Html.ValidationSummary() %>
    <input type="submit" value="Save" />
    <% if (Model.EditMode == EditMode.Edit)
       { %>
    <%=Html.ActionLink("Cancel", "Detail", new { id = Model.Post.ID })%>
    <% } else { %>
    <%=Html.ActionLink("Cancel", "List")%>
    <% } %>
    <% } %>
</asp:Content>
