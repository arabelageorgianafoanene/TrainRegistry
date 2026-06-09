namespace TrainRegistry.API.Exceptions
{
    public class ExceptionsHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionsHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
        }
    }
}
