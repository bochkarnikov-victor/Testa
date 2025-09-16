#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using ContractsInterfaces.UseCasesGameplay;
using Domain.Gameplay.MessagesDTO;
using MessagePipe;
using R3;
using Serilog;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using ILogger = Serilog.ILogger;

namespace Presentation.Gameplay.Presenters
{
    /// <summary>
    /// Отвечает за создание и уничтожение игровых объектов зданий в сцене,
    /// синхронизируя визуальное представление с моделью данных.
    /// </summary>
    public class BuildingSpawner : IInitializable, IDisposable
    {
        [Inject] private readonly ILogger _logger;
        [Inject] private readonly ISubscriber<BuildingPlacementSuccess> _placementSubscriber;
        [Inject] private readonly ISubscriber<BuildingRemovedEvent> _removalSubscriber;
        [Inject] private readonly ISubscriber<GameStateLoadedEvent> _loadSubscriber;
        [Inject] private readonly IBuildingConfig[] _buildingConfigs;

        private readonly Dictionary<Guid, GameObject> _spawnedBuildings = new();
        private readonly CompositeDisposable _disposables = new();

        /// <inheritdoc/>
        public void Initialize()
        {
            this._placementSubscriber
                .Subscribe(this.OnBuildingPlaced)
                .AddTo(this._disposables);

            this._removalSubscriber
                .Subscribe(this.OnBuildingRemoved)
                .AddTo(this._disposables);

            this._loadSubscriber
                .Subscribe(this.OnGameLoaded)
                .AddTo(this._disposables);

            this._logger.Information("[BuildingSpawner.Initialize] Initialized.");
        }

        private void OnBuildingPlaced(BuildingPlacementSuccess e)
        {
            IBuildingConfig? config = this._buildingConfigs.FirstOrDefault(c => c.BuildingType == e.BuildingType);
            if (config == null)
            {
                this._logger.Error("[BuildingSpawner.OnBuildingPlaced] Cannot find config for {BuildingType}",
                    e.BuildingType);
                return;
            }

            Vector3 worldPos = new(e.Position.X, 0, e.Position.Y);
            GameObject? buildingInstance = UnityEngine.Object.Instantiate(config.Prefab, worldPos, Quaternion.identity);

            this._spawnedBuildings[e.BuildingId] = buildingInstance;
            this._logger.Information("[BuildingSpawner.OnBuildingPlaced] Spawned GameObject for building {BuildingId}",
                e.BuildingId);
        }

        private void OnBuildingRemoved(BuildingRemovedEvent e)
        {
            if (this._spawnedBuildings.TryGetValue(e.BuildingId, out GameObject? instance))
            {
                UnityEngine.Object.Destroy(instance);
                this._spawnedBuildings.Remove(e.BuildingId);
                this._logger.Information(
                    "[BuildingSpawner.OnBuildingRemoved] Destroyed GameObject for building {BuildingId}", e.BuildingId);
            }
        }

        private void OnGameLoaded(GameStateLoadedEvent e)
        {
            // Очищаем все существующие здания перед загрузкой
            foreach (KeyValuePair<Guid, GameObject> pair in this._spawnedBuildings)
            {
                UnityEngine.Object.Destroy(pair.Value);
            }

            this._spawnedBuildings.Clear();

            this._logger.Information("[BuildingSpawner.OnGameLoaded] Spawning {Count} buildings from save file...",
                e.GameState.Buildings.Count);
            foreach (BuildingStateDTO buildingState in e.GameState.Buildings)
            {
                BuildingPlacementSuccess successEvent =
                    new(buildingState.Id, buildingState.Type, buildingState.Position);
                this.OnBuildingPlaced(successEvent);
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this._disposables.Dispose();
        }
    }
}