using Microsoft.Extensions.Configuration;

namespace Gems.Services
{
    public class FileService
    {
        private static IConfiguration? _configuration;

        public static IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                    _configuration = new ConfigurationBuilder()
                     .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                     .AddJsonFile("appsettings.json")
                     .AddUserSecrets("279fc5d7-ec06-49bb-9630-b8605eaf6933")
                     .Build();

                return _configuration;
            }
        }
    }
}