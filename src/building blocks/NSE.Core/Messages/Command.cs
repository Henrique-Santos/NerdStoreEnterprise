using FluentValidation.Results;
using MediatR;

namespace NSE.Core.Messages
{
    public class Command : Message, IRequest<ValidationResult> // IRequest faz com que a classe Command seja interpretada pelo MediatR
    {
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; set; }

        protected Command()
        {
            Timestamp = DateTime.Now;
        }

        // Pode ser sobreescrito (override) ou não
        public virtual bool EhValido()
        {
            throw new NotImplementedException();
        }
    }
}