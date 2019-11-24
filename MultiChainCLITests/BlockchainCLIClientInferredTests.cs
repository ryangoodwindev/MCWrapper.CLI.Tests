using MCWrapper.CLI.Connection;
using MCWrapper.CLI.Ledger.Clients;
using MCWrapper.CLI.Tests.Options;
using MCWrapper.CLI.Tests.ServiceHelpers;
using MCWrapper.Data.Models.Blockchain;
using MCWrapper.Data.Models.Wallet;
using MCWrapper.Ledger.Entities;
using MCWrapper.Ledger.Entities.Constants;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCWrapper.CLI.Tests.MultiChainCLITests
{
    [TestFixture]
    public class BlockchainCLIClientInferredTests
    {
        private readonly IMultiChainCliWallet Wallet;
        private readonly IMultiChainCliGeneral Blockchain;

        public BlockchainCLIClientInferredTests()
        {
            var provider = new ServiceHelperParameterlessConstructor();

            Wallet = provider.GetService<IMultiChainCliWallet>();
            Blockchain = provider.GetService<IMultiChainCliGeneral>();
        }

        [Test]
        public async Task GetAssetInfoAsyncTest()
        {
            // Stage - issue a new asset to the blockchain node
            var asset = await Wallet.IssueAsync(
                to_address: Blockchain.CliOptions.ChainAdminAddress,
                asset_params: new AssetEntity(),
                quantity: 10,
                smallest_unit: 0.1);

            // Assert
            Assert.IsEmpty(asset.Error);
            Assert.IsInstanceOf<CliResponse<string>>(asset);

            // Act - get asset info
            var info = await Blockchain.GetAssetInfoAsync(
                asset_identifier: asset.Result,
                verbose: true);

            // Assert
            Assert.IsEmpty(info.Error);
            Assert.IsInstanceOf<CliResponse<GetAssetInfoResult>>(info);
        }

        [Test]
        public async Task GetBestBlockHashAsyncTest()
        {
            // Act - get best block hash
            var blockHash = await Blockchain.GetBestBlockHashAsync();

            // Assert
            Assert.IsEmpty(blockHash.Error);
            Assert.IsInstanceOf<CliResponse<string>>(blockHash);
        }

        [Test]
        public async Task GetBlockAsyncTest()
        {
            // Act - fetch a block from the network
            var block = await Blockchain.GetBlockAsync<GetBlockVerboseResult>(
                hash_or_height: "60",
                verbose: true);

            // Assert
            Assert.IsEmpty(block.Error);
            Assert.IsNotEmpty(block.Request.ChainName);
            Assert.IsInstanceOf<CliResponse<GetBlockVerboseResult>>(block);
        }

        [Test]
        public async Task GetBlockCountAsyncTest()
        {
            // Act - fetch blockchain height
            var count = await Blockchain.GetBlockCountAsync();

            // Assert
            Assert.IsEmpty(count.Error);
            Assert.IsInstanceOf<CliResponse<long>>(count);
        }

        [Test]
        public async Task GetBlockchainInfoAsyncTest()
        {
            // Act - fetch blockchain info
            var info = await Blockchain.GetBlockchainInfoAsync();

            // Assert
            Assert.IsEmpty(info.Error);
            Assert.IsInstanceOf<CliResponse<GetBlockchainInfoResult>>(info);
        }

        [Test]
        public async Task GetBlockHashAsyncTest()
        {
            // Act - get hash of a specific blockchain block
            var hash = await Blockchain.GetBlockHashAsync(index: 30);

            // Assert
            Assert.IsEmpty(hash.Error);
            Assert.IsInstanceOf<CliResponse<string>>(hash);
        }

        [Test]
        public async Task GetDifficultyAsyncTest()
        {
            // Act - get difficulty of a specific blockchain block
            var difficulty = await Blockchain.GetDifficultyAsync();

            // Assert
            Assert.IsEmpty(difficulty.Error);
            Assert.IsInstanceOf<CliResponse<double>>(difficulty);
        }

        [Test]
        public async Task GetFilterCodeAsyncTest()
        {
            // Stage - Create filter
            var filter = await Wallet.CreateAsync(
                entity_type: Entity.TxFilter,
                entity_name: StreamFilterEntity.GetUUID(),
                restrictions_or_open: new { },
                custom_fields: JsCode.DummyTxFilterCodeEscapedForWindowsCLI);

            // Assert
            Assert.IsEmpty(filter.Error);
            Assert.IsNotNull(filter.Result);
            Assert.IsInstanceOf<CliResponse<string>>(filter);

            // Act - Retrieve filtercode by name, txid, or reference
            CliResponse<string> actual = await Blockchain.GetFilterCodeAsync(filter_identifier: filter.Result);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<string>>(actual);
        }

        [Test]
        public async Task GetLastBlockInfoAsyncTest()
        {
            // Act - Ask about recent or last blocks in the network
            CliResponse<GetLastBlockInfoResult> actual = await Blockchain.GetLastBlockInfoAsync(skip: 10);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetLastBlockInfoResult>>(actual);
        }

        [Test]
        public async Task GetMemPoolInfoAsyncTest()
        {
            // Act - Ask blockchain network for mempool information
            CliResponse<GetMemPoolInfoResult> actual = await Blockchain.GetMemPoolInfoAsync();

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetMemPoolInfoResult>>(actual);
        }

        [Test]
        public async Task GetRawMemPoolAsyncTest()
        {
            // Act - Ask blockchain network for raw mempool information
            CliResponse<GetRawMemPoolResult> actual = await Blockchain.GetRawMemPoolAsync(verbose: true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetRawMemPoolResult>>(actual);
        }

        [Test]
        public async Task GetStreamInfoAsyncTest()
        {
            // Act - Fetch information about a specific blockchain stream
            CliResponse<GetStreamInfoResult> actual = await Blockchain.GetStreamInfoAsync(
                stream_identifier: "root",
                verbose: true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetStreamInfoResult>>(actual);
        }

        [Test]
        public async Task GetTxOutAsyncTest()
        {
            // Act - Issue a new asset to the blockchain node
            var asset = await Wallet.IssueAsync(
                to_address: Blockchain.CliOptions.ChainAdminAddress,
                asset_params: new AssetEntity().Name,
                quantity: 1,
                smallest_unit: 0.1);

            Assert.IsEmpty(asset.Error);
            Assert.IsNotNull(asset.Result);
            Assert.IsInstanceOf<CliResponse<string>>(asset);

            // Act - Load new asset Unspent
            var unspent = await Wallet.PrepareLockUnspentAsync(asset_quantities: new Dictionary<string, decimal>
            {
                { asset.Result, 1 }
            });

            Assert.IsEmpty(unspent.Error);
            Assert.IsNotNull(unspent.Result);
            Assert.IsInstanceOf<CliResponse<PrepareLockUnspentResult>>(unspent);

            // Act - Fetch details about unspent transaction output
            var actual = await Blockchain.GetTxOutAsync(
                txid: unspent.Result.Txid,
                n: unspent.Result.Vout,
                include_mem_pool: true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetTxOutResult>>(actual);
        }

        [Test]
        public async Task GetTxOutSetInfoAsyncTest()
        {
            // Act - Statistics about the unspent transaction output set
            CliResponse<GetTxOutSetInfoResult> actual = await Blockchain.GetTxOutSetInfoAsync();

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetTxOutSetInfoResult>>(actual);
        }

        [Test]
        public async Task ListAssetsAsyncTest()
        {
            // Act - Information about a one or many assets
            CliResponse<ListAssetsResult[]> actual = await Blockchain.ListAssetsAsync(
                asset_identifiers: "*",
                verbose: true,
                count: 10,
                start: 0);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<ListAssetsResult[]>>(actual);
        }

        [Test]
        public async Task ListBlocksAsyncTest()
        {
            // Act - Return information about one or many blocks
            CliResponse<ListBlocksResult[]> actual = await Blockchain.ListBlocksAsync(
                block_set_identifier: "18",
                verbose: true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.Greater(actual.Result.Length, 0);
            Assert.IsInstanceOf<CliResponse<ListBlocksResult[]>>(actual);
        }

        [Test]
        public async Task ListPermissionsAsyncTest()
        {
            // Act - List information about one or many permissions pertaining to one or many addresses
            CliResponse<ListPermissionsResult[]> actual = await Blockchain.ListPermissionsAsync(
                permissions: $"{Permission.Send},{Permission.Receive}",
                addresses: Blockchain.CliOptions.ChainAdminAddress,
                verbose: true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<ListPermissionsResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamFiltersAsyncTest()
        {
            // Act - Ask for a list of stream filters
            CliResponse<ListStreamFiltersResult[]> actual = await Blockchain.ListStreamFiltersAsync(
                filter_identifers: "*",
                verbose: true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<ListStreamFiltersResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamsAsyncTest()
        {
            // Act - Ask for a list of streams
            CliResponse<ListStreamsResult[]> actual = await Blockchain.ListStreamsAsync(
                stream_identifiers: "*",
                verbose: true,
                count: 10,
                start: 0);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<ListStreamsResult[]>>(actual);
        }

        [Test]
        public async Task ListTxFiltersAsyncTest()
        {
            // Act - List of transaction filters
            CliResponse<ListTxFiltersResult[]> actual = await Blockchain.ListTxFiltersAsync(
                filter_identifiers: "*",
                verbose: true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<ListTxFiltersResult[]>>(actual);
        }

        [Test]
        public async Task ListUpgradesAsyncTest()
        {
            // Act - List of upgrades
            CliResponse<object> actual = await Blockchain.ListUpgradesAsync(upgrade_identifiers: "*");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task RunStreamFilterAsyncTest()
        {
            // Act - Create filter
            var streamFilter = await Wallet.CreateAsync(
                blockchainName: Blockchain.CliOptions.ChainName,
                entity_type: Entity.StreamFilter,
                entity_name: StreamFilterEntity.GetUUID(),
                restrictions_or_open: new { },
                custom_fields: JsCode.DummyStreamFilterCodeEscapedForWindowsCLI);

            // Assert
            Assert.IsEmpty(streamFilter.Error);
            Assert.IsNotNull(streamFilter.Result);
            Assert.IsInstanceOf<CliResponse<string>>(streamFilter);

            // Act - Execute stream filter
            CliResponse<RunStreamFilterResult> actual = await Blockchain.RunStreamFilterAsync(filter_identifier: streamFilter.Result);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<RunStreamFilterResult>>(actual);
        }

        [Test]
        public async Task RunTxFilterFilterCodeAsyncTest()
        {
            // Stage - List tx filters
            var txFilter = await Blockchain.ListTxFiltersAsync(
                filter_identifiers: "*",
                verbose: true);

            // Act - Execute transaction filter
            CliResponse<RunTxFilterResult> actual = await Blockchain.RunTxFilterAsync(filter_identifier: txFilter.Result.FirstOrDefault().Name);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<RunTxFilterResult>>(actual);
        }

        [Test]
        public async Task TestStreamFilterAsyncTest()
        {
            // Act - Test stream filter
            CliResponse<TestStreamFilterResult> actual = await Blockchain.TestStreamFilterAsync(
                restrictions: new { },
                javascript_code: JsCode.DummyStreamFilterCodeEscapedForWindowsCLI);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<TestStreamFilterResult>>(actual);
        }

        [Test]
        public async Task TestTxFilterAsyncTest()
        {
            // Act - Test transaction filter
            CliResponse<TestTxFilterResult> actual = await Blockchain.TestTxFilterAsync(
                restrictions: new { },
                javascript_code: JsCode.DummyTxFilterCodeEscapedForWindowsCLI);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<TestTxFilterResult>>(actual);
        }

        [Test]
        public async Task VerifyChainAsyncTest()
        {
            // Act - Verify blockchain database
            CliResponse<bool> actual = await Blockchain.VerifyChainAsync(
                check_level: 3,
                num_blocks: 0);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<bool>>(actual);
        }

        [Test]
        public async Task VerifyPermissionAsyncTest()
        {
            // Act - Verify permissions for a specific address
            CliResponse<bool> actual = await Blockchain.VerifyPermissionAsync(
                address: Blockchain.CliOptions.ChainAdminAddress,
                permission: Permission.Admin);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<bool>>(actual);
        }
    }
}
