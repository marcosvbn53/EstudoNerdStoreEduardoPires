using NSE.Core.Messages;

namespace NSE.Core.DomainObjects
{
    public abstract class Entity
    {
        public Guid Id { get; set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        private List<Event> _eventos;
        public IReadOnlyCollection<Event> Eventos => _eventos?.AsReadOnly();

        public void AdicionarEvento(Event evento)
        {
            //O código abaixo diz que se a lista não existir ele criar
            _eventos = _eventos ?? new List<Event>();
            _eventos.Add(evento);
        }

        public void RemoverEvento(Event eventItem)
        {
            _eventos?.Remove(eventItem);
        }

        public void LimparEventos()
        {
            _eventos?.Clear();
        }

        #region Comparação
        //Ele está dizendo que se a nossa classe
        //for comparada com outra instancia da mesma classe
        //Como eu vou saber que estamos tratando de duas entidades unicas
        //iguais ou diferentes
        public override bool Equals(object? obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(this, compareTo)) return false;

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

        //Cadas instancia de um objeto tem um hash code
        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }

        #endregion

    }
}
