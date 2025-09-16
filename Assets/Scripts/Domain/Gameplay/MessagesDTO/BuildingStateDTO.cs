using System;
using Domain.Gameplay.Models;

namespace Domain.Gameplay.MessagesDTO
{
    /// <summary>
    /// DTO для сериализации состояния одного здания.
    /// </summary>
    [Serializable]
    public struct BuildingStateDTO
    {
        /// <summary>
        /// Уникальный идентификатор здания.
        /// </summary>
        public string Id;

        /// <summary>
        /// Тип здания.
        /// </summary>
        public BuildingType Type;

        /// <summary>
        /// Позиция здания на сетке.
        /// </summary>
        public GridPos Position;

        /// <summary>
        /// Текущий уровень здания.
        /// </summary>
        public int Level;
    }
}