using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Void.Audio;
using Void.Events;
using Void.Player;
using UnityEngine;
using Void.States;
using Zenject;

namespace Void.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private StateSettings stateSettings;

        private IEventBus _eventBus;

        private readonly HashSet<EnemyController> _enemies = new HashSet<EnemyController>();

        private readonly List<EnemyController> _level1Pool = new List<EnemyController>();
        private readonly List<EnemyController> _level2Pool = new List<EnemyController>();
        private readonly List<EnemyController> _level3Pool = new List<EnemyController>();


        [Inject]
        private void Construct(IEventBus eventBus)
        {
            _eventBus = eventBus;
            eventBus.SubscribeEvent<GameEvent.StartWave>(OnStartWaveEvent);
        }
        
        private void Awake()
        {
            var maxLvl1 = stateSettings.Waves.Max(w => w.Level1);
            var maxLvl2 = stateSettings.Waves.Max(w => w.Level2);
            var maxLvl3 = stateSettings.Waves.Max(w => w.Level3);
            CreatePool(stateSettings.EnemyData[0], maxLvl1, _level1Pool);
            CreatePool(stateSettings.EnemyData[1], maxLvl2, _level2Pool);
            CreatePool(stateSettings.EnemyData[2], maxLvl3, _level3Pool);
        }

        private void OnStartWaveEvent(GameEvent.StartWave evt)
        {
            var waveData = stateSettings.Waves[evt.Wave];
            SpawnLevel(stateSettings.EnemyData[0], waveData.Level1, _level1Pool);
            SpawnLevel(stateSettings.EnemyData[1], waveData.Level2, _level2Pool);
            SpawnLevel(stateSettings.EnemyData[2], waveData.Level3, _level3Pool);
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
                var direction2d = Random.insideUnitCircle * stateSettings.SpawnRadius;
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
        
            if(_enemies.Count == 0)
            {
                _eventBus.PublishEvent(new GameEvent.EndWave());
            }
        }

        private void OnDestroy()
        {
            _eventBus.UnsubscribeEvent<GameEvent.StartWave>(OnStartWaveEvent);
        }
    }
}
