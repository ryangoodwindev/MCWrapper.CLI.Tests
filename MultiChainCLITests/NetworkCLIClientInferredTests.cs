using MCWrapper.CLI.Connection;
using MCWrapper.CLI.Ledger.Clients;
using MCWrapper.CLI.Tests.ServiceHelpers;
using MCWrapper.Data.Models.Network;
using MCWrapper.Ledger.Entities.Constants;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MCWrapper.CLI.Tests.MultiChainCLITests
{
    [TestFixture]
    public class NetworkCLIClientInferredTests
    {
        // multichain-cli.exe client supports the 'offchain' based methods
        private readonly IMultiChainCliNetwork Network;

        public NetworkCLIClientInferredTests()
        {
            var provider = new ServiceHelperExplicitSource();

            Network = provider.GetService<IMultiChainCliNetwork>();
        }

        [Test, Ignore("AddNode test is ignored since I don't care about peers right now")]
        public async Task AddNodeTestAsync()
        {
            // Act - Add a peer
            var actual = await Network.AddNodeAsync(
                node: "192.168.0.90:3333",
                action: PeerConnection.Add);

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<object>>(actual);
        }

        [Test, Ignore("GetAddNodeInfo test is ignored since I don't care about peers right now")]
        public async Task GetAddNodeInfoTestAsync()
        {
            // Act - Informatinon about added nodes
            var actual = await Network.GetAddedNodeInfoAsync(
                dns: true,
                node: "192.168.0.90:3333");

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetAddNodeInfoResult[]>>(actual);
        }

        [Test]
        public async Task GetChunkQueueInfoTestAsync()
        {
            // Act - Fetch chunk queue information
            var actual = await Network.GetChunkQueueInfoAsync();

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetChunkQueueInfoResult>>(actual);
        }

        [Test]
        public async Task GetChunkQueueTotalsTestAsync()
        {
            // Act - Chunks delivery status
            var actual = await Network.GetChunkQueueTotalsAsync();

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetChunkQueueInfoTotalsResult>>(actual);
        }

        [Test]
        public async Task GetConnectionCountTestAsync()
        {
            // Act - Get number of connection to network
            var actual = await Network.GetConnectionCountAsync();

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<int>>(actual);
        }

        [Test]
        public async Task GetNetTotalsTestAsync()
        {
            // Act - Information about network traffic
            var actual = await Network.GetNetTotalsAsync();

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetNetTotalsResult>>(actual);
        }

        [Test]
        public async Task GetNetworkInfoTestAsync()
        {
            // Act - Request information about the network
            var actual = await Network.GetNetworkInfoAsync();

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<GetNetworkInfoResult>>(actual);
        }

        [Test]
        public async Task GetPeerInfoTestAsync()
        {
            // Act - Request information about any connected peers
            var actual = await Network.GetPeerInfoAsync();

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOf<CliResponse<IList<GetPeerInfoResult>>>(actual);
        }

        [Test]
        public async Task PingTestAsync()
        {
            // Act - Ping connect peers
            var actual = await Network.PingAsync();

            // Assert
            Assert.IsEmpty(actual.Error);
            Assert.IsInstanceOf<object>(actual.Result);
            Assert.IsInstanceOf<CliResponse>(actual);
        }
    }
}
