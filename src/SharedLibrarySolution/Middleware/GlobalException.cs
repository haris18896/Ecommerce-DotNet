namespace Ecommerce.SharedLibrary.Middleware
{
    public class GlobalException(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            // Declare Variables
            string message = "Sorry, Internal server error occured. Kindly try again!";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string title = "Error";

            try
            {
                await next(context);
                // check if Exception is Too many requests // 429 status code
                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    title = "Warning";
                    message = "Too many requests made.";
                    statusCode = StatusCodes.Status429TooManyRequests;

                    await ModifyHeader(context, title, message, statusCode);
                }

                // Check if Response is UnAuthorized // 401 status code
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    title = "Unauthorized";
                    message = "You are not authorized to access this resource.";
                    statusCode = StatusCodes.Status401Unauthorized;

                    await ModifyHeader(context, title, message, statusCode);
                }

                // if  Response if Forbidden // 403 status code
                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Forbidden";
                    message = "You do not have permission to access this resource.";
                    statusCode = StatusCodes.Status403Forbidden;

                    await ModifyHeader(context, title, message, statusCode);
                }
            }
            catch (Exception ex)
            {
                // Log Original Exceptions / File, Debugger, Console
                LogException.LogExceptions(ex);

                // Check if Exception is timeout
                if (ex is TaskCanceledException || ex is TimeoutException)
                {
                    title = "Request Timeout";
                    message = "Request timeout ... try again";
                    statusCode = StatusCodes.Status408RequestTimeout;

                    await ModifyHeader(context, title, message, statusCode);
                }

                // Check if Exception is not found
                if (ex is FileNotFoundException)
                {
                    title = "File Not Found";
                    message = "The requested file was not found.";
                    statusCode = StatusCodes.Status404NotFound;

                    await ModifyHeader(context, title, message, statusCode);
                }
            }
        }

        public async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
        {
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails
            {
                Title = title,
                Detail = message,
                Status = statusCode
            }), CancellationToken.None);

            return;

        }
    }
}