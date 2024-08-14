namespace Lab_Project_WebApi.DbContext
{
    public class MarkDto
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTime DateAwarded { get; set; }
        public SubjectDto Subject { get; set; }
    }
}
