#nullable enable

using Domain.Gameplay.Models;
using UnityEngine;

namespace Presentation.Gameplay.Views
{
    /// <summary>
    /// Отвечает за визуальное отображение игровой сетки и подсветку клеток с помощью Gizmos.
    /// </summary>
    public class GridView : MonoBehaviour
    {
        [Header("Grid Settings")]
        [SerializeField] private int _width = 32;
        [SerializeField] private int _height = 32;
        [SerializeField] private float _cellSize = 1.0f;
        [SerializeField] private Color _gridColor = Color.gray;

        private GridPos? _highlightedCell;
        private Color _highlightColor;

        /// <summary>
        /// Подсвечивает указанную клетку заданным цветом.
        /// </summary>
        /// <param name="position">Позиция клетки для подсветки.</param>
        /// <param name="color">Цвет подсветки.</param>
        public void HighlightCell(GridPos position, Color color)
        {
            this._highlightedCell = position;
            this._highlightColor = color;
        }

        /// <summary>
        /// Убирает подсветку со всех клеток.
        /// </summary>
        public void ClearHighlight()
        {
            this._highlightedCell = null;
        }

        private void OnDrawGizmos()
        {
            DrawGrid();
            DrawHighlight();
        }

        private void DrawGrid()
        {
            Gizmos.color = this._gridColor;
            
            float halfWidth = this._width / 2f * this._cellSize;
            float halfHeight = this._height / 2f * this._cellSize;

            // Вертикальные линии (вдоль оси Z)
            for (int x = 0; x <= this._width; x++)
            {
                var start = new Vector3(x * this._cellSize - halfWidth, 0, -halfHeight);
                var end = new Vector3(x * this._cellSize - halfWidth, 0, halfHeight);
                Gizmos.DrawLine(start, end);
            }

            // Горизонтальные линии (вдоль оси X)
            for (int z = 0; z <= this._height; z++)
            {
                var start = new Vector3(-halfWidth, 0, z * this._cellSize - halfHeight);
                var end = new Vector3(halfWidth, 0, z * this._cellSize - halfHeight);
                Gizmos.DrawLine(start, end);
            }
        }

        private void DrawHighlight()
        {
            if (this._highlightedCell.HasValue)
            {
                Gizmos.color = this._highlightColor;
                var pos = this._highlightedCell.Value;
                
                // Центр клетки в мировых координатах (Y в GridPos соответствует Z в мире)
                var center = new Vector3(pos.X * this._cellSize, 0.05f, pos.Y * this._cellSize);
                
                Gizmos.DrawCube(center, new Vector3(this._cellSize, 0.01f, this._cellSize));
            }
        }
    }
}