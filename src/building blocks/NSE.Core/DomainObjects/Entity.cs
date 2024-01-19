using NSE.Core.Messages;

namespace NSE.Core.DomainObjects
{
    public class Entity
    {
        private const int NUMERO_ARBITRARIO = 907;

        private List<Event> _notificacoes;

        public Guid Id { get; set; }
        public IReadOnlyCollection<Event> Notificacoes => _notificacoes?.AsReadOnly();

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public void AdicionarEvento(Event evento)
        {
            _notificacoes = _notificacoes ?? [];
            _notificacoes.Add(evento);
        }

        public void RemoverEvento(Event eventItem)
        {
            _notificacoes?.Remove(eventItem);
        }

        public void LimparEventos()
        {
            _notificacoes?.Clear();
        }

        #region // Comparacões
        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }
        
        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * NUMERO_ARBITRARIO) + Id.GetHashCode();
        }
        #endregion

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }

    }
}