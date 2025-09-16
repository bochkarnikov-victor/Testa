#nullable enable

using System;
using System.Collections.Generic;
using ContractsInterfaces.UseCasesGameplay;
using Domain.Gameplay.Models;
using R3;
using Serilog;

namespace UseCases.Gameplay
{
    /// <summary>
    /// UseCase, реализующий всю логику по изменению, проверке и запросу состояния ресурсов игрока.
    /// </summary>
    public class ResourceManagerUseCase : IResourceActions, IResourceQueries
    {
        private readonly IPlayerResourcesRepository _repository;
        private readonly ILogger _logger;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="ResourceManagerUseCase"/>.
        /// </summary>
        /// <param name="repository">Репозиторий для доступа к модели ресурсов.</param>
        /// <param name="logger">Интерфейс для логирования.</param>
        public ResourceManagerUseCase(IPlayerResourcesRepository repository, ILogger logger)
        {
            this._repository = repository;
            this._logger = logger.ForContext<ResourceManagerUseCase>();
        }

        /// <inheritdoc />
        public bool HasEnough(Cost cost)
        {
            foreach (ResourceType resourceType in cost.GetResourceTypes())
            {
                int requiredAmount = cost.GetCost(resourceType);
                int currentAmount = this._repository.TryGet(resourceType, out ReactiveProperty<int> res)
                    ? res.Value
                    : 0;
                if (currentAmount < requiredAmount)
                {
                    this._logger.Warning("[ResourceManagerUseCase.HasEnough] Not enough {ResourceType}. Required: {Required}, Have: {Current}",
                        resourceType, requiredAmount, currentAmount);
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc />
        public void Spend(Cost cost)
        {
            this._logger.Debug("[ResourceManagerUseCase.Spend] Spending resources: {Cost}", cost);
            foreach (ResourceType resourceType in cost.GetResourceTypes())
            {
                int requiredAmount = cost.GetCost(resourceType);
                if (this._repository.TryGet(resourceType, out ReactiveProperty<int> resource))
                {
                    resource.Value -= requiredAmount;
                }
                else
                {
                    this._logger.Error("[ResourceManagerUseCase.Spend] Attempted to spend non-existent resource {ResourceType}", resourceType);
                }
            }
        }

        /// <inheritdoc />
        public void Earn(Income income)
        {
            this._logger.Debug("[ResourceManagerUseCase.Earn] Earning resources: {Income}", income);
            foreach (ResourceType resourceType in income.GetResourceTypes())
            {
                int earnedAmount = income.GetIncome(resourceType);
                ReactiveProperty<int> resource = this._repository.GetOrCreate(resourceType);
                resource.Value += earnedAmount;
            }
        }

        /// <inheritdoc />
        public IReadOnlyDictionary<ResourceType, int> GetCurrentResources()
        {
            Dictionary<ResourceType, int> currentResources = new();
            foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
            {
                if (resourceType == ResourceType.None)
                {
                    continue;
                }

                if (this._repository.TryGet(resourceType, out ReactiveProperty<int> resource))
                {
                    currentResources[resourceType] = resource.Value;
                }
                else
                {
                    currentResources[resourceType] = 0;
                }
            }

            return currentResources;
        }
    }
}