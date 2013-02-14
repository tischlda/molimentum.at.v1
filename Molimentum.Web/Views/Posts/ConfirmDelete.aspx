<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<Post>" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="Title" runat="server"><%=Model.Title %></asp:Content>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <%=Html.ValidationSummary() %>
    <%  using (Html.BeginForm("SaveDelete", "Posts", new { id = Model.ID }))
        {%>
        <input type="submit" value="Delete" />
        <%=Html.ActionLink("Cancel", "Detail", new { id = Model.ID })%>
    <% } %>
</asp:Content>
