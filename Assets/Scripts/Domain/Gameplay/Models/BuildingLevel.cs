#nullable enable

namespace Domain.Gameplay.Models
{
    /// <summary>
    /// Представляет данные для определенного уровня здания.
    /// </summary>
    public readonly struct BuildingLevel
    {
        /// <summary>
        /// Номер уровня (начиная с 1).
        /// </summary>
        public int Level { get; }

        /// <summary>
        /// Стоимость постройки или улучшения до этого уровня.
        /// </summary>
        public Cost Cost { get; }

        /// <summary>
        /// Доход, генерируемый на этом уровне.
        /// </summary>
        public Income Income { get; }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="BuildingLevel"/>.
        /// </summary>
        /// <param name="level">Номер уровня.</param>
        /// <param name="cost">Стоимость.</param>
        /// <param name="income">Доход.</param>
        public BuildingLevel(int level, Cost cost, Income income)
        {
            this.Level = level;
            this.Cost = cost;
            this.Income = income;
        }
    }
}