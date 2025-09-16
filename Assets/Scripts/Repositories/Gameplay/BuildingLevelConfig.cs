using System;
using System.Collections.Generic;
using ContractsInterfaces.UseCasesGameplay;
using Domain.Gameplay.MessagesDTO;
using Domain.Gameplay.Models;
using UnityEngine;

namespace Repositories.Gameplay
{
    /// <summary>
    /// Конфигурация одного уровня здания. Содержит "сырые" данные для редактирования в инспекторе,
    /// из которых на лету создаются доменные объекты Cost и Income.
    /// </summary>
    [Serializable]
    public class BuildingLevelConfig : IBuildingLevelData
    {
        [SerializeField] private ResourceAmount[] _costAuthoring = Array.Empty<ResourceAmount>();
        [SerializeField] private ResourceAmount[] _incomeAuthoring = Array.Empty<ResourceAmount>();

        [NonSerialized] private Cost _cost;
        [NonSerialized] private Income _income;
        [NonSerialized] private bool _built;

        /// <summary>
        /// Стоимость постройки или улучшения до этого уровня.
        /// </summary>
        public Cost Cost
        {
            get
            {
                this.BuildIfNeeded();
                return this._cost;
            }
        }

        /// <summary>
        /// Доход, который приносит здание на этом уровне.
        /// </summary>
        public Income Income
        {
            get
            {
                this.BuildIfNeeded();
                return this._income;
            }
        }

        Cost IBuildingLevelData.Cost => this.Cost;
        Income IBuildingLevelData.Income => this.Income;

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.Build(true);
        }
#endif

        private void BuildIfNeeded()
        {
            if (!this._built)
            {
                this.Build(true);
            }
        }

        private void Build(bool force)
        {
            if (this._built && !force)
            {
                return;
            }

            Dictionary<ResourceType, int> cost = new();
            foreach (ResourceAmount ra in this._costAuthoring)
            {
                if (ra.Amount <= 0)
                {
                    continue;
                }

                cost[ra.Type] = cost.GetValueOrDefault(ra.Type, 0) + ra.Amount;
            }

            Dictionary<ResourceType, int> income = new();
            foreach (ResourceAmount ra in this._incomeAuthoring)
            {
                if (ra.Amount <= 0)
                {
                    continue;
                }

                income[ra.Type] = income.GetValueOrDefault(ra.Type, 0) + ra.Amount;
            }

            this._cost = new Cost(cost);
            this._income = new Income(income);
            this._built = true;
        }
    }
}