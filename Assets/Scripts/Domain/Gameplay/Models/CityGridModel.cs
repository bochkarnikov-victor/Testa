#nullable enable

using System;
using System.Collections.Generic;

namespace Domain.Gameplay.Models
{
    /// <summary>
    /// Представляет модель игровой сетки города, управляющую состоянием и размещением зданий.
    /// Является единственным источником правды о том, какие клетки заняты.
    /// </summary>
    public class CityGridModel
    {
        private readonly Dictionary<Guid, BuildingModel> _buildingsById = new();
        private readonly Dictionary<GridPos, BuildingModel> _occupiedCells = new();

        public int Width { get; }
        public int Height { get; }

        public CityGridModel(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Возвращает коллекцию всех зданий, находящихся на сетке.
        /// </summary>
        public IReadOnlyCollection<BuildingModel> Buildings => this._buildingsById.Values;

        /// <summary>
        /// Проверяет, находится ли позиция в пределах границ сетки.
        /// </summary>
        public bool IsWithinBounds(GridPos position)
        {
            return position.X >= 0 && position.X < this.Width && position.Y >= 0 && position.Y < this.Height;
        }

        /// <summary>
        /// Проверяет, занята ли указанная клетка.
        /// </summary>
        /// <param name="position">Позиция для проверки.</param>
        /// <returns>True, если клетка занята, иначе false.</returns>
        public bool IsCellOccupied(GridPos position)
        {
            return this._occupiedCells.ContainsKey(position);
        }

        /// <summary>
        /// Пытается получить здание в указанной клетке.
        /// </summary>
        /// <param name="position">Позиция для поиска.</param>
        /// <returns>Найденное здание или null, если клетка пуста.</returns>
        public BuildingModel? GetBuildingAt(GridPos position)
        {
            return this._occupiedCells.GetValueOrDefault(position);
        }

        /// <summary>
        /// Пытается получить здание по его уникальному ID.
        /// </summary>
        /// <param name="buildingId">Уникальный идентификатор здания.</param>
        /// <returns>Найденное здание или null, если здание с таким ID не найдено.</returns>
        public BuildingModel? GetBuildingById(Guid buildingId)
        {
            return this._buildingsById.GetValueOrDefault(buildingId);
        }

        /// <summary>
        /// Пытается добавить новое здание на сетку.
        /// </summary>
        /// <param name="buildingModel">Модель здания для добавления. Может быть null.</param>
        /// <returns>True, если здание успешно добавлено, иначе false.</returns>
        public bool TryAddBuilding(BuildingModel? buildingModel)
        {
            if (buildingModel is null)
            {
                return false;
            }

            if (this.IsCellOccupied(buildingModel.Position.CurrentValue) ||
                this._buildingsById.ContainsKey(buildingModel.Id))
            {
                return false;
            }

            this._buildingsById[buildingModel.Id] = buildingModel;
            this._occupiedCells[buildingModel.Position.CurrentValue] = buildingModel;
            return true;
        }

        /// <summary>
        /// Пытается удалить здание с сетки.
        /// </summary>
        /// <param name="buildingModel">Модель здания для удаления. Может быть null.</param>
        /// <returns>True, если здание успешно удалено, иначе false.</returns>
        public bool TryRemoveBuilding(BuildingModel? buildingModel)
        {
            if (buildingModel is null)
            {
                return false;
            }

            if (!this._buildingsById.ContainsKey(buildingModel.Id))
            {
                return false;
            }

            this._buildingsById.Remove(buildingModel.Id);
            this._occupiedCells.Remove(buildingModel.Position.CurrentValue);
            return true;
        }

        /// <summary>
        /// Пытается переместить существующее здание на новую позицию.
        /// </summary>
        /// <param name="buildingModel">Модель здания для перемещения. Может быть null.</param>
        /// <param name="newPosition">Новая позиция для здания.</param>
        /// <returns>True, если здание успешно перемещено, иначе false.</returns>
        public bool TryMoveBuilding(BuildingModel? buildingModel, GridPos newPosition)
        {
            if (buildingModel is null)
            {
                return false;
            }

            if (!this._buildingsById.ContainsKey(buildingModel.Id) || this.IsCellOccupied(newPosition))
            {
                return false;
            }

            GridPos oldPosition = buildingModel.Position.CurrentValue;
            this._occupiedCells.Remove(oldPosition);
            this._occupiedCells[newPosition] = buildingModel;
            buildingModel.SetPosition(newPosition);

            return true;
        }
    }
}