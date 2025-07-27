namespace WebApi.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(RequestResponse.Failure("Invalid Model State."));
                context.ExceptionHandled = true;
                return;
            }

            context.Result = new BadRequestObjectResult(RequestResponse.Failure($"An error occurred: {context.Exception.Message}"));
            context.ExceptionHandled = true;
        }
    }
}