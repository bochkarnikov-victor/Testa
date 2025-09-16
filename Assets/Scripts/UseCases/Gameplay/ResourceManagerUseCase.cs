#nullable enable

using System;
using System.Collections.Generic;
using ContractsInterfaces.UseCasesGameplay;
using Domain.Gameplay.MessagesDTO;
using Domain.Gameplay.Models;
using MessagePipe;
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
        private readonly IPublisher<ResourcesChangedEvent> _resourcesChangedPublisher;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="ResourceManagerUseCase"/>.
        /// </summary>
        public ResourceManagerUseCase(IPlayerResourcesRepository repository, ILogger logger, IPublisher<ResourcesChangedEvent> resourcesChangedPublisher)
        {
            this._repository = repository;
            this._logger = logger.ForContext<ResourceManagerUseCase>();
            this._resourcesChangedPublisher = resourcesChangedPublisher;
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
            this.PublishChanges();
        }

        /// <inheritdoc />
        public void Earn(Income income)
        {
            this._logger.Debug("[ResourceManagerUseCase.Earn] Earning resources: {Income}", income);
            foreach (ResourceType resourceType in income.GetResourceTypes())
            {
                int earnedAmount = income.GetIncome(resourceType);
                if (earnedAmount == 0) continue;
                
                ReactiveProperty<int> resource = this._repository.GetOrCreate(resourceType);
                resource.Value += earnedAmount;
            }
            this.PublishChanges();
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
        
        private void PublishChanges()
        {
            IReadOnlyDictionary<ResourceType, int> currentResources = this.GetCurrentResources();
            this._resourcesChangedPublisher.Publish(new ResourcesChangedEvent(currentResources));
        }
    }
}