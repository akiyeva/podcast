using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Query;
using Podcast.B.Helpers;
using Podcast.BLL.Services.Contracts;
using Podcast.BLL.ViewModels.SpeakerViewModels;
using Podcast.BLL.ViewModels.TopicViewModels;
using Podcast.DAL.DataContext.Entities;
using Podcast.DAL.Repositories.Contracts;
using System.Linq.Expressions;

namespace Podcast.BLL.Services;

public class TopicManager : CrudManager<Topic, TopicViewModel, TopicCreateViewModel, TopicUpdateViewModel>, ITopicService
{
    private readonly IRepositoryAsync<Topic> _repository;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly string FOLDER_PATH = "";
    public TopicManager(IRepositoryAsync<Topic> repository, IWebHostEnvironment webHostEnvironment, IMapper mapper) : base(repository, mapper)
    {
        _repository = repository;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
        FOLDER_PATH = Path.Combine(_webHostEnvironment.WebRootPath, "admin", "images", "topics");
    }
    public override async Task<TopicViewModel?> GetAsync(int id)
    {
        var topic = await _repository.GetAsync(id);

        if (topic == null) throw new Exception();

        var topicViewModel = _mapper.Map<TopicViewModel>(topic);
        return topicViewModel;
    }
    public override Task<TopicViewModel?> GetAsync(Expression<Func<Topic, bool>> predicate, Func<IQueryable<Topic>, IIncludableQueryable<Topic, object>>? include = null, Func<IQueryable<Topic>, IOrderedQueryable<Topic>>? orderBy = null)
    {
        return base.GetAsync(predicate, include, orderBy);
    }
    public override async Task<IEnumerable<TopicViewModel>> GetListAsync(Expression<Func<Topic, bool>>? predicate = null, Func<IQueryable<Topic>, IIncludableQueryable<Topic, object>>? include = null, Func<IQueryable<Topic>, IOrderedQueryable<Topic>>? orderBy = null)
    {
        var topicList = await _repository.GetListAsync(predicate, include, orderBy);

        var topicViewModelList = _mapper.Map<List<TopicViewModel>>(topicList);

        return topicViewModelList;
    }

    public override async Task<TopicViewModel> CreateAsync(TopicCreateViewModel createViewModel)
    {
        if (createViewModel.CoverFile is null) throw new Exception();
        if (createViewModel.Name is null) throw new Exception();

        #region FileValidations
        if (!createViewModel.CoverFile.CheckType()) throw new Exception();
        if (!createViewModel.CoverFile.CheckSize(2)) throw new Exception();
        #endregion

        createViewModel.CoverUrl = await createViewModel.CoverFile.CreateImageAsync(FOLDER_PATH);

        var topic = _mapper.Map<Topic>(createViewModel);
        var createdTopic = await _repository.CreateAsync(topic);

        var topicViewModel = _mapper.Map<TopicViewModel>(createdTopic);
        return topicViewModel;
    }


    public override async Task<TopicViewModel> UpdateAsync(TopicUpdateViewModel updateViewModel)
    {
        if (updateViewModel.CoverFile != null)
        {
            if (!updateViewModel.CoverFile.CheckType())
            {
                throw new Exception("Invalid file type");
            }

            if (!updateViewModel.CoverFile.CheckSize(2))
            {
                throw new Exception("File size exceeds the limit");
            }

            updateViewModel.CoverUrl = await updateViewModel.CoverFile.CreateImageAsync(FOLDER_PATH);
        }
        else 
        {
            var existingTopic = await _repository.GetAsync(updateViewModel.Id);
            if (existingTopic == null)
            {
                throw new Exception("Topic not found");
            }

            updateViewModel.CoverUrl = existingTopic.CoverUrl;
        }

        var topic = _mapper.Map<Topic>(updateViewModel);

        var updatedTopic = await _repository.UpdateAsync(topic);

        var topicViewModel = _mapper.Map<TopicViewModel>(updatedTopic);

        return topicViewModel;
    }

    public override async Task<TopicViewModel> RemoveAsync(int id)
    {
        var topic = await _repository.GetAsync(id);

        if (topic == null) throw new Exception("Not found");

        var deletedTopic = await _repository.RemoveAsync(topic);

        var topicViewModel = _mapper.Map<TopicViewModel>(deletedTopic);

        return topicViewModel;
    }
}
