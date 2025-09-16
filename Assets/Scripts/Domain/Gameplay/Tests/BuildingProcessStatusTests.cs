
using NUnit.Framework;

namespace Domain.Gameplay.Models.Tests
{
    [TestFixture]
    public class BuildingProcessStatusTests
    {
        [Test]
        public void EnumValues_AreCorrect()
        {
            // Assert
            Assert.AreEqual(0, (int)BuildingProcessStatus.None);
            Assert.AreEqual(1, (int)BuildingProcessStatus.Constructing);
            Assert.AreEqual(2, (int)BuildingProcessStatus.Constructed);
        }
    }
}
