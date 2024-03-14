using System.Collections.Generic;
using Void.Audio;
using Void.Events;
using Void.Projectiles;
using UnityEngine;
using Zenject;

namespace Void.Player
{
    public class PlayerController : MonoBehaviour
    {
        private static readonly int Moving = Animator.StringToHash("Moving");
        private static readonly int Hit = Animator.StringToHash("Hit");
    
        [SerializeField] private UnityEngine.Camera viewCamera;
        [SerializeField] private Rigidbody body;
        [SerializeField] private float speed;
        [SerializeField] private int maxHealth;
        [SerializeField] private ProjectileData[] projectileData;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform shootPosition;
        [SerializeField] private GameObject deathParticles;
        [SerializeField] private GameObject hitParticles;
        [SerializeField] private int projectilePoolSize;
        [SerializeField] private Transform projectileContainer;

        private IEventBus _eventBus;
        private int _projectileIndex;
        private float _cooldown;
        private int _health;
        private readonly List<Queue<ProjectileController>> _projectilePool = new List<Queue<ProjectileController>>();
    
        private ProjectileData Projectile => projectileData[_projectileIndex];

        [Inject]
        private void Construct(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        private void Awake()
        {
            foreach(var data in projectileData)
            {
                var pool = new Queue<ProjectileController>();
                for(int i = 0; i < projectilePoolSize; i++)
                {
                    var instance = Instantiate(data.Prefab, projectileContainer);
                    instance.gameObject.SetActive(false);
                    instance.OnDispose += OnProjectileDispose;
                    pool.Enqueue(instance);
                }
                _projectilePool.Add(pool);
            }
        }

        private void Start()
        {
            _health = maxHealth;
        }

        private void FixedUpdate()
        {
            var movementDirection = new Vector3(
                Input.GetAxis("Horizontal"),
                0,
                Input.GetAxis("Vertical"));
            movementDirection.Normalize();
        
            animator.SetBool(Moving, movementDirection.sqrMagnitude > 0);

            body.velocity = movementDirection * speed;
        }

        private void Update()
        {
            var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20);
            var lookPosition = viewCamera.ScreenToWorldPoint(mousePosition);
            lookPosition.y = 0;
            transform.LookAt(lookPosition);

            if(_cooldown <= 0 && Input.GetButton("Fire1"))
            {
                _cooldown = Projectile.Cooldown;
                _eventBus.PublishEvent(new AudioEvent.PlaySFX(Projectile.SFX));
            
                var instance = _projectilePool[_projectileIndex].Dequeue();
                instance.Initialize(shootPosition.position, transform.rotation, _projectileIndex);
            }
            else if(_cooldown > 0)
            {
                _cooldown -= Time.deltaTime;
            }
        }

        public void ApplyDamage(int damage)
        {
            _health -= damage;
            _eventBus.PublishEvent(new AudioEvent.PlaySFX(SFXType.Hit));
            _eventBus.PublishEvent(new PlayerEvent.UpdateHealth
            {
                Health = _health,
                MaxHealth = maxHealth
            });
            animator.SetTrigger(Hit);
            Instantiate(hitParticles, transform.position, Quaternion.identity);
            if(_health <= 0)
            {
                // TODO not great, not terrible
                var death = Instantiate(deathParticles, transform.position, Quaternion.Euler(-90, 0, 0));
                death.transform.localScale = Vector3.one / 1.25f;
                Destroy(gameObject);
                _eventBus.PublishEvent(new AudioEvent.PlayBGM(BGMType.GameOver));
                _eventBus.PublishEvent(new GameEvent.GameOver
                {
                    Message = "YOU DIED"
                });
            }
        }

        private void OnProjectileDispose(ProjectileController instance, int index)
        {
            _projectilePool[index].Enqueue(instance);
        }

        public void SetProjectile(int index) => _projectileIndex = index;
    }
}