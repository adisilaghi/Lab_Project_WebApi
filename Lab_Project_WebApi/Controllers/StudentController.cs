using Lab_Project_WebApi.DbContext;
using Lab_Project_WebApi.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Lab_Project_WebApi.Controllers
{
    /// <summary>
    /// Controller for managing students.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService studentService;

        public StudentController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        /// <summary>
        /// Creates a new student.
        /// </summary>
        /// <param name="createStudentDto">The details of the created student.</param>
        /// <returns>The details of the created student.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateStudent(CreateStudentDto createStudentDto)
        {
            try
            {
                var student = await studentService.CreateStudentAsync(createStudentDto);
                return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
        /// <summary>
        /// Get a list of all the students.
        /// </summary>
        /// <returns>A list of all students.</returns>
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await studentService.GetAllStudentsAsync();
            return Ok(students);
        }

        /// <summary>
        /// Gets a student by ID.
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <returns>The details of a student.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStudentById(int id)
        {
            try
            {
                var student = await studentService.GetStudentByIdAsync(id);
                return Ok(student);
            }
            catch (IdNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }


        /// <summary>
        /// Updates an existing student by ID.
        /// </summary>
        /// <param name="id">Student ID.</param>
        /// <param name="updateStudentDto">The details of the updated student.</param>
        /// <returns>No content if the update is successful.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateStudent(int id, CreateStudentDto updateStudentDto)
        {
            try
            {
                await studentService.UpdateStudentAsync(id, updateStudentDto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (IdNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a student by ID.
        /// </summary>
        /// <param name="id">Student ID.</param>
        /// <param name="deleteAddress">Would you like to delete the student's address as well?</param>
        /// <returns>No content if the deletion is successful.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStudent(int id, [FromQuery] bool deleteAddress)
        {
            try
            {
                var (studentId, firstName, lastName) = await studentService.DeleteStudentAsync(id, deleteAddress);
                var message = $"Student with ID {studentId} and name {firstName} {lastName} has been deleted.";
                return Ok(new { Message = message });
            }
            catch (IdNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }

}
