#nullable enable

using System.Collections.Generic;
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
    /// UseCase, отвечающий за логику удаления здания с игровой сетки.
    /// </summary>
    public class RemoveBuildingUseCase : IRequestHandler<RemoveBuildingRequest, Unit>
    {
        private readonly ILogger _logger;
        private readonly ICityGridRepository _cityGridRepository;
        private readonly IResourceActions _resourceActions;
        private readonly IPublisher<BuildingRemovedEvent> _publisher;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="RemoveBuildingUseCase"/>.
        /// </summary>
        public RemoveBuildingUseCase(
            ILogger logger,
            ICityGridRepository cityGridRepository,
            IResourceActions resourceActions,
            IPublisher<BuildingRemovedEvent> publisher)
        {
            this._logger = logger.ForContext<RemoveBuildingUseCase>();
            this._cityGridRepository = cityGridRepository;
            this._resourceActions = resourceActions;
            this._publisher = publisher;
        }

        /// <summary>
        /// Обрабатывает запрос <see cref="RemoveBuildingRequest"/> на удаление здания.
        /// </summary>
        public Unit Invoke(RemoveBuildingRequest request)
        {
            this._logger.Information("[RemoveBuildingUseCase.Invoke] Handling request to remove building {BuildingId}",
                request.BuildingId);

            CityGridModel cityGrid = this._cityGridRepository.Get();
            BuildingModel? building = cityGrid.GetBuildingById(request.BuildingId);

            if (building == null)
            {
                this._logger.Warning(
                    "[RemoveBuildingUseCase.Invoke] Building with ID {BuildingId} not found. Cannot remove.",
                    request.BuildingId);
                return Unit.Default;
            }

            BuildingLevel currentLevelData = building.Levels[building.CurrentLevel.CurrentValue - 1];
            Cost refundCost = currentLevelData.Cost;
            Dictionary<ResourceType, int> refundAmounts = refundCost.GetResourceTypes()
                .ToDictionary(type => type, type => refundCost.GetCost(type) / 2);

            Income refundIncome = new(refundAmounts);
            this._resourceActions.Earn(refundIncome);
            this._logger.Information(
                "[RemoveBuildingUseCase.Invoke] Refunded {Income} for removing building {BuildingId}", refundIncome,
                request.BuildingId);

            if (cityGrid.TryRemoveBuilding(building))
            {
                this._publisher.Publish(new BuildingRemovedEvent(request.BuildingId));
                this._logger.Information(
                    "[RemoveBuildingUseCase.Invoke] Successfully removed building {BuildingId} from grid.",
                    request.BuildingId);
            }
            else
            {
                this._logger.Error(
                    "[RemoveBuildingUseCase.Invoke] Failed to remove building {BuildingId} from grid model after finding it.",
                    request.BuildingId);
            }

            return Unit.Default;
        }
    }
}