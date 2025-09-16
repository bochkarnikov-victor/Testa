#nullable enable

using Domain.Gameplay.Models;
using UnityEngine;

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
        GridPos ScreenToGrid(System.Numerics.Vector2 screenPosition);

        /// <summary>
        /// Преобразует позицию на игровой сетке в позицию в мировых координатах.
        /// </summary>
        /// <param name="gridPos">Позиция в координатах сетки.</param>
        /// <returns>Позиция в мировых координатах.</returns>
        Vector3 GridToWorld(GridPos gridPos);
    }
}