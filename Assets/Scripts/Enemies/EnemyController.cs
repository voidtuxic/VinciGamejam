using System;
using Void.Player;
using UnityEngine;

namespace Void.Enemies
{
    public class EnemyController : MonoBehaviour
    {
        private static readonly int Moving = Animator.StringToHash("Moving");
        private static readonly int Attack = Animator.StringToHash("Attack");
    
        [SerializeField] private Rigidbody body;
        [SerializeField] private Animator animator;
        [SerializeField] private EnemyData data;
        [SerializeField] private GameObject deathParticles;

        private PlayerController _target;
        private int _currentLife;
        private float _attackCooldown;
        private bool _hasSpawned;

        public event Action<EnemyController> OnDestroyed;
    
        public void Initialize(PlayerController target)
        {
            _target = target;
            _currentLife = data.Life;
            _hasSpawned = false;
        }

        public void ApplyDamage(int damage)
        {
            _currentLife -= damage;

            if(_currentLife <= 0)
            {
                // TODO not great, not terrible
                var death = Instantiate(deathParticles, transform.position, Quaternion.Euler(-90, 0, 0));
                death.transform.localScale = Vector3.one / 1.25f;
                OnDestroyed?.Invoke(this);
            }
        }

        public void HasSpawned()
        {
            _hasSpawned = true;
        }

        private void FixedUpdate()
        {
            if(!_hasSpawned || _target == null)
            {
                body.velocity = Vector3.zero;
                animator.SetBool(Moving, false);
                return;
            }

            var direction = _target.transform.position - transform.position;
            direction.Normalize();
            transform.LookAt(_target.transform);
        
            if(Vector3.Distance(transform.position, _target.transform.position) <= data.AttackRange)
            {
                body.velocity = Vector3.zero;
                animator.SetBool(Moving, false);
                if(_attackCooldown <= 0)
                {
                    animator.SetTrigger(Attack);
                    _target.ApplyDamage(data.AttackDamage);
                    _attackCooldown = data.AttackCooldown;
                }
            }
            else
            {
                animator.SetBool(Moving, true);
                body.velocity = direction * data.Speed;
            }
        
            if(_attackCooldown > 0)
            {
                _attackCooldown -= Time.deltaTime;
            }
        }
    }
}