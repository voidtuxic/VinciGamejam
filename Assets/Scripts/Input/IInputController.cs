using UnityEngine;
using Zenject;

namespace Input
{
    public interface IInputController : ITickable
    {
        Vector3 MovementDirection { get; }
        Vector3 LookPosition { get; }
        bool IsFiring { get; }
    }
}
