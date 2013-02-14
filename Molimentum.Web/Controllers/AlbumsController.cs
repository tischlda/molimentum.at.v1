using System;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Molimentum.Model;
using Molimentum.Repositories;

namespace Molimentum.Web.Controllers
{
    [HandleError]
    public class AlbumsController : ItemControllerBase
    {
        private readonly IPictureRepository _pictureRepository;

        private static XNamespace s_namespaceKml = "http://www.opengis.net/kml/2.2";

        public AlbumsController(IPictureRepository pictureRepository)
        {
            _pictureRepository = pictureRepository;
        }

        public ActionResult Index(int? page, string mode)
        {
            if (page == null) page = 1;
            var pageSize = DefaultPageSize;

            var albums = _pictureRepository.ListAlbums(page.Value, pageSize);

            if ("text".Equals(mode, StringComparison.InvariantCultureIgnoreCase))
            {
                return View("ListText", albums);
            }
            else
            {
                return View("List", albums);
            }
        }

        public ActionResult Detail(string id, string mode)
        {
            var album = LoadAlbum(id);

            if ("text".Equals(mode, StringComparison.InvariantCultureIgnoreCase))
            {
                return View("DetailText", album);
            }
            else if ("map".Equals(mode, StringComparison.InvariantCultureIgnoreCase))
            {
                return View("DetailMap", album);
            }
            else
            {
                return View("Detail", album);
            }
        }

        private Album LoadAlbum(string id)
        {
            var album = _pictureRepository.GetAlbum(id);

            if (album == null) throw new HttpException(404, "Album not found.");
            
            return album;
        }

        //public ActionResult DetailMap(string id)
        //{
        //    return View("DetailMap", LoadAlbum(id));
        //}

        //public ActionResult Kml(string id)
        //{
        //    var album = LoadAlbum(id);

        //    var kml = GenerateKml(album);

        //    return Content(kml.ToString(), "application/vnd.google-earth.kml+xml");
        //}

        //private static XDocument GenerateKml(Album album)
        //{
        //    return new XDocument(
        //        new XElement(s_namespaceKml + "kml",
        //            new XElement(s_namespaceKml + "Document",
        //                GeneratePlacemarks(album))
        //    ));
        //}

        //private static IEnumerable<XElement> GeneratePlacemarks(Album album)
        //{
        //    return
        //        from picture in album.Pictures
        //        select new XElement(s_namespaceKml + "Placemark",
        //            new XElement(s_namespaceKml + "name", picture.Title),
        //            GenerateDescription(picture),
        //            GeneratePoint(picture)
        //        );
        //}

        //private static IEnumerable<XElement> GenerateDescription(Picture picture)
        //{
        //    var description = new StringBuilder();

        //    description.AppendFormat("<h1>{0}</h1>", picture.Description);

        //    description.AppendFormat("<div><img src='{0}' /></div>", picture.PictureUri);

        //    if (!string.IsNullOrEmpty(picture.Description)) description.AppendFormat("<p>{0}</p>", picture.Description);

        //    return GetElement("description", new XCData(description.ToString()));
        //}


        //private static IEnumerable<XElement> GeneratePoint(IPosition position)
        //{
        //    if (position.Latitude == null || position.Longitude == null) return new XElement[] { };

        //    return GetElement(
        //        "Point",
        //        new XElement(s_namespaceKml + "coordinates",
        //            position.Longitude.Value.ToString(NumberFormatInfo.InvariantInfo) +
        //            "," +
        //            position.Latitude.Value.ToString(NumberFormatInfo.InvariantInfo) +
        //            ",0"
        //        )
        //    );
        //}

        //private static IEnumerable<XElement> GetElement(string name, XNode content)
        //{
        //    return new[] { new XElement(s_namespaceKml + name, content) };
        //}
    }
}