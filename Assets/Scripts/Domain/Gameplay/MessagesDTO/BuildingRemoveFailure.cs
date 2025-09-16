using System;

namespace Domain.Gameplay.MessagesDTO
{
    /// <summary>
    /// Перечисление причин, по которым не удалось удалить здание.
    /// </summary>
    public enum RemoveFailureReason
    {
        /// <summary>
        /// Неизвестная ошибка.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Здание с указанным ID не было найдено в модели.
        /// </summary>
        BuildingNotFound = 1,
    }

    /// <summary>
    /// DTO-сообщение, анонсирующее неудачную попытку удаления здания.
    /// </summary>
    public readonly struct BuildingRemoveFailure
    {
        /// <summary>
        /// ID здания, которое не удалось удалить.
        /// </summary>
        public Guid BuildingId { get; }

        /// <summary>
        /// Причина, по которой не удалось удалить здание.
        /// </summary>
        public RemoveFailureReason Reason { get; }

        public BuildingRemoveFailure(Guid buildingId, RemoveFailureReason reason)
        {
            BuildingId = buildingId;
            Reason = reason;
        }
    }
}
