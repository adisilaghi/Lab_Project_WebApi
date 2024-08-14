using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Lab_Project_WebApi.Exceptions
{
    public class IdNotFoundException(string message) : Exception(message)
    {
    }
    public class StudentNoAddressException(string message) : Exception(message)
    {
    }
    public class NoMarksFoundException(string message) : Exception(message)
    {
    }
    public class InvalidMarkValueException : Exception
    {
        public InvalidMarkValueException(string message) : base(message)
        {
        }
    }

    public class CustomExceptionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is IdNotFoundException)
            {
                context.Result = new ObjectResult(context.Exception.Message)
                {
                    StatusCode = StatusCodes.Status404NotFound
                };

                context.ExceptionHandled = true;
            }
            else if (context.Exception is StudentNoAddressException)
            {
                context.Result = new ObjectResult(context.Exception.Message)
                {
                    StatusCode = StatusCodes.Status404NotFound
                };

                context.ExceptionHandled = true;
            }
            else if (context.Exception is NoMarksFoundException)
            {
                context.Result = new ObjectResult(context.Exception.Message)
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
                context.ExceptionHandled = true;
            }
            else if (context.Exception is InvalidMarkValueException)
            {
                context.Result = new ObjectResult(context.Exception.Message)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
                context.ExceptionHandled = true;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
