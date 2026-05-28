namespace Infrastructure.Services.Input.Core
{
    public interface IInputService
    {
        public IGameplayInputActions Gameplay { get; }
        public IUIInputActions UI { get; }
        public IFlightInputActions Flight { get; }
        public void SetActive(bool active);
    }
}