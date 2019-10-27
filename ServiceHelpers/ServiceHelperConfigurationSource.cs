using MCWrapper.CLI.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MCWrapper.CLI.Tests.ServiceHelpers
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceHelperConfigurationSource
    {
        /// <summary>
        /// Service provider container; persistent between calls;
        /// </summary>
        private ServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Services container
        /// </summary>
        private ServiceCollection ServiceCollection { get; set; } = new ServiceCollection();

        /// <summary>
        /// Constructor
        /// </summary>
        public ServiceHelperConfigurationSource()
        {
            // fetch JSON config values
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            // Configuration interface
            IConfiguration configuration = builder.Build();

            // Add MultiChain library services to the collection
            ServiceCollection.AddMultiChainCoreCliServices(configuration: configuration);

            // build and store Service provider
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// Locate and return service type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetService<T>() => ServiceProvider.GetService<T>();

        /// <summary>
        /// Managed objects
        /// </summary>
        public void Dispose()
        {
            ServiceProvider.Dispose();
            ServiceCollection.Clear();
        }
    }
}
