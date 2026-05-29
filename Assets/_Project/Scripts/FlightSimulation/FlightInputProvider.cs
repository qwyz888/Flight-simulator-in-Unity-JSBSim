using Infrastructure.Services.Input.Core;
using UnityEngine;

namespace FlightSimulation
{
    public class FlightInputProvider
    {
        private readonly IInputService _input;

        private float _throttle;

        private float _flaps;

        public FlightInputProvider(IInputService input)
        {
            _input = input;
        }

        public FlightInputData GetInput()
        {
            Vector2 pitchRoll =
                _input.Flight.PitchRoll.Value;

            float yaw =
                _input.Flight.Yaw.Value;

            if (_input.Flight.ThrottleUp.Value)
            {
                _throttle += Time.deltaTime * 0.3f;
            }

            if (_input.Flight.ThrottleDown.Value)
            {
                _throttle -= Time.deltaTime * 0.3f;
            }

            _throttle =
                Mathf.Clamp01(_throttle);

            if (_input.Flight.FlapsUp.Value)
                _flaps += Time.deltaTime;

            if (_input.Flight.FlapsDown.Value)
                _flaps -= Time.deltaTime;

            _flaps =
                Mathf.Clamp01(_flaps);

            return new FlightInputData
            {
                Roll = pitchRoll.x,

                Pitch = pitchRoll.y,

                Yaw = -yaw,

                Throttle = _throttle,

                Brake =
                    _input.Flight.Brake.Value,

                Flaps = _flaps
            };
        }
    }
}
