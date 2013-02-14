<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<EditPostCategoryData>" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="Title" runat="server">Kategorie</asp:Content>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <%=Html.ValidationSummary() %>
    <%  using (Html.BeginForm("Save", "PostCategories", new { id = Model.PostCategory.ID, editMode = Model.EditMode }))
        {%>
    <table class="form">
        <tr>
            <td>
                Titel:
            </td>
            <td>
                <%=Html.TextBox("title", Model.PostCategory.Title, new { size = 30, maxLength = 100})%>
            </td>
        </tr>
        <tr>
            <td>
                Body:
            </td>
            <td>
                <%= Html.TextArea("body", Model.PostCategory.Body, new { cols = 80, rows = 20 })%>
            </td>
        </tr>
        <tr>
            <td>
                Order:
            </td>
            <td>
                <%
                    var selectList = new[] {
                                    new SelectListItem
                                    {
                                        Selected = Model.PostCategory.PostListOrder == PostListOrder.Date,
                                        Text = "Date",
                                        Value = "Date"
                                    },
                                    new SelectListItem
                                    {
                                        Selected = Model.PostCategory.PostListOrder == PostListOrder.Title,
                                        Text = "Title",
                                        Value = "Title"
                                    }
                    };
                %>
                <%= Html.DropDownList("postListOrder", selectList)%>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <input type="submit" value="Save" />
                <%=Html.ActionLink("Cancel", "Index")%>
            </td>
        </tr>
    </table>
    <% } %>
</asp:Content>
