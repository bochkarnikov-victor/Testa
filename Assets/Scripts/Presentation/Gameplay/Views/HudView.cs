#nullable enable

using System.Collections.Generic;
using ContractsInterfaces.ViewsGameplay;
using Domain.Gameplay.Models;
using R3;
using UnityEngine.UIElements;
using Utilities;

namespace Presentation.Gameplay.Views
{
    /// <summary>
    /// Реализация HUD View с использованием UI Toolkit.
    /// </summary>
    public class HudView : LayoutViewBase, IHudView
    {
        private Button _saveButton = null!;
        private Button _loadButton = null!;

        private readonly Dictionary<ResourceType, Label> _resourceLabels = new();

        public Observable<Unit> OnSaveClick => this._saveButton.OnClickAsObservable();
        public Observable<Unit> OnLoadClick => this._loadButton.OnClickAsObservable();

        /// <summary>
        /// Находит и кэширует UI-элементы для отображения ресурсов.
        /// </summary>
        protected override void InitializeElements()
        {
            this._resourceLabels[ResourceType.Gold] = this.Root.Q<Label>("Gold");
            this._resourceLabels[ResourceType.Wood] = this.Root.Q<Label>("Wood");
            this._resourceLabels[ResourceType.Crystals] = this.Root.Q<Label>("Crystals");

            this._saveButton = this.Root.Q<Button>("save-button");
            this._loadButton = this.Root.Q<Button>("load-button");
        }

        /// <inheritdoc />
        public void UpdateResource(ResourceType resourceType, int amount)
        {
            if (this._resourceLabels.TryGetValue(resourceType, out Label? label) && label != null)
            {
                label.text = amount.ToString();
            }
        }
    }
}