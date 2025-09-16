#nullable enable

using System;

namespace Domain.Gameplay.MessagesDTO
{
    /// <summary>
    /// Событие, анонсирующее, что пользователь выбрал здание на игровой сетке (или снял выбор).
    /// </summary>
    public readonly struct BuildingSelectedEvent
    {
        /// <summary>
        /// Уникальный идентификатор выбранного здания.
        /// Может быть null, если выбор был снят (например, клик по пустой клетке).
        /// </summary>
        public Guid? BuildingId { get; }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="BuildingSelectedEvent"/>.
        /// </summary>
        /// <param name="buildingId">ID выбранного здания или null для снятия выбора.</param>
        public BuildingSelectedEvent(Guid? buildingId)
        {
            this.BuildingId = buildingId;
        }
    }
}