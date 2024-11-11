# Kafka Setup

Make sure that you have `Confluent.Kafka` installed with NuGet:

```dotnet add package Confluent.Kafka```



Make sure you have added the necessary Kafka configs in `appsettings.json`:

```
"Kafka": {
    "BootstrapServers": "localhost:9092"
}
```

Make sure to add the producer and consumer services in `Program.cs`:

```
// Add the producer service as singletons:
builder.Services.AddSingleton<KafkaTemplateProducer>();

// Add the consumer service as a hosted service (background service that runs for the lifetime of the application):
builder.Services.AddHostedService<KafkaTemplateConsumer>();
```


Modify the services `KafkaTemplateConsumer.cs` and `KafkaTemplateProducer.cs` to consume and produce the correct data. Rename them when you have finished modifying them.



When testing start project and use Swagger to send a message to the topic 'topic' and check the debug window to see the consumer consume and process the message you send 