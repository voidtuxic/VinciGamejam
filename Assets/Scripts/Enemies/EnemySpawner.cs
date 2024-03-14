using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Void.Core.Events;
using Zenject;

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

    private readonly List<EnemyController> level1Pool = new List<EnemyController>();
    private readonly List<EnemyController> level2Pool = new List<EnemyController>();
    private readonly List<EnemyController> level3Pool = new List<EnemyController>();

    private void Awake()
    {
        var maxLvl1 = waves.Max(w => w.Level1);
        var maxLvl2 = waves.Max(w => w.Level2);
        var maxLvl3 = waves.Max(w => w.Level3);
        CreatePool(enemyData[0], maxLvl1, level1Pool);
        CreatePool(enemyData[1], maxLvl2, level2Pool);
        CreatePool(enemyData[2], maxLvl3, level3Pool);
    }

    [Inject]
    private void Construct(IEventBus eventBus)
    {
        this.eventBus = eventBus;
        eventBus.SubscribeEvent<GameEvent.StartGame>(OnStartGameEvent);
    }

    private void OnDestroy()
    {
        eventBus.UnsubscribeEvent<GameEvent.StartGame>(OnStartGameEvent);
    }

    private void OnStartGameEvent(GameEvent.StartGame evt)
    {
        StartCoroutine(StartSpawnWave());
    }

    private void Update()
    {
        if(spawned || wave >= waves.Length)
        {
            return;
        }

        eventBus.PublishEvent(new AudioEvent.PlayBGM(BGMType.Battle));
        eventBus.PublishEvent(new AudioEvent.PlaySFX(SFXType.WaveStart));
        var waveData = waves[wave];
        SpawnLevel(enemyData[0], waveData.Level1, level1Pool);
        SpawnLevel(enemyData[1], waveData.Level2, level2Pool);
        SpawnLevel(enemyData[2], waveData.Level3, level3Pool);
        wave++;
        spawned = true;
    }

    private IEnumerator StartSpawnWave()
    {
        eventBus.PublishEvent(new AudioEvent.PlayBGM(BGMType.Peace));
        for(int i = 0; i < wavePeaceTime; i++)
        {
            eventBus.PublishEvent(new GameEvent.UpdateWave
            {
                Wave = wave + 1,
                TimeLeft = wavePeaceTime - i
            });
            yield return new WaitForSeconds(1);
        }
        eventBus.PublishEvent(new GameEvent.HideWave());
        spawned = false;
    }

    private void CreatePool(EnemyData data, int count, List<EnemyController> pool)
    {
        for(int i = 0; i < count; i++)
        {
            var instance = Instantiate(data.Prefab, transform);
            instance.gameObject.SetActive(false);
            pool.Add(instance);
        }
    }

    private void SpawnLevel(EnemyData data, int count, List<EnemyController> pool)
    {
        for(int i = 0; i < count; i++)
        {
            var instance = pool[i];
            var direction2d = Random.insideUnitCircle * spawnRadius;
            instance.transform.localPosition = new Vector3(direction2d.x, 0, direction2d.y);
            instance.Initialize(player);
            instance.OnDestroyed += OnEnemyDestroyed;
            enemies.Add(instance);
            instance.gameObject.SetActive(true);
        }
    }

    private void OnEnemyDestroyed(EnemyController instance)
    {
        eventBus.PublishEvent(new AudioEvent.PlaySFX(SFXType.Kill));
        instance.OnDestroyed -= OnEnemyDestroyed;
        instance.gameObject.SetActive(false);
        enemies.Remove(instance);
        
        HandleGameProgress();
    }

    private void HandleGameProgress()
    {
        if(enemies.Count == 0)
        {
            if(wave < waves.Length)
            {
                player.SetProjectile(waves[wave - 1].UnlockedProjectile);
                StartCoroutine(StartSpawnWave());
            } else
            {
                eventBus.PublishEvent(new AudioEvent.PlayBGM(BGMType.GameOver));
                eventBus.PublishEvent(new GameEvent.GameOver
                {
                    Message = "YOU SURVIVED"
                });
            }
        }
    }
}
