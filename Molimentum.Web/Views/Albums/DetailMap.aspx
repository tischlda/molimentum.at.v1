<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" CodeBehind="DetailMap.aspx.cs" Inherits="Molimentum.Web.Views.Albums.DetailMap" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="Title" runat="server"><%=Model.Summary.Title %></asp:Content>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript">
        $(document).ready(function() {
            initializeRouteMap();

            $(document).unload(function() {
                GUnload();
            });

        });

        function initializeRouteMap() {
            if (GBrowserIsCompatible()) {
                var map = new GMap2(document.getElementById('map'));
                map.setCenter(new GLatLng(0, -210), 1);
                map.setMapType(G_PHYSICAL_MAP);

                var options = {noshadow: true};
                var url = "<%=Url.Action("Kml", "Albums", new { id = Model.Summary.ID }) %>";

                var csGeoXml = new CsGeoXml('csGeoXml', map, url, options);
                map.addOverlay(csGeoXml);
                
                /*var geoXml = new GGeoXml('<%=Url.Action("Kml", "Albums", new { id = Model.Summary.ID }) %>');
                map.addOverlay(geoXml);*/
            }
        }
    </script>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="entry">
        <h1><%=Model.Summary.Title %></h1>
        
        <div class="entryBody"><%=Model.Summary.Description %></div>

        <div id="map" style="width: 640px; height: 480px;"></div>
        
        <div class="entryFooter"><%=Model.Summary.PositionDateTime%></div>
    </div>
</asp:Content>
