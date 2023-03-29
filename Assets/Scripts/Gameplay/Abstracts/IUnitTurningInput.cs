using System;
using UnityEngine;

namespace Gameplay.Abstracts
{
    public interface IUnitTurningInput
    {
        public event Action<float> HorizontalAxisInput;
    }

    public interface IUnitTurningMouseInput
    {
        public event Action<Vector3> MousePositionInput;
    }
}