<%@ Page Language="C#" CodeBehind="Kml.aspx.cs" Inherits="Molimentum.Web.Views.PositionReports.Kml" %><?xml version="1.0" encoding="UTF-8"?>
<%@ Import Namespace="System.Globalization" %>
<kml xmlns="http://www.opengis.net/kml/2.2">
    <Document>
        <name>Molimentum</name>
        <open>1</open>
        <description></description>
        <Style id="path">
            <LineStyle>
                <color>7f0000ff</color>
                <width>4</width>
            </LineStyle>
        </Style>
        <Style id="currentPosition">
            <IconStyle>
                <Icon>
                    <href>http://molimentum.at/content/images/sailing.png</href>
                </Icon>
            </IconStyle>
        </Style>
        <Style id="pastPosition">
            <IconStyle>
                <Icon>
                    <href>http://molimentum.at/content/images/red-pushpin.png</href>
                </Icon>
            </IconStyle>
        </Style>
        <StyleMap id="currentPositionStyleMap">
            <Pair>
                <key>normal</key>
                <styleUrl>#currentPosition</styleUrl>
            </Pair>
            <Pair>
                <key>highlight</key>
                <styleUrl>#currentPosition</styleUrl>
            </Pair>
        </StyleMap>
        <StyleMap id="pastPositionStyleMap">
            <Pair>
                <key>normal</key>
                <styleUrl>#pastPosition</styleUrl>
            </Pair>
            <Pair>
                <key>highlight</key>
                <styleUrl>#pastPosition</styleUrl>
            </Pair>
        </StyleMap>
        <Folder>
            <name>Positions</name>
            <description></description>
<%
    var first = true;
    foreach (var positionReport in Model.PositionReports.Items)
    {
%>
            <Placemark>
                <name><%=positionReport.PositionDateTime %></name>
                <description><%=positionReport.Comment %></description>
                <visibility>1</visibility>
                <% if (first)
                   { %>
                <styleUrl>#currentPositionStyleMap</styleUrl>
                <% } else { %>
                <styleUrl>#pastPositionStyleMap</styleUrl>
                <% } %>
                <Point>
                    <extrude>1</extrude>
                    <altitudeMode>relativeToGround</altitudeMode>
                    <coordinates><%=positionReport.Position.Longitude.ToString(CultureInfo.InvariantCulture)%>,<%=positionReport.Position.Latitude.ToString(CultureInfo.InvariantCulture)%>,0</coordinates>
                </Point>
            </Placemark>
<%
        first = false;
    }
%>
        </Folder>
        <Placemark>
            <name>Path</name>
            <description></description>
            <styleUrl>#path</styleUrl>
            <LineString>
                <extrude>1</extrude>
                <tessellate>1</tessellate>
                <altitudeMode>absolute</altitudeMode>
                <coordinates>
                <% foreach (var positionReport in Model.PositionReports.Items) { %>
                  <%=positionReport.Position.Longitude.ToString(CultureInfo.InvariantCulture)%>,<%=positionReport.Position.Latitude.ToString(CultureInfo.InvariantCulture)%>,0
                <% } %>
                </coordinates>
            </LineString>
        </Placemark>
    </Document>
</kml>