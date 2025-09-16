using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Gameplay.Models.Tests
{
    [TestFixture]
    public class CityGridModelTests
    {
        private CityGridModel _gridModel;

        [SetUp]
        public void SetUp()
        {
            this._gridModel = new CityGridModel(32, 32);
        }

        private BuildingModel CreateBuilding(GridPos position)
        {
            return new BuildingModel(Guid.NewGuid(), BuildingType.House, position, new List<BuildingLevel>());
        }

        [Test]
        public void TryAddBuilding_WithValidBuilding_ReturnsTrueAndAddsBuilding()
        {
            // Arrange
            BuildingModel building = this.CreateBuilding(new GridPos(1, 1));

            // Act
            bool result = this._gridModel.TryAddBuilding(building);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, this._gridModel.Buildings.Count);
            Assert.IsTrue(this._gridModel.IsCellOccupied(new GridPos(1, 1)));
            Assert.AreEqual(building, this._gridModel.GetBuildingAt(new GridPos(1, 1)));
            Assert.AreEqual(building, this._gridModel.GetBuildingById(building.Id));
        }

        [Test]
        public void TryAddBuilding_ToOccupiedCell_ReturnsFalse()
        {
            // Arrange
            BuildingModel building1 = this.CreateBuilding(new GridPos(1, 1));
            BuildingModel building2 = this.CreateBuilding(new GridPos(1, 1));
            this._gridModel.TryAddBuilding(building1);

            // Act
            bool result = this._gridModel.TryAddBuilding(building2);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(1, this._gridModel.Buildings.Count);
        }

        [Test]
        public void TryAddBuilding_WithExistingId_ReturnsFalse()
        {
            // Arrange
            BuildingModel building1 = this.CreateBuilding(new GridPos(1, 1));
            this._gridModel.TryAddBuilding(building1);
            BuildingModel building2 = new(building1.Id, BuildingType.Farm, new GridPos(2, 2),
                new List<BuildingLevel>());

            // Act
            bool result = this._gridModel.TryAddBuilding(building2);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(1, this._gridModel.Buildings.Count);
        }

        [Test]
        public void TryRemoveBuilding_WithExistingBuilding_ReturnsTrueAndRemovesBuilding()
        {
            // Arrange
            BuildingModel building = this.CreateBuilding(new GridPos(1, 1));
            this._gridModel.TryAddBuilding(building);

            // Act
            bool result = this._gridModel.TryRemoveBuilding(building);

            // Assert
            Assert.IsTrue(result);
            Assert.IsEmpty(this._gridModel.Buildings);
            Assert.IsFalse(this._gridModel.IsCellOccupied(new GridPos(1, 1)));
        }

        [Test]
        public void TryMoveBuilding_ToEmptyCell_ReturnsTrueAndMovesBuilding()
        {
            // Arrange
            BuildingModel building = this.CreateBuilding(new GridPos(1, 1));
            this._gridModel.TryAddBuilding(building);
            GridPos newPosition = new(2, 2);

            // Act
            bool result = this._gridModel.TryMoveBuilding(building, newPosition);

            // Assert
            Assert.IsTrue(result);
            Assert.IsFalse(this._gridModel.IsCellOccupied(new GridPos(1, 1)));
            Assert.IsTrue(this._gridModel.IsCellOccupied(newPosition));
            Assert.AreEqual(building, this._gridModel.GetBuildingAt(newPosition));
            Assert.AreEqual(newPosition, building.Position.CurrentValue);
        }

        [Test]
        public void TryMoveBuilding_ToOccupiedCell_ReturnsFalse()
        {
            // Arrange
            BuildingModel building1 = this.CreateBuilding(new GridPos(1, 1));
            BuildingModel building2 = this.CreateBuilding(new GridPos(2, 2));
            this._gridModel.TryAddBuilding(building1);
            this._gridModel.TryAddBuilding(building2);

            // Act
            bool result = this._gridModel.TryMoveBuilding(building1, new GridPos(2, 2));

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(new GridPos(1, 1), building1.Position.CurrentValue); // Position should not have changed
        }
    }
}