using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Void.Audio;
using Void.Events;
using Void.Player;
using UnityEngine;
using Zenject;

namespace Void.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyData[] enemyData;
        [SerializeField] private PlayerController player;
        [SerializeField] private SpawnWave[] waves;
        [SerializeField] private float spawnRadius;
        [SerializeField] private int wavePeaceTime;

        private int _wave;
        private bool _spawned = true;
        private IEventBus _eventBus;

        private readonly HashSet<EnemyController> _enemies = new HashSet<EnemyController>();

        private readonly List<EnemyController> _level1Pool = new List<EnemyController>();
        private readonly List<EnemyController> _level2Pool = new List<EnemyController>();
        private readonly List<EnemyController> _level3Pool = new List<EnemyController>();

        private void Awake()
        {
            var maxLvl1 = waves.Max(w => w.Level1);
            var maxLvl2 = waves.Max(w => w.Level2);
            var maxLvl3 = waves.Max(w => w.Level3);
            CreatePool(enemyData[0], maxLvl1, _level1Pool);
            CreatePool(enemyData[1], maxLvl2, _level2Pool);
            CreatePool(enemyData[2], maxLvl3, _level3Pool);
        }

        [Inject]
        private void Construct(IEventBus eventBus)
        {
            this._eventBus = eventBus;
            eventBus.SubscribeEvent<GameEvent.StartGame>(OnStartGameEvent);
        }

        private void OnDestroy()
        {
            _eventBus.UnsubscribeEvent<GameEvent.StartGame>(OnStartGameEvent);
        }

        private void OnStartGameEvent(GameEvent.StartGame evt)
        {
            StartCoroutine(StartSpawnWave());
        }

        private void Update()
        {
            if(_spawned || _wave >= waves.Length)
            {
                return;
            }

            _eventBus.PublishEvent(new AudioEvent.PlayBGM(BGMType.Battle));
            _eventBus.PublishEvent(new AudioEvent.PlaySFX(SFXType.WaveStart));
            var waveData = waves[_wave];
            SpawnLevel(enemyData[0], waveData.Level1, _level1Pool);
            SpawnLevel(enemyData[1], waveData.Level2, _level2Pool);
            SpawnLevel(enemyData[2], waveData.Level3, _level3Pool);
            _wave++;
            _spawned = true;
        }

        private IEnumerator StartSpawnWave()
        {
            _eventBus.PublishEvent(new AudioEvent.PlayBGM(BGMType.Peace));
            for(int i = 0; i < wavePeaceTime; i++)
            {
                _eventBus.PublishEvent(new GameEvent.UpdateWave
                {
                    Wave = _wave + 1,
                    TimeLeft = wavePeaceTime - i
                });
                yield return new WaitForSeconds(1);
            }
            _eventBus.PublishEvent(new GameEvent.HideWave());
            _spawned = false;
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
                _enemies.Add(instance);
                instance.gameObject.SetActive(true);
            }
        }

        private void OnEnemyDestroyed(EnemyController instance)
        {
            _eventBus.PublishEvent(new AudioEvent.PlaySFX(SFXType.Kill));
            instance.OnDestroyed -= OnEnemyDestroyed;
            instance.gameObject.SetActive(false);
            _enemies.Remove(instance);
        
            HandleGameProgress();
        }

        private void HandleGameProgress()
        {
            if(_enemies.Count == 0)
            {
                if(_wave < waves.Length)
                {
                    player.SetProjectile(waves[_wave - 1].UnlockedProjectile);
                    StartCoroutine(StartSpawnWave());
                } else
                {
                    _eventBus.PublishEvent(new AudioEvent.PlayBGM(BGMType.GameOver));
                    _eventBus.PublishEvent(new GameEvent.GameOver
                    {
                        Message = "YOU SURVIVED"
                    });
                }
            }
        }
    }
}
