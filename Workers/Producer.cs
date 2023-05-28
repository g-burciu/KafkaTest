using Confluent.Kafka;

namespace Workers
{
    public class Producer : IProducer
    {
        public async Task Produce(string? brokerList, string? connStr, string? topic, string? cacertlocation)
        {
            try
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = brokerList,
                    SecurityProtocol = SecurityProtocol.SaslSsl,
                    SaslMechanism = SaslMechanism.Plain,
                    SaslUsername = "$ConnectionString",
                    SaslPassword = connStr,
                    SslCaLocation = cacertlocation,
                    //Debug = "security,broker,protocol"        //Uncomment for librdkafka debugging information
                };
                using (var producer = new ProducerBuilder<long, string>(config).SetKeySerializer(Serializers.Int64).SetValueSerializer(Serializers.Utf8).Build())
                {
                    Console.WriteLine("Sending 10 messages to topic: " + topic + ", broker(s): " + brokerList);
                    for (int x = 0; x < 10; x++)
                    {
                        var msg = string.Format("Sample message #{0} sent at {1}", x, DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss.ffff"));
                        var deliveryReport = await producer.ProduceAsync(topic, new Message<long, string> { Key = DateTime.UtcNow.Ticks, Value = msg });
                        Console.WriteLine(string.Format("Message {0} sent (value: '{1}')", x, msg));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Exception Occurred - {0}", e.Message));
            }
        }
    }
}