namespace Domain.Gameplay.MessagesDTO
{
    /// <summary>
    /// Событие, анонсирующее, что состояние игры было успешно загружено из сохранения.
    /// </summary>
    public readonly struct GameStateLoadedEvent
    {
        /// <summary>
        /// DTO с полным состоянием загруженной игры.
        /// </summary>
        public GameStateDTO GameState { get; }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="GameStateLoadedEvent"/>.
        /// </summary>
        /// <param name="gameState">Загруженное состояние игры.</param>
        public GameStateLoadedEvent(GameStateDTO gameState)
        {
            this.GameState = gameState;
        }
    }
}