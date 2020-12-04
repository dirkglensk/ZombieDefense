using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Profiles/Level Profile")]
public class LevelProfile : ScriptableObject
{
    public float initialSpawnRate;
    public float initialEnemySpeedScale;
    public float initialEnemyDamageScale;
    public float initialEnemyAttackSpeedScale;
    public float initialAggroRate;

    public float scalingSpawnRate;
    public float scalingEnemySpeedScale;
    public float scalingEnemyDamageScale;
    public float scalingEnemyAttackSpeedScale;
    public float scalingAggroRate;
}
