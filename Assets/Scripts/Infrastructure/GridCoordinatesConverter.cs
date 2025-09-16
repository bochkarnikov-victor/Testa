#nullable enable

using ContractsInterfaces.Infrastructure;
using Domain.Gameplay.Models;
using UnityEngine;
using NVec2 = System.Numerics.Vector2;

namespace Infrastructure
{
    /// <summary>
    /// Реализация сервиса, который преобразует координаты между разными системами.
    /// </summary>
    public class GridCoordinatesConverter : IGridCoordinatesConverter
    {
        private readonly Camera _mainCamera;
        private const float CellSize = 1.0f;

        public GridCoordinatesConverter(Camera mainCamera)
        {
            this._mainCamera = mainCamera;
        }

        /// <inheritdoc/>
        public GridPos ScreenToGrid(NVec2 screenPosition)
        {
            var screenPosVec3 = new Vector3(screenPosition.X, screenPosition.Y, this._mainCamera.nearClipPlane);
            
            Ray ray = this._mainCamera.ScreenPointToRay(screenPosVec3);
            
            var groundPlane = new Plane(Vector3.up, Vector3.zero);

            if (groundPlane.Raycast(ray, out float enter))
            {
                Vector3 worldPoint = ray.GetPoint(enter);
                
                int x = Mathf.RoundToInt(worldPoint.x / CellSize);
                int z = Mathf.RoundToInt(worldPoint.z / CellSize);

                return new GridPos(x, z);
            }

            return new GridPos(int.MinValue, int.MinValue);
        }
    }
}