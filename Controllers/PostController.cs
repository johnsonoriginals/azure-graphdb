using System.Linq;

namespace Hackathon.Controllers
{
    using Hackathon.Models;
    using Hackathon.Services;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class PostController : Controller
    {

        private readonly IAsyncGremlinService _asyncGremlinService;

        public PostController(IAsyncGremlinService asyncGremlinService)
        {
            _asyncGremlinService = asyncGremlinService;
        }

        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            var posts = await _asyncGremlinService.GetPosts(null, "topic");
            return View(posts);
        }

        [ActionName("IndexByAuthor")]
        public async Task<IActionResult> IndexByAuthorAsync(string authorId)
        {
            var posts = await _asyncGremlinService.ByAuthor(authorId);
            return View(posts);
        }

        [ActionName("IndexByReaction")]
        public async Task<IActionResult> IndexByReactionAsync(string reactionType, Boolean has = true)
        {
            var posts = await _asyncGremlinService.ByReaction(reactionType, has);
            return View(posts);
        }

        [ActionName("IndexTopTen")]
        public async Task<IActionResult> IndexTopTenAsync()
        {
            var posts = await _asyncGremlinService.TopTen();
            return View(posts);
        }

        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(string id)
        {
            List<Post> posts = new List<Post>();

            var post = await _asyncGremlinService.GetPosts(id, "topic");

            posts.Add(post.First());

            var replies = await _asyncGremlinService.GetReplies(id);

            foreach (var reply in replies)
            {
                posts.Add(reply);
            }

            return View(posts);
        }

        [ActionName("Edit")]
        public async Task<ActionResult> Edit(string id)
        {
            var post = await _asyncGremlinService.GetPosts(id, "topic");

            return View(post.First());
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CommitEditAsync([Bind("Id,Pk,Content")] Post post)
        {
            if (ModelState.IsValid)
            {
                await _asyncGremlinService.UpdatePost(post);
                return RedirectToAction("Index");
            }

            return View(post);
        }

        [ActionName("Reply")]
        public async Task<ActionResult> ReplyAsync(string id)
        {
            var result = await _asyncGremlinService.GetPosts(id, "topic");
            Post post = result.First();
            post.Id = null;
            post.Pk = null;
            post.Content = null;
            post.ReplyTo = id;
            var persons = await _asyncGremlinService.GetAuthors();

            ViewModel viewModal = new ViewModel();
            viewModal.Post = post;
            viewModal.Persons = viewModal.GetPersonsList(persons);

            return View(viewModal);
        }

        [HttpPost]
        [ActionName("Reply")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CommitReplyAsync([Bind("Pk,Id,Content,ReplyTo,Author")] Post post)
        {
            if (ModelState.IsValid)
            {

                post.Id = Guid.NewGuid().ToString();

                await _asyncGremlinService.CreatePost(post, "reply");

                return RedirectToAction("Index");
            }

            return View();
        }

        [ActionName("Create")]
        public async Task<IActionResult> CreateAsync()
        {
            var persons = await _asyncGremlinService.GetAuthors();
            ViewModel viewModal = new ViewModel();
            viewModal.Post = new Post();
            viewModal.Persons = viewModal.GetPersonsList(persons);
            return View(viewModal);
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateThreadAsync([Bind("Pk,Id,Content,Author")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.Id = Guid.NewGuid().ToString();

                await _asyncGremlinService.CreatePost(post, "topic");

                return RedirectToAction("Index");
            }

            return View();
        }

        [ActionName("Delete")]
        public async Task<ActionResult> DeleteThreadAsync(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var posts = await _asyncGremlinService.GetPosts(id, "topic");

            if (posts.First() == null)
            {
                return NotFound();
            }

            return View(posts.First());
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind("Pk")] string id)
        {
            await _asyncGremlinService.DeletePost(id);

            return RedirectToAction("Index");
        }

        [ActionName("AddReaction")]
        public async Task<ActionResult> addReactionAsync(string id, string reactionType)
        {
            await _asyncGremlinService.AddReaction(id, reactionType);
            return Redirect(Request.Headers["Referer"].ToString());
        }

    }
}