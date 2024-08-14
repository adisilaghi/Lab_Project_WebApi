using Lab_Project_WebApi.DbContext;
using Lab_Project_WebApi.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Lab_Project_WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarkController : ControllerBase
    {
        private readonly IMarkService markService;

        public MarkController(IMarkService markService)
        {
            this.markService = markService;
        }

        /// <summary>
        /// Creates a new mark for a student.
        /// </summary>
        /// <param name="studentId">Student ID</param>
        /// <param name="markDto">Details of the created mark.</param>
        [HttpPost("{studentId}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddMark(int studentId, CreateMarkDto markDto)
        {
            try
            {
                var result = await markService.AddMarkAsync(studentId, markDto);

                if (result.StartsWith("Mark added successfully"))
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (IdNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidMarkValueException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets all marks of a student.
        /// </summary>
        /// <param name="studentId">Student ID</param>
        [HttpGet("{studentId}")]
        [ProducesResponseType(typeof(IEnumerable<MarkDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStudentMarks(int studentId)
        {
            try
            {
                var marks = await markService.GetStudentMarksAsync(studentId);

                if (!marks.Any())
                {
                    return NotFound($"No marks found for student with ID {studentId}.");
                }

                return Ok(marks);
            }
            catch (IdNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Gets all marks of a student in a specific subject.
        /// </summary>
        /// <param name="studentId">Student ID.</param>
        /// <param name="subjectId">Subject ID.</param>
        /// <returns>A list of marks of the student in the specific subject.</returns>
        [HttpGet("{studentId}/{subjectId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStudentMarksBySubject(int studentId, int subjectId)
        {
            try
            {
                var marks = await markService.GetStudentMarksBySubjectAsync(studentId, subjectId);

                if (!marks.Any())
                {

                    return Ok($"No marks found for student with ID {studentId} in subject with ID {subjectId}.");
                }

                return Ok(marks);
            }
            catch (IdNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Gets the average mark for a student in a specific subject.
        /// </summary>
        /// <param name="studentId">Student ID.</param>
        /// <param name="subjectId">Subject ID.</param>
        /// <returns>The average mark of the student in that specific subject.</returns>
        [HttpGet("{studentId}/average/{subjectId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStudentAverageMarkBySubject(int studentId, int subjectId)
        {
            try
            {
                var average = await markService.GetStudentAverageMarkBySubjectAsync(studentId, subjectId);
                return Ok(average);
            }
            catch (IdNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (NoMarksFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Order students by their average mark.
        /// </summary>
        /// <param name="ascending">Would you like to sort the results in ascending order?</param>
        /// <returns>A list of students ordered by their average mark.</returns>
        [HttpGet("average")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStudentsOrderedByAverageMark([FromQuery] bool ascending = true)
        {
            var students = await markService.GetStudentsOrderedByAverageMarkAsync(ascending);
            return Ok(students);
        }
    }
}
