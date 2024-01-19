using MediatR;

namespace NSE.Core.Messages
{
    public class Event : Message, INotification // INotification usado para ser reconhecido como uma notificacão pelo MediatR
    {
        public DateTime Timestamp { get; private set; }

        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}