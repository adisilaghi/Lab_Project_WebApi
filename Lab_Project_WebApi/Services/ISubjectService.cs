

namespace Lab_Project_WebApi.DbContext
{
    public interface ISubjectService
    {
        Task<string> AddSubjectAsync(CreateSubjectDto subjectDto);
        Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync();
        Task UpdateSubjectAsync(int id, UpdateSubjectDto subjectDto);
        Task DeleteSubjectAsync(int id);
        
    }
}
