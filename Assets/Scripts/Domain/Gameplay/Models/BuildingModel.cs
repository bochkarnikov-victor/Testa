#nullable enable

using System;
using System.Collections.Generic;
using R3;

namespace Domain.Gameplay.Models
{
    /// <summary>
    /// Представляет экземпляр здания на игровой карте.
    /// </summary>
    public class BuildingModel : BuildingModelBase
    {
        private readonly ReactiveProperty<GridPos> _position;

        /// <summary>
        /// Реактивное свойство, содержащее текущую позицию здания на сетке.
        /// </summary>
        public ReadOnlyReactiveProperty<GridPos> Position => this._position;

        /// <summary>
        /// Список всех доступных уровней для этого здания.
        /// </summary>
        public IReadOnlyList<BuildingLevel> Levels { get; }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="BuildingModel"/>.
        /// </summary>
        /// <param name="id">Уникальный идентификатор экземпляра.</param>
        /// <param name="type">Тип здания.</param>
        /// <param name="position">Начальная позиция на сетке.</param>
        /// <param name="levels">Список всех доступных уровней.</param>
        public BuildingModel(Guid id, BuildingType type, GridPos position, IReadOnlyList<BuildingLevel> levels)
            : base(id, type, 1)
        {
            this._position = new ReactiveProperty<GridPos>(position);
            this.Levels = levels;
        }

        /// <summary>
        /// Внутренний метод для изменения позиции здания. Используется слоем UseCases.
        /// </summary>
        /// <param name="newPosition">Новая позиция.</param>
        internal void SetPosition(GridPos newPosition)
        {
            this._position.Value = newPosition;
        }

        /// <summary>
        /// Внутренний метод для попытки улучшения здания до следующего уровня.
        /// </summary>
        /// <returns>True, если улучшение прошло успешно, иначе false.</returns>
        public bool TryUpgrade()
        {
            if (this.CurrentLevel.CurrentValue >= this.Levels.Count)
            {
                return false;
            }

            this.SetCurrentLevel(this.CurrentLevel.CurrentValue + 1);
            return true;
        }
    }
}