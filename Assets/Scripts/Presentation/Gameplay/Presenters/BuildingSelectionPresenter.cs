#nullable enable

using ContractsInterfaces.Infrastructure;
using ContractsInterfaces.UseCasesGameplay;
using ContractsInterfaces.ViewsGameplay;
using Domain.Gameplay.Models;
using MessagePipe;
using R3;
using VContainer;

namespace Presentation.Gameplay.Presenters
{
    /// <summary>
    /// Presenter для панели выбора зданий.
    /// </summary>
    public class BuildingSelectionPresenter : LayoutPresenterBase<IBuildingSelectionView>
    {
        [Inject] private readonly IBuildingConfig[] _buildingConfigs;
        [Inject] private readonly IPublisher<BuildingType> _buildingTypePublisher;
        [Inject] private readonly IInputService _inputService;

        private readonly CompositeDisposable _disposables = new();

        /// <inheritdoc/>
        public override void Start()
        {
            base.Start();
            this.LayoutView.Show();

            this.LayoutView.OnBuildingTypeSelected += this.HandleBuildingTypeSelected;
            this._inputService.OnSelectBuildingRequested
                .Subscribe(this.HandleHotkeyPressed)
                .AddTo(this._disposables);

            this.InitializeBuildingSprites();

            this.Logger.Information("[BuildingSelectionPresenter.Start] Initialized and subscribed to view events.");
        }

        private void InitializeBuildingSprites()
        {
            foreach (IBuildingConfig config in this._buildingConfigs)
            {
                this.LayoutView.SetBuildingSprite(config.BuildingType, config.Sprite);
            }
        }

        private void HandleHotkeyPressed(int buildingIndex)
        {
            BuildingType buildingType = buildingIndex switch
            {
                1 => BuildingType.House,
                2 => BuildingType.Farm,
                3 => BuildingType.Mine,
                _ => BuildingType.None
            };

            if (buildingType != BuildingType.None)
            {
                this.HandleBuildingTypeSelected(buildingType);
            }
        }

        private void HandleBuildingTypeSelected(BuildingType buildingType)
        {
            this.Logger.Information(
                "[BuildingSelectionPresenter.HandleBuildingTypeSelected] Publishing request to enter build mode for {BuildingType}.",
                buildingType);
            this._buildingTypePublisher.Publish(buildingType);
        }

        /// <inheritdoc/>
        public override void Dispose()
        {
            base.Dispose();
            this._disposables?.Dispose();
            if (this.LayoutView != null)
            {
                this.LayoutView.OnBuildingTypeSelected -= this.HandleBuildingTypeSelected;
            }
        }
    }
}