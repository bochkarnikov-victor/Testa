#nullable enable

using System;

namespace Domain.Gameplay.MessagesDTO
{
    /// <summary>
    /// Событие, анонсирующее, что здание было успешно улучшено до следующего уровня.
    /// </summary>
    public readonly struct BuildingUpgradedEvent
    {
        /// <summary>
        /// Уникальный идентификатор улучшенного здания.
        /// </summary>
        public Guid BuildingId { get; }

        /// <summary>
        /// Новый уровень здания после улучшения.
        /// </summary>
        public int NewLevel { get; }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="BuildingUpgradedEvent"/>.
        /// </summary>
        /// <param name="buildingId">ID улучшенного здания.</param>
        /// <param name="newLevel">Новый уровень здания.</param>
        public BuildingUpgradedEvent(Guid buildingId, int newLevel)
        {
            this.BuildingId = buildingId;
            this.NewLevel = newLevel;
        }
    }
}