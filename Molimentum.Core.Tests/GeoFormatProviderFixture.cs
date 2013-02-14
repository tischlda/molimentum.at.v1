using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Globalization;

namespace Molimentum.Tests
{
    [TestFixture]
    public class GeoFormatProviderFixture
    {
        [Test]
        public void CanFormatLatitudeTest()
        {
            var latitude = 3.6;
            var expectedString = "03°36.0'N".Replace(".", NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);

            var result = String.Format(GeoFormatProvider.Latitude, "{0}", latitude);

            Assert.That(result, Is.EqualTo(expectedString));
        }

        [Test]
        public void CanFormatLongitudeTest()
        {
            var longitude = 10.6;
            var expectedString = "010°36.0'E".Replace(".", NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);

            var result = String.Format(GeoFormatProvider.Longitude, "{0}", longitude);

            Assert.That(result, Is.EqualTo(expectedString));
        }
    }
}
