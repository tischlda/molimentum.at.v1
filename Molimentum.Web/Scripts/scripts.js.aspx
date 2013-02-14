<%@ Page Language="C#" Inherits="System.Web.UI.Page" ContentType="application/javascript" %>
function loadPostMap(id, from, to) {
    if (GBrowserIsCompatible()) {
        $('#postMap' + id).show();
        var map = new GMap2($('#postMap' + id).get(0));

        map.removeMapType(G_NORMAL_MAP);
        map.removeMapType(G_SATELLITE_MAP);
        map.addMapType(G_HYBRID_MAP);
        map.setMapType(G_HYBRID_MAP);

        map.addControl(new GSmallMapControl(), new GControlPosition(G_ANCHOR_TOP_RIGHT));
        map.setCenter(new GLatLng(90, 0), 1);

        var geoXml = new GGeoXml(
            '<%=Request.Url.Scheme %>://<%=Request.Url.Host %>/PositionReports/Kml.xml?from=' + from + '&to=' + to,
            function () {
            if (geoXml.loadedCorrectly()) {
                geoXml.gotoDefaultViewport(map);
                if (map.getZoom() > 5) map.setZoom(5);
            }
        });

        map.addOverlay(geoXml);
    }
}