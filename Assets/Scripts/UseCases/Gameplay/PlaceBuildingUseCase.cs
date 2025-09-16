#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using ContractsInterfaces.UseCasesGameplay;
using Domain.Gameplay.MessagesDTO;
using Domain.Gameplay.Models;
using MessagePipe;
using Serilog;
using R3;

namespace UseCases.Gameplay
{
    /// <summary>
    /// UseCase, отвечающий за бизнес-логику размещения нового здания на игровой сетке.
    /// </summary> 
    public class PlaceBuildingUseCase : IRequestHandler<PlaceBuildingRequest, Unit>
    {
        private readonly ILogger _logger;
        private readonly ICityGridRepository _cityGridRepository;
        private readonly IResourceActions _resourceActions;
        private readonly IBuildingConfig[] _buildingConfigs;
        private readonly IPublisher<BuildingPlacementSuccess> _successPublisher;
        private readonly IPublisher<BuildingPlacementFailure> _failurePublisher;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="PlaceBuildingUseCase"/>.
        /// </summary>
        public PlaceBuildingUseCase(
            ILogger logger,
            ICityGridRepository cityGridRepository,
            IResourceActions resourceActions,
            IBuildingConfig[] buildingConfigs,
            IPublisher<BuildingPlacementSuccess> successPublisher,
            IPublisher<BuildingPlacementFailure> failurePublisher)
        {
            this._logger = logger.ForContext<PlaceBuildingUseCase>();
            this._cityGridRepository = cityGridRepository;
            this._resourceActions = resourceActions;
            this._buildingConfigs = buildingConfigs;
            this._successPublisher = successPublisher;
            this._failurePublisher = failurePublisher;
        }

        /// <summary>
        /// Обрабатывает запрос на постройку здания <see cref="PlaceBuildingRequest"/>.
        /// </summary>
        public Unit Invoke(PlaceBuildingRequest message)
        {
            this._logger.Information(
                "[PlaceBuildingUseCase.Invoke] Received request to place {BuildingType} at {Position}",
                message.BuildingType, message.Position);

            CityGridModel cityGridModel = this._cityGridRepository.Get();

            IBuildingConfig? config = this._buildingConfigs.FirstOrDefault(c => c.BuildingType == message.BuildingType);
            if (config is null)
            {
                this._logger.Warning("[PlaceBuildingUseCase.Invoke] Failure: Config for {BuildingType} not found",
                    message.BuildingType);
                this._failurePublisher.Publish(
                    new BuildingPlacementFailure(PlacementFailureReason.InvalidBuildingType));
                return Unit.Default;
            }

            if (config.Levels == null || config.Levels.Count == 0)
            {
                this._logger.Warning("[PlaceBuildingUseCase.Invoke] Failure: Config for {BuildingType} has no levels",
                    message.BuildingType);
                this._failurePublisher.Publish(new BuildingPlacementFailure(PlacementFailureReason.InvalidConfig));
                return Unit.Default;
            }

            if (cityGridModel.IsCellOccupied(message.Position))
            {
                this._logger.Warning("[PlaceBuildingUseCase.Invoke] Failure: Cell {Position} is already occupied",
                    message.Position);
                this._failurePublisher.Publish(new BuildingPlacementFailure(PlacementFailureReason.CellIsOccupied));
                return Unit.Default;
            }

            Cost level1Cost = config.Levels[0].Cost;
            if (!this._resourceActions.HasEnough(level1Cost))
            {
                this._logger.Warning(
                    "[PlaceBuildingUseCase.Invoke] Failure: Not enough resources. Required: {RequiredCost}",
                    level1Cost);
                this._failurePublisher.Publish(new BuildingPlacementFailure(PlacementFailureReason.NotEnoughResources));
                return Unit.Default;
            }

            this._resourceActions.Spend(level1Cost);

            List<BuildingLevel> buildingLevels = new();
            for (int i = 0; i < config.Levels.Count; i++)
            {
                IBuildingLevelData levelData = config.Levels[i];
                buildingLevels.Add(new BuildingLevel(i + 1, levelData.Cost, levelData.Income));
            }

            BuildingModel newBuildingModel =
                new(Guid.NewGuid(), message.BuildingType, message.Position, buildingLevels);

            if (cityGridModel.TryAddBuilding(newBuildingModel))
            {
                this._logger.Information(
                    "[PlaceBuildingUseCase.Invoke] Success: Placed {BuildingType} with ID {BuildingId} at {Position}",
                    newBuildingModel.Type, newBuildingModel.Id, newBuildingModel.Position.CurrentValue);
                this._successPublisher.Publish(new BuildingPlacementSuccess(newBuildingModel.Id, newBuildingModel.Type,
                    newBuildingModel.Position.CurrentValue));
            }
            else
            {
                this._logger.Error(
                    "[PlaceBuildingUseCase.Invoke] Critical Failure: Could not add building to grid model after all checks passed.");
                Income refundIncome =
                    new(level1Cost.GetResourceTypes().ToDictionary(rt => rt, rt => level1Cost.GetCost(rt)));
                this._resourceActions.Earn(refundIncome);
                
                this._failurePublisher.Publish(new BuildingPlacementFailure(PlacementFailureReason.Unknown));
            }

            return Unit.Default;
        }
    }
}