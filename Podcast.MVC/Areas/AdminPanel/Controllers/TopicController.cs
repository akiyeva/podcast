using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Podcast.BLL.Services.Contracts;
using Podcast.BLL.ViewModels.TopicViewModels;
using Podcast.DAL.DataContext;
using Podcast.DAL.DataContext.Entities;
using Podcast.MVC.Helpers;

namespace Podcast.MVC.Areas.AdminPanel.Controllers
{
    public class TopicController : AdminController
    {
        private readonly ITopicService _topicService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string FOLDER_PATH = "";
        private readonly AppDbContext _appDbContext;
        public TopicController(ITopicService topicService, IWebHostEnvironment webHostEnvironment, AppDbContext appDbContext)
        {
            _topicService = topicService;
            _webHostEnvironment = webHostEnvironment;
            FOLDER_PATH = Path.Combine(_webHostEnvironment.WebRootPath, "admin", "images", "topics");
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var topicList = await _topicService.GetListAsync(include: x => x.Include(y => y.Episodes!));
            return View(topicList);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TopicCreateViewModel createViewModel)
        {
            try
            {
                await _topicService.CreateAsync(createViewModel);
            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction("Index");
        }
    }
}