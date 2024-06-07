namespace Projekt.Models
{
    public class Statistic
    {
        public int CompletedTasksLastWeek { get; set; }
        public int UncompletedTasksLastWeek { get; set; }
        public int CompletedTasksLastMonth { get; set; }
        public int UncompletedTasksLastMonth { get; set; }
        public int CompletedTasksLastYear { get; set; }
        public int UncompletedTasksLastYear { get; set; }
        public List<CategoryCount> CategoryCounts { get; set; }
        public double EfficiencyLastWeek { get; set; }
        public double EfficiencyLastMonth { get; set; }
        public double EfficiencyLastYear { get; set; }
    }

    public class CategoryCount
    {
        public string Category { get; set; }
        public int Count { get; set; }
        public int CompletedCount { get; set; }
        public double Efficiency { get; set; }
    }

    public class MonthlyCategoryStatistic
    {
        public string Category { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int CompletedTasks { get; set; }
        public int UncompletedTasks { get; set; }
        public double Efficiency { get; set; }
    }
}
