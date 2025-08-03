using TestingApi1.Models;

namespace TestingApi1.SaveData
{
    public class Projects
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProjectManagerId { get; set; }
        public List<string> DeveloperIds { get; set; }
        public ProjectStatus Status { get; set; }
    }
}

