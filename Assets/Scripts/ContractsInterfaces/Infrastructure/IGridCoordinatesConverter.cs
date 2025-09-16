#nullable enable

using System.Numerics;
using Domain.Gameplay.Models;

namespace ContractsInterfaces.Infrastructure
{
    /// <summary>
    /// Определяет контракт для сервиса, который преобразует координаты между разными системами (экран, мир, сетка).
    /// </summary>
    public interface IGridCoordinatesConverter
    {
        /// <summary>
        /// Преобразует позицию на экране в позицию на игровой сетке.
        /// </summary>
        /// <param name="screenPosition">Позиция на экране (в пикселях).</param>
        /// <returns>Позиция в координатах сетки.</returns>
        GridPos ScreenToGrid(Vector2 screenPosition);
    }
}