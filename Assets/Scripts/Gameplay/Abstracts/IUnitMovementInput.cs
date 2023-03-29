using System;

namespace Gameplay.Abstracts
{
    public interface IUnitMovementInput
    {
        public event Action<float> VerticalAxisInput;
    }
}