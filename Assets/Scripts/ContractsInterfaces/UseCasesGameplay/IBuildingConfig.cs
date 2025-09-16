#nullable enable

using System.Collections.Generic;
using Domain.Gameplay.Models;
using UnityEngine;

namespace ContractsInterfaces.UseCasesGameplay
{
    /// <summary>
    /// Контракт, описывающий полную конфигурацию одного типа здания.
    /// </summary>
    public interface IBuildingConfig
    {
        /// <summary>
        /// Префаб здания.
        /// </summary>
        GameObject Prefab { get; }


        /// <summary>
        /// Тип здания.
        /// </summary>
        BuildingType BuildingType { get; }

        /// <summary>
        /// Иконка здания для отображения в UI.
        /// </summary>
        Sprite Sprite { get; }

        /// <summary>
        /// Список данных для каждого уровня здания.
        /// </summary>
        IReadOnlyList<IBuildingLevelData> Levels { get; }
    }
}