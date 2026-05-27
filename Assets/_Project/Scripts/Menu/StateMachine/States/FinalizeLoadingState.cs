using Cysharp.Threading.Tasks;
using Infrastructure.Services.Input.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.Services.TimeScale.Core;
using Infrastructure.Services.Window.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Infrastructure.UI.Windows.LoadingScreen.Core;
using Menu.StateMachine.States.Core;

namespace Menu.StateMachine.States
{
    public class FinalizeLoadingState : IMenuState, IState
    {
        private readonly IStateMachine<IMenuState> _stateMachine;
        private readonly ILogService _logService;
        private readonly IInputService _inputService;
        private readonly IWindowService _windowService;
        private readonly ITimeScaleService _timeScaleService;

        public FinalizeLoadingState(IStateMachine<IMenuState> stateMachine, ILogService logService, IInputService inputService, IWindowService windowService,
            ITimeScaleService timeScaleService)
        {
            _stateMachine = stateMachine;
            _logService = logService;
            _inputService = inputService;
            _windowService = windowService;
            _timeScaleService = timeScaleService;
        }

        public void Enter()
        {
            _logService.Log("Menu.FinalizeLoadingState.Enter");

            _timeScaleService.ResumeTime();
            
            if (_windowService.TryFind(WindowID.LoadingScreen, out IWindow window) == false)
                return;

            ILoadingScreen loadingScreen = (ILoadingScreen)window;

            loadingScreen.Hide().ContinueWith(() => _inputService.SetActive(true)).Forget();
            _stateMachine.Enter<LoopState>();
        }
    }
}