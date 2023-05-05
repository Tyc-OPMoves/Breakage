using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class UnitManager : StaticInstance<UnitManager>
{
    [SerializeField] Transform _enemyPurpleBeastContainer;
    [SerializeField] Transform _allyFishmanContainer;
    static int _AlliesAlive;
    static int _EnemiesAlive;
    
    public void SpawnAllies(ScriptableAllyUnit.Type allyType, Vector3 spawnPos, ScriptableEnemyUnit.Stats statsToModify, int numberToSpawn)
    {
        for (int i = 1; i <= numberToSpawn; i++)
        {
            SpawnAlly(allyType, spawnPos, statsToModify);
        }
        if(GameStateManager.Instance._autoRun) 
        {
            GameStateManager.Instance.UpdateGameState(GameStateManager.GameState.EnemySpawning);
        }
    }
    public void SpawnEnemies(ScriptableEnemyUnit.Type enemyType, Vector3 spawnPos, ScriptableEnemyUnit.Stats statsToModify, int numberToSpawn)
    {
        for(int i = 1; i <=numberToSpawn; i++)
        {
            SpawnEnemy(enemyType,spawnPos,statsToModify);
        }
        if (GameStateManager.Instance._autoRun)
        {
            GameStateManager.Instance.UpdateGameState(GameStateManager.GameState.Combat);
        }
    }

    void SpawnAlly(ScriptableAllyUnit.Type allyType, Vector3 spawnPos, ScriptableAllyUnit.Stats statsToModify)
    {
        ScriptableAllyUnit allyBase = UnitDatabase.Instance.GetBaseAllyStats(allyType);
        UnitBaseLogic currentAllyInstantiation = Instantiate(allyBase._unitPrefab, spawnPos, Quaternion.identity);
        _AlliesAlive++;
        Debug.Log($"(UnitManager): Allies Alive: |{_AlliesAlive}|.");
        var AllyAI = currentAllyInstantiation.GetComponent<AllyAI>();
        AllyAI._patrolPos = UnitDatabase.Instance.GetPatrolPos(null);
        
        currentAllyInstantiation.name = $"(Ally)Fishman-{_AlliesAlive}-";
        currentAllyInstantiation.transform.SetParent(_allyFishmanContainer);

        currentAllyInstantiation.SetStats(statsToModify);
    }

    void SpawnEnemy(ScriptableEnemyUnit.Type enemyType, Vector3 spawnPos, ScriptableEnemyUnit.Stats statsToModify)
    {
        ScriptableEnemyUnit enemyBase = UnitDatabase.Instance.GetBaseEnemyStats(enemyType);
        UnitBaseLogic currentEnemyInstantiation = Instantiate(enemyBase._unitPrefab, spawnPos, Quaternion.identity);
        _EnemiesAlive++;
        Debug.Log($"(UnitManager): Enemies Alive: |{_EnemiesAlive}|.");
        var EnemyAI = currentEnemyInstantiation.GetComponent<EnemyAI>();
        EnemyAI._target = UnitDatabase.Instance._defaultTarget1;
        EnemyAI._canChase = true;

        currentEnemyInstantiation.name = $"(Enemy)PurpleBeast-{_EnemiesAlive}-";
        currentEnemyInstantiation.transform.SetParent(_enemyPurpleBeastContainer);

        currentEnemyInstantiation.SetStats(statsToModify);
    }

    public void UpdateEnemiesCounter()
    {
        _EnemiesAlive--;
        Debug.Log($"(UnitManager): Enemies Alive: |{_EnemiesAlive}|.");
    }
}
