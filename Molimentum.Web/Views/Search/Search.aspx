<%@ Page Language="C#" MasterPageFile="~/Views/Shared/NoSidebar.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="Title" runat="server">Search</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="entry">
        <form id="cse-search-box">
          <div>
            <input type="hidden" name="cx" value="partner-pub-3483513874676139:j765bgmm30q" />
            <input type="hidden" name="cof" value="FORID:9" />
            <input type="hidden" name="ie" value="ISO-8859-1" />
            <input type="text" name="q" size="40" />
            <input type="submit" name="sa" value="Suche" />
          </div>
        </form>
        <script type="text/javascript" src="/cse/brand?form=cse-search-box&amp;lang=de"></script>
    </div>

        <div id="cse-search-results"></div>
            <script type="text/javascript">
              var googleSearchIframeName = "cse-search-results";
              var googleSearchFormName = "cse-search-box";
              var googleSearchFrameWidth = 800;
              var googleSearchDomain = "www.google.at";
              var googleSearchPath = "/cse";
            </script>
        <script type="text/javascript" src="http://www.google.com/afsonline/show_afs_search.js"></script>

</asp:Content>