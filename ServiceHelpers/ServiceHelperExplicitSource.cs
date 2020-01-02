using MCWrapper.CLI.Extensions;
using MCWrapper.Ledger.Entities.Options;
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
                    profile.ChainName = "CurrencyTestCoin"; // your value may differ
                    profile.ChainAdminAddress = "15UxmgMF9AM7JcXZKn4JcKutuQ6q7iSNp4RHVg"; // your value may differ
                    profile.ChainBurnAddress = "1XXXXXXXatXXXXXXP9XXXXXXTdXXXXXXXMSFht"; // your value may differ
                    profile.ChainBinaryLocation = string.Empty; // not required; directory is auto-detected
                    profile.ChainDefaultLocation = string.Empty; // not required; directory is auto-detected
                    profile.ChainDefaultColdNodeLocation = string.Empty; // not required; directory is auto-detected
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
