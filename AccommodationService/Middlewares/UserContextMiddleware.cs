namespace AccommodationService.Middlewares;

public class UserContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated ?? false)
        {
            var name = context.User.FindFirst("name")?.Value;

            context.Items["name"] = name;
        }

        await next(context);
    }
}