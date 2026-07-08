using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SistemRumahMakan.Filters
{
    public class LoginFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(user))
            {
                context.Result = new RedirectToActionResult(
                    "Login",
                    "Account",
                    null);
            }

            base.OnActionExecuting(context);
        }
    }
}