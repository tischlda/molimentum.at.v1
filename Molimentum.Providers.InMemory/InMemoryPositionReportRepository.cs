using System;
using System.Linq;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum;

namespace Molimentum.Providers.InMemory
{
    public class InMemoryPositionReportRepository : InMemoryRepositoryBase, IPositionReportRepository
    {
        public InMemoryPositionReportRepository(IStore store)
            : base(store)
        {

        }

        public PagedList<PositionReport> List(int page, int pageSize)
        {
            var positionReports = (
                from positionReport in Store.PositionReports.Values
                orderby positionReport.PositionDateTime descending
                select positionReport)
                .Skip(pageSize * (page - 1)).Take(pageSize);

            var itemCount = Store.PositionReports.Count();
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
            return Store.PositionReports.ContainsKey(id) ? Store.PositionReports[id] : null;
        }

        public void Save(PositionReport item)
        {
            Store.PositionReports.Add(item.ID, item);
        }

        public void Delete(PositionReport item)
        {
            Store.PositionReports.Remove(item.ID);
        }

        public void SubmitChanges()
        {

        }

        public PagedList<PositionReport> ListPublishedByDate(int page, int pageSize, DateTime from, DateTime to)
        {
            var positionReports = (
                from positionReport in Store.PositionReports.Values
                where positionReport.PositionDateTime <= @from && positionReport.PositionDateTime >= @to
                orderby positionReport.PositionDateTime descending
                select positionReport)
                .Skip(pageSize * (page - 1)).Take(pageSize);

            var itemCount = Store.PositionReports.Where(p => p.Value.PositionDateTime <= from && p.Value.PositionDateTime >= to).Count();
            var pageCount = Utils.CalculatePages(pageSize, itemCount);

            return new PagedList<PositionReport>(positionReports, page, pageSize, pageCount);
        }
    }
}
