using System;
using Domain.Gameplay.Models;

namespace Domain.Gameplay.MessagesDTO
{
    /// <summary>
    /// DTO для представления определенного количества одного типа ресурса.
    /// </summary>
    [Serializable]
    public struct ResourceAmount
    {
        /// <summary>
        /// Тип ресурса.
        /// </summary>
        public ResourceType Type;

        /// <summary>
        /// Количество ресурса.
        /// </summary>
        public int Amount;
    }
}