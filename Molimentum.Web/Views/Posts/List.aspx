<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" CodeBehind="List.aspx.cs" Inherits="Molimentum.Web.Views.Posts.List" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="entryList">
        <% foreach (var post in Model.Items) Html.RenderPartial("PostDetail", post); %>
    </div>
    <div class="entryListFooter">
        <% Html.RenderPartial("PagerControl", Model); %>
        <% if ((bool)ViewData["Editable"]) { %><%=Html.ActionLink("Add", "Add")%><% } %>
    </div>
</asp:Content>