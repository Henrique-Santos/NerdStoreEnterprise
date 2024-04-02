using NSE.Pagamento.API.Data;
using NSE.Pagamento.API.Data.Repository;
using NSE.Pagamento.API.Facade;
using NSE.Pagamento.API.Models;
using NSE.Pagamento.API.Services;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Pagamento.API.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Api
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            // Service
            services.AddScoped<IPagamentoService, PagamentoService>();
            services.AddScoped<IPagamentoFacade, PagamentoCartaoCreditoFacade>();

            // Data
            services.AddScoped<PagamentoContext>();
            services.AddScoped<IPagamentoRepository, PagamentoRepository>();
        }
    }
}