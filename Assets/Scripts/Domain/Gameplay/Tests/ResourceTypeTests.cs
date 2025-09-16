
using NUnit.Framework;

namespace Domain.Gameplay.Models.Tests
{
    [TestFixture]
    public class ResourceTypeTests
    {
        [Test]
        public void EnumValues_AreCorrect()
        {
            // Assert
            Assert.AreEqual(0, (int)ResourceType.None);
            Assert.AreEqual(1, (int)ResourceType.Gold);
            Assert.AreEqual(2, (int)ResourceType.Wood);
            Assert.AreEqual(3, (int)ResourceType.Crystals);
        }
    }
}
