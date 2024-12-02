using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSLambdaAPIGateway;

public class Function
{
    private static readonly string[] Summaries = new[]
    {
        "Cool", "Warm", "Hot", "Rainy", "Chilly", "Freezing"
    };
    public List<weatherForecast> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
    {
        Console.WriteLine(JsonSerializer.Serialize(input));
        string cityName = null;
        input.PathParameters?.TryGetValue("cityName", out cityName);
        cityName = cityName ?? "Ahmedabad";

        var rng = new Random();
        return Enumerable.Range(1, 5).Select(index => new weatherForecast
        {
            City = $"{cityName}",
            Date = DateTime.Now.AddDays(index),
            TemperatureC = rng.Next(-20, 55),
            Summary = Summaries[rng.Next(Summaries.Length)]
        }).ToList();
    }
}
public class weatherForecast
{
    public string City { get; set; }
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public string Summary { get; set; }
}
