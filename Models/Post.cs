namespace Hackathon.Models
{

    using Newtonsoft.Json;
    public class Post
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "pk")]
        public string Pk { get; set; }

        [JsonProperty(PropertyName = "author")]
        public string Author { get; set; }

        [JsonProperty(PropertyName = "authorid")]
        public string AuthorId { get; set; }

        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }

        [JsonProperty(PropertyName = "context")]
        public string Context { get; set; }

        [JsonProperty(PropertyName = "replyto")]
        public string ReplyTo { get; set; }

        [JsonProperty(PropertyName = "replies")]
        public string Replies { get; set; }

        [JsonProperty(PropertyName = "lols")]
        public string Lols { get; set; }

        [JsonProperty(PropertyName = "hearts")]
        public string Hearts { get; set; }

        [JsonProperty(PropertyName = "thumbsup")]
        public string ThumbsUp { get; set; }

        [JsonProperty(PropertyName = "GiraffeFaces")]
        public string GiraffeFaces { get; set; }

    }
}
