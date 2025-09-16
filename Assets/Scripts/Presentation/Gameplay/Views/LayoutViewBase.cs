using ContractsInterfaces.ViewsApplication;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Presentation.Gameplay.Views
{
    /// <summary>
    /// Базовый класс для всех layout views. 
    /// Пассивное хранилище компонентов с UIToolkit.
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public abstract class LayoutViewBase : MonoBehaviour, ILayoutView
    {
        /// <summary>
        /// Корневой элемент UI для этого View.
        /// </summary>
        protected VisualElement Root;

        /// <summary>
        /// Ссылка на компонент UIDocument.
        /// </summary>
        protected UIDocument UiDocument;

        /// <summary>
        /// Стандартный метод MonoBehaviour. Вызывается при загрузке экземпляра скрипта.
        /// </summary>
        public virtual void Awake()
        {
            this.UiDocument = this.GetComponent<UIDocument>();
            this.Root = this.UiDocument.rootVisualElement;
            this.InitializeElements();
            this.Hide();
        }

        /// <summary>
        /// Инициализирует конкретные элементы UI. Предназначен для переопределения в дочерних классах.
        /// </summary>
        protected virtual void InitializeElements()
        {
        }

        /// <inheritdoc/>
        public virtual async UniTask ShowAsync()
        {
            await UniTask.Yield();
            this.Show();
        }

        /// <inheritdoc/>
        public virtual void Show()
        {
            this.Root.style.display = DisplayStyle.Flex;
        }

        /// <inheritdoc/>
        public virtual void Hide()
        {
            this.Root.style.display = DisplayStyle.None;
        }
    }
}