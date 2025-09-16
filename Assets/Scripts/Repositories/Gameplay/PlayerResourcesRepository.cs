using ContractsInterfaces.DomainGameplay;
using ContractsInterfaces.UseCasesGameplay;
using Domain.Application.Models;
using Domain.Gameplay.Models;
using R3;

namespace Repositories.Gameplay
{
    /// <summary>
    /// Репозиторий, предоставляющий доступ к модели ресурсов игрока <see cref="PlayerResourcesModel"/>.
    /// </summary>
    public class PlayerResourcesRepository : IPlayerResourcesModel, IPlayerResourcesRepository
    {
        private readonly PlayerResourcesModel _playerResourcesModel;

        /// <summary>
        /// Предоставляет прямой доступ к доменной модели ресурсов.
        /// </summary>
        public PlayerResourcesModel Model => this._playerResourcesModel;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="PlayerResourcesRepository"/>.
        /// </summary>
        /// <param name="playerResourcesModel">Экземпляр модели <see cref="PlayerResourcesModel"/>, которым будет управлять репозиторий.</param>
        public PlayerResourcesRepository(PlayerResourcesModel playerResourcesModel)
        {
            this._playerResourcesModel = playerResourcesModel;
        }

        /// <inheritdoc />
        public ReadOnlyReactiveProperty<int> GetResource(ResourceType type)
        {
            return this.GetOrCreate(type).ToReadOnlyReactiveProperty();
        }

        /// <inheritdoc />
        public bool TryGet(ResourceType type, out ReactiveProperty<int> property)
        {
            return this._playerResourcesModel.Resources.TryGetValue(type, out property);
        }

        /// <inheritdoc />
        public ReactiveProperty<int> GetOrCreate(ResourceType type)
        {
            if (this._playerResourcesModel.Resources.TryGetValue(type, out ReactiveProperty<int> resource))
            {
                return resource;
            }

            ReactiveProperty<int> newResource = new(0);
            this._playerResourcesModel.Resources[type] = newResource;
            return newResource;
        }
    }
}