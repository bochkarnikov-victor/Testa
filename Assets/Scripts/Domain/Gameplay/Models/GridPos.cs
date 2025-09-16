#nullable enable

using System;

namespace Domain.Gameplay.Models
{
    /// <summary>
    /// Представляет позицию на игровой сетке.
    /// </summary>
    public readonly struct GridPos : IEquatable<GridPos>
    {
        /// <summary>
        /// Координата X.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Координата Y.
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="GridPos"/>.
        /// </summary>
        /// <param name="x">Координата X.</param>
        /// <param name="y">Координата Y.</param>
        public GridPos(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <inheritdoc/>
        public bool Equals(GridPos other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is GridPos other && this.Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(this.X, this.Y);
        }

        /// <summary>
        /// Сравнивает два экземпляра <see cref="GridPos"/> на равенство.
        /// </summary>
        public static bool operator ==(GridPos left, GridPos right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Сравнивает два экземпляра <see cref="GridPos"/> на неравенство.
        /// </summary>
        public static bool operator !=(GridPos left, GridPos right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"({this.X}, {this.Y})";
        }
    }
}