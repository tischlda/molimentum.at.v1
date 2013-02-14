using System.Web.Mvc;
using Molimentum.Web.Models;


namespace Molimentum.Web.Views.PositionReports
{
    public partial class Kml : ViewPage<PositionReportKmlData>
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            Response.ContentType = Model.KmlMode == KmlMode.Xml ? "text/xml" : "application/vnd.google-earth.kml+xml";
        }
    }
}
