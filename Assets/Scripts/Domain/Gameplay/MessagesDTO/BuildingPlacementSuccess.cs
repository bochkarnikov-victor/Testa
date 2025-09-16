#nullable enable

using System;
using Domain.Gameplay.Models;

namespace Domain.Gameplay.MessagesDTO
{
    /// <summary>
    /// DTO-сообщение, анонсирующее успешное размещение нового здания.
    /// </summary>
    public readonly struct BuildingPlacementSuccess
    {
        /// <summary>
        /// Уникальный ID нового здания.
        /// </summary>
        public Guid BuildingId { get; }

        /// <summary>
        /// Тип построенного здания.
        /// </summary>
        public BuildingType BuildingType { get; }

        /// <summary>
        /// Позиция, на которой было размещено здание.
        /// </summary>
        public GridPos Position { get; }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="BuildingPlacementSuccess"/>.
        /// </summary>
        /// <param name="buildingId">ID нового здания.</param>
        /// <param name="buildingType">Тип здания.</param>
        /// <param name="position">Позиция здания.</param>
        public BuildingPlacementSuccess(Guid buildingId, BuildingType buildingType, GridPos position)
        {
            this.BuildingId = buildingId;
            this.BuildingType = buildingType;
            this.Position = position;
        }
    }
}