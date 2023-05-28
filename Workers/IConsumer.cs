using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workers
{
    public interface IConsumer
    {
        void Consume(string? brokerList, string? connStr, string? consumergroup, string? topic, string? cacertlocation);
    }
}
