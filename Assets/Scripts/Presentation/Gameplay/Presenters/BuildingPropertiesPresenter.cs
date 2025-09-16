#nullable enable

using System;
using ContractsInterfaces.UseCasesGameplay;
using ContractsInterfaces.ViewsGameplay;
using Domain.Gameplay.MessagesDTO;
using Domain.Gameplay.Models;
using MessagePipe;
using R3;
using VContainer;

namespace Presentation.Gameplay.Presenters
{
    /// <summary>
    /// Презентер, управляющий панелью свойств выбранного здания.
    /// </summary>
    public class BuildingPropertiesPresenter : LayoutPresenterBase<IBuildingPropertiesView>
    {
        [Inject] private readonly ISubscriber<BuildingSelectedEvent> _buildingSelectedSubscriber;
        [Inject] private readonly ICityGridRepository _cityGridRepository;
        [Inject] private readonly IRequestHandler<UpgradeBuildingRequest, Unit> _upgradeHandler;
        [Inject] private readonly IRequestHandler<RemoveBuildingRequest, Unit> _removeHandler;

        private readonly CompositeDisposable _disposables = new();
        private Guid? _currentBuildingId;

        /// <inheritdoc/>
        public override void Start()
        {
            base.Start();
            this.Logger.Information("[BuildingPropertiesPresenter.Start] Initializing...");

            this._buildingSelectedSubscriber
                .Subscribe(this.OnBuildingSelected)
                .AddTo(this._disposables);

            this.LayoutView.OnUpgradeClicked
                .Subscribe(_ => this.HandleUpgradeClick())
                .AddTo(this._disposables);

            this.LayoutView.OnRemoveClicked
                .Subscribe(_ => this.HandleRemoveClick())
                .AddTo(this._disposables);
        }

        private void OnBuildingSelected(BuildingSelectedEvent e)
        {
            this._currentBuildingId = e.BuildingId;

            if (this._currentBuildingId is null)
            {
                this.LayoutView.Hide();
                this.Logger.Information("[BuildingPropertiesPresenter.OnBuildingSelected] Selection cleared.");
                return;
            }

            CityGridModel cityGrid = this._cityGridRepository.Get();
            BuildingModel? building = cityGrid.GetBuildingById(this._currentBuildingId.Value);

            if (building is null)
            {
                this.Logger.Warning(
                    "[BuildingPropertiesPresenter.OnBuildingSelected] Building with ID {BuildingId} not found in repository.",
                    this._currentBuildingId.Value);
                this.LayoutView.Hide();
                return;
            }

            this.Logger.Information(
                "[BuildingPropertiesPresenter.OnBuildingSelected] Showing properties for building {BuildingId}.",
                building.Id);
            this.LayoutView.Show(building);
        }

        private void HandleUpgradeClick()
        {
            if (this._currentBuildingId is null)
            {
                return;
            }

            this.Logger.Debug(
                "[BuildingPropertiesPresenter.HandleUpgradeClick] Upgrade requested for building {BuildingId}.",
                this._currentBuildingId.Value);
            UpgradeBuildingRequest request = new(this._currentBuildingId.Value);
            this._upgradeHandler.Invoke(request);
        }

        private void HandleRemoveClick()
        {
            if (this._currentBuildingId is null)
            {
                return;
            }

            this.Logger.Debug(
                "[BuildingPropertiesPresenter.HandleRemoveClick] Remove requested for building {BuildingId}.",
                this._currentBuildingId.Value);
            RemoveBuildingRequest request = new(this._currentBuildingId.Value);
            this._removeHandler.Invoke(request);
        }

        /// <inheritdoc/>
        public override void Dispose()
        {
            base.Dispose();
            this._disposables?.Dispose();
        }
    }
}