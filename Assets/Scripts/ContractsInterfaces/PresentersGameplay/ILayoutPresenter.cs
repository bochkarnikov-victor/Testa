using Cysharp.Threading.Tasks;

namespace ContractsInterfaces.PresentersGameplay
{
    /// <summary>
    /// Интерфейс для всех Presenter'ов в MVP паттерне, управляющих логикой отображения.
    /// </summary>
    public interface ILayoutPresenter
    {
        /// <summary>
        /// Асинхронно активирует и показывает связанный с презентером View.
        /// </summary>
        /// <returns>Задача, завершающаяся после активации.</returns>
        UniTask ActivateAsync();
    }
}