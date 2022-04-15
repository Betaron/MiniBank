using Minibank.Core.Exceptions;

namespace Minibank.Web.Middlewares
{
    public class ExceptionMiddleware
    {
        public readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (ValidationException exception)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(
                    new {Message = exception.ValidationMessage});
            }
            catch (FluentValidation.ValidationException exception)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                var errors = exception.Errors
                    .Select(x => $"{x.PropertyName}: {x.ErrorMessage}");

                var errorsMessage = string.Join(Environment.NewLine, errors);

                await httpContext.Response.WriteAsJsonAsync(new {Message = errorsMessage});
            }
            catch (ObjectNotFoundException exception)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                await httpContext.Response.WriteAsJsonAsync(new {Message = exception.Message});
            }
            catch (Exception)
            {
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await httpContext.Response.WriteAsJsonAsync(
                    new {Message = "Возникла внутренняя ошибка"});
            }
        }

    }
}
