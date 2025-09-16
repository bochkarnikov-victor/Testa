using System;
using Domain.Gameplay.Models;

namespace Domain.Gameplay.MessagesDTO
{
    /// <summary>
    /// Запрос на перемещение существующего здания в новую позицию на игровой сетке.
    /// </summary>
    public readonly struct MoveBuildingRequest
    {
        /// <summary>
        /// Уникальный идентификатор здания для перемещения.
        /// </summary>
        public Guid BuildingId { get; }

        /// <summary>
        /// Новая целевая позиция для здания.
        /// </summary>
        public GridPos NewPosition { get; }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="MoveBuildingRequest"/>.
        /// </summary>
        /// <param name="buildingId">ID здания для перемещения.</param>
        /// <param name="newPosition">Новая позиция.</param>
        public MoveBuildingRequest(Guid buildingId, GridPos newPosition)
        {
            this.BuildingId = buildingId;
            this.NewPosition = newPosition;
        }
    }
}