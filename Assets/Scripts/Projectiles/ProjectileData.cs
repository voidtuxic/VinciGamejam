using UnityEngine;

[CreateAssetMenu(menuName = "Game/ProjectileData", fileName = "ProjectileData", order = 0)]
public class ProjectileData : ScriptableObject
{
    [SerializeField] private string projectileName;
    [SerializeField] private float speed;
    [SerializeField] private float cooldown;
    [SerializeField] private float timeToLive;
    [SerializeField] private int damage;
    [SerializeField] private ProjectileController prefab;

    public float Speed => speed;
    public float Cooldown => cooldown;
    public float TimeToLive => timeToLive;
    public int Damage => damage;
    public ProjectileController Prefab => prefab;
}
