using Confluent.Kafka;
using Connectors;
using System.Diagnostics;

namespace Workers
{
    public class Consumer: IConsumer
    {
        private readonly IConnector _connector;

        public Consumer(IConnector connector)
        {
            this._connector = connector;
        }

        public void Consume(string? brokerList, string? connStr, string? consumergroup, string? topic, string? cacertlocation)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = brokerList,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SocketTimeoutMs = 60000,                //this corresponds to the Consumer config `request.timeout.ms`
                SessionTimeoutMs = 30000,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = "$ConnectionString",
                SaslPassword = connStr,
                SslCaLocation = cacertlocation,
                GroupId = consumergroup,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                BrokerVersionFallback = "1.0.0",        //Event Hubs for Kafka Ecosystems supports Kafka v1.0+, a fallback to an older API will fail
                //Debug = "security,broker,protocol"    //Uncomment for librdkafka debugging information
            };

            using (var consumer = new ConsumerBuilder<string, string>(config).SetKeyDeserializer(Deserializers.Utf8).SetValueDeserializer(Deserializers.Utf8).Build())
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

                consumer.Subscribe(topic);

                Console.WriteLine("Consuming messages from topic: " + topic + ", broker(s): " + brokerList);
                while (true)
                {
                    try
                    {
                        var msg = consumer.Consume(cts.Token);
                        var sw = Stopwatch.StartNew();
                        long duration = 0;

                        Console.WriteLine($"Received: '{msg.Message.Value}'");
                        //_connector.Add(new Connectors.Entities.Event { Content = msg.Message.Value });
                        sw.Stop();
                        duration += sw.ElapsedMilliseconds;
                        Console.WriteLine($"Duration: '{duration}'");
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Consume error: {e.Error.Reason}");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error: {e.Message}");
                    }
                }
            }
        }
    }
}
