using Microsoft.AspNetCore.Mvc.Filters;

namespace AutoresApi.Filters
{
    public class myFilter : IActionFilter
    {
        private readonly ILogger<myFilter> _logger;

        public myFilter(ILogger<myFilter> logger)
        {
            this._logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Before Action");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("After the Action");
        }

    }
}
