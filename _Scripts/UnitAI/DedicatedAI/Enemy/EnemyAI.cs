using UnityEngine;

[RequireComponent(typeof(NavMeshAI))]
[RequireComponent(typeof(UnitBaseLogic))]
public class EnemyAI : AIClass
{
    
    private void Update()
    {
        ChangeEnemyAIBehaviorOnGameStateChange();
    }

    void ChangeEnemyAIBehaviorOnGameStateChange()
    {
        if (GameStateManager.Instance.GetCurrentGameState() == GameStateManager.GameState.Combat)
        {
            if (GetCurrentBehavior() == AIBehavior.Idle)
            {
                AIBehaviorUpdate(AIBehavior.Chase);
            }
            else if (GetCurrentBehavior() == AIBehavior.Chase)
            {
                if (_canAttack)
                {
                    AIBehaviorUpdate(AIBehavior.Attack);
                }
            }
        }
        else
        {
            AIBehaviorUpdate(AIBehavior.Idle);
        }
    }
}
