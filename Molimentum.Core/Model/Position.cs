using System.Globalization;
using System.Text.RegularExpressions;

namespace Molimentum.Model
{
    public class Position
    {
        public virtual double Latitude { get; private set; }
        public virtual double Longitude { get; private set; }

        private const string c_latitudePattern = @"(?<latitudeDegrees>\d+)°(?<latitudeMinutes>\d+\.\d+)'(?<latitudeSign>[NS])";
        private const string c_longitudePattern = @"(?<longitudeDegrees>\d+)°(?<longitudeMinutes>\d+\.\d+)'(?<longitudeSign>[EW])";

        private static readonly Regex s_positionPattern = new Regex(@"^" + c_latitudePattern + " " + c_longitudePattern + "$");
        private static readonly Regex s_latitudePattern = new Regex(@"^" + c_latitudePattern + "$");
        private static readonly Regex s_longitudePattern = new Regex(@"^" + c_longitudePattern + "$");


        /// <summary>
        /// Required for NHibernate
        /// </summary>
        protected Position()
        {

        }


        public Position(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }


        public override bool Equals(object obj)
        {
            return Equals(obj as Position);
        }


        public bool Equals(Position other)
        {
            if (object.ReferenceEquals(this, other)) return true;
            if (object.ReferenceEquals(other, null)) return false;

            return this.Latitude == other.Latitude &&
                   this.Longitude == other.Longitude;
        }


        public override int GetHashCode()
        {
            return Latitude.GetHashCode() ^ Longitude.GetHashCode();
        }

        
        public static bool TryParse(string s, out Position result)
        {
            result = null;

            if (s == null) return false;

            var positionMatch = s_positionPattern.Match(s);

            return TryParse(positionMatch, positionMatch, out result);
        }


        public static bool TryParse(string latitudeString, string longitudeString, out Position result)
        {
            result = null;

            if (latitudeString == null) return false;
            if (longitudeString == null) return false;

            var latitudeMatch = s_latitudePattern.Match(latitudeString);
            var longitudeMatch = s_longitudePattern.Match(longitudeString);

            return TryParse(latitudeMatch, longitudeMatch, out result);
        }


        private static bool TryParse(Match latitudeMatch, Match longitudeMatch, out Position result)
        {
            result = null;

            if (!latitudeMatch.Success) return false;
            if (!longitudeMatch.Success) return false;
            
            var latitudeDegrees = double.Parse(latitudeMatch.Groups["latitudeDegrees"].Value, NumberStyles.Any, NumberFormatInfo.InvariantInfo);
            var latitudeMinutes = double.Parse(latitudeMatch.Groups["latitudeMinutes"].Value, NumberStyles.Any, NumberFormatInfo.InvariantInfo);
            var latitude = latitudeDegrees + latitudeMinutes / 60;
            if (latitudeMatch.Groups["latitudeSign"].Value == "S") latitude *= -1;

            var longitudeDegrees = double.Parse(longitudeMatch.Groups["longitudeDegrees"].Value, NumberStyles.Any, NumberFormatInfo.InvariantInfo);
            var longitudeMinutes = double.Parse(longitudeMatch.Groups["longitudeMinutes"].Value, NumberStyles.Any, NumberFormatInfo.InvariantInfo);
            var longitude = longitudeDegrees + longitudeMinutes / 60;
            if (longitudeMatch.Groups["longitudeSign"].Value == "W") longitude *= -1;

            result = new Position(latitude, longitude);
            
            return true;
        }
    }
}