<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="Title" runat="server">Positionen</asp:Content>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(document).bind('googleLoaded', function () {
            if (GBrowserIsCompatible()) {
                var map = new GMap2($('#routeMap').get(0));

                map.removeMapType(G_NORMAL_MAP);
                map.addMapType(G_PHYSICAL_MAP);
                map.setMapType(G_PHYSICAL_MAP);

                map.addControl(new GLargeMapControl3D());
                map.addControl(new GMapTypeControl());
                map.setCenter(new GLatLng(90, 0), 1);

                var geoXml = new GGeoXml('<%=Request.Url.Scheme %>://<%=Request.Url.Host %>/PositionReports/Kml.xml', function () {
                    if (geoXml.loadedCorrectly()) {
                        geoXml.gotoDefaultViewport(map);
                    }
                });

                map.addOverlay(geoXml);
            }
        });
    </script>

    <div class="entry">
        <h1>Reiseverlauf</h1>
        
        <div id="routeMap" style="width: 100%; height: 500px;margin: 10px 10px 10px 0px;">
        </div>
    </div>
</asp:Content>
