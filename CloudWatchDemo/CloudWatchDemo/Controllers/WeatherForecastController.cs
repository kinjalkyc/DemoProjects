using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;
using Amazon.Runtime;
using Microsoft.AspNetCore.Mvc;

namespace CloudWatchDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get(string cityName)
        {
            
            var count = Random.Shared.Next(5, 15);

            //await logUsingClient(cityName, count);
            _logger.LogInformation($"Get weather forecast called for city {cityName} with count of {count}");

            return Enumerable.Range(1, count).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        private static async Task logUsingClient(string cityName,int count)
        {
            var profileName = "TestDefault"; // Replace with the name of your AWS profile
            var region = Amazon.RegionEndpoint.USEast1; // use your region for AWS profile

            var credentials = new StoredProfileAWSCredentials(profileName);

            var logClient = new AmazonCloudWatchLogsClient(credentials, region);
            var logGroupName = "/aws/weather-forecast-app";
            var logStreamName = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");

            var existing = await logClient.DescribeLogGroupsAsync(new DescribeLogGroupsRequest()
            {
                LogGroupNamePrefix = logGroupName
            });
            var logGroupExists = existing.LogGroups.Any(x => x.LogGroupName == logGroupName);

            if (!logGroupExists)
            {
                await logClient.CreateLogGroupAsync(new CreateLogGroupRequest(logGroupName)); // Create Log group 
            }
            await logClient.CreateLogStreamAsync(new CreateLogStreamRequest(logGroupName, logStreamName)); // Create logstream under log group

            await logClient.PutLogEventsAsync(new PutLogEventsRequest()
            {
                LogGroupName = logGroupName,
                LogStreamName = logStreamName,
                LogEvents = new List<InputLogEvent>()
                {
                    new ()
                    {
                        Message = $"Get weather forecast called for city {cityName} with count of {count}", // This message will be logged in its log group under log stream.
                        Timestamp = DateTime.UtcNow
                    }
                }
            });
        }
    }
}
