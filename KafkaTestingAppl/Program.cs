using Confluent.Kafka;

var builder = WebApplication.CreateBuilder(args);

// Kafka Producer Config
builder.Services.AddSingleton<IProducer<Null, string>>(sp =>
{
    var config = new ProducerConfig
    {
        BootstrapServers = "localhost:9092",
        Acks = Acks.All,
        EnableIdempotence = true,
        MessageMaxBytes = 100 * 1024 * 1024
    };
    return new ProducerBuilder<Null, string>(config).Build();
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();




//using Confluent.Kafka;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddSingleton<IProducer<Null, string>>(sp =>
//{
//    var config = new ProducerConfig
//    {
//        BootstrapServers = "localhost:9092",
//        Acks = Acks.All,
//        EnableIdempotence = true,
//        MessageMaxBytes = 100 * 1024 * 1024
//    };
//    return new ProducerBuilder<Null, string>(config).Build();
//});

//var app = builder.Build();

//app.UseSwagger();
//app.UseSwaggerUI();

//app.UseHttpsRedirection();
//app.UseAuthorization();
//app.MapControllers();

//app.Run();
