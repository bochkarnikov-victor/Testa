
using NUnit.Framework;

namespace Domain.Gameplay.Models.Tests
{
    [TestFixture]
    public class GridPosTests
    {
        [Test]
        public void Constructor_AssignsCoordinatesCorrectly()
        {
            // Arrange & Act
            var pos = new GridPos(10, 20);

            // Assert
            Assert.AreEqual(10, pos.X);
            Assert.AreEqual(20, pos.Y);
        }

        [Test]
        public void Equals_WithIdenticalPositions_ReturnsTrue()
        {
            // Arrange
            var pos1 = new GridPos(5, 15);
            var pos2 = new GridPos(5, 15);

            // Assert
            Assert.IsTrue(pos1.Equals(pos2));
        }

        [Test]
        public void Equals_WithDifferentPositions_ReturnsFalse()
        {
            // Arrange
            var pos1 = new GridPos(5, 15);
            var pos2 = new GridPos(15, 5);

            // Assert
            Assert.IsFalse(pos1.Equals(pos2));
        }

        [Test]
        public void EqualityOperator_WithIdenticalPositions_ReturnsTrue()
        {
            // Arrange
            var pos1 = new GridPos(1, 2);
            var pos2 = new GridPos(1, 2);

            // Assert
            Assert.IsTrue(pos1 == pos2);
        }

        [Test]
        public void InequalityOperator_WithDifferentPositions_ReturnsTrue()
        {
            // Arrange
            var pos1 = new GridPos(1, 2);
            var pos2 = new GridPos(2, 1);

            // Assert
            Assert.IsTrue(pos1 != pos2);
        }

        [Test]
        public void GetHashCode_ForIdenticalPositions_AreEqual()
        {
            // Arrange
            var pos1 = new GridPos(100, 200);
            var pos2 = new GridPos(100, 200);

            // Assert
            Assert.AreEqual(pos1.GetHashCode(), pos2.GetHashCode());
        }

        [Test]
        public void ToString_ReturnsCorrectFormat()
        {
            // Arrange
            var pos = new GridPos(7, 42);

            // Act
            var str = pos.ToString();

            // Assert
            Assert.AreEqual("(7, 42)", str);
        }
    }
}
