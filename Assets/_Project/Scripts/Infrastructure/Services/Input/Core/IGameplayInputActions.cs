using UnityEngine;

namespace Infrastructure.Services.Input.Core
{
    public interface IGameplayInputActions
    {
        public IInputAction<Vector2> Move { get; }
        public IInputAction<Vector2> Look { get; }
        public IInputAction<bool> Fire { get; }
        public IInputAction<bool> Pause { get; }

        public void SetActive(bool active);
    }
}