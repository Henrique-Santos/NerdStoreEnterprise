using Grpc.Core;
using NSE.WebApp.MVC.Services;
using Polly.CircuitBreaker;
using Refit;
using System.Net;

namespace NSE.WebApp.MVC.Extensions
{
    public class ExceptionMiddleware
    {
        private static IAutenticacaoService _autenticacaoService;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, IAutenticacaoService autenticacaoService)
        {
            // Não é possivel imjetar serviços scoped em serviços singletons. (Apenas fora do construtor)
            _autenticacaoService = autenticacaoService;

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
            catch (RpcException ex)
            {
                //400 Bad Request	    INTERNAL
                //401 Unauthorized      UNAUTHENTICATED
                //403 Forbidden         PERMISSION_DENIED
                //404 Not Found         UNIMPLEMENTED

                var statusCode = ex.StatusCode switch
                {
                    StatusCode.Internal => 400,
                    StatusCode.Unauthenticated => 401,
                    StatusCode.PermissionDenied => 403,
                    StatusCode.Unimplemented => 404,
                    _ => 500
                };

                var httpStatusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), statusCode.ToString());

                HandleRequestException(context, httpStatusCode);
            }
        }

        private static void HandleRequestException(HttpContext context, HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                if (_autenticacaoService.TokenExpirado())
                {
                    if (_autenticacaoService.RefreshTokenValido().Result)
                    {
                        context.Response.Redirect(context.Request.Path);
                        return;
                    }
                }

                _autenticacaoService.Logout();
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