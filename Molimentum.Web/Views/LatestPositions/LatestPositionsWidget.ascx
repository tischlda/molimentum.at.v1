<%@ Control Language="C#" Inherits="ViewUserControl<PositionReport>" %>
<%@ Import Namespace="System.Globalization"%>

<script type="text/javascript">
    $(document).bind('googleLoaded', function () {
        if (GBrowserIsCompatible()) {
            var map = new GMap2($('#latestPositionsMap').get(0));

            map.removeMapType(G_NORMAL_MAP);
            map.removeMapType(G_SATELLITE_MAP);
            map.addMapType(G_HYBRID_MAP);
            map.setMapType(G_HYBRID_MAP);

            map.addControl(new GSmallMapControl(), new GControlPosition(G_ANCHOR_TOP_RIGHT));
            map.setCenter(new GLatLng(90, 0), 1);

            var geoXml = new GGeoXml('<%=Request.Url.Scheme %>://<%=Request.Url.Host %>/PositionReports/Kml.xml?pageSize=5', function () {
                if (geoXml.loadedCorrectly()) {
                    geoXml.gotoDefaultViewport(map);
                }
            });

            map.addOverlay(geoXml);
        }

        var calcGreatCircle = function (lat1, lon1, lat2, lon2) {
            var lat1r = lat1 / 180 * Math.PI;
            var lon1r = lon1 / 180 * Math.PI;
            var lat2r = lat2 / 180 * Math.PI;
            var lon2r = lon2 / 180 * Math.PI;
        
            var dLat = lat2r - lat1r;
            var dLon = lon2r - lon1r;

        
            var y = Math.sin(dLon) * Math.cos(lat2r);
            var x = Math.cos(lat1r) * Math.sin(lat2r) - Math.sin(lat1r) * Math.cos(lat2r) * Math.cos(dLon);
            var bearing = ((Math.atan2(y, x) * 180 / Math.PI) + 360) % 360;


            var a =
                Math.sin(dLat / 2) * Math.sin(dLat / 2) +
                Math.cos(lat1r) * Math.cos(lat2r) * 
                Math.sin(dLon / 2) * Math.sin(dLon / 2); 
            var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a)); 
        
            var distance = c * 10800 / Math.PI;
        
            return { distance: distance, bearing: bearing };
        }

        var repeatString = function (s, n) {
            var newString = "";
        
            for (var i = 0; i < n; i++) newString += s;
        
            return newString;
        }
    
        var formatGeoAngle = function (angle, degreeDigits, positiveSign, negativeSign) {
            var sign = angle > 0 ? positiveSign : negativeSign;
            var absoluteAngle = Math.abs(angle);
        
            var degrees = Math.floor(absoluteAngle);
            var minutes = (absoluteAngle - degrees) * 60;
    
            var degreeString = degrees.toString();
            if (degreeString.length < degreeDigits) degreeString = repeatString("0", degreeDigits - degreeString.length) + degreeString;
        
            var minutesString = minutes.toFixed(1).toString();
        
            return degreeString + "°" + minutesString + "'" + sign;
        }
    
        var formatPosition = function (latitude, longitude) {
            return { latitude: formatGeoAngle(latitude, 2, "N", "S"), longitude: formatGeoAngle(longitude, 3, "E", "W") };
        }

        var showLocation = function (latitude, longitude) {
            var shipLatitude = <%=Model.Position.Latitude.ToString(NumberFormatInfo.InvariantInfo) %>;
            var shipLongitude = <%=Model.Position.Longitude.ToString(NumberFormatInfo.InvariantInfo) %>;

            var formattedPosition = formatPosition(latitude, longitude);

            var greatCircle = calcGreatCircle(latitude, longitude, shipLatitude, shipLongitude);

            $("#yourLatitude").text(formattedPosition.latitude);
            $("#yourLongitude").text(formattedPosition.longitude);
            $("#distanceNM").text(Math.round(greatCircle.distance));
            $("#distanceKM").text(Math.round(greatCircle.distance * 1.852));
            $("#bearing").text(Math.round(greatCircle.bearing));

            $("#distanceAndBearing").show();
        };

        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                showLocation(position.coords.latitude, position.coords.longitude);
            });
        } else {
            if (google.loader.ClientLocation) {
               showLocation(google.loader.ClientLocation.latitude, google.loader.ClientLocation.longitude);
            }
        }
    });
</script>
<div class="widget">
    <h1>Letzte Position</h1>

    <div id="latestPositionsMap" style="width: 100%; height: 250px; overflow: hidden"></div>

    <p>
        <%: Model.Comment %><br />
        <%: Model.PositionDateTime%><br />
        <%: String.Format(GeoFormatProvider.Latitude, "{0}", Model.Position.Latitude)%> <%: String.Format(GeoFormatProvider.Longitude, "{0}", Model.Position.Longitude)%>
    </p>
    
    <div id="distanceAndBearing" style="display: none">
        <h2>Deine Position</h2>
        <p>
            <span id="yourLatitude"></span> <span id="yourLongitude"></span><br />
            Das ist <span id="distanceNM"></span> NM / <span id="distanceKM"></span> km<br /> von Molimentum entfernt.
        </p>
    </div>
    
    <h2>Andere Ansichten</h2>
    <ul class="linkList">
        <li><%= Html.ActionLink("Google Earth", "Kml.kml", "PositionReports")%></li>
        <li><a href="http://www.winlink.org/dotnet/maps/PositionreportsDetail.aspx?callsign=OE1TDA">Winlink 2000 Position Reporter <img class="inline" src="<%=VirtualPathUtility.ToAbsolute("~/Content/images/external_link_icon.gif")%>" /></a></li>
        <li><a href="http://www.findu.com/cgi-bin/winlink.cgi?call=OE1TDA">findu.com <img class="inline" src="<%=VirtualPathUtility.ToAbsolute("~/Content/images/external_link_icon.gif")%>" /></a></li>
        <li><a href="http://shiptrak.org/?callsign=OE1TDA&filter=0">ShipTrak <img class="inline" src="<%=VirtualPathUtility.ToAbsolute("~/Content/images/external_link_icon.gif")%>" /></a></li>
    </ul>
</div>