using MCWrapper.CLI.Connection;
using MCWrapper.CLI.Ledger.Clients;
using MCWrapper.CLI.Tests.Options;
using MCWrapper.CLI.Tests.ServiceHelpers;
using MCWrapper.Ledger.Entities;
using MCWrapper.Ledger.Entities.Extensions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace MCWrapper.CLI.Tests.MultiChainCLITests
{
    [TestFixture]
    public class WalletCLIExtensionsTests
    {
        private readonly WalletCliClient Wallet;
        private readonly UtilityCliClient Utility;

        public WalletCLIExtensionsTests()
        {
            var provider = new ServiceHelperParameterlessConstructor();

            // fetch services from provider
            Wallet = provider.GetService<WalletCliClient>();
            Utility = provider.GetService<UtilityCliClient>();
        }


        // *** Create Stream extension tests

        [Test]
        public async Task CreateStreamInferredTest()
        {
            // Stage - initialize a new Stream Entity instance
            var stream = new StreamEntity();
            stream.AddOrUpdateCustomField("description", "Testing_text_input_for_stream_custom_fields");
            stream.Restrictions.AddRestriction(StreamRestrictTypes.OffChain);

            // Act - attempt to create a new Stream using the inferred blockchain name
            var createStream = await Wallet.CreateStream(stream);

            // Assert
            Assert.IsEmpty(createStream.Error);
            Assert.IsNotEmpty(createStream.Result);
            Assert.IsInstanceOf<CliResponse<string>>(createStream);
        }

        [Test]
        public async Task CreateStreamExplicitTest()
        {
            // Stage - initialize a new Stream Entity instance
            var stream = new StreamEntity();
            stream.AddOrUpdateCustomField("text", "Testing_text_input_for_stream_custom_fields");
            stream.Restrictions.AddRestriction(StreamRestrictTypes.OffChain);

            // Act - attempt to create a new Stream using the explicit blockchain name
            var createStream = await Wallet.CreateStream(Wallet.CliOptions.ChainName, stream);

            // Assert
            Assert.IsEmpty(createStream.Error);
            Assert.IsNotEmpty(createStream.Result);
            Assert.IsInstanceOf<CliResponse<string>>(createStream);
        }

        [Test]
        public async Task CreateStreamFromInferredTest()
        {
            // Stage - initialize a new Stream Entity instance
            var stream = new StreamEntity();
            stream.AddOrUpdateCustomField("description", "Testing_text_input_for_stream_custom_fields");
            stream.Restrictions.AddRestriction(StreamRestrictTypes.OffChain);

            // Act - attempt to create a new Stream using the inferred blockchain name
            var createStreamFrom = await Wallet.CreateStreamFrom(Wallet.CliOptions.ChainAdminAddress, stream);

            // Assert
            Assert.IsEmpty(createStreamFrom.Error);
            Assert.IsNotEmpty(createStreamFrom.Result);
            Assert.IsInstanceOf<CliResponse<string>>(createStreamFrom);
        }

        [Test]
        public async Task CreateStreamFromExplicitTest()
        {
            // Stage - initialize a new Stream Entity instance
            var stream = new StreamEntity();
            stream.AddOrUpdateCustomField("description", "Testing_text_input_for_stream_custom_fields");
            stream.Restrictions.AddRestriction(StreamRestrictTypes.OffChain);

            // Act - attempt to create a new Stream using the explicit blockchain name
            var createStreamFrom = await Wallet.CreateStreamFrom(Wallet.CliOptions.ChainName, Wallet.CliOptions.ChainAdminAddress, stream);

            // Assert
            Assert.IsEmpty(createStreamFrom.Error);
            Assert.IsNotEmpty(createStreamFrom.Result);
            Assert.IsInstanceOf<CliResponse<string>>(createStreamFrom);
        }


        // *** Create Upgrade extension tests

        [Test, Ignore("Upgrades should be tested individually from other tests")]
        public async Task CreateUpgradeInferredTest()
        {
            // Stage - Initialize new UpgradeCustomFields and UpgradeEntity instances
            var customFields = new UpgradeCustomFields(
                protocolVersion: 20010,
                paramKey: UpgradeParameterKeys.AnyoneCanConnectKey,
                paramValue: 0,
                additionalParameters: null,
                startBlock: 0);

            var upgrade = new UpgradeEntity(name: Guid.NewGuid().ToString("N"), customFields: customFields);

            // Act - attempt to create a new Upgrade using the inferred blockchain name
            var createUpgrade = await Wallet.CreateUpgrade(upgrade);

            // Assert
            Assert.IsEmpty(createUpgrade.Error);
            Assert.IsNotEmpty(createUpgrade.Result);
            Assert.IsInstanceOf<CliResponse<string>>(createUpgrade);
        }

        [Test, Ignore("Upgrades should be tested individually from other tests")]
        public async Task CreateUpgradeExplicitTest()
        {
            // Stage - Initialize new UpgradeCustomFields and UpgradeEntity instances
            var customFields = new UpgradeCustomFields(
                protocolVersion: 20010,
                paramKey: UpgradeParameterKeys.AnyoneCanConnectKey,
                paramValue: 0,
                additionalParameters: null,
                startBlock: 0);

            var upgrade = new UpgradeEntity(name: Guid.NewGuid().ToString("N"), customFields: customFields);

            // Act - attempt to create a new Upgrade using the explicit blockchain name
            var createUpgrade = await Wallet.CreateUpgrade(Wallet.CliOptions.ChainName, upgrade);

            // Assert
            Assert.IsEmpty(createUpgrade.Error);
            Assert.IsNotEmpty(createUpgrade.Result);
            Assert.IsInstanceOf<CliResponse<string>>(createUpgrade);
        }

        [Test, Ignore("Upgrades should be tested individually from other tests")]
        public async Task CreateUpgradeFromInferredTest()
        {
            // Stage - Initialize new UpgradeCustomFields and UpgradeEntity instances
            var customFields = new UpgradeCustomFields(
                protocolVersion: 20010,
                paramKey: UpgradeParameterKeys.AnyoneCanConnectKey,
                paramValue: 0,
                additionalParameters: null,
                startBlock: 0);

            var upgrade = new UpgradeEntity(name: Guid.NewGuid().ToString("N"), customFields: customFields);

            // Act - attempt to create a new Upgrade using the inferred blockchain name
            var createUpgrade = await Wallet.CreateUpgradeFrom(Wallet.CliOptions.ChainAdminAddress, upgrade);

            // Assert
            Assert.IsEmpty(createUpgrade.Error);
            Assert.IsNotEmpty(createUpgrade.Result);
            Assert.IsInstanceOf<CliResponse<string>>(createUpgrade);
        }

        [Test, Ignore("Upgrades should be tested individually from other tests")]
        public async Task CreateUpgradeFromExplicitTest()
        {
            // Stage - Initialize new UpgradeCustomFields and UpgradeEntity instances
            var customFields = new UpgradeCustomFields(
                protocolVersion: 20010,
                paramKey: UpgradeParameterKeys.AnyoneCanConnectKey,
                paramValue: 0,
                additionalParameters: null,
                startBlock: 0);

            var upgrade = new UpgradeEntity(name: Guid.NewGuid().ToString("N"), customFields: customFields);

            // Act - attempt to create a new Upgrade using the explicit blockchain name
            var createUpgrade = await Wallet.CreateUpgradeFrom(Wallet.CliOptions.ChainName, Wallet.CliOptions.ChainAdminAddress, upgrade);

            // Assert
            Assert.IsEmpty(createUpgrade.Error);
            Assert.IsNotEmpty(createUpgrade.Result);
            Assert.IsInstanceOf<CliResponse<string>>(createUpgrade);
        }


        // *** Create Stream Filter extension tests

        [Test]
        public async Task CreateStreamFilterInferredTest()
        {
            // Stage - create Stream Filter entity
            var filter = new StreamFilterEntity
            {
                Name = StreamFilterEntity.GetUUID(),
                JavaScriptCode = JsCode.DummyStreamFilterCodeEscapedForWindowsCLI
            };

            // Act - attempt to create a new Stream Filter using the inferred blockchain name
            var createFilter = await Wallet.CreateStreamFilter(filter);

            // Assert
            Assert.IsEmpty(createFilter.Error);
            Assert.IsNotEmpty(createFilter.Result);
            Assert.IsInstanceOf<CliResponse<string>>(createFilter);
        }

        [Test]
        public async Task CreateStreamFilterExplicitTest()
        {
            // Stage - create Stream Filter entity
            var filter = new StreamFilterEntity
            {
                Name = StreamFilterEntity.GetUUID(),
                JavaScriptCode = JsCode.DummyStreamFilterCodeEscapedForWindowsCLI
            };

            // Act - attempt to create a new Stream Filter using the explicit blockchain name
            var createFilter = await Wallet.CreateStreamFilter(Wallet.CliOptions.ChainName, filter);

            // Assert
            Assert.IsEmpty(createFilter.Error);
            Assert.IsNotEmpty(createFilter.Result);
            Assert.IsInstanceOf<CliResponse<string>>(createFilter);
        }

        [Test]
        public async Task CreateStreamFilterFromInferredTest()
        {
            // Stage - create Stream Filter entity
            var filter = new StreamFilterEntity
            {
                Name = StreamFilterEntity.GetUUID(),
                JavaScriptCode = JsCode.DummyStreamFilterCodeEscapedForWindowsCLI
            };

            // Act - attempt to create a new Stream Filter from an address using the inferred blockchain name
            var createFilterFrom = await Wallet.CreateStreamFilterFrom(Wallet.CliOptions.ChainAdminAddress, filter);

            // Assert
            Assert.IsEmpty(createFilterFrom.Error);
            Assert.IsNotEmpty(createFilterFrom.Result);
            Assert.IsInstanceOf<CliResponse<string>>(createFilterFrom);
        }

        [Test]
        public async Task CreateStreamFilterFromExplicitTest()
        {
            // Stage - create Stream Filter entity
            var filter = new StreamFilterEntity
            {
                Name = StreamFilterEntity.GetUUID(),
                JavaScriptCode = JsCode.DummyStreamFilterCodeEscapedForWindowsCLI
            };

            // Act - attempt to create a new Stream Filter from an address using the explicit blockchain name
            var createFilterFrom = await Wallet.CreateStreamFilterFrom(Wallet.CliOptions.ChainName, Wallet.CliOptions.ChainAdminAddress, filter);

            // Assert
            Assert.IsEmpty(createFilterFrom.Error);
            Assert.IsNotEmpty(createFilterFrom.Result);
            Assert.IsInstanceOf<CliResponse<string>>(createFilterFrom);
        }


        // *** Create Transaction (Tx) Filter extension tests

        [Test]
        public async Task CreateTxFilterInferredBlockchainNameTest()
        {
            // Stage - create Tx Filter entity
            var filter = new TxFilterEntity();
            filter.Restrictions._For = "root";
            filter.JavaScriptCode = JsCode.DummyTxFilterCodeEscapedForWindowsCLI;

            // Act - attempt to create a new Tx Filter using the inferred blockchain name
            var createFilter = await Wallet.CreateTxFilter(filter);

            // Assert
            Assert.IsEmpty(createFilter.Error);
            Assert.IsNotEmpty(createFilter.Result);
            Assert.IsInstanceOf<CliResponse<string>>(createFilter);
        }

        [Test]
        public async Task CreateTxFilterExplicitBlockchainNameTest()
        {
            // Stage - create Tx Filter entity
            var filter = new TxFilterEntity();
            filter.Restrictions._For = "root";
            filter.JavaScriptCode = JsCode.DummyTxFilterCodeEscapedForWindowsCLI;

            // Act - attempt to create a new Tx Filter using the explicit blockchain name
            var createFilter = await Wallet.CreateTxFilter(Wallet.CliOptions.ChainName, filter);

            // Assert
            Assert.IsEmpty(createFilter.Error);
            Assert.IsNotEmpty(createFilter.Result);
            Assert.IsInstanceOf<CliResponse<string>>(createFilter);
        }

        [Test]
        public async Task CreateTxFilterFromInferredBlockchainNameTest()
        {
            // Stage - create Tx Filter entity
            var filter = new TxFilterEntity();
            filter.Restrictions._For = "root";
            filter.JavaScriptCode = JsCode.DummyTxFilterCodeEscapedForWindowsCLI;

            // Act - attempt to create a new Tx Filter from an address using the inferred blockchain name
            var createFilterFrom = await Wallet.CreateTxFilterFrom(Wallet.CliOptions.ChainAdminAddress, filter);

            // Assert
            Assert.IsEmpty(createFilterFrom.Error);
            Assert.IsNotEmpty(createFilterFrom.Result);
            Assert.IsInstanceOf<CliResponse<string>>(createFilterFrom);
        }

        [Test]
        public async Task CreateTxFilterFromExplicitBlockchainNameTest()
        {
            // Stage - create Tx Filter entity
            var filter = new TxFilterEntity();
            filter.Restrictions._For = "root";
            filter.JavaScriptCode = JsCode.DummyTxFilterCodeEscapedForWindowsCLI;

            // Act - attempt to create a new Tx Filter from an address using the explicit blockchain name
            var createFilterFrom = await Wallet.CreateTxFilterFrom(Wallet.CliOptions.ChainName, Wallet.CliOptions.ChainAdminAddress, filter);

            // Assert
            Assert.IsEmpty(createFilterFrom.Error);
            Assert.IsNotEmpty(createFilterFrom.Result);
            Assert.IsInstanceOf<CliResponse<string>>(createFilterFrom);
        }


        // *** Publish Stream Item with a single Key or multiple keys using the inferred blockchain name extension tests

        [Test]
        public async Task PublishStreamItemHexDataInferredTest()
        {
            // Stage - create a new PublishEntity instance
            var streamItem = new PublishEntity("root", PublishEntity.GetUUID(), "Some StreamItem Data".ToHex(), StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await Wallet.PublishStreamItemKey(streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            streamItem = new PublishEntity("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, "Some StreamItem Data".ToHex(), StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await Wallet.PublishStreamItemKeys(streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);
        }

        [Test]
        public async Task PublishStreamItemCachedDataInferredTest()
        {
            // Stage - Create a new BinaryCache and then create a new PublishEntity instance
            var binaryCache = await Utility.CreateBinaryCacheAsync();
            var cachedData = new DataCached(binaryCache.Result);
            var streamItem = new PublishEntity<DataCached>("root", PublishEntity.GetUUID(), cachedData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await Wallet.PublishStreamItemKey(streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);

            // Stage - Create a new BinaryCache and then create a new PublishEntity instance
            binaryCache = await Utility.CreateBinaryCacheAsync();
            cachedData = new DataCached(binaryCache.Result);
            streamItem = new PublishEntity<DataCached>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, cachedData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await Wallet.PublishStreamItemKeys(streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);
        }

        [Test]
        public async Task PublishStreamItemJsonDataInferredTest()
        {
            // Stage - create a new PublishEntity instance
            var jsonData = new DataJson(new { description = "Some Text Stuff".ToHex() });
            var streamItem = new PublishEntity<DataJson>("root", PublishEntity.GetUUID(), jsonData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await Wallet.PublishStreamItemKey(streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            jsonData = new DataJson(new { description = "Some Text Stuff".ToHex() });
            streamItem = new PublishEntity<DataJson>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, jsonData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await Wallet.PublishStreamItemKeys(streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);
        }

        [Test]
        public async Task PublishStreamItemTextDataInferredTest()
        {
            // Stage - create a new PublishEntity instance
            var textData = new DataText("Some_Data_Text_for_the_stream_item.".ToHex());
            var streamItem = new PublishEntity<DataText>("root", PublishEntity.GetUUID(), textData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await Wallet.PublishStreamItemKey(streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            textData = new DataText("Some_Data_Text_for_the_stream_item.".ToHex());
            streamItem = new PublishEntity<DataText>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, textData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await Wallet.PublishStreamItemKeys(streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);
        }


        // *** Publish Stream Item with a single Key or multiple keys using the explicit blockchain name extension tests

        [Test]
        public async Task PublishStreamItemHexDataExplicitTest()
        {
            // Stage - create a new PublishEntity instance
            var streamItem = new PublishEntity("root", PublishEntity.GetUUID(), "Some StreamItem Data".ToHex(), StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await Wallet.PublishStreamItemKey(Wallet.CliOptions.ChainName, streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            streamItem = new PublishEntity("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, "Some StreamItem Data".ToHex(), StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await Wallet.PublishStreamItemKeys(Wallet.CliOptions.ChainName, streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);
        }

        [Test]
        public async Task PublishStreamItemCachedDataExplicitTest()
        {
            // Stage - Create a new BinaryCache and then create a new PublishEntity instance
            var binaryCache = await Utility.CreateBinaryCacheAsync();
            var cachedData = new DataCached(binaryCache.Result);
            var streamItem = new PublishEntity<DataCached>("root", PublishEntity.GetUUID(), cachedData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await Wallet.PublishStreamItemKey(Wallet.CliOptions.ChainName, streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);

            // Stage - Create a new BinaryCache and then create a new PublishEntity instance
            binaryCache = await Utility.CreateBinaryCacheAsync();
            cachedData = new DataCached(binaryCache.Result);
            streamItem = new PublishEntity<DataCached>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, cachedData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await Wallet.PublishStreamItemKeys(Wallet.CliOptions.ChainName, streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);
        }

        [Test]
        public async Task PublishStreamItemJsonDataExplicitTest()
        {
            // Stage - create a new PublishEntity instance
            var jsonData = new DataJson(new { description = "Some Text Stuff".ToHex() });
            var streamItem = new PublishEntity<DataJson>("root", PublishEntity.GetUUID(), jsonData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await Wallet.PublishStreamItemKey(Wallet.CliOptions.ChainName, streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            jsonData = new DataJson(new { description = "Some Text Stuff".ToHex() });
            streamItem = new PublishEntity<DataJson>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, jsonData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await Wallet.PublishStreamItemKeys(Wallet.CliOptions.ChainName, streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);
        }

        [Test]
        public async Task PublishStreamItemTextDataExplicitTest()
        {
            // Stage - create a new PublishEntity instance
            var textData = new DataText("Some_plain_text_for_the_DataText");
            var streamItem = new PublishEntity<DataText>("root", PublishEntity.GetUUID(), textData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await Wallet.PublishStreamItemKey(Wallet.CliOptions.ChainName, streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            textData = new DataText("Some_plain_text_for_the_DataText");
            streamItem = new PublishEntity<DataText>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, textData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await Wallet.PublishStreamItemKeys(Wallet.CliOptions.ChainName, streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);
        }


        // *** Publish Stream Item from an address with a single Key or multiple keys using the inferred blockchain name extension tests

        [Test]
        public async Task PublishStreamItemFromHexDataInferredTest()
        {
            // Stage - create a new PublishEntity instance
            var streamItem = new PublishEntity("root", PublishEntity.GetUUID(), "Some StreamItem Data".ToHex(), StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await Wallet.PublishStreamItemKeyFrom(Wallet.CliOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            streamItem = new PublishEntity("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, "Some StreamItem Data".ToHex(), StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await Wallet.PublishStreamItemKeysFrom(Wallet.CliOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);
        }

        [Test]
        public async Task PublishStreamItemFromCachedDataInferredTest()
        {
            // Stage - Create a new BinaryCache and then create a new PublishEntity instance
            var binaryCache = await Utility.CreateBinaryCacheAsync();
            var cachedData = new DataCached(binaryCache.Result);
            var streamItem = new PublishEntity<DataCached>("root", PublishEntity.GetUUID(), cachedData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await Wallet.PublishStreamItemKeyFrom(Wallet.CliOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);

            // Stage - Create a new BinaryCache and then create a new PublishEntity instance
            binaryCache = await Utility.CreateBinaryCacheAsync();
            cachedData = new DataCached(binaryCache.Result);
            streamItem = new PublishEntity<DataCached>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, cachedData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await Wallet.PublishStreamItemKeysFrom(Wallet.CliOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);
        }

        [Test]
        public async Task PublishStreamItemFromJsonDataInferredTest()
        {
            // Stage - create a new PublishEntity instance
            var jsonData = new DataJson(new { description = "Some Text Stuff".ToHex() });
            var streamItem = new PublishEntity<DataJson>("root", PublishEntity.GetUUID(), jsonData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await Wallet.PublishStreamItemKeyFrom(Wallet.CliOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            jsonData = new DataJson(new { description = "Some Text Stuff".ToHex() });
            streamItem = new PublishEntity<DataJson>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, jsonData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await Wallet.PublishStreamItemKeysFrom(Wallet.CliOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);
        }

        [Test]
        public async Task PublishStreamItemFromTextDataInferredTest()
        {
            // Stage - create a new PublishEntity instance
            var textData = new DataText("Some_plain_text_for_the_DataText");
            var streamItem = new PublishEntity<DataText>("root", PublishEntity.GetUUID(), textData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await Wallet.PublishStreamItemKeyFrom(Wallet.CliOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            textData = new DataText("Some_plain_text_for_the_DataText");
            streamItem = new PublishEntity<DataText>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, textData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await Wallet.PublishStreamItemKeysFrom(Wallet.CliOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);
        }


        // *** Publish Stream Item from an address with a single Key or multiple keys using the explicit blockchain name extension tests

        [Test]
        public async Task PublishStreamItemFromHexDataExplicitTest()
        {
            // Stage - create a new PublishEntity instance
            var streamItem = new PublishEntity("root", PublishEntity.GetUUID(), "Some StreamItem Data".ToHex(), StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await Wallet.PublishStreamItemKeyFrom(Wallet.CliOptions.ChainName, Wallet.CliOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            streamItem = new PublishEntity("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, "Some StreamItem Data".ToHex(), StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await Wallet.PublishStreamItemKeysFrom(Wallet.CliOptions.ChainName, Wallet.CliOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);
        }

        [Test]
        public async Task PublishStreamItemFromCachedDataExplicitTest()
        {
            // Stage - Create a new BinaryCache and then create a new PublishEntity instance
            var binaryCache = await Utility.CreateBinaryCacheAsync();
            var cachedData = new DataCached(binaryCache.Result);
            var streamItem = new PublishEntity<DataCached>("root", PublishEntity.GetUUID(), cachedData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await Wallet.PublishStreamItemKeyFrom(Wallet.CliOptions.ChainName, Wallet.CliOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);

            // Stage - Create a new BinaryCache and then create a new PublishEntity instance
            binaryCache = await Utility.CreateBinaryCacheAsync();
            cachedData = new DataCached(binaryCache.Result);
            streamItem = new PublishEntity<DataCached>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, cachedData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await Wallet.PublishStreamItemKeysFrom(Wallet.CliOptions.ChainName, Wallet.CliOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);
        }

        [Test]
        public async Task PublishStreamItemFromJsonDataExplicitTest()
        {
            // Stage - create a new PublishEntity instance
            var jsonData = new DataJson(new { description = "Some Text Stuff".ToHex() });
            var streamItem = new PublishEntity<DataJson>("root", PublishEntity.GetUUID(), jsonData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await Wallet.PublishStreamItemKeyFrom(Wallet.CliOptions.ChainName, Wallet.CliOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            jsonData = new DataJson(new { description = "Some Text Stuff".ToHex() });
            streamItem = new PublishEntity<DataJson>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, jsonData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await Wallet.PublishStreamItemKeysFrom(Wallet.CliOptions.ChainName, Wallet.CliOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);
        }

        [Test]
        public async Task PublishStreamItemFromTextDataExplicitTest()
        {
            // Stage - create a new PublishEntity instance
            var textData = new DataText("Some_plain_text_for_the_DataText");
            var streamItem = new PublishEntity<DataText>("root", PublishEntity.GetUUID(), textData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            var publish = await Wallet.PublishStreamItemKeyFrom(Wallet.CliOptions.ChainName, Wallet.CliOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);

            // Stage - create a new PublishEntity instance
            textData = new DataText("Some_plain_text_for_the_DataText");
            streamItem = new PublishEntity<DataText>("root", new[] { PublishEntity.GetUUID(), PublishEntity.GetUUID() }, textData, StreamRestrictTypes.OffChain);

            // Act - attempt to Publish a new stream item using the inferred blockchain name
            publish = await Wallet.PublishStreamItemKeysFrom(Wallet.CliOptions.ChainName, Wallet.CliOptions.ChainAdminAddress, streamItem);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);
        }


        // *** PublishMultiStreamItems using an inferred blockchain name test

        [Test]
        public async Task PublishMultiStreamItemsInferredTest()
        {
            // Stage - Create a new PublishMultiItemEntity instance
            var multi = new PublishMultiEntity
            {
                StreamIdentifier = "root",
                Options = StreamRestrictTypes.OffChain
            };

            // Stage - Create a single instance of each available Data object type
            var dataHexEntityKey = new PublishMultiItemKeyEntity
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                DataHex = "Some data string converted to Hex".ToHex()
            };

            var dataHexEntityKeys = new PublishMultiItemKeysEntity
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                DataHex = "Some data string converted to Hex".ToHex()
            };

            var cache = await Utility.CreateBinaryCacheAsync();
            var dataCachedEntityKey = new PublishMultiItemKeyEntity<DataCached>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataCached(cache.Result)
            };

            cache = await Utility.CreateBinaryCacheAsync();
            var dataCachedEntityKeys = new PublishMultiItemKeysEntity<DataCached>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataCached(cache.Result)
            };

            var dataJsonEntityKey = new PublishMultiItemKeyEntity<DataJson>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataJson(new { description = "Some description text in Hex".ToHex() })
            };

            var dataJsonEntityKeys = new PublishMultiItemKeysEntity<DataJson>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataJson(new { description = "Some description text in Hex".ToHex() })
            };

            var dataTextEntityKey = new PublishMultiItemKeyEntity<DataText>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataText("Some_plain_text_for_the_DataText")
            };

            var dataTextEntityKeys = new PublishMultiItemKeysEntity<DataText>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataText("Some_plain_text_for_the_DataText")
            };

            // Stage - Append each data object to an array and assign it to the multi variable
            multi.Items = (new object[]
            {
                dataHexEntityKey,
                dataHexEntityKeys,
                dataCachedEntityKey,
                dataCachedEntityKeys,
                dataJsonEntityKey,
                dataJsonEntityKeys,
                dataTextEntityKey,
                dataTextEntityKeys
            });

            // Asert - Attempt to Publish multiple items to the blockchain stream
            var publish = await Wallet.PublishMultiStreamItems(multi);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);
        }


        // *** PublishMultiStreamItems using an explicit blockchain name test

        [Test]
        public async Task PublishMultiStreamItemsExplicitTest()
        {
            // Stage - Create a new PublishMultiItemEntity instance
            var multi = new PublishMultiEntity
            {
                StreamIdentifier = "root",
                Options = StreamRestrictTypes.OffChain
            };

            // Stage - Create a single instance of each available Data object type
            var dataHexEntityKey = new PublishMultiItemKeyEntity
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                DataHex = "Some data string converted to Hex".ToHex()
            };

            var dataHexEntityKeys = new PublishMultiItemKeysEntity
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                DataHex = "Some data string converted to Hex".ToHex()
            };

            var cache = await Utility.CreateBinaryCacheAsync();
            var dataCachedEntityKey = new PublishMultiItemKeyEntity<DataCached>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataCached(cache.Result)
            };

            cache = await Utility.CreateBinaryCacheAsync();
            var dataCachedEntityKeys = new PublishMultiItemKeysEntity<DataCached>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataCached(cache.Result)
            };

            var dataJsonEntityKey = new PublishMultiItemKeyEntity<DataJson>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataJson(new { description = "Some description text in Hex".ToHex() })
            };

            var dataJsonEntityKeys = new PublishMultiItemKeysEntity<DataJson>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataJson(new { description = "Some description text in Hex".ToHex() })
            };

            var dataTextEntityKey = new PublishMultiItemKeyEntity<DataText>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataText("Some_plain_text_for_the_DataText")
            };

            var dataTextEntityKeys = new PublishMultiItemKeysEntity<DataText>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataText("Some_plain_text_for_the_DataText")
            };

            // Stage - Append each data object to an array and assign it to the multi variable
            multi.Items = (new object[]
            {
                dataHexEntityKey,
                dataHexEntityKeys,
                dataCachedEntityKey,
                dataCachedEntityKeys,
                dataJsonEntityKey,
                dataJsonEntityKeys,
                dataTextEntityKey,
                dataTextEntityKeys
            });

            // Asert - Attempt to Publish multiple items to the blockchain stream
            var publish = await Wallet.PublishMultiStreamItems(Wallet.CliOptions.ChainName, multi);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);
        }


        // *** PublishMultiStreamItemsFrom using an inferred blockchain name test

        [Test]
        public async Task PublishMultiStreamItemsFromInferredTest()
        {
            // Stage - Create a new PublishMultiItemEntity instance
            var multi = new PublishMultiEntity
            {
                StreamIdentifier = "root",
                Options = StreamRestrictTypes.OffChain
            };

            // Stage - Create a single instance of each available Data object type
            var dataHexEntityKey = new PublishMultiItemKeyEntity
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                DataHex = "Some data string converted to Hex".ToHex()
            };

            var dataHexEntityKeys = new PublishMultiItemKeysEntity
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                DataHex = "Some data string converted to Hex".ToHex()
            };

            var cache = await Utility.CreateBinaryCacheAsync();
            var dataCachedEntityKey = new PublishMultiItemKeyEntity<DataCached>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataCached(cache.Result)
            };

            cache = await Utility.CreateBinaryCacheAsync();
            var dataCachedEntityKeys = new PublishMultiItemKeysEntity<DataCached>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataCached(cache.Result)
            };

            var dataJsonEntityKey = new PublishMultiItemKeyEntity<DataJson>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataJson(new { description = "Some description text in Hex".ToHex() })
            };

            var dataJsonEntityKeys = new PublishMultiItemKeysEntity<DataJson>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataJson(new { description = "Some description text in Hex".ToHex() })
            };

            var dataTextEntityKey = new PublishMultiItemKeyEntity<DataText>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataText("Some_plain_text_for_the_DataText")
            };

            var dataTextEntityKeys = new PublishMultiItemKeysEntity<DataText>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataText("Some_plain_text_for_the_DataText")
            };

            // Stage - Append each data object to an array and assign it to the multi variable
            multi.Items = (new object[]
            {
                dataHexEntityKey,
                dataHexEntityKeys,
                dataCachedEntityKey,
                dataCachedEntityKeys,
                dataJsonEntityKey,
                dataJsonEntityKeys,
                dataTextEntityKey,
                dataTextEntityKeys
            });

            // Asert - Attempt to Publish multiple items to the blockchain stream
            var publish = await Wallet.PublishMultiStreamItemsFrom(Wallet.CliOptions.ChainAdminAddress, multi);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);
        }


        // *** PublishMultiStreamItemsFrom using an explicit blockchain name test

        [Test]
        public async Task PublishMultiStreamItemsFromExplicitTest()
        {
            // Stage - Create a new PublishMultiItemEntity instance
            var multi = new PublishMultiEntity
            {
                StreamIdentifier = "root",
                Options = StreamRestrictTypes.OffChain
            };

            // Stage - Create a single instance of each available Data object type
            var dataHexEntityKey = new PublishMultiItemKeyEntity
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                DataHex = "Some data string converted to Hex".ToHex()
            };

            var dataHexEntityKeys = new PublishMultiItemKeysEntity
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                DataHex = "Some data string converted to Hex".ToHex()
            };

            var cache = await Utility.CreateBinaryCacheAsync();
            var dataCachedEntityKey = new PublishMultiItemKeyEntity<DataCached>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataCached(cache.Result)
            };

            cache = await Utility.CreateBinaryCacheAsync();
            var dataCachedEntityKeys = new PublishMultiItemKeysEntity<DataCached>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataCached(cache.Result)
            };

            var dataJsonEntityKey = new PublishMultiItemKeyEntity<DataJson>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataJson(new { description = "Some description text in Hex".ToHex() })
            };

            var dataJsonEntityKeys = new PublishMultiItemKeysEntity<DataJson>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataJson(new { description = "Some description text in Hex".ToHex() })
            };

            var dataTextEntityKey = new PublishMultiItemKeyEntity<DataText>
            {
                For = "root",
                Key = ChainEntity.GetUUID(),
                Options = StreamRestrictTypes.OffChain,
                Data = new DataText("Some_plain_text_for_the_DataText")
            };

            var dataTextEntityKeys = new PublishMultiItemKeysEntity<DataText>
            {
                For = "root",
                Keys = new[] { ChainEntity.GetUUID(), ChainEntity.GetUUID() },
                Options = StreamRestrictTypes.OffChain,
                Data = new DataText("Some_plain_text_for_the_DataText")
            };

            // Stage - Append each data object to an array and assign it to the multi variable
            multi.Items = (new object[]
            {
                    dataHexEntityKey,
                    dataHexEntityKeys,
                    dataCachedEntityKey,
                    dataCachedEntityKeys,
                    dataJsonEntityKey,
                    dataJsonEntityKeys,
                    dataTextEntityKey,
                    dataTextEntityKeys
            });

            // Asert - Attempt to Publish multiple items to the blockchain stream
            var publish = await Wallet.PublishMultiStreamItemsFrom(Wallet.CliOptions.ChainName, Wallet.CliOptions.ChainAdminAddress, multi);

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotEmpty(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);
        }
    }
}
