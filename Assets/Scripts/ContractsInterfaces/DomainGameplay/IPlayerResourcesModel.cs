using Domain.Gameplay.Models;
using R3;

namespace ContractsInterfaces.DomainGameplay
{
    /// <summary>
    /// Интерфейс для модели, управляющей ресурсами игрока. Предоставляет доступ только для чтения.
    /// </summary>
    public interface IPlayerResourcesModel
    {
        /// <summary>
        /// Позволяет получить реактивное свойство для отслеживания количества конкретного ресурса.
        /// </summary>
        /// <param name="type">Тип ресурса.</param>
        /// <returns>Реактивное свойство с количеством ресурса.</returns>
        ReadOnlyReactiveProperty<int> GetResource(ResourceType type);
    }
}