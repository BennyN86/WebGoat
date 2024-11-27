using WebGoatCore.Models;
using WebGoatCore.Data;
using WebGoatCore.Validation; // Import the InputValidator namespace
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WebGoatCore.Controllers
{
    [Route("[controller]/[action]")]
    public class BlogController : Controller
    {
        private readonly BlogEntryRepository _blogEntryRepository;
        private readonly BlogResponseRepository _blogResponseRepository;

        public BlogController(BlogEntryRepository blogEntryRepository, BlogResponseRepository blogResponseRepository, NorthwindContext context)
        {
            _blogEntryRepository = blogEntryRepository;
            _blogResponseRepository = blogResponseRepository;
        }

        public IActionResult Index()
        {
            return View(_blogEntryRepository.GetTopBlogEntries());
        }

        [HttpGet("{entryId}")]
        public IActionResult Reply(int entryId)
        {
            return View(_blogEntryRepository.GetBlogEntry(entryId));
        }

        [HttpPost("{entryId}")]
        public IActionResult Reply(int entryId, string contents)
        {
            // Validate input using InputValidator
            if (!InputValidator.ValidateInput(contents))
            {
                ModelState.AddModelError("Contents", "Invalid input. Please ensure it contains only allowed characters and does not exceed 2400 characters.");
                var blogEntry = _blogEntryRepository.GetBlogEntry(entryId);
                return View(blogEntry); // Return to the same view with error messages
            }

            var userName = User?.Identity?.Name ?? "Anonymous";
            var response = new BlogResponse()
            {
                Author = userName,
                Contents = contents,
                BlogEntryId = entryId,
                ResponseDate = DateTime.Now
            };
            _blogResponseRepository.CreateBlogResponse(response);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(string title, string contents)
        {
            var blogEntry = _blogEntryRepository.CreateBlogEntry(title, contents, User!.Identity!.Name!);
            return View(blogEntry);
        }
    }
}
