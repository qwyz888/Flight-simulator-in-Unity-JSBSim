using Infrastructure.Services.Input.Core;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Infrastructure.Services.Input.NewInputSystem
{
    class FlightInputActions : IFlightInputActions
    {
        public FlightInputActions(InputActions.FlightActions playerActions)
        {
            PitchRoll = new FuncInputAction<Vector2>(() => playerActions.PitchRoll.ReadValue<Vector2>());
            Yaw = new FuncInputAction<float>(() => playerActions.Yaw.ReadValue<float>());
            ThrottleUp = new FuncInputAction<bool>(() => playerActions.ThrottleUp.IsPressed());
            ThrottleDown = new FuncInputAction<bool>(() => playerActions.ThrottleDown.IsPressed());
            Brake = new FuncInputAction<bool>(() => playerActions.Brake.IsPressed());
            FlapsUp = new FuncInputAction<bool>(() => playerActions.FlapsUp.IsPressed());
            FlapsDown = new FuncInputAction<bool>(() => playerActions.FlapsDown.IsPressed());
        }

        public IInputAction<Vector2> PitchRoll { get; }
        public IInputAction<float> Yaw { get; }

        public IInputAction<bool> ThrottleUp { get; }

        public IInputAction<bool> ThrottleDown { get; }

        public IInputAction<bool> Brake { get; }

        public IInputAction<bool> FlapsUp { get; }

        public IInputAction<bool> FlapsDown { get; }

        public void SetActive(bool active)
        {
            PitchRoll.Enabled = active;
            Yaw.Enabled = active;
            ThrottleUp.Enabled = active;
            ThrottleDown.Enabled = active;
            Brake.Enabled = active;
            FlapsUp.Enabled = active;
            FlapsDown.Enabled = active;
        }
      
    }
}
