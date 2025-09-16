#nullable enable

using NUnit.Framework;
using System;
using System.Collections.Generic;
using R3;

namespace Domain.Gameplay.Models.Tests
{
    [TestFixture]
    public class BuildingModelTests
    {
        private List<BuildingLevel> _defaultLevels = null!;

        [SetUp]
        public void SetUp()
        {
            Cost level1Cost = new(new Dictionary<ResourceType, int> { { ResourceType.Gold, 100 } });
            Income level1Income = new(new Dictionary<ResourceType, int> { { ResourceType.Gold, 10 } });
            Cost level2Cost = new(new Dictionary<ResourceType, int> { { ResourceType.Gold, 200 } });
            Income level2Income = new(new Dictionary<ResourceType, int> { { ResourceType.Gold, 20 } });

            this._defaultLevels = new List<BuildingLevel>
            {
                new(1, level1Cost, level1Income),
                new(2, level2Cost, level2Income)
            };
        }

        [Test]
        public void Constructor_WhenCalled_InitializesPropertiesCorrectly()
        {
            Guid id = Guid.NewGuid();
            BuildingType type = BuildingType.House;
            GridPos position = new(1, 1);

            BuildingModel model = new(id, type, position, this._defaultLevels);

            Assert.AreEqual(id, model.Id);
            Assert.AreEqual(type, model.Type);
            Assert.AreEqual(position, model.Position.CurrentValue);
            Assert.AreEqual(1, model.CurrentLevel.CurrentValue, "Building should start at level 1.");
            Assert.AreEqual(this._defaultLevels, model.Levels);
        }

        [Test]
        public void TryUpgrade_WhenNotAtMaxLevel_ReturnsTrueAndIncrementsLevel()
        {
            BuildingModel model = new(Guid.NewGuid(), BuildingType.House, new GridPos(0, 0), this._defaultLevels);
            Assert.AreEqual(1, model.CurrentLevel.CurrentValue, "Pre-condition: Level should be 1.");

            bool result = model.TryUpgrade();

            Assert.IsTrue(result, "TryUpgrade should return true.");
            Assert.AreEqual(2, model.CurrentLevel.CurrentValue, "Level should be incremented to 2.");
        }

        [Test]
        public void TryUpgrade_WhenAtMaxLevel_ReturnsFalseAndKeepsLevel()
        {
            BuildingModel model = new(Guid.NewGuid(), BuildingType.House, new GridPos(0, 0), this._defaultLevels);
            model.TryUpgrade(); // Улучшаем до уровня 2
            Assert.AreEqual(2, model.CurrentLevel.CurrentValue, "Pre-condition: Level should be 2.");

            bool result = model.TryUpgrade();

            Assert.IsFalse(result, "TryUpgrade should return false at max level.");
            Assert.AreEqual(2, model.CurrentLevel.CurrentValue, "Level should remain at max level.");
        }

        [Test]
        public void TryUpgrade_WhenSuccessful_NotifiesSubscribers()
        {
            BuildingModel model = new(Guid.NewGuid(), BuildingType.House, new GridPos(0, 0), this._defaultLevels);
            int receivedLevel = -1;
            using IDisposable subscription = model.CurrentLevel.Subscribe(level => receivedLevel = level);
            Assert.AreEqual(1, receivedLevel, "Pre-condition: Subscriber should receive initial level 1.");

            model.TryUpgrade();

            Assert.AreEqual(2, receivedLevel, "Subscriber should be notified of the new level 2.");
        }

        [Test]
        public void SetPosition_WhenCalled_UpdatesPositionReactiveProperty()
        {
            BuildingModel model = new(Guid.NewGuid(), BuildingType.Farm, new GridPos(5, 5), this._defaultLevels);
            GridPos newPosition = new(10, 10);

            model.SetPosition(newPosition);

            Assert.AreEqual(newPosition, model.Position.CurrentValue);
        }
    }
}