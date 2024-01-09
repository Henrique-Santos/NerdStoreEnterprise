using Polly.CircuitBreaker;
using Refit;
using System.Net;

namespace NSE.WebApp.MVC.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CustomHttpRequestException e)
            {
                HandleRequestException(context, e.StatusCode);
            }
            catch (ValidationApiException e) // Refit
            {
                HandleRequestException(context, e.StatusCode);
            }
            catch (ApiException e)  // Refit
            {
                HandleRequestException(context, e.StatusCode);
            }
            catch (BrokenCircuitException)  // Polly
            {
                HandleCircuitBreakerException(context);
            }
        }

        private static void HandleRequestException(HttpContext context, HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                context.Response.Redirect($"/login?ReturnUrl={context.Request.Path}");
                return;
            }
            context.Response.StatusCode = (int)statusCode;
        }

        private static void HandleCircuitBreakerException(HttpContext context)
        {
            context.Response.Redirect("/sistem-indisponivel");
        }
    }
}