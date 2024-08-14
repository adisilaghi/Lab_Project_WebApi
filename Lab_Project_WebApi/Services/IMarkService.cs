
namespace Lab_Project_WebApi.DbContext
{
    public interface IMarkService
    {
        Task<string> AddMarkAsync(int studentId, CreateMarkDto markDto);
        Task<IEnumerable<MarkDto>> GetStudentMarksAsync(int studentId);
        Task<IEnumerable<MarkDto>> GetStudentMarksBySubjectAsync(int studentId, int subjectId);
        Task<double?> GetStudentAverageMarkBySubjectAsync(int studentId, int subjectId);
        Task<IEnumerable<StudentAverageDto>> GetStudentsOrderedByAverageMarkAsync(bool ascending);
    }
}
