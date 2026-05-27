using Cysharp.Threading.Tasks;
using Gameplay.StateMachine.States.Core;
using Infrastructure.Services.Input.Core;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.UI.Windows.Core;
using UnityEngine;
using VContainer;

namespace Gameplay.UI.Windows
{
    public class PauseWindow : FadeNavigationalWindow
    {
        [SerializeField] private GameObject _firstSelected;

        private IInputService _inputService;
        private IStateMachine<IGameplayState> _stateMachine;

        [Inject]
        public void Construct(IInputService inputService, IStateMachine<IGameplayState> stateMachine)
        {
            _inputService = inputService;
            _stateMachine = stateMachine;
        }

        #region MonoBehaviour

        private void Update()
        {
            if (_inputService.UI.Cancel.Value && IsActive && IsInteractable)
                _stateMachine.Enter<ResumeState>();
        }

        #endregion

        public override UniTask Show()
        {
            return base.Show().ContinueWith(() => SelectGameObjectIfActive(_firstSelected));
        }
    }
}