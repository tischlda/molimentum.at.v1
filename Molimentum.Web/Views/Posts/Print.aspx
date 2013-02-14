<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Print.Master" CodeBehind="Print.aspx.cs" Inherits="Molimentum.Web.Views.Posts.Print" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="Title" runat="server"><%=Model.Title %></asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="entry">
        <h1><%=Html.ActionLink(Model.Title, "Detail", new { id = Model.ID })%></h1>
        <div class="entryBody"><%=Model.Body %></div>
        <div class="entryFooter">
            <span class="data">
                <%=Model.PublishDate%>
            </span>
        </div>
    </div>
</asp:Content>