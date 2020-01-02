using MCWrapper.CLI.Ledger.Clients;
using MCWrapper.CLI.Tests.ServiceHelpers;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace MCWrapper.CLI.Tests.MachineShopTests
{
    [TestFixture]
    public class ForgeMachinistTests
    {
        private readonly IMultiChainCliForge Blocksmith;

        public ForgeMachinistTests()
        {
            var provider = new ServiceHelperExplicitSource();

            Blocksmith = provider.GetService<IMultiChainCliForge>();
        }

        [Test]
        public async Task ForgeTests()
        {
            // Stage - Generate a random blockchain name
            var blockchainName = Guid.NewGuid().ToString("N");

            // Act - Create a new blockchain
            var createBlockchain = await Blocksmith.CreateBlockchainAsync(blockchainName);

            // Assert - Verify 'createBlockchain' was successful
            Assert.IsInstanceOf<ForgeResponse>(createBlockchain);
            Assert.True(createBlockchain.Success);
            Assert.IsEmpty(createBlockchain.StandardError);
            Assert.IsNotEmpty(createBlockchain.StandardOutput);

            // Act - Start the new blockchain
            var startBlockchain = await Blocksmith.StartBlockchainAsync(blockchainName);

            // Assert - Verify 'startBlockchain' was successful
            Assert.IsInstanceOf<ForgeResponse>(startBlockchain);
            Assert.True(startBlockchain.Success);
            Assert.IsEmpty(startBlockchain.StandardError);
            Assert.IsNotEmpty(startBlockchain.StandardOutput);

            // Act - Stop the new blockchain
            var stopBlockchain = await Blocksmith.StopBlockchainAsync(blockchainName);

            // Assert - Verify 'stopBlockchain' was successful
            Assert.IsInstanceOf<ForgeResponse>(stopBlockchain);
            Assert.True(stopBlockchain.Success);
            Assert.IsNotEmpty(stopBlockchain.StandardError);
            Assert.IsNotEmpty(stopBlockchain.StandardOutput);

            // Act - Create a new cold node for the new blockchain
            var createColdNode = await Blocksmith.CreateColdNodeAsync(blockchainName);

            // Assert - Verify 'createColdNode' was successful
            Assert.IsTrue(createColdNode);

            // Act - Start the new cold node for the new blockchain
            var startColdNode = await Blocksmith.StartColdNodeAsync(blockchainName);

            // Assert - Verify 'startColdNode' was successful
            Assert.IsInstanceOf<ForgeResponse>(startColdNode);
            Assert.True(startColdNode.Success);
            Assert.IsEmpty(startColdNode.StandardError);
            Assert.IsNotEmpty(startColdNode.StandardOutput);

            // Act - Stop the cold node
            var stopColdNode = await Blocksmith.StopColdNodeAsync(blockchainName);

            // Assert - Verify 'stopColdNode' was successful
            Assert.IsInstanceOf<ForgeResponse>(stopColdNode);
            Assert.True(stopColdNode.Success);
            Assert.IsNotEmpty(stopColdNode.StandardError);
            Assert.IsNotEmpty(stopColdNode.StandardOutput);
        }

        [Test, Ignore("Test is passing, just takes too long during normal development stages/modes")]
        public async Task CreateOneHundredBlockchainsAndColdNodes()
        {
            for (int i = 0; i < 100; i++)
            {
                // Stage - Generate a random blockchain name
                var blockchainName = Guid.NewGuid().ToString("N");

                // Act - Create a new blockchain
                var createBlockchain = await Blocksmith.CreateBlockchainAsync(blockchainName);

                // Assert - Verify 'createBlockchain' was successful
                Assert.IsInstanceOf<ForgeResponse>(createBlockchain);
                Assert.True(createBlockchain.Success);
                Assert.IsEmpty(createBlockchain.StandardError);
                Assert.IsNotEmpty(createBlockchain.StandardOutput);

                // Act - Start the new blockchain
                var startBlockchain = await Blocksmith.StartBlockchainAsync(blockchainName);

                // Assert - Verify 'startBlockchain' was successful
                Assert.IsInstanceOf<ForgeResponse>(startBlockchain);
                Assert.True(startBlockchain.Success);
                Assert.IsEmpty(startBlockchain.StandardError);
                Assert.IsNotEmpty(startBlockchain.StandardOutput);

                // Act - Stop the new blockchain
                var stopBlockchain = await Blocksmith.StopBlockchainAsync(blockchainName);

                // Assert - Verify 'stopBlockchain' was successful
                Assert.IsInstanceOf<ForgeResponse>(stopBlockchain);
                Assert.True(stopBlockchain.Success);
                Assert.IsNotEmpty(stopBlockchain.StandardError);
                Assert.IsNotEmpty(stopBlockchain.StandardOutput);

                // Act - Create a new cold node for the new blockchain
                var createColdNode = await Blocksmith.CreateColdNodeAsync(blockchainName);

                // Assert - Verify 'createColdNode' was successful
                Assert.IsTrue(createColdNode);

                // Act - Start the new cold node for the new blockchain
                var startColdNode = await Blocksmith.StartColdNodeAsync(blockchainName);

                // Assert - Verify 'startColdNode' was successful
                Assert.IsInstanceOf<ForgeResponse>(startColdNode);
                Assert.True(startColdNode.Success);
                Assert.IsEmpty(startColdNode.StandardError);
                Assert.IsNotEmpty(startColdNode.StandardOutput);

                // Act - Stop the cold node
                var stopColdNode = await Blocksmith.StopColdNodeAsync(blockchainName);

                // Assert - Verify 'stopColdNode' was successful
                Assert.IsInstanceOf<ForgeResponse>(stopColdNode);
                Assert.True(stopColdNode.Success);
                Assert.IsNotEmpty(stopColdNode.StandardError);
                Assert.IsNotEmpty(stopColdNode.StandardOutput);
            }
        }
    }
}
