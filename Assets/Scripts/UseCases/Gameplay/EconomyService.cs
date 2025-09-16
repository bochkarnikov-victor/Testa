#nullable enable

using System;
using System.Threading;
using ContractsInterfaces.UseCasesGameplay;
using Cysharp.Threading.Tasks;
using Domain.Gameplay.Models;
using Serilog;
using VContainer.Unity;

namespace UseCases.Gameplay
{
    /// <summary>
    /// Фоновый сервис, отвечающий за пассивное начисление дохода от зданий.
    /// </summary>
    public class EconomyService : IInitializable, IDisposable
    {
        private const int TickIntervalMilliseconds = 1000;

        private readonly ILogger _logger;
        private readonly ICityGridRepository _cityGridRepository;
        private readonly IResourceActions _resourceActions;
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="EconomyService"/>.
        /// </summary>
        public EconomyService(
            ILogger logger,
            ICityGridRepository cityGridRepository,
            IResourceActions resourceActions)
        {
            this._logger = logger;
            this._cityGridRepository = cityGridRepository;
            this._resourceActions = resourceActions;
        }

        /// <summary>
        /// Запускает фоновый процесс начисления дохода при старте игры.
        /// </summary>
        public void Initialize()
        {
            this._logger.Information("[EconomyService.Initialize] Starting economy tick loop");
            this.StartTicking(this._cancellationTokenSource.Token).Forget();
        }

        private async UniTaskVoid StartTicking(CancellationToken ct)
        {
            try
            {
                while (!ct.IsCancellationRequested)
                {
                    this.Tick();

                    await UniTask.Delay(TickIntervalMilliseconds, cancellationToken: ct);
                }
            }
            catch (OperationCanceledException)
            {
                /* нормальное завершение */
            }
            catch (Exception ex)
            {
                this._logger.Error(ex, "[EconomyService] Background loop failed");
            }
        }

        private void Tick()
        {
            CityGridModel cityGridModel = this._cityGridRepository.Get();
            Income totalIncome = Income.Zero;
            foreach (BuildingModel building in cityGridModel.Buildings)
            {
                BuildingLevel currentLevelData = building.Levels[building.CurrentLevel.CurrentValue - 1];
                totalIncome += currentLevelData.Income;
            }

            if (totalIncome.Equals(Income.Zero))
            {
                return;
            }

            this._resourceActions.Earn(totalIncome);
            this._logger.Debug("[EconomyService.Tick] Earned {Income}", totalIncome);
        }

        /// <summary>
        /// Останавливает фоновый процесс при уничтожении контейнера.
        /// </summary>
        public void Dispose()
        {
            this._logger.Information("[EconomyService.Dispose] Stopping economy tick loop");
            this._cancellationTokenSource?.Cancel();
            this._cancellationTokenSource?.Dispose();
        }
    }
}