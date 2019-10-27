using MCWrapper.CLI.Ledger.Forge;
using MCWrapper.CLI.Tests.ServiceHelpers;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace MCWrapper.CLI.Tests.MachineShopTests
{
    [TestFixture]
    public class ForgeMachinistTests
    {
        private readonly ForgeClient Blacksmith;

        public ForgeMachinistTests()
        {
            var provider = new ServiceHelperParameterlessConstructor();

            Blacksmith = provider.GetService<ForgeClient>();
        }

        [Test]
        public async Task CreateBlockchainTest()
        {
            var createBlockchain = await Blacksmith.CreateBlockchainAsync(Guid.NewGuid().ToString("N"));

            Assert.IsInstanceOf<ForgeClient>(createBlockchain);
        }
    }
}
