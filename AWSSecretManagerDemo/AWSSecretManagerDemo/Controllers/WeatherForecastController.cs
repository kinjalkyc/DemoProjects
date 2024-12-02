using Amazon.Runtime;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AWSSecretManagerDemo.Controllers
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

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<WeatherForecast> GetAsync()
        {
            var password = await GetPasswordFromSecretManager();
            var data = new WeatherForecast();

            data.Date = DateTime.Now;
            data.TemperatureC = Random.Shared.Next(-20, 55);
            data.Summary = Summaries[Random.Shared.Next(Summaries.Length)];
            data.Password = password;
            return data;
        }

        private static async Task<string> GetPasswordFromSecretManager()
        {
            var profileName = "default"; // Replace with the name of your AWS profile
            var region = Amazon.RegionEndpoint.USEast1; // use your region for AWS profile

            var credentials = new StoredProfileAWSCredentials(profileName);

            var client = new AmazonSecretsManagerClient(credentials, region);


            var response = await client.GetSecretValueAsync(new GetSecretValueRequest()
            {
                SecretId = "MySecretPassword",
                VersionStage = "AWSCURRENT" // Get Current version (Default)
                //AWSPREVIOUS - Get Previous value (Version)
            });
           
            string currentPassword = response.SecretString;
            string currentVersionId = response.VersionId;

            // Simulate secret rotation by generating a new password
            string newPassword = RotatePassword();
            var newResponse = await UpdateSecretValue(client, "MySecretPassword", newPassword);

            string newVersionId = newResponse.VersionId;

            // Promote the pending version to current
            await client.UpdateSecretVersionStageAsync(new UpdateSecretVersionStageRequest
            {
                SecretId = "MySecretPassword",
                VersionStage = "AWSCURRENT",
                MoveToVersionId = newVersionId,
                RemoveFromVersionId = currentVersionId
            });            

            return currentPassword;
        }

        private static string RotatePassword()
        {
            // Implement your logic to generate a new password
            // For demonstration, we'll generate a new random password
            return Guid.NewGuid().ToString("N").Substring(0, 12);
        }

        private static async Task<PutSecretValueResponse> UpdateSecretValue(AmazonSecretsManagerClient client, 
            string secretId, string newSecretValue)
        {
            var request = new PutSecretValueRequest
            {
                SecretId = secretId,
                SecretString = newSecretValue,
                VersionStages = new List<string> { "AWSPENDING" } // Staging the new version as AWSPENDING
            };

            return await client.PutSecretValueAsync(request);
        }
    }
}
