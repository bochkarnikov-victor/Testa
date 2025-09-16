#nullable enable

using ContractsInterfaces.ViewsGameplay;
using Domain.Gameplay.Models;
using R3;
using UnityEngine.UIElements;

namespace Presentation.Gameplay.Views
{
    /// <summary>
    /// View, отвечающий за отображение панели свойств выбранного здания.
    /// </summary>
    public class BuildingPropertiesView : LayoutViewBase, IBuildingPropertiesView
    {
        private readonly Subject<Unit> _onUpgradeClicked = new();

        /// <inheritdoc/>
        public Observable<Unit> OnUpgradeClicked => this._onUpgradeClicked;

        private readonly Subject<Unit> _onRemoveClicked = new();

        /// <inheritdoc/>
        public Observable<Unit> OnRemoveClicked => this._onRemoveClicked;

        private Button? _upgradeButton;
        private Button? _removeButton;
        private Label? _buildingNameLabel;
        private Label? _buildingLevelLabel;

        private readonly CompositeDisposable _buildingSubscriptions = new();

        /// <inheritdoc/>
        protected override void InitializeElements()
        {
            this._upgradeButton = this.Root.Q<Button>("UpgradeButton");
            this._removeButton = this.Root.Q<Button>("RemoveButton");
            this._buildingNameLabel = this.Root.Q<Label>("BuildingName");
            this._buildingLevelLabel = this.Root.Q<Label>("BuildingLevel");
        }

        /// <inheritdoc/>
        public override void Awake()
        {
            base.Awake();
            this.RegisterCallbacks();
        }

        private void OnDestroy()
        {
            this.UnregisterCallbacks();
            this._onUpgradeClicked.Dispose();
            this._onRemoveClicked.Dispose();
            this._buildingSubscriptions.Dispose();
        }

        private void RegisterCallbacks()
        {
            this._upgradeButton?.RegisterCallback<ClickEvent>(this.OnUpgrade);
            this._removeButton?.RegisterCallback<ClickEvent>(this.OnRemove);
        }

        private void UnregisterCallbacks()
        {
            this._upgradeButton?.UnregisterCallback<ClickEvent>(this.OnUpgrade);
            this._removeButton?.UnregisterCallback<ClickEvent>(this.OnRemove);
        }

        private void OnUpgrade(ClickEvent _)
        {
            this._onUpgradeClicked.OnNext(Unit.Default);
        }

        private void OnRemove(ClickEvent _)
        {
            this._onRemoveClicked.OnNext(Unit.Default);
        }

        /// <inheritdoc/>
        public void Show(BuildingModel building)
        {
            this._buildingSubscriptions.Clear();

            if (this._buildingNameLabel != null)
            {
                this._buildingNameLabel.text = building.Type.ToString();
            }

            building.CurrentLevel
                .Subscribe(level =>
                {
                    if (this._buildingLevelLabel != null)
                    {
                        this._buildingLevelLabel.text = $"Уровень: {level}";
                    }

                    bool isMaxLevel = level >= building.Levels.Count;
                    this._upgradeButton?.SetEnabled(!isMaxLevel);
                })
                .AddTo(this._buildingSubscriptions);

            base.Show();
        }

        /// <inheritdoc/>
        public override void Hide()
        {
            base.Hide();
            this._buildingSubscriptions.Clear();
        }
    }
}