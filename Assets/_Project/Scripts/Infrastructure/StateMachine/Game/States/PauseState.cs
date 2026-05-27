using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Gameplay;
using Gameplay.StateMachine.States.Core;
using Infrastructure.Services.Input.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.Services.TimeScale.Core;
using Infrastructure.Services.Window.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Plugins.AudioService.Core;
using UnityEngine;
using UnityEngine.Audio;

namespace Infrastructure.StateMachine.Game.States
{
    public class PauseState : IGameplayState, IState
    {
        private readonly ILogService _logService;
        private readonly IInputService _inputService;
        private readonly IWindowService _windowService;
        private readonly ITimeScaleService _timeScaleService;
        private readonly IAudioService _audioService;
        private readonly Preferences _preferences;
        private readonly PauseButtonHandler _pauseButtonHandler;

        public PauseState(ILogService logService, IInputService inputService, IWindowService windowService, ITimeScaleService timeScaleService,
            IAudioService audioService, Preferences preferences, PauseButtonHandler pauseButtonHandler)
        {
            _logService = logService;
            _inputService = inputService;
            _windowService = windowService;
            _timeScaleService = timeScaleService;
            _audioService = audioService;
            _preferences = preferences;
            _pauseButtonHandler = pauseButtonHandler;
        }

        public void Enter()
        {
            _logService.Log("Gameplay.PauseState.Enter");
            
            _timeScaleService.PauseTime();

            _pauseButtonHandler.Enabled = false;
            
            _inputService.Gameplay.SetActive(false);

            _audioService.PauseAll(audio => _preferences.GroupsToPause.Contains(audio.AudioMixerGroup));

            _windowService.GetOrCreateWindow(WindowID.PauseWindow).ContinueWith(window => window.Show()).Forget();
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private List<AudioMixerGroup> _groupsToPause;

            public IReadOnlyList<AudioMixerGroup> GroupsToPause => _groupsToPause;
        }
    }
}