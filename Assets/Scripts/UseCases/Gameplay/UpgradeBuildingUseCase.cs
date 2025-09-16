#nullable enable

using System.Linq;
using ContractsInterfaces.UseCasesGameplay;
using Domain.Gameplay.MessagesDTO;
using Domain.Gameplay.Models;
using MessagePipe;
using R3;
using Serilog;

namespace UseCases.Gameplay
{
    /// <summary>
    /// UseCase, отвечающий за логику улучшения здания до следующего уровня.
    /// </summary>
    public class UpgradeBuildingUseCase : IRequestHandler<UpgradeBuildingRequest, Unit>
    {
        private readonly ILogger _logger;
        private readonly ICityGridRepository _cityGridRepository;
        private readonly IResourceActions _resourceActions;
        private readonly IPublisher<BuildingUpgradedEvent> _publisher;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="UpgradeBuildingUseCase"/>.
        /// </summary>
        public UpgradeBuildingUseCase(
            ILogger logger,
            ICityGridRepository cityGridRepository,
            IResourceActions resourceActions,
            IPublisher<BuildingUpgradedEvent> publisher)
        {
            this._logger = logger.ForContext<UpgradeBuildingUseCase>();
            this._cityGridRepository = cityGridRepository;
            this._resourceActions = resourceActions;
            this._publisher = publisher;
        }

        /// <summary>
        /// Обрабатывает запрос <see cref="UpgradeBuildingRequest"/> на улучшение здания.
        /// </summary>
        public Unit Invoke(UpgradeBuildingRequest request)
        {
            this._logger.Information("[UpgradeBuildingUseCase.Invoke] Handling request to upgrade building {BuildingId}", request.BuildingId);

            CityGridModel cityGrid = this._cityGridRepository.Get();
            BuildingModel? building = cityGrid.GetBuildingById(request.BuildingId);

            if (building == null)
            {
                this._logger.Warning("[UpgradeBuildingUseCase.Invoke] Building with ID {BuildingId} not found. Cannot upgrade.", request.BuildingId);
                return Unit.Default;
            }

            if (building.CurrentLevel.CurrentValue >= building.Levels.Count)
            {
                this._logger.Warning("[UpgradeBuildingUseCase.Invoke] Building {BuildingId} is already at max level {MaxLevel}. Cannot upgrade.",
                    request.BuildingId, building.CurrentLevel.CurrentValue);
                return Unit.Default;
            }

            Cost upgradeCost = building.Levels[building.CurrentLevel.CurrentValue].Cost;

            if (!this._resourceActions.HasEnough(upgradeCost))
            {
                this._logger.Warning("[UpgradeBuildingUseCase.Invoke] Not enough resources to upgrade building {BuildingId}. Required: {Cost}",
                    request.BuildingId, upgradeCost);
                return Unit.Default;
            }

            this._resourceActions.Spend(upgradeCost);

            if (building.TryUpgrade())
            {
                this._publisher.Publish(new BuildingUpgradedEvent(request.BuildingId,
                    building.CurrentLevel.CurrentValue));
                this._logger.Information("[UpgradeBuildingUseCase.Invoke] Successfully upgraded building {BuildingId} to level {NewLevel}.",
                    request.BuildingId, building.CurrentLevel.CurrentValue);
            }
            else
            {
                this._logger.Error(
                    "[UpgradeBuildingUseCase.Invoke] Failed to upgrade building {BuildingId} in model after all checks passed. Refunding cost.",
                    request.BuildingId);
                Income refundIncome =
                    new(upgradeCost.GetResourceTypes().ToDictionary(t => t, t => upgradeCost.GetCost(t)));
                this._resourceActions.Earn(refundIncome);
            }

            return Unit.Default;
        }
    }
}