#nullable enable

using System;
using System.Collections.Generic;

namespace Domain.Gameplay.Models
{
    /// <summary>
    /// Представляет доход в игровых ресурсах. Может быть составным.
    /// </summary>
    [Serializable]
    public readonly struct Income : IEquatable<Income>
    {
        private readonly IReadOnlyDictionary<ResourceType, int> _resourceIncomes;

        /// <summary>
        /// Пустой доход.
        /// </summary>
        public static readonly Income Zero = new(new Dictionary<ResourceType, int>());

        /// <summary>
        /// Инициализирует доход на основе словаря.
        /// </summary>
        public Income(IReadOnlyDictionary<ResourceType, int> resourceIncomes)
        {
            this._resourceIncomes = resourceIncomes;
        }

        /// <summary>
        /// Инициализирует доход для одного ресурса.
        /// </summary>
        public Income(ResourceType type, int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Income value cannot be negative.");
            }

            this._resourceIncomes = new Dictionary<ResourceType, int> { { type, value } };
        }

        /// <summary>
        /// Возвращает доход для конкретного типа ресурса.
        /// </summary>
        public int GetIncome(ResourceType type)
        {
            return this._resourceIncomes.GetValueOrDefault(type, 0);
        }

        /// <summary>
        /// Возвращает все типы ресурсов, которые приносит этот доход.
        /// </summary>
        public IEnumerable<ResourceType> GetResourceTypes()
        {
            return this._resourceIncomes.Keys;
        }

        /// <inheritdoc/>
        public bool Equals(Income other)
        {
            if (this._resourceIncomes.Count != other._resourceIncomes.Count)
            {
                return false;
            }

            foreach (KeyValuePair<ResourceType, int> kvp in this._resourceIncomes)
            {
                if (!other._resourceIncomes.TryGetValue(kvp.Key, out int otherValue) || otherValue != kvp.Value)
                {
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is Income other && this.Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            HashCode hc = new();
            foreach (KeyValuePair<ResourceType, int> kvp in this._resourceIncomes)
            {
                hc.Add(kvp.Key);
                hc.Add(kvp.Value);
            }

            return hc.ToHashCode();
        }

        /// <summary>
        /// Складывает два дохода.
        /// </summary>
        public static Income operator +(Income a, Income b)
        {
            Dictionary<ResourceType, int> result = new(a._resourceIncomes);
            foreach (KeyValuePair<ResourceType, int> kvp in b._resourceIncomes)
            {
                result[kvp.Key] = result.GetValueOrDefault(kvp.Key, 0) + kvp.Value;
            }

            return new Income(result);
        }
    }
}