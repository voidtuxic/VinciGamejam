using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime = 0.3F;
    
    private Vector3 velocity = Vector3.zero;
    private void LateUpdate()
    {
        if(target == null)
        {
            return;
        }

        transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, 
            ref velocity, smoothTime);
    }
}
