using Connectors.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Connectors
{
    public class SqlConnector : IConnector
    {
        public void Add(Event entity)
        {
            using var db = new EventContext();
            db.Add(new Event { EventId = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Content = entity.Content }); ;
            db.SaveChanges();
        }
    }
}
