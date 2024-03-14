using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Void.Core.Events;
using Zenject;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyData[] enemyData;
    [SerializeField] private PlayerController player;
    [SerializeField] private SpawnWave[] waves;
    [SerializeField] private float spawnRadius;
    [SerializeField] private int wavePeaceTime;

    private int wave;
    private bool spawned = true;
    private IEventBus eventBus;

    private readonly HashSet<EnemyController> enemies = new HashSet<EnemyController>();
    
    [Inject]
    private void Construct(IEventBus eventBus)
    {
        this.eventBus = eventBus;
        eventBus.SubscribeEvent<PlayerEvent.StartGame>(OnStartGameEvent);
    }

    private void OnDestroy()
    {
        eventBus.UnsubscribeEvent<PlayerEvent.StartGame>(OnStartGameEvent);
    }

    private void OnStartGameEvent(PlayerEvent.StartGame evt)
    {
        StartCoroutine(StartSpawnWave());
    }

    private void Update()
    {
        if(spawned || wave >= waves.Length)
        {
            return;
        }

        var waveData = waves[wave];
        SpawnLevel(enemyData[0], waveData.Level1);
        SpawnLevel(enemyData[1], waveData.Level2);
        SpawnLevel(enemyData[2], waveData.Level3);
        wave++;
        spawned = true;
    }

    private IEnumerator StartSpawnWave()
    {
        for(int i = 0; i < wavePeaceTime; i++)
        {
            eventBus.PublishEvent(new PlayerEvent.UpdateWave
            {
                Wave = wave + 1,
                TimeLeft = wavePeaceTime - i
            });
            yield return new WaitForSeconds(1);
        }
        eventBus.PublishEvent(new PlayerEvent.HideWave());
        spawned = false;
    }

    private void SpawnLevel(EnemyData data, int count)
    {
        for(int i = 0; i < count; i++)
        {
            var instance = Instantiate(data.Prefab, transform);
            var direction2d = Random.insideUnitCircle * spawnRadius;
            instance.transform.localPosition = new Vector3(direction2d.x, 0, direction2d.y);
            instance.Initialize(eventBus, player);
            instance.OnDestroyed += OnEnemyDestroyed;
            enemies.Add(instance);
        }
    }

    private void OnEnemyDestroyed(EnemyController instance)
    {
        instance.OnDestroyed -= OnEnemyDestroyed;
        enemies.Remove(instance);
        if(enemies.Count == 0)
        {
            if(wave < waves.Length)
            {
                player.SetProjectile(waves[wave - 1].UnlockedProjectile);
                StartCoroutine(StartSpawnWave());
            } else
            {
                eventBus.PublishEvent(new PlayerEvent.GameOver
                {
                    Message = "YOU SURVIVED"
                });
            }
        }
    }
}
