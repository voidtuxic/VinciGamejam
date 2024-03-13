using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Void.Core.Events;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera viewCamera;
    [SerializeField] private Rigidbody body;
    [SerializeField] private float speed;
    [SerializeField] private int maxHealth;
    [SerializeField] private ProjectileData[] projectileData;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private GameObject deathParticles;

    private int projectileIndex;
    private float cooldown;
    private int health;
    private IEventBus eventBus;
    private ProjectileData projectile => projectileData[projectileIndex];

    [Inject]
    private void Construct(IEventBus eventBus)
    {
        this.eventBus = eventBus;
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
        
        animator.SetBool("Moving", movementDirection.sqrMagnitude > 0);

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
            var instance = Instantiate(projectile.Prefab, shootPosition.position, transform.rotation);
            instance.Initialize(projectile);
            cooldown = projectile.Cooldown;
        }
        else if(cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;
        eventBus.PublishEvent(new PlayerEvent.UpdateHealth
        {
            Health = health,
            MaxHealth = maxHealth
        });
        if(health <= 0)
        {
            // TODO not great, not terrible
            var death = Instantiate(deathParticles, transform.position, Quaternion.Euler(-90, 0, 0));
            death.transform.localScale = Vector3.one / 1.25f;
            Destroy(gameObject);
            eventBus.PublishEvent(new PlayerEvent.GameOver
            {
                Message = "YOU DIED"
            });
        }
    }
}