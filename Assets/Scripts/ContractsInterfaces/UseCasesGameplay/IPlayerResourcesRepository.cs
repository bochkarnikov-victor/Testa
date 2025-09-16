using Domain.Gameplay.Models;
using R3;

namespace ContractsInterfaces.UseCasesGameplay
{
    /// <summary>
    /// Порт для доступа к хранилищу ресурсов игрока.
    /// Позволяет получать и создавать реактивные свойства ресурсов.
    /// </summary>
    public interface IPlayerResourcesRepository
    {
        /// <summary>
        /// Пытается получить ресурс по типу.
        /// </summary>
        /// <param name="type">Тип ресурса.</param>
        /// <param name="property">Свойство ресурса.</param>
        /// <returns>True, если ресурс найден.</returns>
        bool TryGet(ResourceType type, out ReactiveProperty<int> property);

        /// <summary>
        /// Получает или создает ресурс по типу.
        /// </summary>
        /// <param name="type">Тип ресурса.</param>
        /// <returns>Свойство ресурса.</returns>
        ReactiveProperty<int> GetOrCreate(ResourceType type);
    }
}
