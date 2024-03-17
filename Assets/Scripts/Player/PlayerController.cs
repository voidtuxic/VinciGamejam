using System.Collections.Generic;
using Input;
using Void.Events;
using Void.Projectiles;
using UnityEngine;
using Void.States;
using Zenject;

namespace Void.Player
{
    public class PlayerController : MonoBehaviour
    {
        private static readonly int Moving = Animator.StringToHash("Moving");
        private static readonly int Hit = Animator.StringToHash("Hit");
    
        [SerializeField] private StateSettings stateSettings;
        [SerializeField] private Rigidbody body;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform shootPosition;
        [SerializeField] private int projectilePoolSize;
        [SerializeField] private Transform projectileContainer;

        private IEventBus _eventBus;
        private IInputController _inputController;
        private readonly List<Queue<ProjectileController>> _projectilePool = new List<Queue<ProjectileController>>();

        [Inject]
        private void Construct(IEventBus eventBus, IInputController inputController)
        {
            _eventBus = eventBus;
            _inputController = inputController;
            
            eventBus.SubscribeEvent<PlayerEvent.Kill>(OnKillEvent);
            eventBus.SubscribeEvent<PlayerEvent.Fire>(OnFireEvent);
        }

        private void Awake()
        {
            foreach(var data in stateSettings.ProjectileData)
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

        private void FixedUpdate()
        {
            animator.SetBool(Moving, _inputController.MovementDirection.sqrMagnitude > 0);
            body.velocity = _inputController.MovementDirection * stateSettings.PlayerSpeed;
        }

        private void Update()
        {
            transform.LookAt(_inputController.LookPosition);
        }

        private void OnProjectileDispose(ProjectileController instance, int index)
        {
            _projectilePool[index].Enqueue(instance);
        }

        public void ApplyDamage(int damage)
        {
            animator.SetTrigger(Hit);
            Instantiate(stateSettings.PlayerHitParticles, transform.position, Quaternion.identity);
            _eventBus.PublishEvent(new PlayerEvent.Damage(damage));
        }

        private void OnFireEvent(PlayerEvent.Fire evt)
        {
            var instance = _projectilePool[evt.ProjectileIndex].Dequeue();
            instance.Initialize(shootPosition.position, transform.rotation, evt.ProjectileIndex);
        }

        private void OnKillEvent(PlayerEvent.Kill evt)
        {
            // TODO not great, not terrible
            var death = Instantiate(stateSettings.PlayerDeathParticles, transform.position, Quaternion.Euler(-90, 0, 0));
            death.transform.localScale = Vector3.one / 1.25f;
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _eventBus.UnsubscribeEvent<PlayerEvent.Kill>(OnKillEvent);
        }
    }
}