#nullable enable

using System;
using R3;

namespace Domain.Gameplay.Models
{
    /// <summary>
    /// Базовый класс для всех моделей зданий, содержащий общие свойства.
    /// </summary>
    public abstract class BuildingModelBase
    {
        /// <summary>
        /// Уникальный идентификатор экземпляра здания.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Тип здания.
        /// </summary>
        public BuildingType Type { get; }

        private readonly ReactiveProperty<int> _currentLevel;

        /// <summary>
        /// Реактивное свойство, содержащее текущий уровень здания.
        /// <remarks>
        /// Уровень является 1-based (начинается с 1), в то время как индексы в списках уровней (Levels) являются 0-based.
        /// </remarks>
        /// </summary>
        public ReadOnlyReactiveProperty<int> CurrentLevel => this._currentLevel;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="BuildingModelBase"/>.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="type">Тип здания.</param>
        /// <param name="initialLevel">Начальный уровень.</param>
        protected BuildingModelBase(Guid id, BuildingType type, int initialLevel)
        {
            this.Id = id;
            this.Type = type;
            this._currentLevel = new ReactiveProperty<int>(initialLevel);
        }

        /// <summary>
        /// Устанавливает новый уровень здания. Предназначено для использования дочерними классами.
        /// </summary>
        /// <param name="newLevel">Новый уровень.</param>
        protected void SetCurrentLevel(int newLevel)
        {
            this._currentLevel.Value = newLevel;
        }
    }
}