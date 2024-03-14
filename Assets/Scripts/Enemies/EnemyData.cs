using UnityEngine;

namespace Void.Enemies
{
    [CreateAssetMenu(menuName = "Game/EnemyData", fileName = "EnemyData", order = 0)]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private int life;
        [SerializeField] private float speed;
        [SerializeField] private float attackRange;
        [SerializeField] private float attackCooldown;
        [SerializeField] private int attackDamage;
        [SerializeField] private int score;
        [SerializeField] private EnemyController prefab;


        public int Life => life;
        public float Speed => speed;
        public float AttackCooldown => attackCooldown;
        public float AttackRange => attackRange;
        public int AttackDamage => attackDamage;
        public int Score => score;
        public EnemyController Prefab => prefab;

    }
}
