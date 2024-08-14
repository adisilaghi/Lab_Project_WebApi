
using Lab_Project_WebApi.Exceptions;
using Lab_Project_WebApi.Models;
using Microsoft.EntityFrameworkCore;


namespace Lab_Project_WebApi.DbContext
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext ctx;

        public StudentService(ApplicationDbContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            return await ctx.Students
                .Select(s => new StudentDto
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Age = s.Age,
                    Address = s.Address != null ? new AddressDto
                    {
                        Id = s.Address.Id,
                        City = s.Address.City,
                        Street = s.Address.Street,
                        Number = s.Address.Number
                    } : null
                })
                .ToListAsync();
        }

        public async Task<StudentDto?> GetStudentByIdAsync(int id)
        {
            var student = await ctx.Students.FindAsync(id);
            if (student == null)
            {
                throw new IdNotFoundException($"Student with ID {id} not found.");
            }

            return await ctx.Students
                .Where(s => s.Id == id)
                .Select(s => new StudentDto
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Age = s.Age,
                    Address = s.Address != null ? new AddressDto
                    {
                        Id = s.Address.Id,
                        City = s.Address.City,
                        Street = s.Address.Street,
                        Number = s.Address.Number
                    } : null
                })
                .FirstOrDefaultAsync();
        }

        public async Task<StudentDto> CreateStudentAsync(CreateStudentDto createStudentDto)
        {
            
            if (string.IsNullOrWhiteSpace(createStudentDto.FirstName) || createStudentDto.FirstName == "string" ||
                string.IsNullOrWhiteSpace(createStudentDto.LastName) || createStudentDto.LastName == "string" ||
                createStudentDto.Age == 0)
            {
                throw new ArgumentException("Invalid input. Please fill out all fields correctly.");
            }

            bool exists = await ctx.Students.AnyAsync(s => s.FirstName == createStudentDto.FirstName && s.LastName == createStudentDto.LastName);
            if (exists)
            {
                throw new InvalidOperationException("A student with the same name already exists.");
            }

            var student = new Student
            {
                FirstName = createStudentDto.FirstName,
                LastName = createStudentDto.LastName,
                Age = createStudentDto.Age
            };

            ctx.Students.Add(student);
            await ctx.SaveChangesAsync();

            return new StudentDto
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Age = student.Age,
            };
        }


        public async Task UpdateStudentAsync(int id, CreateStudentDto updateStudentDto)
        {

            var student = await ctx.Students.Include(s => s.Address).FirstOrDefaultAsync(s => s.Id == id);
           
            if (student == null)
            {
                throw new IdNotFoundException($"Student with ID {id} not found.");
            }
            if (string.IsNullOrWhiteSpace(updateStudentDto.FirstName) || updateStudentDto.FirstName == "string" ||
               string.IsNullOrWhiteSpace(updateStudentDto.LastName) || updateStudentDto.LastName == "string" ||
               updateStudentDto.Age == 0)
            {
                throw new ArgumentException("Invalid input. Please fill out all fields correctly.");
            }

            student.FirstName = updateStudentDto.FirstName;
            student.LastName = updateStudentDto.LastName;
            student.Age = updateStudentDto.Age;

            await ctx.SaveChangesAsync();
        }


        public async Task<IEnumerable<StudentAverageDto>> GetStudentsOrderedByAverageMarkAsync(bool ascending)
        {
            var students = await ctx.Students
                .Select(s => new
                {
                    s.Id,
                    s.FirstName,
                    s.LastName,
                    AverageGrade = s.Marks.Any() ? (double?)s.Marks.Average(m => m.Value) : null
                })
                .ToListAsync();

            if (ascending)
            {
                students = students.OrderBy(s => s.AverageGrade ?? double.MaxValue).ToList(); 
            }
            else
            {
                students = students.OrderByDescending(s => s.AverageGrade ?? double.MinValue).ToList(); 
            }

            return students.Select(s => new StudentAverageDto
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                AverageGrade = s.AverageGrade ?? 0 
            });
        }
        public async Task<(int Id, string FirstName, string LastName)> DeleteStudentAsync(int id, bool deleteAddress)
        {
            var student = await ctx.Students.Include(s => s.Address).FirstOrDefaultAsync(s => s.Id == id);
            if (student == null)
            {
                throw new IdNotFoundException($"Student with ID {id} not found.");
            }

            if (deleteAddress && student.Address != null)
            {
                ctx.Addresses.Remove(student.Address);
            }

            ctx.Students.Remove(student);
            await ctx.SaveChangesAsync();

            
            return (student.Id, student.FirstName, student.LastName);
        }
    }
}

