using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text;

namespace Molimentum.Model
{
    public class PostComment : IItem
    {
        public virtual string ID { get; set; }

        //[Display(Name = "PostComment_Author_Display", Description = "PostComment_Author_Display", ResourceType = typeof(Messages))]
        [DisplayName("Name")]
        [Required(ErrorMessageResourceName = "PostComment_Author_Required_Error", ErrorMessageResourceType = typeof(Messages))]
        [StringLength(100, ErrorMessageResourceName = "PostComment_Author_Length_Error", ErrorMessageResourceType = typeof(Messages))]
        public virtual string Author { get; set; }

        //[Display(Name = "PostComment_Title_Display", ResourceType = typeof(Messages))]
        [DisplayName("Titel")]
        [Required(ErrorMessageResourceName = "PostComment_Title_Required_Error", ErrorMessageResourceType = typeof(Messages))]
        [StringLength(100, ErrorMessageResourceName = "PostComment_Title_Length_Error", ErrorMessageResourceType = typeof(Messages))]
        public virtual string Title { get; set; }

        //[Display(Name = "PostComment_Email_Display", ResourceType = typeof(Messages))]
        [DisplayName("E-Mail")]
        [Required(ErrorMessageResourceName = "PostComment_Email_Required_Error", ErrorMessageResourceType = typeof(Messages))]
        [StringLength(100, ErrorMessageResourceName = "PostComment_Email_Length_Error", ErrorMessageResourceType = typeof(Messages))]
        [RegularExpression(@"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$", ErrorMessageResourceName = "PostComment_Email_Format_Error", ErrorMessageResourceType = typeof(Messages))]
        [DataType(DataType.EmailAddress)]
        public virtual string Email { get; set; }

        //[Display(Name = "PostComment_Website_Display", ResourceType = typeof(Messages))]
        [DisplayName("Website")]
        [StringLength(100, ErrorMessageResourceName = "PostComment_Website_Length_Error", ErrorMessageResourceType = typeof(Messages))]
        [RegularExpression(@"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$", ErrorMessageResourceName = "PostComment_Website_Format_Error", ErrorMessageResourceType = typeof(Messages))]
        [DataType(DataType.Url)]
        public virtual string Website { get; set; }

        //[Display(Name = "PostComment_Body_Display", ResourceType = typeof(Messages))]
        [DisplayName("Kommentar")]
        [Required(ErrorMessageResourceName = "PostComment_Body_Required_Error", ErrorMessageResourceType = typeof(Messages))]
        public virtual string Body { get; set; }

        //[Display(Name = "PostComment_PublishDate_Display", ResourceType = typeof(Messages))]
        [DisplayName("Datum")]
        public virtual DateTime? PublishDate { get; set; }

        public virtual Post Post { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("Post: {0} {1}", Post == null ? "" : Post.ID, Post == null ? "" : Post.Title);
            sb.AppendLine();
            sb.AppendFormat("Author: {0}", Author);
            sb.AppendLine();
            sb.AppendFormat("PublishDate: {0}", PublishDate);
            sb.AppendLine();
            sb.AppendFormat("Title: {0}", Title);
            sb.AppendLine();
            sb.AppendFormat("Email: {0}", Email);
            sb.AppendLine();
            sb.AppendFormat("Website: {0}", Website);
            sb.AppendLine();
            sb.AppendLine();
            sb.Append(Body);

            return sb.ToString();
        }
    }
}