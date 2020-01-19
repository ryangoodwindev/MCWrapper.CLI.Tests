using MCWrapper.CLI.Connection;
using MCWrapper.CLI.Ledger.Clients;
using MCWrapper.CLI.Tests.ServiceHelpers;
using MCWrapper.Data.Models.Utility;
using MCWrapper.Ledger.Entities.Extensions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MCWrapper.CLI.Tests.MultiChainCLITests
{
    [TestFixture]
    public class UtilityCLIClientExplicitTests
    {
        // multichain-cli.exe client supports the 'utility' based methods
        private readonly IMultiChainCliWallet Wallet;
        private readonly IMultiChainCliUtility Utility;

        public UtilityCLIClientExplicitTests()
        {
            var provider = new ServiceHelperExplicitSource();

            Wallet = provider.GetService<IMultiChainCliWallet>();
            Utility = provider.GetService<IMultiChainCliUtility>();
        }

        [Test]
        public async Task CreateAppendToAndDeleteBinaryCacheAsyncTest()
        {
            // Act - Create a new binary cache, append some Hex data to the new cache, then delete the binary cache
            // returns new binary cache identifier
            var cache = await Utility.CreateBinaryCacheAsync(Utility.CliOptions.ChainName);

            // Assert
            Assert.IsEmpty(cache.Error);
            Assert.IsInstanceOf<CliResponse<string>>(cache);

            // Act - Append some string data converted to hex
            // Discard
            _ = await Utility.AppendBinaryCacheAsync(Utility.CliOptions.ChainName, cache.Result, "Some data to append to the binary cache".ToHex());

            // Act - Append an object that is serialized to JSON and then converted to a hex string representation
            // returns byte size int
            var append = await Utility.AppendBinaryCacheAsync(Utility.CliOptions.ChainName, cache.Result, "Some data to append to the binary cache".ToHex());

            // Assert
            Assert.IsEmpty(append.Error);
            Assert.IsInstanceOf<CliResponse<int>>(append);

            // Act - Delete the new binary cache
            var delete = await Utility.DeleteBinaryCacheAsync(Utility.CliOptions.ChainName, cache.Result);

            // Assert
            Assert.IsEmpty(delete.Error);
            Assert.IsInstanceOf<CliResponse>(delete);
        }

        [Test]
        public async Task CreateKeyPairsAsyncTest()
        {
            // Act - fetch 4 key pairs
            var keyPairs = await Utility.CreateKeyPairsAsync(Utility.CliOptions.ChainName, 4);

            // Assert
            Assert.IsEmpty(keyPairs.Error);
            Assert.IsInstanceOf<CliResponse<IList<CreateKeyPairsResult>>>(keyPairs);
        }

        [Test]
        public async Task CreateMultiSigAsyncTest()
        {
            // Act - fetch a new multi sig address
            var multiSig = await Utility.CreateMultiSigAsync(Utility.CliOptions.ChainName, 1, new[] { Utility.CliOptions.ChainAdminAddress });

            // Assert
            Assert.IsEmpty(multiSig.Error);
            Assert.IsInstanceOf<CliResponse<CreateMultiSigResult>>(multiSig);
        }

        [Test]
        public async Task EstimateFeeAsyncTest()
        {
            // Act - fetch fee
            var fee = await Utility.EstimateFeeAsync(Utility.CliOptions.ChainName, 60);

            // Assert
            Assert.IsEmpty(fee.Error);
            Assert.IsInstanceOf<CliResponse<long>>(fee);
        }

        [Test]
        public async Task EstimatePriorityAsyncTest()
        {
            // Act - fetch priority
            var priority = await Utility.EstimatePriorityAsync(Utility.CliOptions.ChainName, 60);

            // Assert
            Assert.IsEmpty(priority.Error);
            Assert.IsInstanceOf<CliResponse<float>>(priority);
        }

        [Test]
        public async Task ValidateAddressAsyncTest()
        {
            // Act - validate blockchain address
            var validate = await Utility.ValidateAddressAsync(Utility.CliOptions.ChainName, Utility.CliOptions.ChainAdminAddress);

            // Assert
            Assert.IsEmpty(validate.Error);
            Assert.IsInstanceOf<CliResponse<ValidateAddressResult>>(validate);
        }

        [Test]
        public async Task VerifyMessageAsyncTest()
        {
            // Stage - sign blockchain message
            var message = "Test for signing a message on the blockchain network";
            var signature = await Wallet.SignMessageAsync(Wallet.CliOptions.ChainName, Wallet.CliOptions.ChainAdminAddress, message);

            // Act - verify blockchain message
            var verifyMessage = await Utility.VerifyMessageAsync(Utility.CliOptions.ChainName, Utility.CliOptions.ChainAdminAddress, signature.Result, message);

            // Assert
            Assert.IsEmpty(verifyMessage.Error);
            Assert.IsInstanceOf<CliResponse<bool>>(verifyMessage);
        }
    }
}
