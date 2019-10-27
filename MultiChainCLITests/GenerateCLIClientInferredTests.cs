﻿using MCWrapper.CLI.Connection;
using MCWrapper.CLI.Ledger.Clients;
using MCWrapper.CLI.Tests.ServiceHelpers;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MCWrapper.CLI.Tests.MultiChainCLITests
{
    [TestFixture]
    public class GenerateCLIClientInferredTests
    {
        // multichain-cli.exe client supports the 'generate' based methods
        private readonly GenerateCliClient Generate;

        public GenerateCLIClientInferredTests()
        {
            var provider = new ServiceHelperParameterlessConstructor();

            Generate = provider.GetService<GenerateCliClient>();
        }

        [Test]
        public async Task GetGenerateAsyncTest()
        {
            // Act - fetch boolean indicator toward coin generation on the local node
            // ! True => Coin (native currency) generation is occurring
            // ! False => Coin (native currency) generation is not occurring
            var getGenerate = await Generate.GetGeneratedAsync();

            // Assert
            Assert.IsEmpty(getGenerate.Error);
            Assert.IsInstanceOf<bool>(getGenerate.Result);
            Assert.IsInstanceOf<CliResponse<bool>>(getGenerate);
            Assert.IsInstanceOf<CLIClientRequestObject>(getGenerate.Request);
        }

        [Test]
        public async Task GetHashesPerSecAsyncTest()
        {
            // Act - fetch hashes per second value on the local node
            var hashes = await Generate.GetHashesPerSecAsync();

            // Assert
            Assert.IsEmpty(hashes.Error);
            Assert.IsInstanceOf<int>(hashes.Result);
            Assert.IsInstanceOf<CliResponse<int>>(hashes);
            Assert.IsInstanceOf<CLIClientRequestObject>(hashes.Request);
        }

        [Test]
        public async Task SetGenerateAsyncTest()
        {
            // Stage - Set max number of processors used for mining and coin generation
            var maxProcessors = 4;

            // Act - Set coin generation on the local network
            // ! True => Coin (native currency) generation should be occurring
            // ! False => Coin (native currency) generation should not be occurring
            var setGenerate = await Generate.SetGenerateAsync(true, maxProcessors);

            // Assert
            Assert.IsEmpty(setGenerate.Error);
            Assert.IsNull(setGenerate.Result);
            Assert.IsInstanceOf<CliResponse<object>>(setGenerate);
            Assert.IsInstanceOf<CLIClientRequestObject>(setGenerate.Request);
        }
    }
}
