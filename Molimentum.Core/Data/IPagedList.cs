namespace Molimentum.Data
{
    public interface IPagedList
    {
        int Page { get; }
        int PageSize { get; }
        int Pages { get; }
    }
}