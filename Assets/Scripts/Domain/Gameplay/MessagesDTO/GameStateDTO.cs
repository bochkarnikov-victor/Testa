using System;
using System.Collections.Generic;

namespace Domain.Gameplay.MessagesDTO
{
    /// <summary>
    /// DTO для сериализации полного состояния игровой сессии.
    /// </summary>
    [Serializable]
    public struct GameStateDTO
    {
        /// <summary>
        /// Список состояний всех зданий на карте.
        /// </summary>
        public List<BuildingStateDTO> Buildings;

        /// <summary>
        /// Список всех ресурсов игрока и их количество.
        /// </summary>
        public List<ResourceAmount> Resources;
    }
}