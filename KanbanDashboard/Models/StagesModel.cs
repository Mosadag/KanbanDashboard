namespace KanbanDashboard.Models
{
    public class StagesModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<TaskModel> TaskList { get; set; }
    }
    public class TaskModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Comments { get; set; }

        //[JsonProperty("badge-text")]
        public string Badgetext { get; set; }
        public string Badge { get; set; }

        //[JsonProperty("due-date")]
        public string Duedate { get; set; }
        public string Attachments { get; set; }
        public List<string> Assigned { get; set; }
        public List<string> Members { get; set; }
        public string Image { get; set; }
    }
}
