using System;

namespace Domain.Gameplay.MessagesDTO
{
    /// <summary>
    /// Запрос на улучшение здания до следующего уровня.
    /// </summary>
    public readonly struct UpgradeBuildingRequest
    {
        /// <summary>
        /// Уникальный идентификатор здания для улучшения.
        /// </summary>
        public Guid BuildingId { get; }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="UpgradeBuildingRequest"/>.
        /// </summary>
        /// <param name="buildingId">ID здания для улучшения.</param>
        public UpgradeBuildingRequest(Guid buildingId)
        {
            this.BuildingId = buildingId;
        }
    }
}