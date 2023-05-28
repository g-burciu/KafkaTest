﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workers
{
    public interface IProducer
    {
        Task Produce(string? brokerList, string? connStr, string? topic, string? cacertlocation);
    }
}
