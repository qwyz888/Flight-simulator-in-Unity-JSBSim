using System;
using Cysharp.Threading.Tasks;
using Gameplay.StateMachine.States.Core;
using Infrastructure.Data.Models.Static.Core;
using Infrastructure.Extensions;
using Infrastructure.Services.Input.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.Services.Window.Core;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Infrastructure.UI.Windows.LoadingScreen.Core;

namespace Gameplay.StateMachine.States
{
    public class LoadMenuState : IGameplayState, IState
    {
        private readonly IStateMachine<IGameplayState> _gameplayStateMachine;
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly ILogService _logService;
        private readonly IInputService _inputService;
        private readonly IStaticDataModel _staticDataModel;
        private readonly IWindowService _rootWindowService;

        public LoadMenuState(IStateMachine<IGameplayState> gameplayStateMachine, IStateMachine<IGameState> gameStateMachine, ILogService logService,
            IInputService inputService, IStaticDataModel staticDataModel, IWindowService windowService)
        {
            _gameplayStateMachine = gameplayStateMachine;
            _gameStateMachine = gameStateMachine;
            _logService = logService;
            _inputService = inputService;
            _staticDataModel = staticDataModel;
            _rootWindowService = windowService.GetRootWindowService();
        }

        public void Enter()
        {
            _logService.Log("Gameplay.LoadMenuState.Enter");

            _inputService.SetActive(false);

            _rootWindowService
                .GetOrCreateWindow(WindowID.LoadingScreen)
                .ContinueWith(window =>
                {
                    ILoadingScreen loadingScreen = (ILoadingScreen)window;

                    loadingScreen.Show()
                        .ContinueWith(() =>
                        {
                            Progress<float> progress = new Progress<float>(x => loadingScreen.SetProgress(x));

                            _gameplayStateMachine.Enter<SaveDataState, Action>(null);

                            LoadSceneState.Payload payload = new LoadSceneState.Payload
                            {
                                SceneName = _staticDataModel.Config.MenuScene,
                                Progress = progress
                            };

                            _gameStateMachine.Enter<LoadSceneState, LoadSceneState.Payload>(payload);
                        })
                        .Forget();
                })
                .Forget();
        }
    }
}