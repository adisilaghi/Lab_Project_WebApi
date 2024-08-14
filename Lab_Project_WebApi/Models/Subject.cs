namespace Lab_Project_WebApi.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Mark> Marks { get; set; } = new List<Mark>();
    }
}
