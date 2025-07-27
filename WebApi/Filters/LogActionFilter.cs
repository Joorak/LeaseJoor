namespace WebApi.Filters
{
    public class LogActionFilter : IActionFilter
    {
        private readonly ILogger<LogActionFilter> _logger;

        public LogActionFilter(ILogger<LogActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context?.HttpContext == null) return;
            if (context.HttpContext.Request.Method.ToLower() == "post" && context.HttpContext.Request.Path.Value!.ToLower().StartsWith("/not_log_path")) return;
            context.HttpContext.Items.Add("StartTime", DateTime.Now);
            context.HttpContext.Items.Add("ActionArguments", context.ActionArguments);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context?.HttpContext == null) return;
            if (context.HttpContext.Request.Method.ToLower() == "post" && context.HttpContext.Request.Path.Value!.ToLower().StartsWith("/not_log_path")) return;

            var startTime = (DateTime)context.HttpContext.Items.FirstOrDefault(i => i.Key.ToString() == "StartTime").Value!;
            var actionArguments = context.HttpContext.Items.FirstOrDefault(i => i.Key.ToString() == "ActionArguments").Value;
            var persianDate = PersianCalendarService.Now.ToString("yyyy/MM/dd");

            _logger.LogInformation("{Date}\t{Time}\t{UserId}\t{Method}\t{Path}\t{LogParam}\t{ElapsedTime}",
                persianDate,
                startTime.ToString("HH:mm:ss.fff"),
                context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0",
                context.HttpContext.Request.Method,
                context.HttpContext.Request.Path.Value,
                JsonSerializer.Serialize(actionArguments),
                DateTime.Now.Subtract(startTime).ToString(@"dd\.hh\:mm\:ss\.fff"));
        }
    }
}