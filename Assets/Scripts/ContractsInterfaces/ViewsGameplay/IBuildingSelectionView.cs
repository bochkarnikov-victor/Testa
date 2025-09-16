#nullable enable

using System;
using ContractsInterfaces.ViewsApplication;
using Domain.Gameplay.Models;
using UnityEngine;

namespace ContractsInterfaces.ViewsGameplay
{
    /// <summary>
    /// Интерфейс для View, отображающего панель выбора зданий.
    /// </summary>
    public interface IBuildingSelectionView : ILayoutView
    {
        /// <summary>
        /// Событие, которое вызывается при выборе типа здания для постройки.
        /// </summary>
        event Action<BuildingType> OnBuildingTypeSelected;

        /// <summary>
        /// Устанавливает иконку для кнопки выбора здания.
        /// </summary>
        /// <param name="type">Тип здания.</param>
        /// <param name="sprite">Спрайт для установки.</param>
        void SetBuildingSprite(BuildingType type, Sprite sprite);
    }
}