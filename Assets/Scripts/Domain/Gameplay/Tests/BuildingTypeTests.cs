
using NUnit.Framework;

namespace Domain.Gameplay.Models.Tests
{
    [TestFixture]
    public class BuildingTypeTests
    {
        [Test]
        public void EnumValues_AreCorrect()
        {
            // Assert
            Assert.AreEqual(0, (int)BuildingType.None);
            Assert.AreEqual(1, (int)BuildingType.House);
            Assert.AreEqual(2, (int)BuildingType.Farm);
            Assert.AreEqual(3, (int)BuildingType.Mine);
        }
    }
}
