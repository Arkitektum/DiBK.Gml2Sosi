using DiBK.Gml2Sosi.Application.Utils;

namespace DiBK.Gml2Sosi.Web.Middleware
{
    public class SerilogMiddleware
    {
        private readonly RequestDelegate _next;

        public SerilogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            using (ContextCorrelator.BeginCorrelationScope("CorrelationId", Guid.NewGuid().ToString()))
            {
                await _next(context);
            }
        }
    }
}
