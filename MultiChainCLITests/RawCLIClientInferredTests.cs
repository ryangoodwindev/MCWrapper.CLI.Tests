using MCWrapper.CLI.Connection;
using MCWrapper.CLI.Ledger.Clients;
using MCWrapper.CLI.Tests.ServiceHelpers;
using MCWrapper.Data.Models.Raw;
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
    public class RawCLIClientInferredTests
    {
        // multichain-cli.exe client supports the 'raw' based methods
        private readonly IMultiChainCliRaw Raw;
        private readonly IMultiChainCliWallet Wallet;

        public RawCLIClientInferredTests()
        {
            var provider = new ServiceHelperExplicitSource();

            Raw = provider.GetService<IMultiChainCliRaw>();
            Wallet = provider.GetService<IMultiChainCliWallet>();
        }

        [Test]
        public async Task RawTransactionTest()
        {
            // Stage - instantiate two new Assets
            var assetModel_0 = new AssetEntity();
            var assetModel_1 = new AssetEntity();

            var asset_0 = await Wallet.IssueAsync(
                toAddress: Wallet.CliOptions.ChainAdminAddress,
                assetParams: assetModel_0,
                quantity: 100,
                smallestUnit: 1, default, default);

            Assert.IsEmpty(asset_0.Error);
            Assert.IsNotEmpty(asset_0.Result);
            Assert.IsInstanceOf<CliResponse<string>>(asset_0);

            var asset_1 = await Wallet.IssueAsync(
                toAddress: Wallet.CliOptions.ChainAdminAddress,
                assetParams: assetModel_1,
                quantity: 100,
                smallestUnit: 1, default, default);

            Assert.IsEmpty(asset_1.Error);
            Assert.IsNotEmpty(asset_1.Result);
            Assert.IsInstanceOf<CliResponse<string>>(asset_1);


            var newAddress_0 = await Wallet.GetNewAddressAsync();

            Assert.IsEmpty(newAddress_0.Error);
            Assert.IsNotEmpty(newAddress_0.Result);
            Assert.IsInstanceOf<CliResponse<string>>(newAddress_0);


            var grant = await Wallet.GrantAsync(newAddress_0.Result, $"{Permission.Receive},{Permission.Send}");

            Assert.IsEmpty(grant.Error);
            Assert.IsNotNull(grant.Result);
            Assert.IsInstanceOf<CliResponse<string>>(grant);

            var listUnspent = await Wallet.ListUnspentAsync(0, 9999, new[] { Wallet.CliOptions.ChainAdminAddress });

            Assert.IsEmpty(listUnspent.Error);
            Assert.IsNotNull(listUnspent.Result);
            Assert.IsInstanceOf<CliResponse<ListUnspentResult[]>>(listUnspent);

            var unspentAsset_0 = listUnspent.Result.SingleOrDefault(s => s.Assets.Any(a => a.Name == assetModel_0.Name));
            var unspentAsset_1 = listUnspent.Result.SingleOrDefault(s => s.Assets.Any(a => a.Name == assetModel_1.Name));

            var createRaw = await Raw.CreateRawTransactionAsync(
                transactions: new object[]
                {
                    new Dictionary<string, object>
                    {
                        { "txid", unspentAsset_0.Txid },
                        { "vout", unspentAsset_0.Vout }
                    },
                    new Dictionary<string, object>
                    {
                        { "txid", unspentAsset_1.Txid },
                        { "vout", unspentAsset_1.Vout }
                    }
                },
                addresses: new Dictionary<string, Dictionary<string, int>>
                {
                    {
                        newAddress_0.Result, new Dictionary<string, int>
                        {
                            { assetModel_0.Name, 1 },
                            { assetModel_1.Name, 2 }
                        }
                    }
                }, null, null);

            Assert.IsEmpty(createRaw.Error);
            Assert.IsNotNull(createRaw.Result);
            Assert.IsInstanceOf<CliResponse<object>>(createRaw);

            var decode = await Raw.DecodeRawTransactionAsync($"{createRaw.Result}");

            Assert.IsEmpty(decode.Error);
            Assert.IsNotNull(decode.Result);
            Assert.IsInstanceOf<CliResponse<DecodeRawTransactionResult>>(decode);

            var rawChange = await Raw.AppendRawChangeAsync(tx_hex: $"{createRaw.Result}", address: Raw.CliOptions.ChainAdminAddress, 0);

            Assert.IsEmpty(rawChange.Error);
            Assert.IsNotNull(rawChange.Result);
            Assert.IsInstanceOf<CliResponse<object>>(rawChange);

            var rawData = await Raw.AppendRawDataAsync($"{rawChange.Result}", new
            {
                text = "TestDataText"
            });

            Assert.IsEmpty(rawData.Error);
            Assert.IsNotNull(rawData.Result);
            Assert.IsInstanceOf<CliResponse<object>>(rawData);

            var signRaw = await Raw.SignRawTransactionAsync(tx_hex: $"{rawData.Result}", null, null, null);

            Assert.IsEmpty(signRaw.Error);
            Assert.IsNotNull(signRaw.Result);
            Assert.IsInstanceOf<CliResponse<SignRawTransactionResult>>(signRaw);

            var sendRaw = await Raw.SendRawTransactionAsync(signRaw.Result.Hex, false);

            Assert.IsEmpty(sendRaw.Error);
            Assert.IsNotNull(sendRaw.Result);
            Assert.IsInstanceOf<CliResponse<object>>(sendRaw);
        }
    }
}
