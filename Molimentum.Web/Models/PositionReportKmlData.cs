using Molimentum;
using Molimentum.Model;

namespace Molimentum.Web.Models
{
    public enum KmlMode
    {
        Kml,
        Xml
    }

    public class PositionReportKmlData
    {
        public PagedList<PositionReport> PositionReports { get; set; }
        public KmlMode KmlMode { get; set; }
    }
}
