using Domain.Gameplay.Models;

namespace Domain.Gameplay.MessagesDTO
{
    /// <summary>
    /// Запрос на размещение нового здания на игровой сетке.
    /// </summary>
    public readonly struct PlaceBuildingRequest
    {
        /// <summary>
        /// Тип здания, которое необходимо разместить.
        /// </summary>
        public BuildingType BuildingType { get; }

        /// <summary>
        /// Целевая позиция на сетке для размещения.
        /// </summary>
        public GridPos Position { get; }

        /// <summary>
        /// Инициализирует новый экземпляр запроса <see cref="PlaceBuildingRequest"/>.
        /// </summary>
        /// <param name="buildingType">Тип здания.</param>
        /// <param name="position">Позиция на сетке.</param>
        public PlaceBuildingRequest(BuildingType buildingType, GridPos position)
        {
            this.BuildingType = buildingType;
            this.Position = position;
        }
    }
}