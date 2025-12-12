using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;
using System;

namespace Hackathon.Models
{
    public sealed class Posts : Identifiable<Guid>
    {
        [Attr]
        public override Guid Id { get; set; }

        [Attr]
        public string Pk { get; set; }

        [Attr]
        public string Author { get; set; }

        [Attr]
        public string AuthorId { get; set; }

        [Attr]
        public string Content { get; set; }

        [Attr]
        public string Context { get; set; }

        [Attr]
        public string ReplyTo { get; set; }

        [Attr]
        public string Replies { get; set; }

        [Attr]
        public string Lols { get; set; }

        [Attr]
        public string Hearts { get; set; }

        [Attr]
        public string ThumbsUp { get; set; }

        [Attr]
        public string GiraffeFaces { get; set; }

        [Attr]
        public bool IsBlocked { get; set; }

        [Attr]
        public string Title { get; set; }

        [Attr]
        public long DurationInHours { get; set; }

    }
}
