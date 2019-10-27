using MCWrapper.CLI.Extensions;
using MCWrapper.Ledger.Entities.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MCWrapper.CLI.Tests.ServiceHelpers
{
    public class ServiceHelperExplicitSource
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
        public ServiceHelperExplicitSource()
        {
            // fetch JSON config values
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            // Configuration interface
            IConfiguration configuration = builder.Build();

            // Add MultiChain library services to the collection
            // Our values are null and empty since our local testing environment has the necessary
            // variables preloaded as environment variables. This just demonstrates how a 
            // consumer would go about implementing this type of service container injection. If
            // environment variables are not available then the values listed below MUST be 
            // explicitly set when using this specific and explicit extension method.
            //
            // Even though it is not necessary since there are no values to pass, we still
            // implemented a RuntimeParamOptions instance and passed it to the service container. This
            // parameter is entirely option.
            //
            ServiceCollection.AddMultiChainCoreCliServices(profile => 
                {
                    profile.ChainName = "";
                    profile.ChainAdminAddress = "";
                    profile.ChainBurnAddress = "";
                    profile.ChainBinaryLocation = @"";
                    profile.ChainDefaultLocation = @"";
                    profile.ChainDefaultColdNodeLocation = @"";
                }, runtime => new RuntimeParamOptions());

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
