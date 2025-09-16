#nullable enable

using ContractsInterfaces.UseCasesGameplay;
using Domain.Gameplay.Models;

namespace Repositories.Gameplay
{
    /// <summary>
    /// Репозиторий, который хранит и предоставляет доступ к единственному экземпляру <see cref="CityGridModel"/>.
    /// </summary>
    public class CityGridRepository : ICityGridRepository
    {
        private readonly CityGridModel _instance;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="CityGridRepository"/>.
        /// </summary>
        /// <param name="instance">Экземпляр <see cref="CityGridModel"/>, который будет храниться в репозитории.</param>
        public CityGridRepository(CityGridModel instance)
        {
            this._instance = instance;
        }

        /// <inheritdoc />
        public CityGridModel Get()
        {
            return this._instance;
        }
    }
}