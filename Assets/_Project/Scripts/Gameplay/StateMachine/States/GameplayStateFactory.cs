using System;
using System.Collections.Generic;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Main.States.Core;
using Infrastructure.StateMachine.Main.States.Factory;
using VContainer;

namespace Gameplay.StateMachine.States
{
    public class GameplayStateFactory : StateFactory
    {
        public GameplayStateFactory(IObjectResolver resolver) : base(resolver) { }

        protected override Dictionary<Type, Func<IBaseState>> BuildStatesMap() =>
            new Dictionary<Type, Func<IBaseState>>
            {
                //chained
                [typeof(BootstrapState)] = Resolver.Resolve<BootstrapState>,
                [typeof(SetupLevelState)] = Resolver.Resolve<SetupLevelState>,
                [typeof(SetupUIState)] = Resolver.Resolve<SetupUIState>,
                [typeof(FinalizeLoadingState)] = Resolver.Resolve<FinalizeLoadingState>,
                [typeof(LoopState)] = Resolver.Resolve<LoopState>,

                //other
                [typeof(SaveDataState)] = Resolver.Resolve<SaveDataState>,
                [typeof(LoadMenuState)] = Resolver.Resolve<LoadMenuState>,
                [typeof(PauseState)] = Resolver.Resolve<PauseState>,
                [typeof(ResumeState)] = Resolver.Resolve<ResumeState>,
            };
    }
}