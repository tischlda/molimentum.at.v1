using System;
using System.Linq;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum;

namespace Molimentum.Providers.NHibernate
{
    public class NHibernatePositionReportRepository : NHibernateRepositoryBase, IPositionReportRepository
    {
        public NHibernatePositionReportRepository(ISessionBuilder sessionBuilder)
            : base(sessionBuilder)
        {

        }

        public PagedList<PositionReport> List(int page, int pageSize)
        {
            var positionReports = Session.CreateQuery("from PositionReport order by PositionDateTime desc")
                .SetFirstResult((page - 1) * pageSize)
                .SetMaxResults(pageSize)
                .List()
                .Cast<PositionReport>();

            var itemCount = Session.CreateQuery("select count(*) from PositionReport").UniqueResult<Int64>();
            var pageCount = Utils.CalculatePages(pageSize, itemCount);

            return new PagedList<PositionReport>(positionReports, page, pageSize, pageCount);
        }

        public PagedList<PositionReport> ListPublished(int page, int pageSize)
        {
            return List(page, pageSize);
        }

        public PositionReport Create()
        {
            var positionReport = new PositionReport
                                     {
                                         ID = Guid.NewGuid().ToString(),
                                         PositionDateTime = DateTime.Now,
                                         WindDirection = 0,
                                         WindSpeed = 0,
                                         Course = 0,
                                         Speed = 0
                                     };

            return positionReport;
        }

        public PositionReport Get(string id)
        {
            return Session.Get<PositionReport>(id);
        }

        public void Save(PositionReport item)
        {
            Session.Save(item);
        }

        public void Delete(PositionReport item)
        {
            Session.Delete(item);
        }

        public void SubmitChanges()
        {
            Session.Flush();
        }

        public PagedList<PositionReport> ListPublishedByDate(int page, int pageSize, DateTime from, DateTime to)
        {
            var clause = "from PositionReport where PositionDateTime >= (select max(PositionDateTime) from PositionReport where PositionDateTime <= :from) and PositionDateTime <= :to";

            var positionReports = Session.CreateQuery(clause + " order by PositionDateTime desc")
                .SetParameter("from", from)
                .SetParameter("to", to)
                .SetFirstResult((page - 1) * pageSize)
                .SetMaxResults(pageSize)
                .List()
                .Cast<PositionReport>();

            var itemCount = Session.CreateQuery("select count(*) " + clause)
                .SetParameter("from", from)
                .SetParameter("to", to)
                .UniqueResult<Int64>();
            var pageCount = Utils.CalculatePages(pageSize, itemCount);

            return new PagedList<PositionReport>(positionReports, page, pageSize, pageCount);
        }
    }
}
