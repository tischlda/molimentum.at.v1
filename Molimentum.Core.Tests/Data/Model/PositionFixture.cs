using Molimentum.Model;
using NUnit.Framework;

namespace Molimentum.Tests.Model
{
    [TestFixture]
    public class PositionFixture
    {
        [Test]
        public void EqualsTest()
        {
            var position1 = new Position(42, 15);
            var position2 = new Position(42, 15);

            var result = position1.Equals(position2);

            Assert.That(result, Is.True);
        }


        [Test]
        public void NotEqualsTest1()
        {
            var position1 = new Position(42, 15);
            var position2 = new Position(42, 14);

            var result = position1.Equals(position2);

            Assert.That(result, Is.Not.True);
        }


        [Test]
        public void NotEqualsTest2()
        {
            var position1 = new Position(42, 15);
            var position2 = new Position(41, 15);

            var result = position1.Equals(position2);

            Assert.That(result, Is.Not.True);
        }


        [Test]
        public void EqualHashCodeTest()
        {
            var hashCode1 = new Position(42, 15).GetHashCode();
            var hashCode2 = new Position(42, 15).GetHashCode();

            Assert.That(hashCode1, Is.EqualTo(hashCode2));
        }


        [Test]
        public void UnEqualHashCodeTest()
        {
            var hashCode1 = new Position(42, 15).GetHashCode();
            var hashCode2 = new Position(42, 14).GetHashCode();

            Assert.That(hashCode1, Is.Not.EqualTo(hashCode2));
        }

        
        [Test, Sequential]
        public void ParsePositionTest(
            [Values("01°00.00'N 002°00.00'E", "03°00.00'S 004°00.00'W")] string s,
            [Values(1, -3)] double expectedLatitude,
            [Values(2, -4)] double expectedLongitude)
        {
            Position result;

            var success = Position.TryParse(s, out result);

            Assert.That(success, Is.True);
            Assert.That(result.Latitude, Is.EqualTo(expectedLatitude));
            Assert.That(result.Longitude, Is.EqualTo(expectedLongitude));
        }


        [Test, Sequential]
        public void ParsePositionTest(
            [Values("01°00.00'N", "03°00.00'S")] string latitudeString,
            [Values("002°00.00'E", "004°00.00'W")] string longitudeString,
            [Values(1, -3)] double expectedLatitude,
            [Values(2, -4)] double expectedLongitude)
        {
            Position result;

            var success = Position.TryParse(latitudeString, longitudeString, out result);

            Assert.That(success, Is.True);
            Assert.That(result.Latitude, Is.EqualTo(expectedLatitude));
            Assert.That(result.Longitude, Is.EqualTo(expectedLongitude));
        }

        
        [Test, Sequential]
        public void ParseInvalidPositionTest(
            [Values(null, "", "invalid",
                "xx°00.00'N 002°00.00'E",
                "01°xx.xx'N 002°00.00'E",
                "01°00.00'x 002°00.00'E",
                "01°00.00'N xxx°00.00'E",
                "01°00.00'N 002°xx.xx'E",
                "01°00.00'N 002°00.00'x")] string s)
        {
            Position result;

            var success = Position.TryParse(s, out result);

            Assert.That(success, Is.False);
        }


        [Test]
        [TestCase(null, "002°00.00'E")]
        [TestCase("", "002°00.00'E")]
        [TestCase("invalid", "002°00.00'E")]
        [TestCase("01°00.00'N", null)]
        [TestCase("01°00.00'N", "")]
        [TestCase("01°00.00'N", "invalid")]
        public void ParseInvalidPositionTest(
            string latitudeString,
            string longitudeString)
        {
            Position result;

            var success = Position.TryParse(latitudeString, longitudeString, out result);

            Assert.That(success, Is.False);
        }
    }
}
