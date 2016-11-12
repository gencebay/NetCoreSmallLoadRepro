using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace Common.Attributes
{
    public class DelayAttribute : ActionFilterAttribute
    {
        public DelayAttribute(int milliseconds)
        {
            Delay = TimeSpan.FromMilliseconds(milliseconds);
        }

        public TimeSpan Delay { get; private set; }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // slow down incoming requests
            await Task.Delay(Delay);
            var executedContext = await next();
        }
    }
}
