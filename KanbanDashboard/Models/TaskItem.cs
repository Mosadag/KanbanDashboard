namespace KanbanDashboard.Models
{
	public class TaskItem
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public int OrderId { get; set; } = 1;
		public int TaskStatusModelId { get; set; }
	}
}
