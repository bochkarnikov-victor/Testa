#nullable enable

using System;

namespace Domain.Gameplay.MessagesDTO
{
    /// <summary>
    /// Событие, анонсирующее, что здание было успешно удалено с игровой сетки.
    /// </summary>
    public readonly struct BuildingRemovedEvent
    {
        /// <summary>
        /// Уникальный идентификатор удаленного здания.
        /// </summary>
        public Guid BuildingId { get; }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="BuildingRemovedEvent"/>.
        /// </summary>
        /// <param name="buildingId">ID удаленного здания.</param>
        public BuildingRemovedEvent(Guid buildingId)
        {
            this.BuildingId = buildingId;
        }
    }
}