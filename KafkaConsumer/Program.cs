using Confluent.Kafka;
using Connectors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using Workers;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddScoped<IConnector, SqlConnector>();
        services.AddScoped<IConsumer, Consumer>();
        services.AddScoped<IProducer, Producer>();
    })
    .Build();

TestKafka(host.Services);

await host.RunAsync();

static void TestKafka(IServiceProvider hostProvider)
{
    string? brokerList = ConfigurationManager.AppSettings["EH_FQDN"];
    string? connectionString = ConfigurationManager.AppSettings["EH_CONNECTION_STRING"];
    string? topic = ConfigurationManager.AppSettings["EH_NAME"];
    string? caCertLocation = ConfigurationManager.AppSettings["CA_CERT_LOCATION"];
    string? consumerGroup = ConfigurationManager.AppSettings["CONSUMER_GROUP"];

    using IServiceScope serviceScope = hostProvider.CreateScope();
    IServiceProvider provider = serviceScope.ServiceProvider;

    Console.WriteLine("Initializing Producer");
    var producer = provider.GetRequiredService<IProducer>();
    producer.Produce(brokerList, connectionString, topic, caCertLocation).Wait();
    Console.WriteLine();

    Console.WriteLine("Initializing Consumer");
    var consumer = provider.GetRequiredService<IConsumer>();
    consumer.Consume(brokerList, connectionString, consumerGroup, topic, caCertLocation);


    Console.ReadKey();
}


