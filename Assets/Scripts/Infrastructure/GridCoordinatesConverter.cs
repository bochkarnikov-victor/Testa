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
        private readonly int _width;
        private readonly int _height;
        private const float CellSize = 1.0f;

        public GridCoordinatesConverter(Camera mainCamera, int width, int height)
        {
            this._mainCamera = mainCamera;
            this._width = width;
            this._height = height;
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

                float halfWidth = this._width / 2f * CellSize;
                float halfHeight = this._height / 2f * CellSize;

                int x = Mathf.FloorToInt((worldPoint.x + halfWidth) / CellSize);
                int z = Mathf.FloorToInt((worldPoint.z + halfHeight) / CellSize);

                return new GridPos(x, z);
            }

            return new GridPos(int.MinValue, int.MinValue);
        }

        /// <inheritdoc/>
        public Vector3 GridToWorld(GridPos gridPos)
        {
            float halfWidth = this._width / 2f * CellSize;
            float halfHeight = this._height / 2f * CellSize;

            return new Vector3(
                gridPos.X * CellSize - halfWidth + CellSize * 0.5f,
                0,
                gridPos.Y * CellSize - halfHeight + CellSize * 0.5f);
        }
    }
}