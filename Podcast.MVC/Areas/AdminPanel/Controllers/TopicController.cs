using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Podcast.BLL.Services.Contracts;
using Podcast.BLL.ViewModels.TopicViewModels;
using AutoMapper;
using NuGet.Protocol.Core.Types;

namespace Podcast.MVC.Areas.AdminPanel.Controllers
{
    public class TopicController : AdminController
    {
        private readonly ITopicService _topicService;
        private readonly IMapper _mapper;

        public TopicController(ITopicService topicService, IMapper mapper)
        {
            _topicService = topicService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var topicList = await _topicService.GetListAsync();
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

        public async Task<IActionResult> Edit(int id)
        {
            var topic = await _topicService.GetAsync(id);

            var updateViewModel = _mapper.Map<TopicUpdateViewModel>(topic);

            return View(updateViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(TopicViewModel topicViewModel)
        {
            try 
            {
                await _topicService.RemoveAsync(topicViewModel.Id);
            }
            catch(Exception)
            {
                throw;
            }

            return RedirectToAction("Index");
        }
    }
}