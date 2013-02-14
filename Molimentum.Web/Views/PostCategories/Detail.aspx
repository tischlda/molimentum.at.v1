<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" CodeBehind="Detail.aspx.cs" Inherits="Molimentum.Web.Views.PostCategories.Detail" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="Title" runat="server"><%=Model.Title %></asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="entry">
        <h1><%=Model.Title%></h1>
        <div class="entryBody">
            <%=Model.Body%>
        </div>
        <div class="entryBody">
            <h3>Artikel</h3>
            <ul class="linkList">
        <%
           foreach(var post in
               Model.PostListOrder == PostListOrder.Date ?
                Model.Posts.OrderByDescending(p => p.PublishDate) :
                Model.Posts.OrderBy(p => p.Title))
           {
               if (post.IsPublished)
               {%>
                <li>
                <% if(Model.PostListOrder == PostListOrder.Date) {%><%=post.PublishDate.Value.ToShortDateString() %> <% } %>
                <%=Html.ActionLink(post.Title, "Detail", "Posts", new { id = post.ID }, null)%>
                </li>
        <%      }
           } %>
            </ul>
        </div>
        <div class="entryFooter">
            <span class="links">
            <% if ((bool)ViewData["Editable"])
               { %>
                <%=Html.ActionLink("Edit", "Edit", new { id = Model.ID })%>
                |
                <%=Html.ActionLink("Delete", "Delete", new { id = Model.ID })%>
            <% } %>
            </span>
        </div>
    </div>    
</asp:Content>