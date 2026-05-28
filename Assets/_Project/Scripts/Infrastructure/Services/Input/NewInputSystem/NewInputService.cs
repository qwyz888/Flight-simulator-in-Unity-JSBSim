using Infrastructure.Services.Input.Core;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using VContainer.Unity;

namespace Infrastructure.Services.Input.NewInputSystem
{
    public class NewInputService : IInputService, IInitializable
    {
        private readonly InputSystemUIInputModule _uiInputModule;

        public NewInputService(InputSystemUIInputModule uiInputModule)
        {
            _uiInputModule = uiInputModule;
        }

        private InputActions _inputActions;

        public IGameplayInputActions Gameplay { get; private set; }
        public IUIInputActions UI { get; private set; }
        public IFlightInputActions Flight { get; private set; }

        public void Initialize()
        {
            _inputActions = new InputActions();
            _inputActions.Enable();
            Gameplay = new GameplayInputActions(_inputActions.Gameplay);
            UI = new UIInputActions(_inputActions.UI);
            Flight = new FlightInputActions(_inputActions.Flight);

            SetActive(false);
        }

        public void SetActive(bool active)
        {
            _uiInputModule.enabled = active;
            Gameplay.SetActive(active);
            UI.SetActive(active);
            Flight.SetActive(active);
        }
    }
}