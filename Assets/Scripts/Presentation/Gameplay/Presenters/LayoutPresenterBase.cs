#nullable enable

using System;
using ContractsInterfaces.PresentersGameplay;
using ContractsInterfaces.ViewsApplication;
using Cysharp.Threading.Tasks;
using Serilog;
using VContainer;
using VContainer.Unity;

namespace Presentation.Gameplay.Presenters
{
    /// <summary>
    /// Базовый класс для всех LayoutPresenter.
    /// Автоматически получает ссылку на ILayoutView и ILogger.
    /// </summary>
    public abstract class LayoutPresenterBase<TView> : ILayoutPresenter, IStartable, IDisposable
        where TView : class, ILayoutView
    {
        [Inject] protected TView LayoutView;
        [Inject] protected ILogger Logger;

        /// <summary> 
        /// Вызывается VContainer после того, как все Awake() были выполнены.
        /// Гарантирует, что View готово к использованию.
        /// </summary>
        public virtual void Start()
        {
        }

        /// <summary>
        /// Асинхронно показывает View.
        /// </summary>
        public virtual async UniTask ActivateAsync()
        {
            await this.LayoutView.ShowAsync();
        }

        /// <summary>
        /// Освобождает ресурсы.
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}