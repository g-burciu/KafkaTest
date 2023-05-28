using Connectors.Entities;

namespace Connectors
{
    public interface IConnector
    {
        void Add(Event entity);
    }
}