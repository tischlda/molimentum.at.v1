using System;
using Molimentum.Model;
using System.Collections.Generic;

namespace Molimentum.Providers.InMemory
{
    public interface IStore
    {
        IDictionary<string, PositionReport> PositionReports { get; }
        IDictionary<string, Post> Posts { get; }
        IDictionary<string, PostCategory> PostCategories { get; }
        IDictionary<string, PostComment> PostComments { get; }
    }
}