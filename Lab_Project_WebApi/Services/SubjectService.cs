using Lab_Project_WebApi.DbContext;
using Lab_Project_WebApi.Exceptions;
using Lab_Project_WebApi.Models;
using Microsoft.EntityFrameworkCore;


namespace Project_GradeBook_Web.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ApplicationDbContext ctx;

        public SubjectService(ApplicationDbContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<string?> AddSubjectAsync(CreateSubjectDto subjectDto)
        {
            var existingSubject = await ctx.Subjects
                .Where(s => s.Name == subjectDto.Name)
                .FirstOrDefaultAsync();

            if (existingSubject != null)
            {
                return $"Subject exists with id {existingSubject.Id}";
            }

            var newSubject = new Subject
            {
                Name = subjectDto.Name
            };

            ctx.Subjects.Add(newSubject);
            await ctx.SaveChangesAsync();

            return null;
        }

        public async Task DeleteSubjectAsync(int id)
        {
            var subject = await ctx.Subjects.FindAsync(id);
            if (subject == null)
            {
                throw new IdNotFoundException($"Subject with ID {id} not found.");
            }

            ctx.Subjects.Remove(subject);
            await ctx.SaveChangesAsync();
        }
        public async Task UpdateSubjectAsync(int id, UpdateSubjectDto subjectDto)
        {
            var subject = await ctx.Subjects.FindAsync(id);
            if (subject == null)
            {
                throw new IdNotFoundException($"Subject with ID {id} not found.");
            }

            subject.Name = subjectDto.Name;

            ctx.Subjects.Update(subject);
            await ctx.SaveChangesAsync();
        }

        public async Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync()
        {
            return await ctx.Subjects
                .Select(s => new SubjectDto
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToListAsync();
        }
    }
}
