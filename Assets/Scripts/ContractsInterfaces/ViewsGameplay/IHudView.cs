#nullable enable

using System;
using ContractsInterfaces.ViewsApplication;
using Domain.Gameplay.Models;
using R3;

namespace ContractsInterfaces.ViewsGameplay
{
    /// <summary>
    /// Контракт для View, отображающего основную игровую информацию (HUD).
    /// </summary>
    public interface IHudView : ILayoutView
    {
        /// <summary>
        /// Событие нажатия на кнопку 'Сохранить'.
        /// </summary>
        Observable<Unit> OnSaveClick { get; }

        /// <summary>
        /// Событие нажатия на кнопку 'Загрузить'.
        /// </summary>
        Observable<Unit> OnLoadClick { get; }

        /// <summary>
        /// Обновляет отображаемое количество указанного ресурса.
        /// </summary>
        /// <param name="resourceType">Тип ресурса для обновления.</param>
        /// <param name="amount">Новое количество ресурса.</param>
        void UpdateResource(ResourceType resourceType, int amount);
    }
}