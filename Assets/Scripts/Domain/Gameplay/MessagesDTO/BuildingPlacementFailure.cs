#nullable enable

namespace Domain.Gameplay.MessagesDTO
{
    /// <summary>
    /// Перечисление причин, по которым не удалось разместить здание.
    /// </summary>
    public enum PlacementFailureReason
    {
        /// <summary>
        /// Неизвестная или необработанная ошибка.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Указанная клетка уже занята другим зданием.
        /// </summary>
        CellIsOccupied = 1,

        /// <summary>
        /// У игрока недостаточно ресурсов для постройки.
        /// </summary>
        NotEnoughResources = 2,

        /// <summary>
        /// Конфигурация для данного типа здания не найдена в репозиториях.
        /// </summary>
        InvalidBuildingType = 3,

        /// <summary>
        /// Конфигурация для здания некорректна (например, отсутствуют уровни).
        /// </summary>
        InvalidConfig = 4
    }

    /// <summary>
    /// DTO-сообщение, анонсирующее неудачную попытку размещения здания.
    /// </summary>
    public readonly struct BuildingPlacementFailure
    {
        /// <summary>
        /// Причина, по которой не удалось разместить здание.
        /// </summary>
        public PlacementFailureReason Reason { get; }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="BuildingPlacementFailure"/>.
        /// </summary>
        /// <param name="reason">Причина сбоя.</param>
        public BuildingPlacementFailure(PlacementFailureReason reason)
        {
            this.Reason = reason;
        }
    }
}