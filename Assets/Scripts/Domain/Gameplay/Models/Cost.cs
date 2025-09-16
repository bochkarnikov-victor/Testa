#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Gameplay.Models
{
    /// <summary>
    /// Представляет стоимость в игровых ресурсах. Может быть составной (например, 100 золота + 50 дерева).
    /// </summary>
    [Serializable]
    public readonly struct Cost : IEquatable<Cost>
    {
        private readonly IReadOnlyDictionary<ResourceType, int> _resourceCosts;

        /// <summary>
        /// Пустая стоимость.
        /// </summary>
        public static readonly Cost Zero = new(new Dictionary<ResourceType, int>());

        /// <summary>
        /// Инициализирует новую стоимость на основе словаря.
        /// </summary>
        public Cost(IReadOnlyDictionary<ResourceType, int> resourceCosts)
        {
            this._resourceCosts = resourceCosts;
        }

        /// <summary>
        /// Инициализирует новую стоимость для одного ресурса.
        /// </summary>
        public Cost(ResourceType type, int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Cost value cannot be negative.");
            }

            this._resourceCosts = new Dictionary<ResourceType, int> { { type, value } };
        }

        /// <summary>
        /// Возвращает стоимость для конкретного типа ресурса.
        /// </summary>
        public int GetCost(ResourceType type)
        {
            return this._resourceCosts.GetValueOrDefault(type, 0);
        }

        /// <summary>
        /// Возвращает все типы ресурсов, которые требуются для этой стоимости.
        /// </summary>
        public IEnumerable<ResourceType> GetResourceTypes()
        {
            return this._resourceCosts.Keys;
        }

        /// <inheritdoc/>
        public bool Equals(Cost other)
        {
            if (this._resourceCosts.Count != other._resourceCosts.Count)
            {
                return false;
            }

            foreach (KeyValuePair<ResourceType, int> kvp in this._resourceCosts)
            {
                if (!other._resourceCosts.TryGetValue(kvp.Key, out int otherValue) || otherValue != kvp.Value)
                {
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is Cost other && this.Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            HashCode hc = new();
            foreach (KeyValuePair<ResourceType, int> kvp in this._resourceCosts)
            {
                hc.Add(kvp.Key);
                hc.Add(kvp.Value);
            }

            return hc.ToHashCode();
        }


        /// <summary>
        /// Складывает две стоимости.
        /// </summary>
        public static Cost operator +(Cost a, Cost b)
        {
            Dictionary<ResourceType, int> result = new(a._resourceCosts);
            foreach (KeyValuePair<ResourceType, int> kvp in b._resourceCosts)
            {
                result[kvp.Key] = result.GetValueOrDefault(kvp.Key, 0) + kvp.Value;
            }

            return new Cost(result);
        }
    }
}