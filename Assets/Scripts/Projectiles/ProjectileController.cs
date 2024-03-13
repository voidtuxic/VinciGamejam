using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    [SerializeField] private GameObject deathParticles;
    
    private ProjectileData data;
    private float timeToLive;

    public void Initialize(ProjectileData projectileData)
    {
        data = projectileData;
    }
    
    private void Start()
    {
        body.velocity = transform.rotation * Vector3.forward * data.Speed;
    }

    private void Update()
    {
        timeToLive += Time.deltaTime;
        if(timeToLive >= data.TimeToLive)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            var enemy = other.gameObject.GetComponent<EnemyController>();
            enemy.ApplyDamage(data.Damage);
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}