using Gameplay.StateMachine.States.Core;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Main.Core;
using UI.Common;
using VContainer;

namespace Gameplay.UI.Buttons
{
    public class ResumeGameButton : BaseButton
    {
        private IStateMachine<IGameplayState> _gameplayStateMachine;

        [Inject]
        public void Construct(IStateMachine<IGameplayState> gameplayStateMachine)
        {
            _gameplayStateMachine = gameplayStateMachine;
        }

        protected override void OnClick() => _gameplayStateMachine.Enter<ResumeState>();
    }
}