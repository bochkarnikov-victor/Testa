#nullable enable

using System;
using Domain.Gameplay.Models;

namespace Domain.Gameplay.MessagesDTO
{
    /// <summary>
    /// Событие, анонсирующее, что здание было успешно перемещено в новую позицию.
    /// </summary>
    public readonly struct BuildingMovedEvent
    {
        /// <summary>
        /// Уникальный идентификатор перемещенного здания.
        /// </summary>
        public Guid BuildingId { get; }

        /// <summary>
        /// Новая позиция здания после перемещения.
        /// </summary>
        public GridPos NewPosition { get; }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="BuildingMovedEvent"/>.
        /// </summary>
        /// <param name="buildingId">ID перемещенного здания.</param>
        /// <param name="newPosition">Новая позиция здания.</param>
        public BuildingMovedEvent(Guid buildingId, GridPos newPosition)
        {
            this.BuildingId = buildingId;
            this.NewPosition = newPosition;
        }
    }
}