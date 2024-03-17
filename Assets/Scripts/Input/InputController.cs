using UnityEngine;

namespace Input
{
    public class InputController : IInputController
    {
        private readonly Camera _viewCamera;

        public Vector3 MovementDirection { get; private set; }
        public Vector3 LookPosition { get; private set; }
        public bool IsFiring { get; private set; }

        public InputController(Camera viewCamera)
        {
            _viewCamera = viewCamera;
        }
        
        public void Tick()
        {
            MovementDirection = new Vector3(
                UnityEngine.Input.GetAxis("Horizontal"),
                0,
                UnityEngine.Input.GetAxis("Vertical"));
            MovementDirection.Normalize();
            var mousePosition = new Vector3(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y, 20);
            var lookPosition = _viewCamera.ScreenToWorldPoint(mousePosition);
            lookPosition.y = 0;
            LookPosition = lookPosition;
            IsFiring = UnityEngine.Input.GetButton("Fire1");
        }
    }
}
