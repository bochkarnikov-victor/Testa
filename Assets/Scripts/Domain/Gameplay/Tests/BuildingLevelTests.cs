
using NUnit.Framework;
using System.Collections.Generic;

namespace Domain.Gameplay.Models.Tests
{
    [TestFixture]
    public class BuildingLevelTests
    {
        [Test]
        public void Constructor_AssignsPropertiesCorrectly()
        {
            // Arrange
            var cost = new Cost(new Dictionary<ResourceType, int> { { ResourceType.Gold, 100 } });
            var income = new Income(new Dictionary<ResourceType, int> { { ResourceType.Gold, 10 } });
            var level = 1;

            // Act
            var buildingLevel = new BuildingLevel(level, cost, income);

            // Assert
            Assert.AreEqual(level, buildingLevel.Level);
            Assert.AreEqual(cost, buildingLevel.Cost);
            Assert.AreEqual(income, buildingLevel.Income);
        }
    }
}
