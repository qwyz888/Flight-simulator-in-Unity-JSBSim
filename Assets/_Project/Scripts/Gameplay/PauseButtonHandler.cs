using Gameplay.StateMachine.States.Core;
using Infrastructure.Extensions;
using Infrastructure.Services.Input.Core;
using Infrastructure.Services.Window.Core;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Main.Core;
using UI.Windows.Gameplay;
using VContainer;
using ITickable = VContainer.Unity.ITickable;

namespace Gameplay
{
    public class PauseButtonHandler : ITickable
    {
        private IInputService _inputService;
        private IStateMachine<IGameplayState> _gameplayStateMachine;
        private IWindowService _windowService;

        [Inject]
        public void Construct(IInputService inputService, IStateMachine<IGameplayState> gameplayStateMachine, IWindowService windowService)
        {
            _inputService = inputService;
            _gameplayStateMachine = gameplayStateMachine;
            _windowService = windowService;
        }

        public bool Enabled;

        public void Tick()
        {
            if (Enabled == false)
                return;

            if (_inputService.Gameplay.Pause.Value &&
                _gameplayStateMachine.ActiveStateType != typeof(PauseState) &&
                _windowService.TryFind(WindowID.PauseWindow, out IWindow _) == false &&
                _windowService.IsLoadingAnyWindowIncludingParents() == false &&
                _windowService.GetTopWindowIncludingParents() is GameplayInitialWindow)
            {
                _gameplayStateMachine.Enter<PauseState>();
            }
        }
    }
}