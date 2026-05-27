using Cysharp.Threading.Tasks;
using Gameplay.StateMachine.States.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.Services.Window.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Gameplay.StateMachine.States
{
    public class SetupUIState : IGameplayState, IState
    {
        private readonly IStateMachine<IGameplayState> _stateMachine;
        private readonly ILogService _logService;
        private readonly IWindowService _windowService;

        public SetupUIState(IStateMachine<IGameplayState> stateMachine, ILogService logService, IWindowService windowService)
        {
            _stateMachine = stateMachine;
            _logService = logService;
            _windowService = windowService;
        }

        public void Enter()
        {
            _logService.Log("Gameplay.SetupUIState.Enter");

            _windowService
                .CreateWindow(WindowID.GameplayInitialWindow)
                .ContinueWith(window =>
                {
                    window.Show();
                    _stateMachine.Enter<FinalizeLoadingState>();
                })
                .Forget();
        }
    }
}