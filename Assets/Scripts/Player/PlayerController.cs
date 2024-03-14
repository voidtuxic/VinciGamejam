using System.Collections.Generic;
using UnityEngine;
using Void.Core.Events;
using Zenject;

public class PlayerController : MonoBehaviour
{
    private static readonly int Moving = Animator.StringToHash("Moving");
    private static readonly int Hit = Animator.StringToHash("Hit");
    
    [SerializeField] private Camera viewCamera;
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

    private int projectileIndex;
    private float cooldown;
    private int health;
    private IEventBus eventBus;
    private ProjectileData projectile => projectileData[projectileIndex];
    private readonly List<Queue<ProjectileController>> projectilePool = new List<Queue<ProjectileController>>();

    [Inject]
    private void Construct(IEventBus eventBus)
    {
        this.eventBus = eventBus;
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
            projectilePool.Add(pool);
        }
    }

    private void Start()
    {
        health = maxHealth;
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

        if(cooldown <= 0 && Input.GetButton("Fire1"))
        {
            cooldown = projectile.Cooldown;
            eventBus.PublishEvent(new AudioEvent.PlaySFX(projectile.SFX));
            
            var instance = projectilePool[projectileIndex].Dequeue();
            instance.Initialize(shootPosition.position, transform.rotation, projectileIndex);
        }
        else if(cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;
        eventBus.PublishEvent(new AudioEvent.PlaySFX(SFXType.Hit));
        eventBus.PublishEvent(new PlayerEvent.UpdateHealth
        {
            Health = health,
            MaxHealth = maxHealth
        });
        animator.SetTrigger(Hit);
        Instantiate(hitParticles, transform.position, Quaternion.identity);
        if(health <= 0)
        {
            // TODO not great, not terrible
            var death = Instantiate(deathParticles, transform.position, Quaternion.Euler(-90, 0, 0));
            death.transform.localScale = Vector3.one / 1.25f;
            Destroy(gameObject);
            eventBus.PublishEvent(new AudioEvent.PlayBGM(BGMType.GameOver));
            eventBus.PublishEvent(new GameEvent.GameOver
            {
                Message = "YOU DIED"
            });
        }
    }

    private void OnProjectileDispose(ProjectileController instance, int index)
    {
        projectilePool[index].Enqueue(instance);
    }

    public void SetProjectile(int index) => projectileIndex = index;
}