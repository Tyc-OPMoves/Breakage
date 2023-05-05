using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//Holds ALL info about unit stats or general scene transforms (like spawns and targets)
public class UnitDatabase : StaticInstance<UnitDatabase>
{
    public List<ScriptableAllyUnit> _AllyBaseUnitList;
    [SerializeField]Dictionary<ScriptableAllyUnit.Type, ScriptableAllyUnit> _AllyBaseUnitDictionary;

    public List<ScriptableEnemyUnit> _EnemyBaseUnitList;
    Dictionary<ScriptableEnemyUnit.Type, ScriptableEnemyUnit> _EnemyBaseUnitDictionary;

    [SerializeField]Transform[] _patrolPositionArray = new Transform[10];

    public Transform _enemySpawnPos1;
    public Transform _allySpawnPos1;
    public Transform _defaultTarget1;
    

    protected override void Awake()
    {
        base.Awake();
        AssembleAllyDatabase();
        AssembleEnemyDatabse();
    }
    void AssembleAllyDatabase()
    {
        _AllyBaseUnitList = Resources.LoadAll<ScriptableAllyUnit>("Ally").ToList();
        _AllyBaseUnitDictionary = _AllyBaseUnitList.ToDictionary(allyUnit => allyUnit.AllyType, allyUnit => allyUnit);
    }
    void AssembleEnemyDatabse()
    {
        _EnemyBaseUnitList = Resources.LoadAll<ScriptableEnemyUnit>("Enemy").ToList();
        _EnemyBaseUnitDictionary = _EnemyBaseUnitList.ToDictionary(enemyUnit => enemyUnit.EnemyType, enemyUnit => enemyUnit);
    }
    public ScriptableAllyUnit GetBaseAllyStats(ScriptableAllyUnit.Type allyType) => _AllyBaseUnitDictionary[allyType];
    public ScriptableEnemyUnit GetBaseEnemyStats(ScriptableEnemyUnit.Type enemyType) => _EnemyBaseUnitDictionary[enemyType];
    public Transform GetPatrolPos(Transform currentPos)
    {
        int randomRange = UnityEngine.Random.Range(0, 10);
        var newPos = _patrolPositionArray[randomRange];
        if(newPos != currentPos)
        {
            return newPos;
        }
        else
        {
            var exceptionRandomRange = UnityEngine.Random.Range(UnityEngine.Random.Range(0, randomRange), UnityEngine.Random.Range(randomRange+1, 10));
            var exceptionPos = _patrolPositionArray[exceptionRandomRange];
            return exceptionPos;
        }
        
    }
}
