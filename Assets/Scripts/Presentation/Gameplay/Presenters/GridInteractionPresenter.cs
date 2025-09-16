#nullable enable

using System;
using System.Linq;
using ContractsInterfaces.Infrastructure;
using ContractsInterfaces.UseCasesGameplay;
using Domain.Gameplay.MessagesDTO;
using Domain.Gameplay.Models;
using MessagePipe;
using Presentation.Gameplay.Views;
using R3;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using ILogger = Serilog.ILogger;
using Vector2 = System.Numerics.Vector2;

namespace Presentation.Gameplay.Presenters
{
    /// <summary>
    /// Презентер, отвечающий за обработку всех прямых взаимодействий пользователя с игровой сеткой.
    /// </summary>
    public class GridInteractionPresenter : IStartable, IDisposable
    {
        [Inject] private readonly ILogger _logger;
        [Inject] private readonly IInputService _inputService;
        [Inject] private readonly ICityGridRepository _cityGridRepository;
        [Inject] private readonly IPublisher<BuildingSelectedEvent> _buildingSelectedPublisher;
        [Inject] private readonly IGridCoordinatesConverter _coordinatesConverter;
        [Inject] private readonly GridView _gridView;
        [Inject] private readonly BuildingGhostView _ghostView;
        [Inject] private readonly ISubscriber<BuildingType> _buildingTypeSelectedSubscriber;
        [Inject] private readonly IBuildingConfig[] _buildingConfigs;
        [Inject] private readonly IRequestHandler<PlaceBuildingRequest, Unit> _placeBuildingHandler;

        private readonly CompositeDisposable _disposables = new();
        private BuildingType _currentBuildingMode = BuildingType.None;
        private IBuildingConfig? _currentBuildingConfig;

        private readonly Color _validPlacementColor = new(0.1f, 1f, 0.1f, 0.5f);
        private readonly Color _invalidPlacementColor = new(1f, 0.1f, 0.1f, 0.5f);

        /// <inheritdoc/>
        public void Start()
        {
            this._inputService.OnPlace
                .Subscribe(_ => this.HandleGridClick())
                .AddTo(this._disposables);

            this._inputService.PointerPosition
                .Subscribe(this.HandlePointerMove)
                .AddTo(this._disposables);

            this._buildingTypeSelectedSubscriber
                .Subscribe(this.EnterBuildMode)
                .AddTo(this._disposables);

            this._inputService.OnCancel
                .Subscribe(_ => this.ExitBuildMode())
                .AddTo(this._disposables);

            this._logger.Information("[GridInteractionPresenter.Start] Started and initialized.");
        }

        private void EnterBuildMode(BuildingType buildingType)
        {
            Debug.Log("Place");
            if (buildingType == BuildingType.None)
            {
                return;
            }

            this._currentBuildingMode = buildingType;
            this._logger.Information("[GridInteractionPresenter.EnterBuildMode] Entered build mode for {BuildingType}",
                buildingType);

            this._currentBuildingConfig = this._buildingConfigs.FirstOrDefault(c => c.BuildingType == buildingType);
            if (this._currentBuildingConfig != null)
            {
                this._ghostView.Show(this._currentBuildingConfig.Prefab);
                this.HandlePointerMove(this._inputService.PointerPosition.CurrentValue);
            }
            else
            {
                this._logger.Error("[GridInteractionPresenter.EnterBuildMode] Could not find config for {BuildingType}",
                    buildingType);
            }
        }

        private void ExitBuildMode()
        {
            if (this._currentBuildingMode == BuildingType.None)
            {
                return;
            }

            this._currentBuildingMode = BuildingType.None;
            this._currentBuildingConfig = null;
            this._ghostView.Hide();
            this._logger.Information("[GridInteractionPresenter.ExitBuildMode] Exited build mode.");
        }

        private void HandleGridClick()
        {
            Vector2 screenPosition = this._inputService.PointerPosition.CurrentValue;
            GridPos gridPos = this._coordinatesConverter.ScreenToGrid(screenPosition);
            CityGridModel cityGrid = this._cityGridRepository.Get();

            if (!cityGrid.IsWithinBounds(gridPos))
            {
                this._logger.Debug(
                    "[GridInteractionPresenter.HandleGridClick] Clicked outside of grid bounds at {GridPos}", gridPos);
                return;
            }

            if (this._currentBuildingMode != BuildingType.None)
            {
                if (cityGrid.IsCellOccupied(gridPos))
                {
                    this._logger.Warning(
                        "[GridInteractionPresenter.HandleGridClick] Attempted to place on an occupied cell {GridPos}",
                        gridPos);
                    return;
                }

                this._logger.Debug(
                    "[GridInteractionPresenter.HandleGridClick] Attempting to place {BuildingType} at {GridPos}",
                    this._currentBuildingMode, gridPos);
                PlaceBuildingRequest request = new(this._currentBuildingMode, gridPos);
                this._placeBuildingHandler.Invoke(request);
                this.ExitBuildMode();
            }
            else
            {
                this._logger.Debug("[GridInteractionPresenter.HandleGridClick] Selecting object at {GridPos}", gridPos);
                BuildingModel? building = cityGrid.GetBuildingAt(gridPos);
                this._buildingSelectedPublisher.Publish(new BuildingSelectedEvent(building?.Id));
            }
        }

        private void HandlePointerMove(Vector2 screenPosition)
        {
            GridPos gridPos = this._coordinatesConverter.ScreenToGrid(screenPosition);
            CityGridModel cityGrid = this._cityGridRepository.Get();

            if (!cityGrid.IsWithinBounds(gridPos))
            {
                this._gridView.ClearHighlight();
                if (this._currentBuildingMode != BuildingType.None)
                {
                    this._ghostView.Hide();
                }

                return;
            }

            bool isOccupied = cityGrid.IsCellOccupied(gridPos);
            Color highlightColor = isOccupied ? this._invalidPlacementColor : this._validPlacementColor;

            this._gridView.HighlightCell(gridPos, highlightColor);

            if (this._currentBuildingMode != BuildingType.None)
            {
                if (this._currentBuildingConfig != null)
                {
                    this._ghostView.Show(this._currentBuildingConfig.Prefab);
                    Vector3 worldPos = this._coordinatesConverter.GridToWorld(gridPos);
                    this._ghostView.SetPosition(worldPos);
                    this._ghostView.SetColor(highlightColor);
                }
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this._disposables?.Dispose();
        }
    }
}