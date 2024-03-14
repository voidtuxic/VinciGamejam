using System;
using Void.Enemies;
using UnityEngine;

namespace Void.Projectiles
{
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private GameObject deathParticles;
        [SerializeField] private ProjectileData data;
    
        private int _projectileIndex;
        private float _timeToLive;

        public Action<ProjectileController, int> OnDispose;

        public void Initialize(Vector3 position, Quaternion rotation, int index)
        {
            transform.position = position;
            transform.rotation = rotation;
            body.velocity = rotation * Vector3.forward * data.Speed;
            _timeToLive = 0;
            _projectileIndex = index;
            gameObject.SetActive(true);
        }

        private void Update()
        {
            _timeToLive += Time.deltaTime;
            if(_timeToLive >= data.TimeToLive)
            {
                gameObject.SetActive(false);
                OnDispose?.Invoke(this, _projectileIndex);
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
            OnDispose?.Invoke(this, _projectileIndex);
        }
    }
}