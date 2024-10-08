using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Podcast.BLL.Services.Contracts;
using Podcast.BLL.ViewModels.TopicViewModels;

namespace Podcast.MVC.Areas.AdminPanel.Controllers
{
    public class TopicController : AdminController
    {
        private readonly ITopicService _topicService;
        public TopicController(ITopicService topicService)
        {
            _topicService = topicService;
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

        public IActionResult Edit() 
        {
            return View();
        }

        public async Task<IActionResult> Edit(TopicUpdateViewModel updateViewModel)
        {
            try
            {
                await _topicService.UpdateAsync(updateViewModel);
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("Index");
        }
    }
}