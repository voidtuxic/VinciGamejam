using System;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private ProjectileData data;
    
    private int projectileIndex;
    private float timeToLive;

    public Action<ProjectileController, int> OnDispose;

    public void Initialize(Vector3 position, Quaternion rotation, int index)
    {
        transform.position = position;
        transform.rotation = rotation;
        body.velocity = rotation * Vector3.forward * data.Speed;
        timeToLive = 0;
        projectileIndex = index;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        timeToLive += Time.deltaTime;
        if(timeToLive >= data.TimeToLive)
        {
            gameObject.SetActive(false);
            OnDispose?.Invoke(this, projectileIndex);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            var enemy = other.gameObject.GetComponent<EnemyController>();
            enemy.ApplyDamage(data.Damage);
        }
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        OnDispose?.Invoke(this, projectileIndex);
    }
}