using Lab_Project_WebApi.DbContext;
using Lab_Project_WebApi.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Lab_Project_WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService addressService;

        public AddressController(IAddressService addressService)
        {
            this.addressService = addressService;
        }

        /// <summary>
        /// Gets the address of a student by ID.
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <returns>The address of the student.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStudentAddress(int id)
        {
            try
            {
                var result = await addressService.GetStudentAddressAsync(id);
                return Ok(result);
            }
            catch (IdNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (StudentNoAddressException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Updates the address of a student by ID.
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <param name="addressDto">The address details to update.</param>
        /// <returns>The updated address of the student.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStudentAddress(int id, CreateAddressDto addressDto)
        {
            try
            {
                var updatedAddress = await addressService.UpdateStudentAddressAsync(id, addressDto);
                return Ok(updatedAddress);
            }
            catch (IdNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
