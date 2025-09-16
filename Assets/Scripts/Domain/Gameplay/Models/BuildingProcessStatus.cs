namespace Domain.Gameplay.Models
{
    /// <summary>
    /// Перечисление, представляющее статус процесса строительства здания
    /// </summary>
    public enum BuildingProcessStatus
    {
        /// <summary>
        /// Здание не строится
        /// </summary>
        None = 0,

        /// <summary>
        /// Здание в процессе строительства
        /// </summary>
        Constructing = 1,

        /// <summary>
        /// Здание построено
        /// </summary>
        Constructed = 2
    }
}