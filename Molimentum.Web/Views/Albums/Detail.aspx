<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" CodeBehind="Detail.aspx.cs" Inherits="Molimentum.Web.Views.Albums.Detail" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="Title" runat="server"><%=Model.Summary.Title %></asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="entry">
        <h1><%=Model.Summary.Title %></h1>
        
        <div class="entryBody"><%=Model.Summary.Description %></div>

        <ul class="pictureList">
        <%  foreach (var picture in Model.Pictures) { %>
            <li>
                <a href="<%=picture.PictureUri %>" rel="lightbox" title="<%=picture.Description %>">
                    <img src="<%=picture.ThumbnailUri %>" alt="<%=picture.Description %>" />
                </a>
            </li>
        <%  } %>
        </ul>

        <div class="entryFooter"><%=Model.Summary.PositionDateTime%></div>
    </div>
</asp:Content>
