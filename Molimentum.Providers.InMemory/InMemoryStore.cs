using System;
using System.Collections.Generic;
using Molimentum.Model;

namespace Molimentum.Providers.InMemory
{
    public class InMemoryStore : IStore
    {
        private IDictionary<string, PositionReport> _positionReports = new Dictionary<string, PositionReport>();
        
        public IDictionary<string, PositionReport> PositionReports
        {
            get { return _positionReports; }
        }


        private IDictionary<string, Post> _posts = new Dictionary<string, Post>();

        public IDictionary<string, Post> Posts
        {
            get { return _posts; }
        }


        private IDictionary<string, PostCategory> _postCategories = new Dictionary<string, PostCategory>();

        public IDictionary<string, PostCategory> PostCategories
        {
            get { return _postCategories; }
        }


        private IDictionary<string, PostComment> _postComments = new Dictionary<string, PostComment>();

        public IDictionary<string, PostComment> PostComments
        {
            get { return _postComments; }
        }
    }
}