#nullable enable

using System.Threading;
using ContractsInterfaces.Infrastructure;
using Cysharp.Threading.Tasks;
using Domain.Gameplay.MessagesDTO;
using MessagePipe;
using R3;
using Serilog;

namespace UseCases.Gameplay
{
    /// <summary>
    /// UseCase, отвечающий за запуск процесса загрузки игры и публикацию загруженных данных.
    /// </summary>
    public class LoadGameUseCase : IAsyncRequestHandler<LoadGameCommand, Unit>
    {
        private readonly ILogger _logger;
        private readonly IGameStateRepository _gameStateRepository;
        private readonly IPublisher<GameStateLoadedEvent> _eventPublisher;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="LoadGameUseCase"/>.
        /// </summary>
        public LoadGameUseCase(
            ILogger logger,
            IGameStateRepository gameStateRepository,
            IPublisher<GameStateLoadedEvent> eventPublisher)
        {
            this._logger = logger.ForContext<LoadGameUseCase>();
            this._gameStateRepository = gameStateRepository;
            this._eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Асинхронно обрабатывает команду <see cref="LoadGameCommand"/>, загружает состояние и публикует событие.
        /// </summary>
        public async UniTask<Unit> InvokeAsync(LoadGameCommand request, CancellationToken ct)
        {
            this._logger.Information("[LoadGameUseCase] Starting game load process...");

            GameStateDTO? gameState = await this._gameStateRepository.LoadStateAsync();

            if (gameState.HasValue)
            {
                this._eventPublisher.Publish(new GameStateLoadedEvent(gameState.Value));
                this._logger.Information("[LoadGameUseCase] Game state loaded and event published.");
            }
            else
            {
                this._logger.Warning("[LoadGameUseCase] No save file found. Cannot load game state.");
            }

            return Unit.Default;
        }
    }
}