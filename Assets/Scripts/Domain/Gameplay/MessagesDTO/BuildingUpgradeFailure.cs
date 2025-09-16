using System;

namespace Domain.Gameplay.MessagesDTO
{
    /// <summary>
    /// Перечисление причин, по которым не удалось улучшить здание.
    /// </summary>
    public enum UpgradeFailureReason
    {
        /// <summary>
        /// Неизвестная ошибка.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Здание с указанным ID не было найдено в модели.
        /// </summary>
        BuildingNotFound = 1,

        /// <summary>
        /// Здание уже достигло максимального уровня.
        /// </summary>
        AlreadyAtMaxLevel = 2,

        /// <summary>
        /// У игрока недостаточно ресурсов для улучшения.
        /// </summary>
        NotEnoughResources = 3,
    }

    /// <summary>
    /// DTO-сообщение, анонсирующее неудачную попытку улучшения здания.
    /// </summary>
    public readonly struct BuildingUpgradeFailure
    {
        /// <summary>
        /// ID здания, которое не удалось улучшить.
        /// </summary>
        public Guid BuildingId { get; }

        /// <summary>
        /// Причина, по которой не удалось улучшить здание.
        /// </summary>
        public UpgradeFailureReason Reason { get; }

        public BuildingUpgradeFailure(Guid buildingId, UpgradeFailureReason reason)
        {
            BuildingId = buildingId;
            Reason = reason;
        }
    }
}
