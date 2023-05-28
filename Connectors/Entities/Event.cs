using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connectors.Entities
{
    public class Event
    {
        public Guid EventId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
