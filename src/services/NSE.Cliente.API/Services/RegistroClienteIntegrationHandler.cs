using NSE.Clientes.API.Application.Commands;
using NSE.Core.Mediator;
using NSE.Core.Messages.Integration;
using NSE.MessageBus;

namespace NSE.Clientes.API.Services
{
    public class RegistroClienteIntegrationHandler : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IMessageBus _bus;

        public RegistroClienteIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetResponder();

            return Task.CompletedTask;
        }

        private void SetResponder()
        {
            _bus.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(RegistrarCliente);

            _bus.AdvancedBus.Connected += OnConnect; // Evento disparado quando o RabbitMQ se conecta a aplicacão
        }

        private void OnConnect(object s, EventArgs e)
        {
            SetResponder(); // Tenta renovar a subscription quando há uma reconeccão
        }

        private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistradoIntegrationEvent message)
        {
            var clienteCommand = new RegistrarClienteCommand(message.Id, message.Nome, message.Email, message.Cpf);

            using var scope = _serviceProvider.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();

            return new ResponseMessage(await mediator.EnviarComando(clienteCommand));

        }
    }
}