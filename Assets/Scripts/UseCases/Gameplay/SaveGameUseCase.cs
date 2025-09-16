#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ContractsInterfaces.Infrastructure;
using ContractsInterfaces.UseCasesGameplay;
using Cysharp.Threading.Tasks;
using Domain.Gameplay.MessagesDTO;
using Domain.Gameplay.Models;
using MessagePipe;
using R3;
using Serilog;

namespace UseCases.Gameplay
{
    /// <summary>
    /// UseCase, отвечающий за сбор данных о состоянии игры и запуск процесса сохранения.
    /// </summary>
    public class SaveGameUseCase : IAsyncRequestHandler<SaveGameCommand, Unit>
    {
        private readonly ILogger _logger;
        private readonly ICityGridRepository _cityGridRepository;
        private readonly IResourceQueries _resourceQueries;
        private readonly IGameStateRepository _gameStateRepository;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="SaveGameUseCase"/>.
        /// </summary>
        public SaveGameUseCase(
            ILogger logger,
            ICityGridRepository cityGridRepository,
            IResourceQueries resourceQueries,
            IGameStateRepository gameStateRepository)
        {
            this._logger = logger.ForContext<SaveGameUseCase>();
            this._cityGridRepository = cityGridRepository;
            this._resourceQueries = resourceQueries;
            this._gameStateRepository = gameStateRepository;
        }

        /// <summary>
        /// Асинхронно обрабатывает команду <see cref="SaveGameCommand"/>, собирает данные и сохраняет их.
        /// </summary>
        public async UniTask<Unit> InvokeAsync(SaveGameCommand request, CancellationToken ct)
        {
            this._logger.Information("[SaveGameUseCase] Starting game save process...");

            CityGridModel cityGrid = this._cityGridRepository.Get();
            IReadOnlyDictionary<ResourceType, int> resources = this._resourceQueries.GetCurrentResources();

            List<BuildingStateDTO> buildingStates = cityGrid.Buildings
                .Select(b => new BuildingStateDTO
                {
                    Id = b.Id.ToString(),
                    Type = b.Type,
                    Level = b.CurrentLevel.CurrentValue,
                    Position = b.Position.CurrentValue
                })
                .ToList();

            List<ResourceAmount> resourceStates = resources
                 .Select(r => new ResourceAmount { Type = r.Key, Amount = r.Value })
                .ToList();

            GameStateDTO gameState = new()
            {
                Buildings = buildingStates,
                Resources = resourceStates
            };

            await this._gameStateRepository.SaveStateAsync(gameState);

            this._logger.Information("[SaveGameUseCase] Game state saved successfully.");

            return Unit.Default;
        }
    }
}