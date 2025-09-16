#nullable enable

using System.Collections.Generic;
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
    /// Presenter для HUD. Отвечает за обновление отображения ресурсов и обработку команд сохранения/загрузки.
    /// </summary>
    public class HudPresenter : LayoutPresenterBase<IHudView>
    {
        private readonly ISubscriber<ResourcesChangedEvent> _resourcesChangedSubscriber;
        private readonly IAsyncRequestHandler<SaveGameCommand, Unit> _saveGameHandler;
        private readonly IAsyncRequestHandler<LoadGameCommand, Unit> _loadGameHandler;
        private readonly IResourceQueries _resourceQueries;

        private readonly CompositeDisposable _disposables = new();

        [Inject]
        public HudPresenter(
            ISubscriber<ResourcesChangedEvent> resourcesChangedSubscriber,
            IAsyncRequestHandler<SaveGameCommand, Unit> saveGameHandler,
            IAsyncRequestHandler<LoadGameCommand, Unit> loadGameHandler,
            IResourceQueries resourceQueries)
        {
            this._resourcesChangedSubscriber = resourcesChangedSubscriber;
            this._saveGameHandler = saveGameHandler;
            this._loadGameHandler = loadGameHandler;
            this._resourceQueries = resourceQueries;
        }

        /// <inheritdoc/>
        public override void Start()
        {
            base.Start();
            this.LayoutView.Show();

            // Отображаем начальные ресурсы
            IReadOnlyDictionary<ResourceType, int> initialResources = this._resourceQueries.GetCurrentResources();
            foreach (KeyValuePair<ResourceType, int> resource in initialResources)
            {
                this.LayoutView.UpdateResource(resource.Key, resource.Value);
            }

            this._resourcesChangedSubscriber
                .Subscribe(e =>
                {
                    foreach (KeyValuePair<ResourceType, int> resource in e.AllResources)
                    {
                        this.LayoutView.UpdateResource(resource.Key, resource.Value);
                    }
                })
                .AddTo(this._disposables);

            this.LayoutView.OnSaveClick
                .SubscribeAwait(async (_, ct) => await this._saveGameHandler.InvokeAsync(new SaveGameCommand(), ct))
                .AddTo(this._disposables);

            this.LayoutView.OnLoadClick
                .SubscribeAwait(async (_, ct) => await this._loadGameHandler.InvokeAsync(new LoadGameCommand(), ct))
                .AddTo(this._disposables);
        }

        /// <inheritdoc/>
        public override void Dispose()
        {
            base.Dispose();
            this._disposables.Dispose();
        }
    }
}