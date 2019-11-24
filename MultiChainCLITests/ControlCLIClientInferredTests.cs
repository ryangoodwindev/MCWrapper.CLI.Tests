using MCWrapper.CLI.Connection;
using MCWrapper.CLI.Ledger.Clients;
using MCWrapper.CLI.Tests.ServiceHelpers;
using MCWrapper.Data.Models.Control;
using MCWrapper.Ledger.Actions;
using MCWrapper.Ledger.Entities.Constants;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.CLI.Tests.MultiChainCLITests
{
    [TestFixture]
    public class ControlCLIClientInferredTests
    {
        // multichain-cli.exe client supports the 'offchain' based methods
        private readonly IMultiChainCliControl Control;

        public ControlCLIClientInferredTests()
        {
            var provider = new ServiceHelperParameterlessConstructor();

            Control = provider.GetService<IMultiChainCliControl>();
        }

        [Test, Ignore("ClearMemPoolTests should be ran independent of other tests since the network must be paused for incoming and mining tasks")]
        public async Task ClearMemPoolTestAsync()
        {
            // Act - Pause blockchain network actions
            CliResponse<object> pause = await Control.PauseAsync(tasks: NodeTask.All);

            // Act - Clear blockchain mem pool
            CliResponse<string> clearMemPool = await Control.ClearMemPoolAsync();

            // Act - Resume blockchain network actions
            CliResponse<object> resume = await Control.ResumeAsync(tasks: NodeTask.All);

            // Assert
            Assert.IsEmpty(pause.Error);
            Assert.IsNotNull(pause.Result);
            Assert.IsInstanceOf<CliResponse<object>>(pause);

            // Assert
            Assert.IsEmpty(pause.Error);
            Assert.IsNotNull(pause.Result);
            Assert.IsInstanceOf<CliResponse<string>>(clearMemPool);

            // Assert
            Assert.IsEmpty(pause.Error);
            Assert.IsNotNull(pause.Result);
            Assert.IsInstanceOf<CliResponse<object>>(resume);
        }

        [Test]
        public async Task GetBlockchainParamsTestAsync()
        {
            // Act - Ask network for blockchain params
            CliResponse<GetBlockchainParamsResult> actual = await Control.GetBlockchainParamsAsync(display_names: true, with_upgrades: true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetBlockchainParamsResult>>(actual);
        }

        [Test]
        public async Task GetInfoTestAsync()
        {
            // Act - Ask network for information about this blockchain
            CliResponse<GetInfoResult> actual = await Control.GetInfoAsync();

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetInfoResult>>(actual);
        }

        [Test]
        public async Task GetRuntimeParamsTestAsync()
        {
            // Act - Ask blockchain network for runtime parameters
            CliResponse<GetRuntimeParamsResult> actual = await Control.GetRuntimeParamsAsync();

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetRuntimeParamsResult>>(actual);
        }

        [Test]
        public async Task HelpTestAsync()
        {
            // Act - Get help information based on blockchain method name
            CliResponse<object> actual = await Control.HelpAsync(command: BlockchainAction.GetAssetInfoMethod);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test, Ignore("Test is ignored since it can be destructive to the current blockchain")]
        public async Task SetLastBlockTestAsync()
        {
            // Act - Sets last block in blockchain
            CliResponse<object> actual = await Control.SetLastBlockAsync(hash_or_height: "60");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task SetRuntimeParamTestAsync()
        {
            // Stage - One mebibyte
            var OneMiB = "1048576";

            // ### Act - Set a specific runtime parameter with a specific value
            var actual = await Control.SetRuntimeParamAsync(
                parameter_name: RuntimeParam.MaxShownData,
                parameter_value: OneMiB);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test, Ignore("Test is ignored since it can be destructive to the current blockchain")]
        public async Task StopTestAsync()
        {
            // Act - Stops the current blockchain network
            var actual = await Control.StopAsync();

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<string>>(actual);
        }
    }
}
