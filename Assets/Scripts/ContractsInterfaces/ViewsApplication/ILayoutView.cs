using Cysharp.Threading.Tasks;

namespace ContractsInterfaces.ViewsApplication
{
    /// <summary>
    /// Интерфейс для всех View в MVP паттерне, управляющих слоями UI.
    /// </summary>
    public interface ILayoutView
    {
        /// <summary>
        /// Асинхронно показывает View.
        /// </summary>
        /// <returns>Задача, завершающаяся после отображения View.</returns>
        UniTask ShowAsync();

        /// <summary>
        /// Синхронно и немедленно показывает View.
        /// </summary>
        void Show();

        /// <summary>
        /// Синхронно и немедленно скрывает View.
        /// </summary>
        void Hide();
    }
}