using Lab_Project_WebApi.Exceptions;
using Lab_Project_WebApi.Models;
using Microsoft.EntityFrameworkCore;


namespace Lab_Project_WebApi.DbContext
{
    public class AddressService : IAddressService
    {
        private readonly ApplicationDbContext ctx;

        public AddressService(ApplicationDbContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<AddressDto> GetStudentAddressAsync(int studentId)
        {
            var student = await ctx.Students.Include(s => s.Address).FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
            {
                throw new IdNotFoundException($"Student with ID {studentId} not found.");
            }

            if (student.Address == null)
            {
                throw new StudentNoAddressException("Student has no address.");
            }

            return new AddressDto
            {
                Id = student.Address.Id,
                City = student.Address.City,
                Street = student.Address.Street,
                Number = student.Address.Number
            };
        }

        public async Task<AddressDto> UpdateStudentAddressAsync(int id, CreateAddressDto addressDto)
        {
            var student = await ctx.Students.Include(s => s.Address).FirstOrDefaultAsync(s => s.Id == id);
            if (student == null)
            {
                throw new IdNotFoundException($"Student with ID {id} not found.");
            }

            if (student.Address == null)
            {
                student.Address = new Address
                {
                    City = addressDto.City,
                    Street = addressDto.Street,
                    Number = addressDto.Number
                };
            }
            else
            {
                student.Address.City = addressDto.City;
                student.Address.Street = addressDto.Street;
                student.Address.Number = addressDto.Number;
            }

            await ctx.SaveChangesAsync();

            return new AddressDto
            {
                Id = student.Address.Id,
                City = student.Address.City,
                Street = student.Address.Street,
                Number = student.Address.Number
            };
        }
    }
}
