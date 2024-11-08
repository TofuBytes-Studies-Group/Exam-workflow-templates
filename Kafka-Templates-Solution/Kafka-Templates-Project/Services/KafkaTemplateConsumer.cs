
using System.Text.RegularExpressions;
using Confluent.Kafka;

namespace Kafka_Templates_Project.Services
{
    // Extend BackgroundService to run in the background for the lifetime of the application
    public class KafkaTemplateConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<KafkaTemplateConsumer> _logger;

        // The Confluent.Kafka IConsumer interface with Key-Value, like IProducer
        private readonly IConsumer<string, string> _consumer;

        public KafkaTemplateConsumer(IConfiguration configuration, ILogger<KafkaTemplateConsumer> logger)
        {
            _configuration = configuration;
            _logger = logger;

            var config = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],

                //These configs should probably also be setup in application.json:
                // The consumer group ID; consumers with the same group ID share load and process messages as a group. 
                GroupId = "groupId",
                // Configures the consumer to read from the beginning of the topic if there’s no offset.
                AutoOffsetReset = AutoOffsetReset.Earliest

                // If transactions:
                // Set IsonlationLevel to readCommitted to ensure that the consumer reads only the messages committed as part of the transaction. 
                // IsolationLevel = IsolationLevel.ReadCommitted //It is default in .NET though
            };

            // We build the consumer instance with the specified configs.
            _consumer = new ConsumerBuilder<string, string>(config).Build();

        }

        // Using this method for the BackgroundService part
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Subscribe the consumer to the specified topic so it can receive messages from it. Should also be configured somewhere
            _consumer.Subscribe("topic");

            while (!stoppingToken.IsCancellationRequested)
            {
                // Processes the consumed message from the Kafka topic. Could fx call other services
                ProcessMessage(stoppingToken);

                // Adding a delay which is non-blocking and will stop, if the application is shutting down. (I'm not sure if it is strictly necessary tbh)
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            // Close the consumer to commit the offsets and release resources.
            _consumer.Close();
        }

        public void ProcessMessage(CancellationToken stoppingToken)
        {
            try
            {
                // Consume a single message. This will block until a new message is received or the timeout is reached.
                var consumeResult = _consumer.Consume(stoppingToken);

                // The message from the kafka message is accessible like this:
                var message = consumeResult.Message.Value;

                // We can log it fx
                _logger.LogInformation($"Recieved Message: {message}, Key: {consumeResult.Message.Key}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing Kafka message: {ex.Message}");
            }
        }

    }
}
