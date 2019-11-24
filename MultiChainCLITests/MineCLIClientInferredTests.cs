using MCWrapper.CLI.Connection;
using MCWrapper.CLI.Ledger.Clients;
using MCWrapper.CLI.Tests.ServiceHelpers;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.CLI.Tests.MultiChainCLITests
{
    [TestFixture]
    public class MineCLIClientInferredTests
    {
        // multichain-cli.exe client supports the 'mine' based methods
        private readonly IMultiChainCliMining _cliClient;

        public MineCLIClientInferredTests()
        {
            var provider = new ServiceHelperParameterlessConstructor();

            _cliClient = provider.GetService<IMultiChainCliMining>();
        }

        [Test, Ignore("Not supported by MultiChain v2.02")]
        public async Task GetBlockTemplateAsyncTest()
        {
            // Act - fetch response
            var template = await _cliClient.GetBlockTemplateAsync(string.Empty);

            // Assert
            Assert.IsNotNull(template);
        }

        [Test]
        public async Task GetMiningInfoAsyncTest()
        {
            var miningInfo = await _cliClient.GetMiningInfoAsync();

            Assert.IsNotNull(miningInfo.Result);
            Assert.IsInstanceOf<CliResponse<object>>(miningInfo);
        }

        [Test]
        public async Task GetNetworkHashPsAsyncTest()
        {
            var hashPs = await _cliClient.GetNetworkHashPsAsync(60, 60);

            Assert.IsNotNull(hashPs.Result);
            Assert.IsInstanceOf<CliResponse<object>>(hashPs);
        }

        [Test, Ignore("Not supported by MultiChain v2.02")]
        public async Task PrioritiseTransactionAsyncTest()
        {
            // Act - fetch response
            var prioritise = await _cliClient.PrioritiseTransactionAsync("txid", 1, 1);

            // Assert
            Assert.IsNotNull(prioritise);
        }

        [Test, Ignore("SubmitBlock is ignored because I don't understand how to use it yet")]
        public async Task SubmitBlockAsyncTest()
        {
            // Act - fetch response
            var submit = await _cliClient.SubmitBlockAsync("hex_data", "json_data");

            // Assert
            Assert.IsNotNull(submit);
        }
    }
}
