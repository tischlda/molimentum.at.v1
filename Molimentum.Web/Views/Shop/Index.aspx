<%@ Page Language="C#" MasterPageFile="~/Views/Shared/NoSidebar.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="Title" runat="server">Shop</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="entry" style="text-align: right">
        <a href="http://astore.amazon.de/molimentum-21" target="shopframe"><img src="<%=ResolveUrl("~/Content/images/Flags/at.gif") %>" alt="AT" border="0" /> / <img src="<%=ResolveUrl("~/Content/images/Flags/de.gif") %>" alt="DE" border="0"  /> amazon.de</a>
        &nbsp;&nbsp;|&nbsp;&nbsp;
        <a href="http://astore.amazon.co.uk/molimentum0e-21" target="shopframe"><img src="<%=ResolveUrl("~/Content/images/Flags/gb.gif") %>" alt="UK" border="0"  /> amazon.co.uk</a>
        &nbsp;&nbsp;|&nbsp;&nbsp;
        <a href="http://astore.amazon.com/molimentum-20" target="shopframe"><img src="<%=ResolveUrl("~/Content/images/Flags/us.gif") %>" alt="US" border="0"  /> amazon.com</a>
    </div>
    <div class="entry">
        Hier kannst du von uns ausgewählte Bücher zum Thema Segeln bei amazon.de, amazon.co.uk und amazon.com bestellen. Über die Suche findest du auch alle anderen Bücher aus dem amazon-Sortiment.
    </div>
    <div class="entry">
        <iframe name="shopframe" src="http://astore.amazon.de/molimentum-21" width="100%" height="2000" scrolling="auto" frameborder="0"></iframe>
    </div>
</asp:Content>