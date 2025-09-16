using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Domain.Gameplay.Models.Tests
{
    [TestFixture]
    public class CostTests
    {
        [Test]
        public void Constructor_WithNegativeValue_ThrowsArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Cost(ResourceType.Gold, -100));
        }

        [Test]
        public void GetCost_ForExistingResource_ReturnsCorrectValue()
        {
            // Arrange
            Cost cost = new(ResourceType.Gold, 100);

            // Act
            int value = cost.GetCost(ResourceType.Gold);

            // Assert
            Assert.AreEqual(100, value);
        }

        [Test]
        public void GetCost_ForNonExistentResource_ReturnsZero()
        {
            // Arrange
            Cost cost = new(ResourceType.Gold, 100);

            // Act
            int value = cost.GetCost(ResourceType.Wood);

            // Assert
            Assert.AreEqual(0, value);
        }

        [Test]
        public void AdditionOperator_WhenCombiningTwoCosts_CorrectlySumsValues()
        {
            // Arrange
            Cost cost1 = new(new Dictionary<ResourceType, int>
            {
                { ResourceType.Gold, 100 },
                { ResourceType.Wood, 50 }
            });
            Cost cost2 = new(new Dictionary<ResourceType, int>
            {
                { ResourceType.Wood, 25 }
            });

            // Act
            Cost totalCost = cost1 + cost2;

            // Assert
            Assert.AreEqual(100, totalCost.GetCost(ResourceType.Gold));
            Assert.AreEqual(75, totalCost.GetCost(ResourceType.Wood));
        }

        [Test]
        public void Equals_WithIdenticalCosts_ReturnsTrue()
        {
            // Arrange
            Cost cost1 = new(new Dictionary<ResourceType, int>
            {
                { ResourceType.Gold, 100 },
                { ResourceType.Wood, 50 }
            });
            Cost cost2 = new(new Dictionary<ResourceType, int>
            {
                { ResourceType.Gold, 100 },
                { ResourceType.Wood, 50 }
            });

            // Assert
            Assert.IsTrue(cost1.Equals(cost2));
        }

        [Test]
        public void Equals_WithDifferentCosts_ReturnsFalse()
        {
            // Arrange
            Cost cost1 = new(ResourceType.Gold, 100);
            Cost cost2 = new(ResourceType.Gold, 200);

            // Assert
            Assert.IsFalse(cost1.Equals(cost2));
        }

        [Test]
        public void Zero_IsCorrectlyDefined()
        {
            // Act
            Cost zeroCost = Cost.Zero;

            // Assert
            Assert.AreEqual(0, zeroCost.GetCost(ResourceType.Gold));
            Assert.AreEqual(0, zeroCost.GetCost(ResourceType.Wood));
        }
    }
}