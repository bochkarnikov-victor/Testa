#nullable enable

using ContractsInterfaces.Infrastructure;
using Domain.Gameplay.Models;
using Presentation.Gameplay.Views;
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
        private readonly GridView _gridView;
        private const float CellSize = 1.0f;

        public GridCoordinatesConverter(Camera mainCamera, GridView gridView)
        {
            this._mainCamera = mainCamera;
            this._gridView = gridView;
        }

        /// <inheritdoc/>
        public GridPos ScreenToGrid(NVec2 screenPosition)
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);

            var screenPosVec3 = new Vector3(screenPosition.X, screenPosition.Y, this._mainCamera.nearClipPlane);
            Ray ray = this._mainCamera.ScreenPointToRay(screenPosVec3);

            if (groundPlane.Raycast(ray, out float enter))
            {
                Vector3 worldPoint = ray.GetPoint(enter);

                Vector3 gridOriginOffset = new Vector3(this._gridView.Width / 2f * CellSize, 0, this._gridView.Height / 2f * CellSize);
                Vector3 gridBottomLeft = Vector3.zero - gridOriginOffset;
                Vector3 relativePoint = worldPoint - gridBottomLeft;

                int x = Mathf.FloorToInt(relativePoint.x / CellSize);
                int z = Mathf.FloorToInt(relativePoint.z / CellSize);

                return new GridPos(x, z);
            }

            return new GridPos(int.MinValue, int.MinValue);
        }

        /// <inheritdoc/>
        public Vector3 GridToWorld(GridPos gridPos)
        {
            var relativePos = new Vector3(
                gridPos.X * CellSize + CellSize * 0.5f,
                0,
                gridPos.Y * CellSize + CellSize * 0.5f);

            Vector3 gridOriginOffset = new Vector3(this._gridView.Width / 2f * CellSize, 0, this._gridView.Height / 2f * CellSize);
            Vector3 gridBottomLeft = Vector3.zero - gridOriginOffset;

            return gridBottomLeft + relativePos;
        }
    }
}
