using KanbanDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

namespace KanbanDashboard.Pages
{
    public class IndexWithThemModel : PageModel
    {
        private readonly ILogger<IndexWithThemModel> _logger;
        [BindProperty]
        public List<StagesModel>? StagesModelList { get; set; }
        [BindProperty]
        public List<TaskModel> TasklList { get; set; }


        private IMemoryCache _cache;


        public IndexWithThemModel(ILogger<IndexWithThemModel> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _cache = memoryCache;
            StagesModelList = new List<StagesModel>();
            TasklList = new List<TaskModel>();
        }

        public void OnGet()
        {
            LoadInMemoryData();


        }

        private void LoadInMemoryData()
        {
            StagesModelList = new List<StagesModel>();
            if (_cache.TryGetValue("TaskStatus", out List<StagesModel> statusInMemory) &&
                _cache.TryGetValue("TaskList", out List<TaskModel> taskItemInMemory)
                )
            {
                StagesModelList = statusInMemory;
                TasklList = taskItemInMemory;
            }
            else
            {
                StagesModelList = new List<StagesModel>();
                StagesModelList.Add(new StagesModel
                {
                    Id = "board-in-progress",
                    Title = "In Progress",
                    TaskList = new List<TaskModel>
    {
       new TaskModel
       {
       Id= "in-progress-1",
        Title= "Research FAQ page UX",
        Comments= "12",
        Badgetext="UX",
        Badge= "success",
        Duedate= "5 April",
        Attachments= "4",
       },
      new TaskModel
       {
       Id= "in-progress-2",
        Title= "Review Javascript code",
        Comments= "8",
        Badgetext="Code Review",
        Badge= "danger",
        Duedate= "10 April",
        Attachments= "2",
       },
    }
                });
                StagesModelList.Add(new StagesModel
                {
                    Id = "board-in-review",
                    Title = "In Review",
                    TaskList = new List<TaskModel>
    {
       new TaskModel
       {
       Id= "in-review-1",
        Title= "Review completed Apps",
        Comments= "17",
        Badgetext="Info",
        Badge= "Info",
        Duedate= "8 April",
        Attachments= "8",
       },
      new TaskModel
       {
       Id= "in-review-2",
        Title= "Find new images for pages",
        Comments= "18",
        Badgetext="Images",
        Badge= "warning",
        Duedate= "2 April",
        Attachments= "2",
       },
    }
                });



                var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(6000))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(360000))
                        .SetPriority(CacheItemPriority.Normal)
                        .SetSize(1024);
                _cache.Set("TaskStatus", StagesModelList, cacheEntryOptions);
                _cache.Set("TaskList", TasklList, cacheEntryOptions);
            }
        }

        public JsonResult OnPostUpdateTask(TaskModel model)
        {
            LoadInMemoryData();
            var item = TasklList.FirstOrDefault(s => s.Id == model.Id);
            if (item != null)
            {
                item.Title = model.Title;
            }
            UpdateCash();



            return new JsonResult(new { Status = "OK" });
        }

        private void UpdateCash()
        {
            _cache.Remove("TaskStatus");
            _cache.Remove("TaskList");
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                         .SetSlidingExpiration(TimeSpan.FromSeconds(6000))
                         .SetAbsoluteExpiration(TimeSpan.FromSeconds(360000))
                         .SetPriority(CacheItemPriority.Normal)
                         .SetSize(1024);
            _cache.Set("TaskStatus", StagesModelList, cacheEntryOptions);
            _cache.Set("TaskList", TasklList, cacheEntryOptions);
        }
    }
}
