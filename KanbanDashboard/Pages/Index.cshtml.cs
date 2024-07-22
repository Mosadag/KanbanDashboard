using KanbanDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

namespace KanbanDashboard.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		[BindProperty]
		public List<TaskStatusModel>? TaskStatus { get; set; }
		[BindProperty]
		public List<TaskItem> TaskList { get; set; }


		private IMemoryCache _cache;


		public IndexModel(ILogger<IndexModel> logger, IMemoryCache memoryCache)
		{
			_logger = logger;
			_cache = memoryCache;
			TaskStatus = new List<TaskStatusModel>();
			TaskList = new List<TaskItem>();
		}

		public void OnGet()
		{
			LoadInMemoryData();


		}

		private void LoadInMemoryData()
		{
			TaskStatus = new List<TaskStatusModel>();
			if (_cache.TryGetValue("TaskStatus", out List<TaskStatusModel> statusInMemory) &&
				_cache.TryGetValue("TaskList", out List<TaskItem> taskItemInMemory)
				)
			{
				TaskStatus = statusInMemory;
				TaskList = taskItemInMemory;
			}
			else
			{
				TaskStatus = new List<TaskStatusModel>
			{
				new() {
					Id = 1,
					StatusName = "New"
				},
				new() {
					Id = 2,
					StatusName = "UnderProcess"
				},
				new() {
					Id = 3,
					StatusName = "Done"
				}
			};
				TaskList = new List<TaskItem>
			{
				new() {
					Id = 1,

					TaskStatusModelId = 1,
					Title = "Task New 1",
					OrderId=1
				},
				new() {
					Id = 2,

					TaskStatusModelId = 1,
					Title = "Task2 New",
					OrderId=1
				},
				new() {
					Id = 3,

					TaskStatusModelId = 2,
					Title = "Task3 New wew 2",
					OrderId=1
				},
				new() {
					Id = 4,

					TaskStatusModelId = 2,
					Title = "Task4 New 2",
					OrderId=2
				},
				new() {
					Id = 5,

					TaskStatusModelId = 3,
					Title = "Task5 New wew 3",
					OrderId=1
				}
			};
				var cacheEntryOptions = new MemoryCacheEntryOptions()
						 .SetSlidingExpiration(TimeSpan.FromSeconds(6000))
						 .SetAbsoluteExpiration(TimeSpan.FromSeconds(360000))
						 .SetPriority(CacheItemPriority.Normal)
						 .SetSize(1024);
				_cache.Set("TaskStatus", TaskStatus, cacheEntryOptions);
				_cache.Set("TaskList", TaskList, cacheEntryOptions);
			}
		}

		public JsonResult OnPostUpdateTask(TaskItem model)
		{
			LoadInMemoryData();

			return new JsonResult(new { Status = "OK" });
		}
	}
}
