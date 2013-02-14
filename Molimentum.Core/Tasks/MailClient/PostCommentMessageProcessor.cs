using Molimentum.Repositories;

namespace Molimentum.Tasks.MailClient
{
    public class PostCommentMessageProcessor : MessageProcessorBase
    {
        private readonly IPostCommentRepository _postCommentRepository;
        private readonly IPostRepository _postRepository;

        public PostCommentMessageProcessor(IPostCommentRepository postCommentRepository, IPostRepository postRepository)
            : base("POSTCOMMENT")
        {
            _postCommentRepository = postCommentRepository;
            _postRepository = postRepository;
        }

        public override void ProcessMessage(ParsedMessage parsedMessage)
        {
            base.ProcessMessage(parsedMessage);

            var postComment = _postCommentRepository.Create();

            postComment.Title = parsedMessage["TITLE"];
            postComment.Body = parsedMessage.Body;
            postComment.Author = parsedMessage["AUTHOR"];
            if(parsedMessage["POSTID"] != null) postComment.Post = _postRepository.Get(parsedMessage["POSTID"]);
            if (parsedMessage["EMAIL"] != null) postComment.Email = parsedMessage["EMAIL"];
            if (parsedMessage["WEBSITE"] != null) postComment.Website = parsedMessage["WEBSITE"];
            postComment.PublishDate = ParseDateTime(parsedMessage);

            _postCommentRepository.Save(postComment);

            _postCommentRepository.SubmitChanges();
        }
    }
}