using System;
using UnityEngine;

public class GameStateManager : StaticInstance<GameStateManager>
{
    [Serializable]
    public enum GameState
    {
        Setup,
        AllySpawning,
        EnemySpawning,
        Combat,
        Aftermath
    }

    GameState _GameState;
    [SerializeField] GameState _desiredGameState;
    [SerializeField] ScriptableAllyUnit.Type _selectedAlly;
    [SerializeField] int alliesToSpawn = 1;

    [SerializeField] ScriptableEnemyUnit.Type _selectedEnemy;
    [SerializeField] int enemiesToSpawn = 1;
    public bool _autoRun { get; private set; }


    [SerializeField] ScriptableUnitBaseStats.Stats _allyStatsToModify = new();
    [SerializeField] ScriptableUnitBaseStats.Stats _enemyStatsToModify = new();

    
    void Update()
    {
        GameStateUpdate();
        AutoStart();
    }
    void AutoStart()
    {
        
    }
    void GameStateUpdate()
    {
        if (_GameState != _desiredGameState)
        {
            _GameState = _desiredGameState;
            switch (_GameState)
            {
                case GameState.Setup:
                    {
                        Debug.Log($"Current Game state is: |{_GameState}|.");

                    }
                    break;
                case GameState.AllySpawning:
                    {
                        Debug.Log($"Current Game state is: |{_GameState}|.");

                        var spawnPos = UnitDatabase.Instance._allySpawnPos1.position;
                        var allyStats = UnitDatabase.Instance.GetBaseAllyStats(_selectedAlly).BaseStats;

                        allyStats._MaxHealth += _allyStatsToModify._MaxHealth;
                        allyStats._CurrentHealth = allyStats._MaxHealth;
                        allyStats._Armour += _allyStatsToModify._Armour;
                        allyStats._AttackSpeed += _allyStatsToModify._AttackSpeed;
                        allyStats._Damage += _allyStatsToModify._Damage;
                        allyStats._MovementSpeed += _allyStatsToModify._MovementSpeed;
                        allyStats._Price += _allyStatsToModify._Price;
                        UnitManager.Instance.SpawnAllies(_selectedAlly, spawnPos, allyStats, alliesToSpawn);
                    }
                    break;
                case GameState.EnemySpawning:
                    {
                        Debug.Log($"Current Game state is: |{_GameState}|.");

                        var spawnPos = UnitDatabase.Instance._enemySpawnPos1.position;
                        var enemyStats = UnitDatabase.Instance.GetBaseEnemyStats(_selectedEnemy).BaseStats;

                        enemyStats._MaxHealth += _enemyStatsToModify._MaxHealth;
                        enemyStats._CurrentHealth = enemyStats._MaxHealth;
                        enemyStats._Armour += _enemyStatsToModify._Armour;
                        enemyStats._AttackSpeed += _enemyStatsToModify._AttackSpeed;
                        enemyStats._Damage += _enemyStatsToModify._Damage;
                        enemyStats._MovementSpeed += _enemyStatsToModify._MovementSpeed;
                        enemyStats._Price += _enemyStatsToModify._Price;

                        UnitManager.Instance.SpawnEnemies(_selectedEnemy, spawnPos, enemyStats, enemiesToSpawn);
                    }
                    break;
                case GameState.Combat:
                    {
                        Debug.Log($"Current Game state is: |{_GameState}|.");
                        AutoRun(false);
                    }
                    break;
                case GameState.Aftermath:
                    {
                        Debug.Log($"Current Game state is: |{_GameState}|.");
                    }
                    break;
            }
        }
        else return;
    }

    public GameState GetCurrentGameState()
    {
        return _GameState;
    }
    public void UpdateGameState(GameState state)
    {
        _desiredGameState = state;
    }

    public void AutoRun(bool autoRunState)
    {
        _autoRun = autoRunState;    
    }
}
