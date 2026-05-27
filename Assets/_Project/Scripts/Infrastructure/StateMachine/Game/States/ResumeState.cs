using System.Linq;
using Cysharp.Threading.Tasks;
using Gameplay;
using Gameplay.StateMachine.States.Core;
using Infrastructure.Services.Input.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.Services.TimeScale.Core;
using Infrastructure.Services.Window.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Plugins.AudioService.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class ResumeState : IGameplayState, IState
    {
        private readonly ILogService _logService;
        private readonly IStateMachine<IGameplayState> _stateMachine;
        private readonly IInputService _inputService;
        private readonly IWindowService _windowService;
        private readonly ITimeScaleService _timeScaleService;
        private readonly IAudioService _audioService;
        private readonly PauseState.Preferences _preferences;
        private readonly PauseButtonHandler _pauseButtonHandler;

        public ResumeState(ILogService logService, IStateMachine<IGameplayState> stateMachine, IInputService inputService, IWindowService windowService,
            ITimeScaleService timeScaleService, IAudioService audioService, PauseState.Preferences preferences, PauseButtonHandler pauseButtonHandler)
        {
            _logService = logService;
            _stateMachine = stateMachine;
            _inputService = inputService;
            _windowService = windowService;
            _timeScaleService = timeScaleService;
            _audioService = audioService;
            _preferences = preferences;
            _pauseButtonHandler = pauseButtonHandler;
        }

        public void Enter()
        {
            _logService.Log("Gameplay.ResumeState.Enter");
            
            _timeScaleService.ResumeTime();

            _pauseButtonHandler.Enabled = true;
            
            _inputService.Gameplay.SetActive(true);

            _audioService.ResumeAll(audio => _preferences.GroupsToPause.Contains(audio.AudioMixerGroup));

            _windowService.GetOrCreateWindow(WindowID.PauseWindow).ContinueWith(window => window.Hide()).Forget();

            _stateMachine.Enter<Gameplay.StateMachine.States.LoopState>();
        }
    }
}