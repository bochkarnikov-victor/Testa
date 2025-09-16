namespace Domain.Gameplay.Models
{
    /// <summary>
    /// Перечисление типов игровых ресурсов.
    /// </summary>
    public enum ResourceType
    {
        /// <summary>
        /// Неопределенный тип ресурса.
        /// </summary>
        None = 0,

        /// <summary>
        /// Золото.
        /// </summary>
        Gold = 1,

        /// <summary>
        /// Дерево.
        /// </summary>
        Wood = 2,

        /// <summary>
        /// Кристаллы.
        /// </summary>
        Crystals = 3,
    }
}
