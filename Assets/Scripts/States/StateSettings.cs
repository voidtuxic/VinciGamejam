using UnityEngine;
using Void.Enemies;
using Void.Projectiles;

namespace Void.States
{
    [CreateAssetMenu(menuName = "Game/State Settings", fileName = "StateSettings", order = 0)]
    public class StateSettings : ScriptableObject
    {
        [Header("General")] 
        [SerializeField] private StateId initialState;

        public StateId InitialState => initialState;

        [Header("Waves")]
        [SerializeField] private int peaceTime;
        [SerializeField] private float spawnRadius;
        [SerializeField] private EnemyData[] enemyData;
        [SerializeField] private SpawnWave[] waves;

        public SpawnWave[] Waves => waves;
        public EnemyData[] EnemyData => enemyData;
        public float SpawnRadius => spawnRadius;
        public int PeaceTime => peaceTime;
        
        [Header("Player")]
        [SerializeField] private float playerSpeed;
        [SerializeField] private int playerMaxHealth;
        [SerializeField] private ProjectileData[] projectileData;
        [SerializeField] private GameObject playerDeathParticles;
        [SerializeField] private GameObject playerHitParticles;

        public GameObject PlayerHitParticles => playerHitParticles;
        public GameObject PlayerDeathParticles => playerDeathParticles;
        public ProjectileData[] ProjectileData => projectileData;
        public int PlayerMaxHealth => playerMaxHealth;
        public float PlayerSpeed => playerSpeed;
    }
}
