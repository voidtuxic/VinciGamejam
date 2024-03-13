using UnityEngine;
using Void.Core.Events;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyData[] enemyData;
    [SerializeField] private PlayerController player;
    [SerializeField] private float spawnRate;
    [SerializeField] private float spawnCooldown;
    [SerializeField] private float spawnRadius;

    private float lastSpawnTime;
    private IEventBus eventBus;
    
    [Inject]
    private void Construct(IEventBus eventBus)
    {
        this.eventBus = eventBus;
    }
    
    private void Update()
    {
        if(lastSpawnTime >= spawnCooldown)
        {
            var data = enemyData[0];

            for(int i = 0; i < spawnRate; i++)
            {
                var instance = Instantiate(data.Prefab, transform);
                var direction2d = Random.insideUnitCircle * spawnRadius;
                instance.transform.localPosition = new Vector3(direction2d.x, 0, direction2d.y);
                instance.Initialize(eventBus, player);
            }

            lastSpawnTime = 0;
        }
        else
        {
            lastSpawnTime += Time.deltaTime;
        }
    }
}
