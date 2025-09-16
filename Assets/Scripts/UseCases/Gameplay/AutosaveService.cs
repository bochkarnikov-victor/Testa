#nullable enable

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Domain.Gameplay.MessagesDTO;
using MessagePipe;
using R3;
using Serilog;
using VContainer.Unity;

namespace UseCases.Gameplay
{
    /// <summary>
    /// Фоновый сервис, отвечающий за автоматическое сохранение игры через заданные промежутки времени.
    /// </summary>
    public class AutosaveService : IInitializable, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IAsyncRequestHandler<SaveGameCommand, Unit> _saveGameHandler;
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly TimeSpan _autosaveInterval;

        public AutosaveService(ILogger logger, IAsyncRequestHandler<SaveGameCommand, Unit> saveGameHandler)
        {
            this._logger = logger.ForContext<AutosaveService>();
            this._saveGameHandler = saveGameHandler;
            this._autosaveInterval = TimeSpan.FromSeconds(10); // Интервал 10 секунд
        }

        public void Initialize()
        {
            this._logger.Information("[AutosaveService.Initialize] Starting autosave loop with interval {Interval}s",
                this._autosaveInterval.TotalSeconds);
            this.StartAutosaveLoop(this._cancellationTokenSource.Token).Forget();
        }

        private async UniTaskVoid StartAutosaveLoop(CancellationToken ct)
        {
            try
            {
                while (!ct.IsCancellationRequested)
                {
                    await UniTask.Delay(this._autosaveInterval, cancellationToken: ct);

                    this._logger.Information("[AutosaveService.StartAutosaveLoop] Performing autosave...");
                    await this._saveGameHandler.InvokeAsync(new SaveGameCommand(), ct);
                }
            }
            catch (OperationCanceledException)
            {
                // Нормальное завершение работы
            }
            catch (Exception ex)
            {
                this._logger.Error(ex, "[AutosaveService.StartAutosaveLoop] Autosave loop failed unexpectedly.");
            }
        }

        public void Dispose()
        {
            this._cancellationTokenSource.Cancel();
            this._cancellationTokenSource.Dispose();
            this._logger.Information("[AutosaveService.Dispose] Autosave loop stopped.");
        }
    }
}