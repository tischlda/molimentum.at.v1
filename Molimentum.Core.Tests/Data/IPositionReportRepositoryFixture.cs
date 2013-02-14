using System;
using System.Linq;
using Molimentum.Model;
using Molimentum.Repositories;
using NUnit.Framework;

namespace Molimentum.Tests
{
    [TestFixture]
    public abstract class IPositionReportRepositoryFixture
    {
        [Test]
        public void PositionReportNotFoundTest()
        {
            using (var repository = CreateIPositionReportRepository())
            {
                var positionReport = repository.Get(Guid.Empty.ToString());

                Assert.IsNull(positionReport);
            }
        }

        [Test]
        public void DeletePositionReportTest()
        {
            var comment = "Comment";
            var dateTime = new DateTime(2009, 06, 20);
            var latitude = 42.24f;
            var longitude = 23.32f;

            using (var repository = CreateIPositionReportRepository())
            {
                var positionReportID = CreatePositionReport(repository, comment, dateTime, latitude, longitude);

                var positionReport = repository.Get(positionReportID);
                repository.Delete(positionReport);
                repository.SubmitChanges();

                var positionReport2 = repository.Get(positionReportID);

                Assert.IsNull(positionReport2);
            }
        }

        [Test]
        public void LoadAndSavePositionReportTest()
        {
            var comment = "Comment";
            var dateTime = new DateTime(2009, 06, 20);
            var latitude = 42.24f;
            var longitude = 23.32f;

            using (var repository = CreateIPositionReportRepository())
            {
                var positionReportID = CreatePositionReport(repository, comment, dateTime, latitude, longitude);

                var positionReport = repository.Get(positionReportID);

                Assert.AreEqual(dateTime, positionReport.PositionDateTime);
                Assert.AreEqual(latitude, positionReport.Position.Latitude);
                Assert.AreEqual(longitude, positionReport.Position.Longitude);
            }
        }

        [Test]
        public void ListPositionReportsTest()
        {
            var comment = "Comment";
            var positionDateTime = DateTime.Now;
            var latitude = 42.24f;
            var longitude = 23.32f;

            using (var repository = CreateIPositionReportRepository())
            {
                for (var i = 0; i < 20; i++)
                {
                    CreatePositionReport(repository, comment, positionDateTime.AddDays(i), latitude, longitude);
                }

                var positionReports = repository.List(1, 20);

                Assert.That(positionReports, Is.Not.Null);
                Assert.That(positionReports.Items.Count(), Is.EqualTo(20));
            }
        }

        [Test]
        [Ignore("Ignored temporary.")]
        public void ListPositionReportsByDateTest()
        {
            var comment = "Comment";
            var positionDateTime = new DateTime(2010, 1, 1);
            var latitude = 42.24f;
            var longitude = 23.32f;

            using (var repository = CreateIPositionReportRepository())
            {
                for (var i = 0; i < 20; i++)
                {
                    CreatePositionReport(repository, comment, positionDateTime.AddDays(i), latitude, longitude);
                }

                var positionReports = repository.ListPublishedByDate(1, 20, new DateTime(2010, 1, 5), new DateTime(2010, 1, 15));

                Assert.That(positionReports, Is.Not.Null);
                Assert.That(positionReports.Items.Count(), Is.EqualTo(11));
            }
        }

        private static string CreatePositionReport(IPositionReportRepository repository, string comment, DateTime dateTime, float latitude, float longitude)
        {
            var positionReport = repository.Create();

            positionReport.Comment = comment;
            positionReport.PositionDateTime = dateTime;
            positionReport.Position = new Position(latitude, longitude);

            repository.Save(positionReport);

            repository.SubmitChanges();

            return positionReport.ID;
        }

        protected abstract IPositionReportRepository CreateIPositionReportRepository();
    }
}