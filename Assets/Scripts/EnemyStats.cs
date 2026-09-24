using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public GameObject prefab;

    public int lives;
    public int damage;
    public int speed;
    public int recourceReward;
    public int scoreReward;

}
