#nullable enable

using System.Collections.Generic;
using ContractsInterfaces.DomainGameplay;
using Domain.Gameplay.Models;
using R3;

namespace Domain.Application.Models
{
    /// <summary>
    /// Реализация модели, хранящей информацию о ресурсах игрока.
    /// Является "глупым" хранилищем данных.
    /// </summary>
    public class PlayerResourcesModel : IPlayerResourcesModel
    {
        /// <summary>
        /// Словарь с реактивными свойствами для каждого ресурса.
        /// </summary>
        public readonly Dictionary<ResourceType, ReactiveProperty<int>> Resources = new();

        /// <summary>
        /// Инициализирует новую модель с начальным набором ресурсов.
        /// </summary>
        /// <param name="startingResources">Словарь с начальным количеством ресурсов.</param>
        public PlayerResourcesModel(IReadOnlyDictionary<ResourceType, int> startingResources)
        {
            foreach (KeyValuePair<ResourceType, int> kvp in startingResources)
            {
                this.Resources[kvp.Key] = new ReactiveProperty<int>(kvp.Value);
            }
        }

        /// <inheritdoc />
        public ReadOnlyReactiveProperty<int> GetResource(ResourceType type)
        {
            // Явно возвращаем как ReadOnlyReactiveProperty, чтобы соответствовать интерфейсу
            return this.Resources[type].ToReadOnlyReactiveProperty();
        }
    }
}