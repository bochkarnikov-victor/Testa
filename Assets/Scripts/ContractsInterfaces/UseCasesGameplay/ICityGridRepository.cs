#nullable enable

using Domain.Gameplay.Models;

namespace ContractsInterfaces.UseCasesGameplay
{
    /// <summary>
    /// Контракт для репозитория, предоставляющего доступ к единственному экземпляру модели игровой сетки.
    /// </summary>
    public interface ICityGridRepository
    {
        /// <summary>
        /// Возвращает экземпляр модели игровой сетки.
        /// </summary>
        CityGridModel Get();
    }
}
