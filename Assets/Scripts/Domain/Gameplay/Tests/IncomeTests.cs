using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Domain.Gameplay.Models.Tests
{
    [TestFixture]
    public class IncomeTests
    {
        [Test]
        public void Constructor_WithNegativeValue_ThrowsArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Income(ResourceType.Gold, -100));
        }

        [Test]
        public void GetIncome_ForExistingResource_ReturnsCorrectValue()
        {
            // Arrange
            Income income = new(ResourceType.Gold, 100);

            // Act
            int value = income.GetIncome(ResourceType.Gold);

            // Assert
            Assert.AreEqual(100, value);
        }

        [Test]
        public void GetIncome_ForNonExistentResource_ReturnsZero()
        {
            // Arrange
            Income income = new(ResourceType.Gold, 100);

            // Act
            int value = income.GetIncome(ResourceType.Wood);

            // Assert
            Assert.AreEqual(0, value);
        }

        [Test]
        public void AdditionOperator_WhenCombiningTwoIncomes_CorrectlySumsValues()
        {
            // Arrange
            Income income1 = new(new Dictionary<ResourceType, int>
            {
                { ResourceType.Gold, 100 },
                { ResourceType.Wood, 50 }
            });
            Income income2 = new(new Dictionary<ResourceType, int>
            {
                { ResourceType.Wood, 25 }
            });

            // Act
            Income totalIncome = income1 + income2;

            // Assert
            Assert.AreEqual(100, totalIncome.GetIncome(ResourceType.Gold));
            Assert.AreEqual(75, totalIncome.GetIncome(ResourceType.Wood));
        }

        [Test]
        public void Equals_WithIdenticalIncomes_ReturnsTrue()
        {
            // Arrange
            Income income1 = new(new Dictionary<ResourceType, int>
            {
                { ResourceType.Gold, 100 },
                { ResourceType.Wood, 50 }
            });
            Income income2 = new(new Dictionary<ResourceType, int>
            {
                { ResourceType.Gold, 100 },
                { ResourceType.Wood, 50 }
            });

            // Assert
            Assert.IsTrue(income1.Equals(income2));
        }

        [Test]
        public void Equals_WithDifferentIncomes_ReturnsFalse()
        {
            // Arrange
            Income income1 = new(ResourceType.Gold, 100);
            Income income2 = new(ResourceType.Gold, 200);

            // Assert
            Assert.IsFalse(income1.Equals(income2));
        }

        [Test]
        public void Zero_IsCorrectlyDefined()
        {
            // Act
            Income zeroIncome = Income.Zero;

            // Assert
            Assert.AreEqual(0, zeroIncome.GetIncome(ResourceType.Gold));
            Assert.AreEqual(0, zeroIncome.GetIncome(ResourceType.Wood));
        }
    }
}