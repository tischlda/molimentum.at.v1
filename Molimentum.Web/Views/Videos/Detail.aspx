<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<Video>" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="Title" runat="server"><%=Model.Title %></asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="entry">
        <h1><%=Model.Title%></h1>
        <div class="entryBody">
            <%=Model.EmbedHtml%>
         </div>
         <div class="entryFooter"><%=Model.PositionDateTime%></div>
    </div>
</asp:Content>
