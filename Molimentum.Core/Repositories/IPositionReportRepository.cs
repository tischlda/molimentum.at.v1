using System;
using Molimentum.Model;

namespace Molimentum.Repositories
{
    public interface IPositionReportRepository : IItemRepository<PositionReport>
    {
        PagedList<PositionReport> ListPublishedByDate(int page, int pageSize, DateTime from, DateTime to);
    }
}