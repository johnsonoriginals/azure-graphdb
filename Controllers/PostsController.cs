using Hackathon.Models;
using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Services;
using Microsoft.Extensions.Logging;
using System;

namespace NoEntityFrameworkExample.Controllers
{
    public sealed class PostsController : JsonApiController<Posts, Guid>
    {
        public PostsController(
            IJsonApiOptions options,
            ILoggerFactory loggerFactory,
            IResourceService<Posts, Guid> resourceService)
            : base(options, loggerFactory, resourceService)
        { }
    }
}
