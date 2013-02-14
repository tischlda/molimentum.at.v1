<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<PostComment>" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="Title" runat="server"><%=Model.Title %></asp:Content>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <%=Html.ValidationSummary() %>
    <%  using (Html.BeginForm("SaveDelete", "PostComments", new { id = Model.ID }))
        {%>
        <input type="submit" value="Delete" />
                <% if(Model.Post != null) { %>
                <%=Html.ActionLink("Cancel", "Detail", "Posts", new { id = Model.Post.ID }, null)%>
                <% } else { %>
                <%=Html.ActionLink("Cancel", "Index")%>
                <% } %>
    <% } %>
</asp:Content>
