<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<PagedList<Video>>" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="Title" runat="server">Videos</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="entry">
        <h1>Videos</h1>
        <div class="entryBody">
            <p>Hinweis: In diesem Teil der Website gibt es nur dann Neues, wenn wir Zugang zu "echtem" Internet, z.B. über einen offenen WiFi Access Point
            oder in einem Internet-Café, haben.</p>
        </div>
    </div>
    <div class="entryList">
        <%  foreach (var video in Model.Items) { %>
        <div class="entry">
            <h1><a href="<%=Url.Action("Detail", new { id = video.ID }) %>"><%=video.Title %></a></h1>
            <div class="entryBody">
                <p><%=video.Description %></p>
                <%=video.EmbedHtml %>
             </div>
             <div class="entryFooter"><%=video.PositionDateTime %></div>
        </div>
        <%  } %>
    </div>
    <div class="entryListFooter">
        <% Html.RenderPartial("PagerControl", Model); %>
    </div>
</asp:Content>
