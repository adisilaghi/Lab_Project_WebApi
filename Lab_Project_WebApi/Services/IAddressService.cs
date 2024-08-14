
namespace Lab_Project_WebApi.DbContext
{
    public interface IAddressService
    {
        Task<AddressDto> GetStudentAddressAsync(int studentId);
        Task<AddressDto> UpdateStudentAddressAsync(int studentId, CreateAddressDto addressDto);
    }
}
