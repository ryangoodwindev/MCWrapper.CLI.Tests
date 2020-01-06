using MCWrapper.CLI.Connection;
using MCWrapper.CLI.Ledger.Clients;
using MCWrapper.CLI.Options;
using MCWrapper.CLI.Tests.Options;
using MCWrapper.CLI.Tests.ServiceHelpers;
using MCWrapper.Data.Models.Wallet;
using MCWrapper.Ledger.Entities;
using MCWrapper.Ledger.Entities.Constants;
using MCWrapper.Ledger.Entities.Extensions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MCWrapper.CLI.Tests.MultiChainCLITests
{
    [TestFixture]
    public class WalletCLIClientExplicitTests

    {
        // multichain-cli.exe client supports the 'offchain' based methods
        private readonly IMultiChainCliWallet Wallet;
        private readonly IMultiChainCliUtility Utility;
        private readonly IMultiChainCliGeneral Blockchain;

        private readonly CliOptions Options;
        private readonly string Node = string.Empty;

        public WalletCLIClientExplicitTests()
        {
            var provider = new ServiceHelperExplicitSource();

            Wallet = provider.GetService<IMultiChainCliWallet>();
            Utility = provider.GetService<IMultiChainCliUtility>();
            Blockchain = provider.GetService<IMultiChainCliGeneral>();

            Options = Blockchain.CliOptions;
            Node = Options.ChainAdminAddress;
        }

        [Test]
        public async Task AddMultiSigAddressTestAsync()
        {
            // Act
            var actual = await Wallet.AddMultiSigAddressAsync(
                blockchainName: Options.ChainName,
                n_required: 1,
                keys: new[] { Node });

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task AppendRawExchangeTestAsync()
        {
            // Stage - Issue a new asset to the blockchain node 
            var asset = await Wallet.IssueAsync(
                blockchainName: Options.ChainName,
                toAddress: Options.ChainAdminAddress,
                assetName: new AssetEntity().Name,
                quantity: 100,
                smallestUnit: 1, default, default);

            // Assert
            Assert.IsEmpty(asset.Error);
            Assert.IsNotNull(asset.Result);
            Assert.IsInstanceOf<CliResponse<string>>(asset);

            // Act
            var prepareLockUnspent = await Wallet.PrepareLockUnspentFromAsync(
                blockchainName: Options.ChainName,
                from_address: Options.ChainAdminAddress,
                asset_quantities: new Dictionary<string, decimal> { { asset.Result, 10 } },
                _lock: true);

            // Assert
            Assert.IsEmpty(prepareLockUnspent.Error);
            Assert.IsNotNull(prepareLockUnspent.Result);
            Assert.IsInstanceOf<CliResponse<PrepareLockUnspentFromResult>>(prepareLockUnspent);

            // Act
            var rawExchange = await Wallet.CreateRawExchangeAsync(
                blockchainName: Options.ChainName,
                txid: prepareLockUnspent.Result.Txid,
                vout: prepareLockUnspent.Result.Vout,
                ask_assets: new Dictionary<string, int> { });

            // Assert
            Assert.IsEmpty(rawExchange.Error);
            Assert.IsNotNull(rawExchange.Result);
            Assert.IsInstanceOf<CliResponse<string>>(rawExchange);

            // Act
            var appendRaw = await Wallet.AppendRawExchangeAsync(
                blockchainName: Options.ChainName,
                hex: rawExchange.Result,
                txid: prepareLockUnspent.Result.Txid,
                vout: prepareLockUnspent.Result.Vout,
                ask_assets: new Dictionary<string, int> { });

            // Assert
            Assert.IsEmpty(appendRaw.Error);
            Assert.IsNotNull(appendRaw.Result);
            Assert.IsInstanceOf<CliResponse<AppendRawExchangeResult>>(appendRaw);

            // Act
            var disable = await Wallet.DisableRawTransactionAsync(
                blockchainName: Options.ChainName,
                tx_hex: appendRaw.Result.Hex);

            // Assert
            Assert.IsEmpty(disable.Error);
            Assert.IsNotNull(disable.Result);
            Assert.IsInstanceOf<CliResponse<object>>(disable);
        }

        [Test]
        public async Task ApproveFromTestAsync()
        {
            // Stage
            var filter = await Wallet.CreateAsync(Options.ChainName, Entity.StreamFilter, StreamEntity.GetUUID(), new { }, JsCode.DummyStreamFilterCode);

            // Assert
            Assert.IsEmpty(filter.Error);
            Assert.IsNotNull(filter.Result);
            Assert.IsInstanceOf<CliResponse<string>>(filter);

            // Act
            var actual = await Wallet.ApproveFromAsync(Options.ChainName, Node, filter.Result, new { approve = true, @for = "root" }); // we are going to expect this to fail since there are no upgrades available

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test, Ignore("BackupWallet test ignored since it halts the blockchain network")]
        public async Task BackupWalletTestAsync()
        {
            // Act
            var actual = await Wallet.BackupWalletAsync(Options.ChainName, "backup.dat");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task CombineUnspentTestAsync()
        {
            // Act
            var actual = await Wallet.CombineUnspentAsync(Options.ChainName, Options.ChainAdminAddress, 1, 100, 2, 1000, 15);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task CompleteRawExchangeTestAsync()
        {
            // Act
            var prepareLockUnspent = await Wallet.PrepareLockUnspentAsync(
                blockchainName: Options.ChainName,
                asset_quantities: NativeCurrency.Coins(0),
                _lock: true);

            // Assert
            Assert.IsEmpty(prepareLockUnspent.Error);
            Assert.IsNotNull(prepareLockUnspent.Result);
            Assert.IsInstanceOf<CliResponse<PrepareLockUnspentResult>>(prepareLockUnspent);

            // Act
            var rawExchange = await Wallet.CreateRawExchangeAsync(
                blockchainName: Options.ChainName,
                txid: prepareLockUnspent.Result.Txid,
                vout: prepareLockUnspent.Result.Vout,
                ask_assets: NativeCurrency.Coins(0));

            // Assert
            Assert.IsEmpty(rawExchange.Error);
            Assert.IsNotNull(rawExchange.Result);
            Assert.IsInstanceOf<CliResponse<string>>(rawExchange);

            // Act
            var appendRaw = await Wallet.AppendRawExchangeAsync(
                blockchainName: Options.ChainName,
                hex: rawExchange.Result,
                txid: prepareLockUnspent.Result.Txid,
                vout: prepareLockUnspent.Result.Vout,
                ask_assets: NativeCurrency.Coins(0));

            // Assert
            Assert.IsEmpty(appendRaw.Error);
            Assert.IsNotNull(appendRaw.Result);
            Assert.IsInstanceOf<CliResponse<AppendRawExchangeResult>>(appendRaw);

            // Act
            var complete = await Wallet.CompleteRawExchangeAsync(
                blockchainName: Options.ChainName,
                hex: appendRaw.Result.Hex,
                txid: prepareLockUnspent.Result.Txid,
                vout: prepareLockUnspent.Result.Vout,
                ask_assets: NativeCurrency.Coins(0),
                data: "test".ToHex());

            // Assert
            Assert.IsEmpty(complete.Error);
            Assert.IsNotNull(complete.Result);
            Assert.IsInstanceOf<CliResponse<object>>(complete);
        }

        [Test]
        public async Task CreateFromTestAsync()
        {
            // Act
            var actual = await Wallet.CreateFromAsync(Options.ChainName, Node, Entity.Stream, StreamEntity.GetUUID(), true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<string>>(actual);
        }

        [Test]
        public async Task CreateRawExchangeTestAsync()
        {
            // Act
            var prepareLockUnspent = await Wallet.PrepareLockUnspentAsync(Options.ChainName, new Dictionary<string, int> { { "", 0 } }, true);

            // Act
            var rawExchange = await Wallet.CreateRawExchangeAsync(Options.ChainName, prepareLockUnspent.Result.Txid, prepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Act
            var disable = await Wallet.DisableRawTransactionAsync(Options.ChainName, rawExchange.Result);

            // Assert
            Assert.IsEmpty(prepareLockUnspent.Error);
            Assert.IsNotNull(prepareLockUnspent.Result);
            Assert.IsInstanceOf<CliResponse<PrepareLockUnspentResult>>(prepareLockUnspent);

            // Assert
            Assert.IsEmpty(rawExchange.Error);
            Assert.IsNotNull(rawExchange.Result);
            Assert.IsInstanceOf<CliResponse<string>>(rawExchange);

            // Assert
            Assert.IsEmpty(disable.Error);
            Assert.IsNotNull(disable.Result);
            Assert.IsInstanceOf<CliResponse<object>>(disable);
        }

        [Test]
        public async Task CreateRawSendFromTestAsync()
        {
            // Act
            var actual = await Wallet.CreateRawSendFromAsync(Options.ChainName, Node, new Dictionary<string, double> { { Node, 0 } });

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task CreateTestAsync()
        {
            // Act
            var actual = await Wallet.CreateAsync(Options.ChainName, Entity.Stream, StreamEntity.GetUUID(), true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<string>>(actual);
        }

        [Test]
        public async Task DecodeRawExchangeTestAsync()
        {
            // Act
            var prepareLockUnspent = await Wallet.PrepareLockUnspentAsync(Options.ChainName, new Dictionary<string, int> { { "", 0 } }, true);

            // Act
            var rawExchange = await Wallet.CreateRawExchangeAsync(Options.ChainName, prepareLockUnspent.Result.Txid, prepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Act
            var decode = await Wallet.DecodeRawExchangeAsync(Options.ChainName, rawExchange.Result, true);

            // Act
            var disable = await Wallet.DisableRawTransactionAsync(Options.ChainName, rawExchange.Result);

            // Assert
            Assert.IsEmpty(prepareLockUnspent.Error);
            Assert.IsNotNull(prepareLockUnspent.Result);
            Assert.IsInstanceOf<CliResponse<PrepareLockUnspentResult>>(prepareLockUnspent);

            // Assert
            Assert.IsEmpty(rawExchange.Error);
            Assert.IsNotNull(rawExchange.Result);
            Assert.IsInstanceOf<CliResponse<string>>(rawExchange);

            // Assert
            Assert.IsEmpty(decode.Error);
            Assert.IsNotNull(decode.Result);
            Assert.IsInstanceOf<CliResponse<DecodeRawExchangeResult>>(decode);

            // Assert
            Assert.IsEmpty(disable.Error);
            Assert.IsNotNull(disable.Result);
            Assert.IsInstanceOf<CliResponse<object>>(disable);
        }

        [Test]
        public async Task DisableRawTransactionTestAsync()
        {
            // Act
            var prepareLockUnspent = await Wallet.PrepareLockUnspentAsync(Options.ChainName, new Dictionary<string, int> { { "", 0 } }, true);

            // Act
            var rawExchange = await Wallet.CreateRawExchangeAsync(Options.ChainName, prepareLockUnspent.Result.Txid, prepareLockUnspent.Result.Vout, new Dictionary<string, int> { { "", 0 } });

            // Act
            var disable = await Wallet.DisableRawTransactionAsync(Options.ChainName, rawExchange.Result);

            // Assert
            Assert.IsEmpty(prepareLockUnspent.Error);
            Assert.IsNotNull(prepareLockUnspent.Result);
            Assert.IsInstanceOf<CliResponse<PrepareLockUnspentResult>>(prepareLockUnspent);

            // Assert
            Assert.IsEmpty(rawExchange.Error);
            Assert.IsNotNull(rawExchange.Result);
            Assert.IsInstanceOf<CliResponse<string>>(rawExchange);

            // Assert
            Assert.IsEmpty(disable.Error);
            Assert.IsNotNull(disable.Result);
            Assert.IsInstanceOf<CliResponse<object>>(disable);
        }

        [Test]
        public async Task DumpPrivKeyTestAsync()
        {
            // Act
            var actual = await Wallet.DumpPrivKeyAsync(Options.ChainName, Node);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test, Ignore("Dumping the wallet seems to slow down the network. Test is passing and ignored.")]
        public async Task DumpWalletTestAync()
        {
            // Act
            var actual = await Wallet.DumpWalletAsync(Options.ChainName, "test_async");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test, Ignore("Test is implemented and ignored since I don't want to encrypt my wallet in staging")]
        public async Task EncryptWalletTestAsync()
        {
            // Act
            var actual = await Wallet.EncryptWalletAsync(Options.ChainName, "some_password");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetAccountAddressTestAsync()
        {
            // Act
            var actual = await Wallet.GetAccountAddressAsync(Options.ChainName, "some_account_name");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetAccountTestAsync()
        {
            // Act
            var actual = await Wallet.GetAccountAsync(Options.ChainName, Options.ChainAdminAddress);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task GetAddressBalancesTestAsync()
        {
            // Act
            var actual = await Wallet.GetAddressBalancesAsync(Options.ChainName, Options.ChainAdminAddress);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetAddressBalancesResult[]>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetAddressesByAccountTestAsync()
        {
            // Act
            var actual = await Wallet.GetAddressesByAccountAsync(Options.ChainName, "some_account_name");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task GetAddressesTestAsync()
        {
            // Act
            var actual = await Wallet.GetAddressesAsync(Options.ChainName, true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetAddressesResult[]>>(actual);
        }

        [Test]
        public async Task GetAddressTransactionTestAsync()
        {
            // Stage
            var transaction = await Wallet.IssueAsync(Options.ChainName, Node, new AssetEntity().Name, 100, 1, 0, default);

            // Act
            var actual = await Wallet.GetAddressTransactionAsync(Options.ChainName, Node, transaction.Result, true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetAddressTransactionResult>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetAssetBalancesTestAsync()
        {
            // Act
            var actual = await Wallet.GetAssetBalancesAsync(Options.ChainName, "some_account_name", 2, true, true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task GetAssetTransactionTestAsync()
        {
            // Stage
            var asset = await Wallet.IssueAsync(Options.ChainName, Node, new AssetEntity().Name, 100, 1, default, default);

            // Stage
            await Wallet.SubscribeAsync(Options.ChainName, asset.Result, false);

            // Act
            var actual = await Wallet.GetAssetTransactionAsync(Options.ChainName, asset.Result, asset.Result, true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetAssetTransactionResult>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetBalanceTestAsync()
        {
            // Act
            var actual = await Wallet.GetBalanceAsync(Options.ChainName, "");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task GetMultiBalancesTestAsync()
        {
            // Act
            var actual = await Wallet.GetMultiBalancesAsync(Options.ChainName, Node);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task GetNewAddressTestAsync()
        {
            // Act
            var actual = await Wallet.GetNewAddressAsync(blockchainName: Options.ChainName);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<string>>(actual);
        }

        [Test]
        public async Task GetRawChangeAddressTestAsync()
        {
            // Act
            var actual = await Wallet.GetRawChangeAddressAsync(Options.ChainName);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetReceivedByAccountTestAsync()
        {
            // Act
            var actual = await Wallet.GetReceivedByAccountAsync(Options.ChainName, "some_account_name", 2);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need accounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task GetReceivedByAddressTestAsync()
        {
            // Act
            var actual = await Wallet.GetReceivedByAddressAsync(Options.ChainName, Options.ChainAdminAddress, 2);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task GetStreamItemTestAsync()
        {
            // Stage
            var publish = await Wallet.PublishFromAsync(Options.ChainName, Node, "root", ChainEntity.GetUUID(), "Stream item data".ToHex(), "offchain");

            // Assert
            Assert.IsEmpty(publish.Error);
            Assert.IsNotNull(publish.Result);
            Assert.IsInstanceOf<CliResponse<string>>(publish);

            // Act
            var actual = await Wallet.GetStreamItemAsync(Options.ChainName, "root", publish.Result, true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetStreamItemResult>>(actual);
        }

        [Test]
        public async Task GetStreamKeySummaryTestAsync()
        {
            // Stage
            var streamKey = ChainEntity.GetUUID();

            // Stage
            await Wallet.PublishFromAsync(Options.ChainName, Node, "root", streamKey, "Stream item data".ToHex());

            // Act
            var actual = await Wallet.GetStreamKeySummaryAsync(Options.ChainName, "root", streamKey, "jsonobjectmerge,ignore,recursive");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task GetStreamPublisherSummaryTestAsync()
        {
            // Act
            var actual = await Wallet.GetStreamPublisherSummaryAsync(Options.ChainName, "root", Node, "jsonobjectmerge,ignore,recursive");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task GetTotalBalancesTestAsync()
        {
            // Act
            var actual = await Wallet.GetTotalBalancesAsync(Options.ChainName, 1, true, false);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetTotalBalancesResult[]>>(actual);
        }

        [Test]
        public async Task GetTransactionTestAsync()
        {
            // Stage
            var txid = await Wallet.IssueFromAsync(Options.ChainName, Node, Node, new AssetEntity().Name, 1000, 0.1, default, default);

            // Assert
            Assert.IsEmpty(txid.Error);
            Assert.IsNotNull(txid.Result);
            Assert.IsInstanceOf<CliResponse<string>>(txid);

            // Act
            var actual = await Wallet.GetTransactionAsync(Options.ChainName, txid.Result, true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetTransactionResult>>(actual);
        }

        [Test]
        public async Task GetTxOutDataTestAsync()
        {
            // Stage
            var publish = await Wallet.PublishFromAsync(Options.ChainName, Node, "root", ChainEntity.GetUUID(), "Stream item data".ToHex(), "offchain");

            // Stage
            var transaction = await Wallet.GetTransactionAsync(Options.ChainName, publish.Result, true);

            // Act
            var actual = await Wallet.GetTxOutDataAsync(Options.ChainName, transaction.Result.Txid, 0, 10, 0);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task GetUnconfirmedBalanceTestAsync()
        {
            // Act
            var actual = await Wallet.GetUnconfirmedBalanceAsync(Options.ChainName);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task GetWalletInfoTestAsync()
        {
            // Act
            var actual = await Wallet.GetWalletInfoAsync(Options.ChainName);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetWalletInfoResult>>(actual);
        }

        [Test]
        public async Task GetWalletTransactionTestAsync()
        {
            // Stage
            var publish = await Wallet.PublishFromAsync(blockchainName: Options.ChainName, Node, "root", ChainEntity.GetUUID(), "Stream item data".ToHex());

            // Act
            var actual = await Wallet.GetWalletTransactionAsync(Options.ChainName, publish.Result, true, true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetWalletTransactionResult>>(actual);
        }

        [Test]
        public async Task GrantFromTestAsync()
        {
            // Stage
            var newAddress = await Wallet.GetNewAddressAsync(blockchainName: Options.ChainName);

            // Act
            var actual = await Wallet.GrantFromAsync(Options.ChainName, Node, newAddress.Result, Permission.Receive);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<string>>(actual);
        }

        [Test]
        public async Task GrantTestAsync()
        {
            // Act
            var actual = await Wallet.GrantAsync(Options.ChainName, Node, Permission.Receive);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<string>>(actual);
        }

        [Test]
        public async Task GrantWithDataFromTestAsync()
        {
            // Stage
            var newAddress = await Wallet.GetNewAddressAsync(blockchainName: Options.ChainName);

            // Act
            var actual = await Wallet.GrantWithDataFromAsync(blockchainName: Options.ChainName, Node, newAddress.Result, Permission.Receive, "some_data".ToHex());

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<string>>(actual);
        }

        [Test]
        public async Task GrantWithDataTestAsync()
        {
            // Stage
            var newAddress = await Wallet.GetNewAddressAsync(blockchainName: Options.ChainName);

            // Act
            var actual = await Wallet.GrantWithDataAsync(Options.ChainName, newAddress.Result, Permission.Receive, "some_data".ToHex());

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<string>>(actual);
        }

        [Test, Ignore("I don't want to import any addresses during unit testing")]
        public async Task ImportAddressTestAsync()
        {
            // Act
            var actual = await Wallet.ImportAddressAsync(Options.ChainName, "some_external_address", "some_label", false);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test, Ignore("I don't want to import any private keys during unit testing")]
        public async Task ImportPrivKeyTestAsync()
        {
            // Act
            var actual = await Wallet.ImportPrivKeyAsync(Options.ChainName, "some_external_private_key", "some_label", false);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test, Ignore("Tests impacting the current wallet are ignore while general tests are running")]
        public async Task ImportWalletTestAsync()
        {
            // Act
            var actual = await Wallet.ImportWalletAsync(Options.ChainName, "test", false);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        public async Task IssueFromTestStronglyTypedAsync()
        {
            // Act
            var act_1 = await Wallet.IssueFromAsync(Options.ChainName, Node, Node, new AssetEntity().Name, 100, 1, 0.1m, new Dictionary<string, string> { { "text", "Text for the chain".ToHex() } });

            // Assert
            Assert.IsEmpty(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<CliResponse<string>>(act_1);

            // Act
            var act_2 = await Wallet.IssueFromAsync(Options.ChainName, Node, Node, new AssetEntity().Name, 100, 1, 0.1m, new { text = "Text for the chain".ToHex() });

            // Assert
            Assert.IsEmpty(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<CliResponse<string>>(act_2);
        }

        public async Task IssueFromTestGenericallyTypedAsync()
        {
            // Act
            var act_1 = await Wallet.IssueFromAsync(Options.ChainName, Node, Node, new { name = UUID.NoHyphens }, 100, 1, 0.1m, new Dictionary<string, string> { { "text", "Text for the chain".ToHex() } });

            // Assert
            Assert.IsEmpty(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<CliResponse<string>>(act_1);

            // Act
            var act_2 = await Wallet.IssueFromAsync(Options.ChainName, Node, Node, new { name = UUID.NoHyphens }, 100, 1, 0.1m, new { text = "Text for the chain".ToHex() });

            // Assert
            Assert.IsEmpty(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<CliResponse<string>>(act_2);
        }

        public async Task IssueFromTestStringNameAsync()
        {
            // Act
            var act_1 = await Wallet.IssueFromAsync(Options.ChainName, Node, Node, UUID.NoHyphens, 100, 1, 0.1m, new Dictionary<string, string> { { "text", "Text for the chain".ToHex() } });

            // Assert
            Assert.IsEmpty(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<CliResponse<string>>(act_1);

            // Act
            var act_2 = await Wallet.IssueFromAsync(Options.ChainName, Node, Node, UUID.NoHyphens, 100, 1, 0.1m, new { text = "Text for the chain".ToHex() });

            // Assert
            Assert.IsEmpty(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<CliResponse<string>>(act_2);
        }

        [Test]
        public async Task IssueMoreFromTestAsync()
        {
            // Stage
            var issue = await Wallet.IssueFromAsync(Options.ChainName, Node, Node, new AssetEntity(), 100, 1, 0, new Dictionary<string, string> { { "text", "Text for the chain".ToHex() } });

            // Assert
            Assert.IsEmpty(issue.Error);
            Assert.IsNotNull(issue.Result);
            Assert.IsInstanceOf<CliResponse<string>>(issue);

            // Act
            var act_1 = await Wallet.IssueMoreFromAsync(Options.ChainName, Node, Node, issue.Result.ToString(), 100, 0, new Dictionary<string, string> { { "text", "Text for the chain".ToHex() } });

            // Assert
            Assert.IsEmpty(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<CliResponse<object>>(act_1);

            // Act
            var act_2 = await Wallet.IssueMoreFromAsync(Options.ChainName, Node, Node, issue.Result.ToString(), 100, 0, new { text = "Text for the chain".ToHex() });

            // Assert
            Assert.IsEmpty(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<CliResponse<object>>(act_2);
        }

        [Test]
        public async Task IssueMoreTestAsync()
        {
            // Stage
            var issue = await Wallet.IssueAsync(Options.ChainName, Node, new AssetEntity(), 100, 1, 0, new Dictionary<string, string> { { "text", "Text for the chain".ToHex() } });

            // Assert
            Assert.IsEmpty(issue.Error);
            Assert.IsNotNull(issue.Result);
            Assert.IsInstanceOf<CliResponse<string>>(issue);

            // Act
            var act_1 = await Wallet.IssueMoreAsync(Options.ChainName, Node, issue.Result.ToString(), 100, 0, new Dictionary<string, string> { { "text", "Text for the chain".ToHex() } });

            // Assert
            Assert.IsEmpty(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<CliResponse<object>>(act_1);

            // Act
            var act_2 = await Wallet.IssueMoreAsync(Options.ChainName, Node, issue.Result.ToString(), 100, 0, new { text = "Text for the chain".ToHex() });

            // Assert
            Assert.IsEmpty(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<CliResponse<object>>(act_2);
        }

        [Test]
        public async Task IssueTestStronglyTypedAsync()
        {
            // Act
            var act_1 = await Wallet.IssueAsync(Options.ChainName, Options.ChainAdminAddress, new AssetEntity(), 100, 1, 0, new Dictionary<string, string> { { "text", "Text for the chain".ToHex() } });

            // Assert
            Assert.IsEmpty(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<CliResponse<string>>(act_1);

            // Act
            var act_2 = await Wallet.IssueAsync(Options.ChainName, Options.ChainAdminAddress, new AssetEntity(), 100, 1, 0, new { text = "Text for the chain".ToHex() });

            // Assert
            Assert.IsEmpty(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<CliResponse<string>>(act_2);
        }

        [Test]
        public async Task IssueTestGenericallyTypedAsync()
        {
            // Act
            var act_1 = await Wallet.IssueAsync(Options.ChainName, Options.ChainAdminAddress, new { name = UUID.NoHyphens }, 100, 1, 0, new Dictionary<string, string> { { "text", "Text for the chain".ToHex() } });

            // Assert
            Assert.IsEmpty(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<CliResponse<string>>(act_1);

            // Act
            var act_2 = await Wallet.IssueAsync(Options.ChainName, Options.ChainAdminAddress, new { name = UUID.NoHyphens }, 100, 1, 0, new { text = "Text for the chain".ToHex() });

            // Assert
            Assert.IsEmpty(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<CliResponse<string>>(act_2);
        }

        [Test]
        public async Task IssueTestStringNameAsync()
        {
            // Act
            var act_1 = await Wallet.IssueAsync(Options.ChainName, Options.ChainAdminAddress, UUID.NoHyphens, 100, 1, 0, new Dictionary<string, string> { { "text", "Text for the chain".ToHex() } });

            // Assert
            Assert.IsEmpty(act_1.Error);
            Assert.IsNotNull(act_1.Result);
            Assert.IsInstanceOf<CliResponse<string>>(act_1);

            // Act
            var act_2 = await Wallet.IssueAsync(Options.ChainName, Options.ChainAdminAddress, UUID.NoHyphens, 100, 1, 0, new { text = "Text for the chain".ToHex() });

            // Assert
            Assert.IsEmpty(act_2.Error);
            Assert.IsNotNull(act_2.Result);
            Assert.IsInstanceOf<CliResponse<string>>(act_2);
        }

        [Test]
        public async Task KeyPoolRefillTestAsync()
        {
            // Act
            var actual = await Wallet.KeyPoolRefillAsync(Options.ChainName, 200);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need listaccounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task ListAccountsTestAsync()
        {
            // Act
            var actual = await Wallet.ListAccountsAsync(Options.ChainName, 2, true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task ListAddressesTestAsync()
        {
            // Act
            var actual = await Wallet.ListAddressesAsync(Options.ChainName, "*", true, 1, 0);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<ListAddressesResult[]>>(actual);
        }

        [Test]
        public async Task ListAddressGroupingsTestAsync()
        {
            // Act
            var actual = await Wallet.ListAddressGroupingsAsync(Options.ChainName);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task ListAddressTransactionsTestAsync()
        {
            // Act
            var actual = await Wallet.ListAddressTransactionsAsync(Options.ChainName, Options.ChainAdminAddress, 10, 0, true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<ListAddressTransactionsResult[]>>(actual);
        }

        [Test]
        public async Task ListAssetTransactionsTestAsync()
        {
            // Stage
            var issue = await Wallet.IssueAsync(Options.ChainName, Node, new AssetEntity().Name, 100, 1, default, default);

            // Stage
            await Wallet.SubscribeAsync(Options.ChainName, issue.Result, false);

            // Act
            var actual = await Wallet.ListAssetTransactionsAsync(Options.ChainName, issue.Result, true, 10, 0, true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<ListAssetTransactionsResult[]>>(actual);
        }

        [Test]
        public async Task ListLockUnspentTestAsync()
        {
            // Act
            var actual = await Wallet.ListLockUnspentAsync(Options.ChainName);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need listaccounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task ListReceivedByAccountTestAsync()
        {
            // Act
            var actual = await Wallet.ListReceivedByAccountAsync(Options.ChainName, 2, true, true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need listaccounts, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task ListReceivedByAddressTestAsync()
        {
            // Act
            var actual = await Wallet.ListReceivedByAddressAsync(Options.ChainName, 2, true, true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task ListSinceBlockTestAsync()
        {
            // Stage
            var lastBlock = await Blockchain.GetLastBlockInfoAsync(Options.ChainName, 0);

            // Act
            var actual = await Wallet.ListSinceBlockAsync(Options.ChainName, lastBlock.Result.Hash, 1, true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task ListStreamBlockItemsTestAsync()
        {
            // Act
            var actual = await Wallet.ListStreamBlockItemsAsync(Options.ChainName, "root", "61-65", true, 10, 0);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task ListStreamItemsTestAsync()
        {
            // Act
            var actual = await Wallet.ListStreamItemsAsync(Options.ChainName, "root", true, 10, 0, true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<ListStreamItemsResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamKeyItemsTestAsync()
        {
            // Act
            var actual = await Wallet.ListStreamKeyItemsAsync(Options.ChainName, "root", "some_key", true, 10, 0, true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<ListStreamKeyItemsResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamKeysTestAsync()
        {
            // Act
            var actual = await Wallet.ListStreamKeysAsync(Options.ChainName, "root", "*", true, 10, 0, true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<ListStreamKeysResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamPublisherItemsTestAsync()
        {
            // Act
            var actual = await Wallet.ListStreamPublisherItemsAsync(Options.ChainName, "root", Options.ChainAdminAddress, true, 10, 0, true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<ListStreamPublisherItemsResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamPublishersTestAsync()
        {
            // Act
            var actual = await Wallet.ListStreamPublishersAsync(Options.ChainName, "root", "*", true, 10, 0, true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<ListStreamPublishersResult[]>>(actual);
        }

        [Test]
        public async Task ListStreamQueryItemsTestAsync()
        {
            // Act
            var actual = await Wallet.ListStreamQueryItemsAsync(Options.ChainName, "root", new { publisher = Node }, true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task ListStreamTxItemsTestAsync()
        {
            // Stage
            var txid = await Wallet.PublishAsync(Options.ChainName, "root", ChainEntity.GetUUID(), "Some Stream Item Data".ToHex(), "offchain");

            // Act
            var actual = await Wallet.ListStreamTxItemsAsync(Options.ChainName, "root", txid.Result, true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test, Ignore("Not supported with scalable wallet - if you need listtransactions, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task ListTransactionsTestAsync()
        {
            // Act
            var actual = await Wallet.ListTransactionsAsync(Options.ChainName, "some_account", 10, 0, true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<ListTransactionsResult[]>>(actual);
        }

        [Test]
        public async Task ListUnspentTestAsync()
        {
            // Act
            var actual = await Wallet.ListUnspentAsync(Options.ChainName, 2, 100, new[] { Options.ChainAdminAddress });

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<ListUnspentResult[]>>(actual);
        }

        [Test]
        public async Task ListWalletTransactionsTestAsync()
        {
            // Act
            var actual = await Wallet.ListWalletTransactionsAsync(Options.ChainName, 10, 0, true, true);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<ListWalletTransactionsResult[]>>(actual);
        }

        [Test]
        public async Task LockUnspentTestAsync()
        {
            // Stage
            var unspent = await Wallet.PrepareLockUnspentAsync(Options.ChainName, NativeCurrency.Coins(0), false);

            // Assert
            Assert.IsEmpty(unspent.Error);
            Assert.IsNotNull(unspent.Result);
            Assert.IsInstanceOf<CliResponse<PrepareLockUnspentResult>>(unspent);

            // Act
            var actual = await Wallet.LockUnspentAsync(Options.ChainName, false, new object[] { new { txid = unspent.Result.Txid, vout = unspent.Result.Vout } });

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need move, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task MoveTestAsync()
        {
            // Act
            var actual = await Wallet.MoveAsync(Options.ChainName, "from_account", "to_account", 0.01, 6, "Testing the Move function");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task PrepareLockUnspentTestAsync()
        {
            // Act
            var actual = await Wallet.PrepareLockUnspentAsync(Options.ChainName, new Dictionary<string, double> { { "", 0 } }, false);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<PrepareLockUnspentResult>>(actual);
        }

        [Test]
        public async Task PrepareLockUnspentFromTestAsync()
        {
            // Act
            var actual = await Wallet.PrepareLockUnspentFromAsync(Options.ChainName, Node, new Dictionary<string, double> { { "", 0 } }, false);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<PrepareLockUnspentFromResult>>(actual);
        }

        [Test]
        public async Task PublishTestAsync()
        {
            // Act
            var actual = await Wallet.PublishAsync(Options.ChainName, "root", "test_key", "some_data".ToHex(), "offchain");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<string>>(actual);
        }

        [Test]
        public async Task PublishFromTestAsync()
        {
            // Act
            var actual = await Wallet.PublishFromAsync(Options.ChainName, Options.ChainAdminAddress, "root", "test_key", "some_data".ToHex(), "offchain");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<string>>(actual);
        }

        [Test]
        public async Task PublishMultiTestAsync()
        {
            // Act
            var actual = await Wallet.PublishMultiAsync(Options.ChainName, "root", new object[] { new { key = "some_key", data = "some_data".ToHex() } }, "offchain");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<string>>(actual);
        }

        [Test]
        public async Task PublishMultiFromTestAsync()
        {
            // Act
            var actual = await Wallet.PublishMultiFromAsync(Options.ChainName, Node, "root", new object[] { new { key = "some_key", data = "some_data".ToHex() } }, "offchain");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<string>>(actual);
        }

        [Test, Ignore("ResendWalletTransaction test is deffered from normal unit testing")]
        public async Task ResendWalletTransactionsTestAsync()
        {
            // Act - ttempt to resend the current wallet's transaction
            var actual = await Wallet.ResendWalletTransactionsAsync(Options.ChainName);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task RevokeTestAsync()
        {
            // Stage - Ask the blockchain network for a new address
            var newAddress = await Wallet.GetNewAddressAsync(blockchainName: Options.ChainName);

            // Assert
            Assert.IsEmpty(newAddress.Error);
            Assert.IsNotNull(newAddress.Result);
            Assert.IsInstanceOf<CliResponse<string>>(newAddress);

            // Stage - Grant new address receive permissions
            var grant = await Wallet.GrantFromAsync(Options.ChainName, Node, newAddress.Result, Permission.Receive);

            // Assert
            Assert.IsEmpty(grant.Error);
            Assert.IsNotNull(grant.Result);
            Assert.IsInstanceOf<CliResponse<string>>(grant);

            // Act - Revoke send permission
            var actual = await Wallet.RevokeAsync(Options.ChainName, newAddress.Result, "send");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task RevokeFromTestAsync()
        {
            // Stage - Ask the blockchain network for a new address
            var newAddress = await Wallet.GetNewAddressAsync(blockchainName: Options.ChainName);

            // Stage - Grant new address receive permissions
            await Wallet.GrantAsync(Options.ChainName, newAddress.Result, Permission.Receive);

            // Act - Revoke send permission
            var actual = await Wallet.RevokeFromAsync(Options.ChainName, Node, newAddress.Result, "send");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task SendTestAsync()
        {
            // Act
            var actual = await Wallet.SendAsync(Options.ChainName, Node, 0, "Comment_text", "Comment_To_text");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<string>>(actual);
        }

        [Test]
        public async Task SendAssetTestAsync()
        {
            // Stage
            var asset = await Wallet.IssueAsync(Options.ChainName, Node, new AssetEntity().Name, 100, 1, default, default);

            // Act
            var actual = await Wallet.SendAssetAsync(Options.ChainName, Node, asset.Result, 1);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task SendAssetFromTestAsync()
        {
            // Stage
            var asset = await Wallet.IssueAsync(Options.ChainName, Node, new AssetEntity(), 100, smallestUnit: 1, default, default);

            // Act
            var actual = await Wallet.SendAssetFromAsync(Options.ChainName, Node, Node, asset.Result, asset_quantity: 1);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task SendFromTestAsync()
        {
            // Act
            var actual = await Wallet.SendFromAsync(Options.ChainName, Node, Node, 0, "Comment_text", "Comment_To_text");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need sendfrom, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task SendFromAccountTestAsync()
        {
            // Act
            var actual = await Wallet.SendFromAccountAsync(Options.ChainName, Node, Node, .001, 2, "Comment Text", "Comment_To text");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need sendmany, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task SendManyTestAsync()
        {
            // Act
            var actual = await Wallet.SendManyAsync(Options.ChainName, "", new object[] { new Dictionary<string, double> { { Node, 1 } } }, 2, "Comment text");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task SendWithDataTestAsync()
        {
            // Act
            var actual = await Wallet.SendWithDataAsync(Options.ChainName, Node, 0, "some data".ToHex());

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task SendWithDataFromTestAsync()
        {
            // Act
            var actual = await Wallet.SendWithDataFromAsync(Options.ChainName, Node, Node, 0, "some data".ToHex());

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test, Ignore("Accounts are not supported with scalable wallet - if you need move, run multichaind -walletdbversion=1 -rescan, but the wallet will perform worse")]
        public async Task SetAccountTestAsync()
        {
            // Act
            var actual = await Wallet.SetAccountAsync(Options.ChainName, Node, "master_account");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test, Ignore("Ignored since I do not want to change the TxFee while other transactions are runningh")]
        public async Task SetTxFeeTestAsync()
        {
            // Act
            var actual = await Wallet.SetTxFeeAsync(Options.ChainName, 0.0001);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task SignMessageTestAsync()
        {
            // Act
            var actual = await Wallet.SignMessageAsync(Options.ChainName, Node, "Testing the SignMessage function");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<string>>(actual);
        }

        [Test]
        public async Task SubscribeTestAsync()
        {
            // Act
            var actual = await Wallet.SubscribeAsync(Options.ChainName, "root", false);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test]
        public async Task TxOutToBinaryCacheTestAsync()
        {
            // Stage
            var binaryCache = await Utility.CreateBinaryCacheAsync(Options.ChainName);

            // Stage
            var publish = await Wallet.PublishFromAsync(Options.ChainName, Node, "root", ChainEntity.GetUUID(), "A bunch of text data that will be transcribed to this this publish event and this one is async brotato chip".ToHex(), "offchain");

            // Stage
            var transaction = await Wallet.GetAddressTransactionAsync(Options.ChainName, Node, publish.Result, true);

            // Act
            var actual = await Wallet.TxOutToBinaryCacheAsync(Options.ChainName, binaryCache.Result, transaction.Result.Txid, transaction.Result.Vout[0].N, 100000, 0);

            // Act
            await Utility.DeleteBinaryCacheAsync(Options.ChainName, binaryCache.Result);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<double>>(actual);
        }

        [Test]
        public async Task UnsubscribeTestAsync()
        {
            // Act
            var actual = await Wallet.UnsubscribeAsync(Options.ChainName, "root", false);

            // Act
            await Wallet.SubscribeAsync(Options.ChainName, "root", false);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test, Ignore("Wallet related tests are ignored while general tests are running")]
        public async Task WalletLockTestAsync()
        {
            // Act
            var actual = await Wallet.WalletLockAsync(Options.ChainName);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test, Ignore("Wallet related tests are ignored while general tests are running")]
        public async Task WalletPassphraseTestAsync()
        {
            // Act
            var actual = await Wallet.WalletPassphraseAsync(Options.ChainName, "wallet_passphrase", 10);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test, Ignore("Wallet related tests are ignored while general tests are running")]
        public async Task WalletPassphraseChangeTestAsync()
        {
            // Act
            var actual = await Wallet.WalletPassphraseChangeAsync(Options.ChainName, "old_passphrase", "new_passphrase");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }
    }
}
