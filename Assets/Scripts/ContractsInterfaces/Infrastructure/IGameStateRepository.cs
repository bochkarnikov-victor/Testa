#nullable enable

using Cysharp.Threading.Tasks;
using Domain.Gameplay.MessagesDTO;

namespace ContractsInterfaces.Infrastructure
{
    /// <summary>
    /// Контракт для репозитория, отвечающего за физическое сохранение и загрузку состояния игры.
    /// </summary>
    public interface IGameStateRepository
    {
        /// <summary>
        /// Асинхронно сохраняет состояние игры.
        /// </summary>
        /// <param name="gameState">DTO с состоянием игры для сохранения.</param>
        UniTask SaveStateAsync(GameStateDTO gameState);

        /// <summary>
        /// Асинхронно загружает состояние игры.
        /// </summary>
        /// <returns>Загруженный DTO с состоянием игры или null, если сохранение не найдено.</returns>
        UniTask<GameStateDTO?> LoadStateAsync();
    }
}