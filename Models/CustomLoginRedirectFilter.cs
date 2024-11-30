using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

public class CustomLoginRedirectFilter : IActionFilter
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomLoginRedirectFilter(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var path = context.HttpContext.Request.Path;
        var isAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;

        // Avoid redirecting to login if the user is already on the login page
        if (!isAuthenticated)
        {
            if (path.StartsWithSegments("/Dashboard/Users"))
            {
                context.Result = new RedirectToActionResult("Admin", "Login", null);
            }
            else
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
