#nullable enable

using System.Collections.Generic;
using Domain.Gameplay.Models;

namespace Domain.Gameplay.MessagesDTO
{
    /// <summary>
    /// Событие, возникающее при изменении количества ресурсов у игрока.
    /// Содержит полный снимок текущего состояния всех ресурсов.
    /// </summary>
    public readonly struct ResourcesChangedEvent
    {
        /// <summary>
        /// Словарь, содержащий текущее количество для каждого типа ресурса.
        /// </summary>
        public readonly IReadOnlyDictionary<ResourceType, int> AllResources;

        public ResourcesChangedEvent(IReadOnlyDictionary<ResourceType, int> allResources)
        {
            this.AllResources = allResources;
        }
    }
}