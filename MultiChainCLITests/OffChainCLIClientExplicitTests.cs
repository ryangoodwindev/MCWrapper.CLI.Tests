using MCWrapper.CLI.Ledger.Clients;
using MCWrapper.CLI.Tests.ServiceHelpers;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.CLI.Tests.MultiChainCLITests
{
    [TestFixture]
    class OffChainCLIClientExplicitTests
    {
        // multichain-cli.exe client supports the 'offchain' based methods
        private readonly OffChainCliClient OffChain;

        public OffChainCLIClientExplicitTests()
        {
            var provider = new ServiceHelperParameterlessConstructor();

            OffChain = provider.GetService<OffChainCliClient>();
        }

        [Test, Ignore("Ignored until I can test with enterprise edition")]
        public async Task PurgePublishedItemsAsyncTest()
        {
            var purge = await OffChain.PurgePublishedItemsAsync(OffChain.CliOptions.ChainName, "some_txid(s)");

            Assert.IsNotNull(purge);
        }

        [Test, Ignore("Ignored until I can test with enterprise edition")]
        public async Task PurgeStreamItemsAsyncTest()
        {
            var purge = await OffChain.PurgeStreamItemsAsync(OffChain.CliOptions.ChainName, "some_stream_identifier", "some_txid(s)");

            Assert.IsNotNull(purge);
        }

        [Test, Ignore("Ignored until I can test with enterprise edition")]
        public async Task RetrieveStreamItemsAsyncTest()
        {
            var retrieve = await OffChain.RetrieveStreamItemsAsync(OffChain.CliOptions.ChainName, "some_stream_identifier", "some_txid(s)");

            Assert.IsNotNull(retrieve);
        }
    }
}
