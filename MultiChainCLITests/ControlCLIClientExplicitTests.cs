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
    public class ControlCLIClientExplicitTests
    {
        // multichain-cli.exe client supports the 'offchain' based methods
        private readonly IMultiChainCliControl _control;

        public ControlCLIClientExplicitTests()
        {
            var provider = new ServiceHelperExplicitSource();

            _control = provider.GetService<IMultiChainCliControl>();
        }

        [Test, Ignore("ClearMemPoolTests should be ran independent of other tests since the network must be paused for incoming and mining tasks")]
        public async Task ClearMemPoolTestAsync()
        {
            // Act - Pause blockchain network actions
            CliResponse<string> pause = await _control.PauseAsync(
                blockchainName: _control.CliOptions.ChainName,
                tasks: NodeTask.All);

            // Act - Clear blockchain mem pool
            CliResponse<string> clearMemPool = await _control.ClearMemPoolAsync(_control.CliOptions.ChainName);

            // Act - Resume blockchain network actions
            CliResponse<string> resume = await _control.ResumeAsync(
                blockchainName: _control.CliOptions.ChainName,
                tasks: NodeTask.All);

            // Assert
            Assert.IsEmpty(pause.Error);
            Assert.IsNotNull(pause.Result);
            Assert.IsInstanceOf<CliResponse<string>>(pause);

            // Assert
            Assert.IsEmpty(pause.Error);
            Assert.IsNotNull(pause.Result);
            Assert.IsInstanceOf<CliResponse<string>>(clearMemPool);

            // Assert
            Assert.IsEmpty(pause.Error);
            Assert.IsNotNull(pause.Result);
            Assert.IsInstanceOf<CliResponse<string>>(resume);
        }

        [Test]
        public async Task GetBlockchainParamsTestAsync()
        {
            // Act - Ask network for blockchain params
            CliResponse<GetBlockchainParamsResult> actual = await _control.GetBlockchainParamsAsync(
                blockchainName: _control.CliOptions.ChainName,
                display_names: true,
                with_upgrades: true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetBlockchainParamsResult>>(actual);
        }

        [Test]
        public async Task GetInfoTestAsync()
        {
            // Act - Ask network for information about this blockchain
            CliResponse<GetInfoResult> actual = await _control.GetInfoAsync(_control.CliOptions.ChainName);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetInfoResult>>(actual);
        }

        [Test]
        public async Task GetInitStatusTestAsync()
        {
            // Act - Ask network for information about this blockchain
            CliResponse<GetInitStatusResult> actual = await _control.GetInitStatusAsync(_control.CliOptions.ChainName);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetInitStatusResult>>(actual);
        }

        [Test]
        public async Task GetRuntimeParamsTestAsync()
        {
            // Act - Ask blockchain network for runtime parameters
            CliResponse<GetRuntimeParamsResult> actual = await _control.GetRuntimeParamsAsync(_control.CliOptions.ChainName);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetRuntimeParamsResult>>(actual);
        }

        [Test]
        public async Task HelpTestAsync()
        {
            // Act - Get help information based on blockchain method name
            CliResponse<string> actual = await _control.HelpAsync(
                blockchainName: _control.CliOptions.ChainName,
                command: BlockchainAction.GetAssetInfoMethod);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<string>>(actual);
        }

        [Test, Ignore("Test is ignored since it can be destructive to the current blockchain")]
        public async Task SetLastBlockTestAsync()
        {
            // Act - Sets last block in blockchain
            CliResponse<string> actual = await _control.SetLastBlockAsync(
                blockchainName: _control.CliOptions.ChainName,
                hash_or_height: "60");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<string>>(actual);
        }

        [Test]
        public async Task SetRuntimeParamTestAsync()
        {
            // Stage - One mebibyte
            var OneMiB = "1048576";

            // ### Act - Set a specific runtime parameter with a specific value
            await _control.SetRuntimeParamAsync(
                blockchainName: _control.CliOptions.ChainName,
                parameter_name: RuntimeParam.MaxShownData,
                parameter_value: OneMiB);
        }

        [Test, Ignore("Test is ignored since it can be destructive to the current blockchain")]
        public async Task StopTestAsync()
        {
            // Act - Stops the current blockchain network
            var actual = await _control.StopAsync(_control.CliOptions.ChainName);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<string>>(actual);
        }
    }
}
