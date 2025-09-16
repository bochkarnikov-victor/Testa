#nullable enable

using System;
using System.Collections.Generic;
using ContractsInterfaces.ViewsGameplay;
using Domain.Gameplay.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace Presentation.Gameplay.Views
{
    /// <summary>
    /// View, отвечающий за отображение панели выбора зданий.
    /// </summary>
    public class BuildingSelectionView : LayoutViewBase, IBuildingSelectionView
    {
        /// <inheritdoc/>
        public event Action<BuildingType> OnBuildingTypeSelected = delegate { };

        private readonly Dictionary<BuildingType, Button?> _buttons = new();
        private readonly Dictionary<BuildingType, Image?> _sprites = new();

        /// <inheritdoc/>
        protected override void InitializeElements()
        {
            this._buttons[BuildingType.House] = this.Root.Q<Button>("House");
            this._buttons[BuildingType.Farm] = this.Root.Q<Button>("Farm");
            this._buttons[BuildingType.Mine] = this.Root.Q<Button>("Mine");

            this._sprites[BuildingType.House] = this.Root.Q<Image>("HouseSprite");
            this._sprites[BuildingType.Farm] = this.Root.Q<Image>("FarmSprite");
            this._sprites[BuildingType.Mine] = this.Root.Q<Image>("MineSprite");
        }

        public override void Awake()
        {
            base.Awake();
            this.RegisterCallbacks();
        }

        private void OnDestroy()
        {
            this.UnregisterCallbacks();
        }

        private void RegisterCallbacks()
        {
            this._buttons[BuildingType.House]?.RegisterCallback<ClickEvent>(this.OnHouseClicked);
            this._buttons[BuildingType.Farm]?.RegisterCallback<ClickEvent>(this.OnFarmClicked);
            this._buttons[BuildingType.Mine]?.RegisterCallback<ClickEvent>(this.OnMineClicked);
        }

        private void UnregisterCallbacks()
        {
            this._buttons[BuildingType.House]?.UnregisterCallback<ClickEvent>(this.OnHouseClicked);
            this._buttons[BuildingType.Farm]?.UnregisterCallback<ClickEvent>(this.OnFarmClicked);
            this._buttons[BuildingType.Mine]?.UnregisterCallback<ClickEvent>(this.OnMineClicked);
        }

        private void OnHouseClicked(ClickEvent _)
        {
            this.OnBuildingTypeSelected.Invoke(BuildingType.House);
        }

        private void OnFarmClicked(ClickEvent _)
        {
            this.OnBuildingTypeSelected.Invoke(BuildingType.Farm);
        }

        private void OnMineClicked(ClickEvent _)
        {
            this.OnBuildingTypeSelected.Invoke(BuildingType.Mine);
        }

        /// <inheritdoc/>
        public void SetBuildingSprite(BuildingType type, Sprite sprite)
        {
            if (this._sprites.TryGetValue(type, out Image? image) && image != null)
            {
                image.style.backgroundImage = new StyleBackground(sprite);
            }
        }
    }
}