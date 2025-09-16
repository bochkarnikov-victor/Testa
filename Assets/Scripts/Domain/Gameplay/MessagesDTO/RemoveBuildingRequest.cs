using System;

namespace Domain.Gameplay.MessagesDTO
{
    /// <summary>
    /// Запрос на удаление здания с игровой сетки.
    /// </summary>
    public readonly struct RemoveBuildingRequest
    {
        /// <summary>
        /// Уникальный идентификатор здания, которое нужно удалить.
        /// </summary>
        public Guid BuildingId { get; }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="RemoveBuildingRequest"/>.
        /// </summary>
        /// <param name="buildingId">ID здания для удаления.</param>
        public RemoveBuildingRequest(Guid buildingId)
        {
            this.BuildingId = buildingId;
        }
    }
}