#nullable enable

using System.Collections.Generic;
using Domain.Gameplay.Models;

namespace ContractsInterfaces.UseCasesGameplay
{
    /// <summary>
    /// Контракт для запроса информации о текущем состоянии ресурсов игрока.
    /// </summary>
    public interface IResourceQueries
    {
        /// <summary>
        /// Возвращает словарь, содержащий текущее количество каждого ресурса.
        /// </summary>
        /// <returns>Словарь, где ключ - тип ресурса, а значение - его количество.</returns>
        IReadOnlyDictionary<ResourceType, int> GetCurrentResources();
    }
}