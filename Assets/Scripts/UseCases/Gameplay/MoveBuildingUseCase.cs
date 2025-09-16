#nullable enable

using ContractsInterfaces.UseCasesGameplay;
using Domain.Gameplay.MessagesDTO;
using Domain.Gameplay.Models;
using MessagePipe;
using R3;
using Serilog;

namespace UseCases.Gameplay
{
    /// <summary>
    /// UseCase, отвечающий за логику перемещения существующего здания на новую позицию.
    /// </summary>
    public class MoveBuildingUseCase : IRequestHandler<MoveBuildingRequest, Unit>
    {
        private readonly ILogger _logger;
        private readonly ICityGridRepository _cityGridRepository;
        private readonly IPublisher<BuildingMovedEvent> _publisher;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="MoveBuildingUseCase"/>.
        /// </summary>
        public MoveBuildingUseCase(
            ILogger logger,
            ICityGridRepository cityGridRepository,
            IPublisher<BuildingMovedEvent> publisher)
        {
            this._logger = logger.ForContext<MoveBuildingUseCase>();
            this._cityGridRepository = cityGridRepository;
            this._publisher = publisher;
        }

        /// <summary>
        /// Обрабатывает запрос <see cref="MoveBuildingRequest"/> на перемещение здания.
        /// </summary>
        public Unit Invoke(MoveBuildingRequest request)
        {
            this._logger.Information("[MoveBuildingUseCase.Invoke] Handling request to move building {BuildingId} to {NewPosition}",
                request.BuildingId, request.NewPosition);

            CityGridModel cityGrid = this._cityGridRepository.Get();
            BuildingModel? building = cityGrid.GetBuildingById(request.BuildingId);

            if (building == null)
            {
                this._logger.Warning("[MoveBuildingUseCase.Invoke] Building with ID {BuildingId} not found. Cannot move.", request.BuildingId);
                return Unit.Default;
            }

            if (cityGrid.TryMoveBuilding(building, request.NewPosition))
            {
                this._publisher.Publish(new BuildingMovedEvent(request.BuildingId, request.NewPosition));
                this._logger.Information("[MoveBuildingUseCase.Invoke] Successfully moved building {BuildingId} to {NewPosition}.",
                    request.BuildingId, request.NewPosition);
            }
            else
            {
                this._logger.Warning(
                    "[MoveBuildingUseCase.Invoke] Failed to move building {BuildingId} to {NewPosition}. The cell might be occupied.",
                    request.BuildingId, request.NewPosition);
            }

            return Unit.Default;
        }
    }
}