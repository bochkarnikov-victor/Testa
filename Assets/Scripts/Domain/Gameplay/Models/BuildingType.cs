#nullable enable

namespace Domain.Gameplay.Models
{
    /// <summary>
    /// Перечисление, представляющее типы зданий в игре.
    /// </summary>
    public enum BuildingType
    {
        /// <summary>
        /// Неопределенный тип здания.
        /// </summary>
        None = 0,

        /// <summary>
        /// Дом, приносящий доход.
        /// </summary>
        House = 1,

        /// <summary>
        /// Ферма для производства ресурсов.
        /// </summary>
        Farm = 2,

        /// <summary>
        /// Шахта для добычи ресурсов.
        /// </summary>
        Mine = 3
    }
}