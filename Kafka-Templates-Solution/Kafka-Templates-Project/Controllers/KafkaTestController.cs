using Kafka_Templates_Project.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kafka_Templates_Project.Controllers;

[ApiController]
[Route("[controller]")]
public class KafkaTestController : ControllerBase
{
    private readonly KafkaTemplateProducer _producer;
    private readonly ILogger<KafkaTestController> _logger;

    public KafkaTestController(KafkaTemplateProducer producer, ILogger<KafkaTestController> logger)
    {
        _producer = producer;
        _logger = logger;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromQuery] string topic, [FromQuery] string key, [FromQuery] string value)
    {
        _logger.LogInformation($"Sending message to topic: {topic} with key: {key} and value: {value}");

        try
        {
            await _producer.ProduceAsync(topic, key, value);
            return Ok("Message sent successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending message: {ex.Message}");
            return StatusCode(500, "An error occurred while sending the message.");
        }
    }

}