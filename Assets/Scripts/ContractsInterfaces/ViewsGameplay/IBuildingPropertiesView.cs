#nullable enable

using ContractsInterfaces.ViewsApplication;
using Domain.Gameplay.Models;
using R3;

namespace ContractsInterfaces.ViewsGameplay
{
    /// <summary>
    /// Контракт для View, отображающего панель свойств выбранного здания.
    /// </summary>
    public interface IBuildingPropertiesView : ILayoutView
    {
        /// <summary>
        /// Событие, вызываемое при нажатии на кнопку "Улучшить".
        /// </summary>
        Observable<Unit> OnUpgradeClicked { get; }

        /// <summary>
        /// Событие, вызываемое при нажатии на кнопку "Удалить".
        /// </summary>
        Observable<Unit> OnRemoveClicked { get; }

        /// <summary>
        /// Показывает и обновляет панель с данными о выбранном здании.
        /// </summary>
        /// <param name="building">Модель здания для отображения.</param>
        void Show(BuildingModel building);
    }
}