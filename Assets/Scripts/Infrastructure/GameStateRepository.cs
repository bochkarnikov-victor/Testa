#nullable enable

using System.IO;
using ContractsInterfaces.Infrastructure;
using Cysharp.Threading.Tasks;
using Domain.Gameplay.MessagesDTO;
using UnityEngine;

namespace Infrastructure
{
    /// <summary>
    /// Реализация репозитория для сохранения и загрузки состояния игры в локальный JSON-файл.
    /// </summary>
    public class GameStateRepository : IGameStateRepository
    {
        private const string SaveFileName = "gamestate.json";

        private readonly string _saveFilePath;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="GameStateRepository"/>,
        /// определяя путь для сохранения файла.
        /// </summary>
        public GameStateRepository()
        {
            this._saveFilePath = Path.Combine(Application.persistentDataPath, SaveFileName);
        }

        /// <inheritdoc />
        public async UniTask SaveStateAsync(GameStateDTO gameState)
        {
            string json = JsonUtility.ToJson(gameState, true);
            await File.WriteAllTextAsync(this._saveFilePath, json);
        }

        /// <inheritdoc />
        public async UniTask<GameStateDTO?> LoadStateAsync()
        {
            if (!File.Exists(this._saveFilePath))
            {
                return null;
            }

            string json = await File.ReadAllTextAsync(this._saveFilePath);
            return JsonUtility.FromJson<GameStateDTO>(json);
        }
    }
}