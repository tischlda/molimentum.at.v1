using System;
using Molimentum.Model;
using Molimentum.Repositories;
using NUnit.Framework;
using Rhino.Mocks;

namespace Molimentum.Tasks.MailClient.Tests
{
    public class PositionReportMessageProcessorFixture : MessageProcessorFixtureBase<PositionReportMessageProcessor>
    {
        protected override string GetMessageCategory()
        {
            return "POSITION REPORT";
        }

        protected override PositionReportMessageProcessor CreateMessageProcessor()
        {
            return new PositionReportMessageProcessor(null);
        }

        [Test]
        public void ProcessPositionReportMessageTest1()
        {
            var subject = "POSITION REPORT";
            var messageBody = "KEY: " + c_key + "\nLATITUDE: 01°00.00'N\nLONGITUDE: 002°00.00'E\nTIME: 1.2.2009 00:00\nCOMMENT: Testbody";

            var expectedComment = "Testbody";
            var expectedLatitude = 1;
            var expectedLongitude = 2;
            var expectedPositionDateTime = new DateTime(2009, 2, 1, 0, 0, 0);

            var positionReport = ProcessPositionReportMessageTest(subject, messageBody, expectedComment, expectedLatitude, expectedLongitude);

            Assert.That(positionReport.PositionDateTime, Is.EqualTo(expectedPositionDateTime));
        }

        private static PositionReport ProcessPositionReportMessageTest(string subject, string messageBody, string expectedComment, double? expectedLatitude, double? expectedLongitude)
        {
            var positionReport = ProcessMessage(subject, messageBody);

            Assert.That(positionReport.Comment, Is.EqualTo(expectedComment));
            Assert.That(positionReport.Position.Latitude, Is.EqualTo(expectedLatitude));
            Assert.That(positionReport.Position.Longitude, Is.EqualTo(expectedLongitude));

            return positionReport;
        }

        private static PositionReport ProcessMessage(string subject, string messageBody)
        {
            var positionReport = new PositionReport();

            var positionReportRepositoryMock = MockRepository.GenerateMock<IPositionReportRepository>();
            positionReportRepositoryMock.Expect(b => b.Create()).Return(positionReport);
            positionReportRepositoryMock.Expect(b => b.SubmitChanges());

            var parsedMessage = new ParsedMessage("me@here.com", "you@there.com", subject, messageBody, null);

            var positionReportMessageProcessor = new PositionReportMessageProcessor(positionReportRepositoryMock);
            positionReportMessageProcessor.ProcessMessage(parsedMessage);

            positionReportRepositoryMock.VerifyAllExpectations();
            return positionReport;
        }
    }
}
