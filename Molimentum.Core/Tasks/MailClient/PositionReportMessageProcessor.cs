using Molimentum.Repositories;

namespace Molimentum.Tasks.MailClient
{
    public class PositionReportMessageProcessor : MessageProcessorBase
    {
        private readonly IPositionReportRepository _positionReportRepository;

        public PositionReportMessageProcessor(IPositionReportRepository positionReportRepository)
            : base("POSITION REPORT")
        {
            _positionReportRepository = positionReportRepository;
        }

        public override void ProcessMessage(ParsedMessage parsedMessage)
        {
            base.ProcessMessage(parsedMessage);

            var positionReport = _positionReportRepository.Create();

            positionReport.Comment = parsedMessage["COMMENT"] ?? "";

            SetPosition(parsedMessage, positionReport);

            _positionReportRepository.Save(positionReport);

            _positionReportRepository.SubmitChanges();
        }
    }
}