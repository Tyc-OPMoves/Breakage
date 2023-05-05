using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableEnemyUnit")]
public class ScriptableEnemyUnit : ScriptableUnitBaseStats
{
    public Type EnemyType;

    [Serializable]
    public enum Type
    {
        PurpleBeast,
        BeholderEye,
        Armadillo
    }
}
