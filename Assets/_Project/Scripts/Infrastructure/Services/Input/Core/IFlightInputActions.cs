using UnityEngine;

namespace Infrastructure.Services.Input.Core
{
    public interface IFlightInputActions
    {
        IInputAction<Vector2> PitchRoll { get; }

        IInputAction<float> Yaw { get; }

        IInputAction<bool> ThrottleUp { get; }

        IInputAction<bool> ThrottleDown { get; }

        IInputAction<bool> Brake { get; }

        IInputAction<bool> FlapsUp { get; }

        IInputAction<bool> FlapsDown { get; }

        public void SetActive(bool active);
    }
}
