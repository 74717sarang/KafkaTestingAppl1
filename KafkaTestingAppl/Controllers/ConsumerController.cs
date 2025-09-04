using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ConsumerController : ControllerBase
{
    [HttpGet("consume")]
    public IActionResult ConsumeMessage()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "json-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe("json-test-topic");

        var result = consumer.Consume(TimeSpan.FromSeconds(5));
        if (result == null)
            return NotFound("No message available");

        var size = System.Text.Encoding.UTF8.GetByteCount(result.Message.Value);
        return Ok(new
        {
            Message = result.Message.Value,
            SizeKB = size / 1024.0,
            Offset = result.Offset,
            Partition = result.Partition
        });
    }
}
