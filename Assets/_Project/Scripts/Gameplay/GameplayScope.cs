using Gameplay.StateMachine;
using Gameplay.StateMachine.States;
using Gameplay.StateMachine.States.Core;
using Infrastructure.Services.Window.Core;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Main.Core;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using BootstrapState = Gameplay.StateMachine.States.BootstrapState;
using FinalizeLoadingState = Gameplay.StateMachine.States.FinalizeLoadingState;
using LoopState = Gameplay.StateMachine.States.LoopState;
using SaveDataState = Gameplay.StateMachine.States.SaveDataState;

namespace Gameplay
{
    public class GameplayScope : LifetimeScope, IInitializable
    {
        [SerializeField] private PauseState.Preferences _pausePreferences;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterStateMachine(builder);
            RegisterServices(builder);
            MakeInitializable(builder);
        }

        public void Initialize() => Container.Resolve<IStateMachine<IGameplayState>>().Enter<BootstrapState>();

        private void RegisterStateMachine(IContainerBuilder builder)
        {
            RegisterStates(builder);
            builder.Register<GameplayStateFactory>(Lifetime.Singleton);
            builder.Register<GameplayStateMachine>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void RegisterStates(IContainerBuilder builder)
        {
            IWindowService parentWindowService = Parent.Container.Resolve<IWindowService>();

            //chained
            builder.Register<BootstrapState>(Lifetime.Singleton);
            builder.Register<SetupLevelState>(Lifetime.Singleton);
            builder.Register<SetupUIState>(Lifetime.Singleton);
            builder.Register<FinalizeLoadingState>(Lifetime.Singleton).WithParameter(parentWindowService);
            builder.Register<LoopState>(Lifetime.Singleton);

            //other
            builder.Register<SaveDataState>(Lifetime.Singleton);
            builder.Register<LoadMenuState>(Lifetime.Singleton).WithParameter(parentWindowService);
            builder.Register<PauseState>(Lifetime.Singleton).WithParameter(_pausePreferences);
            builder.Register<ResumeState>(Lifetime.Singleton).WithParameter(_pausePreferences);
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<PauseButtonHandler>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }
        
        private void MakeInitializable(IContainerBuilder builder)
        {
            builder.Register<IInitializable>(c => this, Lifetime.Singleton).As<IInitializable>();
        }
    }
}