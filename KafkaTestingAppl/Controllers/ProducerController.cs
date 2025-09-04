using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProducerController : ControllerBase
{
    private readonly IProducer<Null, string> _producer;

    public ProducerController(IProducer<Null, string> producer)
    {
        _producer = producer;
    }

    [HttpPost("send")]
    [RequestSizeLimit(100 * 1024 * 1024)]
    public async Task<IActionResult> SendJson()
    {
        using var reader = new StreamReader(Request.Body);
        var jsonContent = await reader.ReadToEndAsync();

        if (string.IsNullOrWhiteSpace(jsonContent))
            return BadRequest("Empty JSON payload");

        var sizeInBytes = System.Text.Encoding.UTF8.GetByteCount(jsonContent);

        try
        {
            var result = await _producer.ProduceAsync("json-test-topic", new Message<Null, string>
            {
                Value = jsonContent
            });

            return Ok(new
            {
                Status = "Message sent to Kafka",
                SizeKB = sizeInBytes / 1024.0,
                Offset = result.Offset,
                Partition = result.Partition
            });
        }
        catch (ProduceException<Null, string> ex)
        {
            return Problem(detail: ex.Error.Reason, statusCode: 500, title: "Kafka Produce Error");
        }
    }
}




//using Confluent.Kafka;
//using Microsoft.AspNetCore.Mvc;

//[ApiController]
//[Route("api/[controller]")]
//public class ProducerController : ControllerBase
//{
//    private readonly IProducer<Null, string> _producer;

//    public ProducerController(IProducer<Null, string> producer)
//    {
//        _producer = producer;
//    }

//    [HttpPost("send")]
//    [RequestSizeLimit(100 * 1024 * 1024)]
//    public async Task<IActionResult> SendJson()
//    {
//        using var reader = new StreamReader(Request.Body);
//        var jsonContent = await reader.ReadToEndAsync();

//        if (string.IsNullOrWhiteSpace(jsonContent))
//            return BadRequest("Empty JSON payload");

//        var sizeInBytes = System.Text.Encoding.UTF8.GetByteCount(jsonContent);

//        try
//        {
//            var result = await _producer.ProduceAsync("json-test-topic", new Message<Null, string>
//            {
//                Value = jsonContent
//            });

//            return Ok(new
//            {
//                Status = "Message sent to Kafka",   
//                SizeKB = sizeInBytes / 1024.0,
//                Offset = result.Offset,
//                Partition = result.Partition
//            });
//        }
//        catch (ProduceException<Null, string> ex)
//        {
//            return Problem(detail: ex.Error.Reason, statusCode: 500, title: "Kafka Produce Error");
//        }
//    }
//}
