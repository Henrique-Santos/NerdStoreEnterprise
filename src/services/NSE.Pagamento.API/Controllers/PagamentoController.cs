using Microsoft.AspNetCore.Authorization;
using NSE.WebAPI.Core.Controllers;

namespace NSE.Pagamento.API.Controllers
{
    [Authorize]
    public class PagamentoController : MainController
    {
    }
}