using UnityEngine;
using Void.Core.Events;
using Zenject;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyData data;
    [SerializeField] private GameObject deathParticles;

    private PlayerController target;
    private int currentLife;
    private float attackCooldown;
    private IEventBus eventBus;
    private bool hasSpawned;
    
    public void Initialize(IEventBus eventBus, PlayerController target)
    {
        this.target = target;
        this.eventBus = eventBus;
        currentLife = data.Life;
        hasSpawned = false;
    }

    public void ApplyDamage(int damage)
    {
        currentLife -= damage;

        if(currentLife <= 0)
        {
            eventBus.PublishEvent(new PlayerEvent.UpdateScore
            {
                AddedScore = data.Score
            });
            var death = Instantiate(deathParticles, transform.position, Quaternion.identity);
            death.transform.localScale = Vector3.one / 2f;
            Destroy(gameObject);
        }
    }

    public void HasSpawned()
    {
        hasSpawned = true;
    }

    private void FixedUpdate()
    {
        if(!hasSpawned || target == null)
        {
            body.velocity = Vector3.zero;
            animator.SetBool("Moving", false);
            return;
        }

        var direction = target.transform.position - transform.position;
        direction.Normalize();
        transform.LookAt(target.transform);
        
        if(Vector3.Distance(transform.position, target.transform.position) <= data.AttackRange)
        {
            body.velocity = Vector3.zero;
            animator.SetBool("Moving", false);
            if(attackCooldown <= 0)
            {
                animator.SetTrigger("Attack");
                target.ApplyDamage(data.AttackDamage);
                attackCooldown = data.AttackCooldown;
            }
        }
        else
        {
            animator.SetBool("Moving", true);
            body.velocity = direction * data.Speed;
        }
        
        if(attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
    }
}