using System;
using System.Collections.Generic;
using System.IO;
using ContractsInterfaces.DomainGameplay;
using ContractsInterfaces.Infrastructure;
using ContractsInterfaces.UseCasesGameplay;
using ContractsInterfaces.ViewsGameplay;
using Domain.Application.Models;
using Domain.Gameplay.MessagesDTO;
using Domain.Gameplay.Models;
using Infrastructure;
using MessagePipe;
using MessagePipe.VContainer;
using Presentation.Gameplay.Presenters;
using Presentation.Gameplay.Views;
using Repositories.Gameplay;
using R3;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Unity3D;
using UnityEngine;
using UnityEngine.InputSystem;
using UseCases.Gameplay;
using VContainer;
using VContainer.Unity;
using ILogger = Serilog.ILogger;
using Logger = Serilog.Core.Logger;

namespace Installers.Gameplay
{
    public class GameplayInstaller : LifetimeScope
    {
        [Header("Scene Dependencies")] [SerializeField]
        private Camera _mainCamera = null!;

        [Header("Views")] [SerializeField] private HudView _hudView = null!;
        [SerializeField] private BuildingSelectionView _buildingSelectionView = null!;
        [SerializeField] private BuildingPropertiesView _buildingPropertiesView = null!;
        [SerializeField] private GridView _gridView = null!;
        [SerializeField] private BuildingGhostView _buildingGhostView = null!;

        [Header("Repositories")] [SerializeField]
        private BuildingRepository[] _buildingRepositories = Array.Empty<BuildingRepository>();

        [Header("Input")] [SerializeField] private InputActionAsset _inputActions = null!;

        protected override void Configure(IContainerBuilder builder)
        {
            this.ConfigureLogging(builder);
            MessagePipeOptions options = this.ConfigureMessagePipe(builder);

            this.ConfigureInfrastructure(builder);
            this.ConfigureDomainAndRepositories(builder);
            this.ConfigureUseCasesAndServices(builder, options);
            this.ConfigurePresentation(builder);
        }

        private void ConfigureLogging(IContainerBuilder builder)
        {
            string logsDir = Path.Combine(Application.persistentDataPath, "logs");
            Directory.CreateDirectory(logsDir);

            Logger logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("AppVersion", Application.version)
                .Enrich.WithProperty("Platform", Application.platform.ToString())
                .WriteTo.Unity3D()
                .WriteTo.File(
                    Path.Combine(logsDir, "game-.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    restrictedToMinimumLevel: LogEventLevel.Debug)
                .CreateLogger();

            builder.RegisterInstance<ILogger>(logger);
            builder.Register<Func<Type, ILogger>>(_ => type => logger.ForContext(type), Lifetime.Singleton);
        }

        private MessagePipeOptions ConfigureMessagePipe(IContainerBuilder builder)
        {
            MessagePipeOptions options = builder.RegisterMessagePipe();
            builder.RegisterBuildCallback(c => GlobalMessagePipe.SetProvider(c.AsServiceProvider()));

            builder.RegisterMessageBroker<GameStateLoadedEvent>(options);
            builder.RegisterMessageBroker<BuildingRemovedEvent>(options);
            builder.RegisterMessageBroker<BuildingMovedEvent>(options);
            builder.RegisterMessageBroker<BuildingPlacementFailure>(options);
            builder.RegisterMessageBroker<BuildingUpgradedEvent>(options);
            builder.RegisterMessageBroker<BuildingSelectedEvent>(options);
            builder.RegisterMessageBroker<BuildingType>(options);
            builder.RegisterMessageBroker<ResourcesChangedEvent>(options);

            return options;
        }

        private void ConfigureInfrastructure(IContainerBuilder builder)
        {
            builder.RegisterInstance(this._mainCamera);
            builder.Register<IGridCoordinatesConverter, GridCoordinatesConverter>(Lifetime.Singleton);
            builder.RegisterInstance(this._inputActions);
            builder.Register<InputService>(Lifetime.Singleton).As<IInputService, IInitializable>();
            builder.Register<IGameStateRepository, GameStateRepository>(Lifetime.Singleton);
        }

        private void ConfigureDomainAndRepositories(IContainerBuilder builder)
        {
            // --- Domain Models ---
            builder.Register(_ => new CityGridModel(32, 32), Lifetime.Singleton);
            builder.Register(container =>
            {
                Dictionary<ResourceType, int> startingResources = new()
                {
                    { ResourceType.Gold, 1000 },
                    { ResourceType.Wood, 500 }
                };
                return new PlayerResourcesModel(startingResources);
            }, Lifetime.Singleton).As<IPlayerResourcesModel, PlayerResourcesModel>();

            // --- Repositories ---
            builder.RegisterInstance<IBuildingConfig[]>(this._buildingRepositories);
            builder.Register<ICityGridRepository, CityGridRepository>(Lifetime.Singleton);
            builder.Register<IPlayerResourcesRepository, PlayerResourcesRepository>(Lifetime.Singleton);
        }

        private void ConfigureUseCasesAndServices(IContainerBuilder builder, MessagePipeOptions options)
        {
            builder.Register<ResourceManagerUseCase>(Lifetime.Singleton)
                .As<IResourceActions, IResourceQueries>();

            builder.Register<EconomyService>(Lifetime.Singleton).As<IInitializable>();
            builder.Register<AutosaveService>(Lifetime.Singleton).As<IInitializable>();

            builder.RegisterRequestHandler<PlaceBuildingRequest, Unit, PlaceBuildingUseCase>(options);
            builder.RegisterRequestHandler<RemoveBuildingRequest, Unit, RemoveBuildingUseCase>(options);
            builder.RegisterRequestHandler<MoveBuildingRequest, Unit, MoveBuildingUseCase>(options);
            builder.RegisterRequestHandler<UpgradeBuildingRequest, Unit, UpgradeBuildingUseCase>(options);

            builder.RegisterAsyncRequestHandler<SaveGameCommand, Unit, SaveGameUseCase>(options);
            builder.RegisterAsyncRequestHandler<LoadGameCommand, Unit, LoadGameUseCase>(options);
        }

        private void ConfigurePresentation(IContainerBuilder builder)
        {
            builder.RegisterComponent(this._gridView);
            builder.RegisterComponent(this._buildingGhostView);

            builder.RegisterComponent(this._hudView).As<IHudView>();
            builder.Register<HudPresenter>(Lifetime.Singleton).As<IStartable>();

            builder.RegisterComponent(this._buildingSelectionView).As<IBuildingSelectionView>();
            builder.Register<BuildingSelectionPresenter>(Lifetime.Singleton).As<IStartable>();

            builder.RegisterComponent(this._buildingPropertiesView).As<IBuildingPropertiesView>();
            builder.Register<BuildingPropertiesPresenter>(Lifetime.Singleton).As<IStartable>();

            builder.Register<GridInteractionPresenter>(Lifetime.Singleton).As<IStartable>();
        }
    }
}