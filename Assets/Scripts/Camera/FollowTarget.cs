using UnityEngine;

namespace Void.Camera
{
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float smoothTime = 0.3F;
    
        private Vector3 _velocity = Vector3.zero;
    
        private void FixedUpdate()
        {
            if(target == null)
            {
                return;
            }

            transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, 
                ref _velocity, smoothTime);
        }
    }
}
