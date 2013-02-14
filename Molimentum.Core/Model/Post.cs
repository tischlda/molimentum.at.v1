using System;
using System.Collections.Generic;

namespace Molimentum.Model
{
    public class Post : IItem, IEditablePosition, ITaggable
    {
        public virtual string ID { get; set; }

        public virtual string Title { get; set; }

        public virtual string Body { get; set; }

        public virtual Guid AuthorID { get; set; }

        public virtual DateTime? PublishDate { get; set; }

        public virtual DateTime? DateFrom { get; set; }

        public virtual DateTime? DateTo { get; set; }

        public virtual DateTime? LastUpdatedTime { get; set; }

        private bool _isPublished;

        public virtual bool IsPublished
        {
            get
            {
                return _isPublished;
            }
            set
            {
                if (PublishDate == null && !_isPublished && value)
                {
                    PublishDate = DateTime.Now.ToUniversalTime();
                }

                _isPublished = value;
            }
        }

        
        public virtual DateTime? PositionDateTime
        {
            get { return PublishDate; }
            set { PublishDate = value; }
        }

        public virtual Position Position { get; set; }


        public virtual PostCategory Category { get; set; }


        private readonly ICollection<string> _tags = new List<string>();

        public virtual ICollection<string> Tags
        {
            get { return _tags; }
        }

        private IList<PostComment> _postComments = new List<PostComment>();

        public virtual IList<PostComment> Comments
        {
            get { return _postComments; }
            set { _postComments = value;  }
        }
    }
}