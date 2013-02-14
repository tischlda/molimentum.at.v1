using System.Collections.Generic;

namespace Molimentum.Model
{
    public enum PostListOrder
    {
        Date,
        Title
    }

    public class PostCategory : IItem
    {
        public virtual string ID { get; set; }

        public virtual string Title { get; set; }

        public virtual string Body { get; set; }

        public virtual PostListOrder PostListOrder { get; set; }

        private IList<Post> _posts = new List<Post>();

        public virtual IList<Post> Posts
        {
            get { return _posts; }
            set { _posts = value; }
        }
    }
}