using System.Collections.Generic;
using Molimentum.Model;

namespace Molimentum.Web.Models
{
    public class EditPostData : EditData
    {
        public Post Post { get; set; }

        public IEnumerable<PostCategory> PostCategories { get; set; }
    }
}
