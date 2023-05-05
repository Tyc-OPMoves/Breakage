using UnityEngine;

[RequireComponent(typeof(NavMeshAI))]
[RequireComponent(typeof(UnitBaseLogic))]
public class AllyAI : AIClass
{
   
    private void Update()
    {
        ChangeAllyAIBehaviorOnGameStateChange();
    }

    void ChangeAllyAIBehaviorOnGameStateChange()
    {
        if (GameStateManager.Instance.GetCurrentGameState() == GameStateManager.GameState.Combat)
        {
            if (GetCurrentBehavior() == AIBehavior.Idle)
            {
                AIBehaviorUpdate(AIBehavior.Patrol);
                
            }
            else if (GetCurrentBehavior() == AIBehavior.Patrol)
            {
                if (_canChase)
                {
                    AIBehaviorUpdate(AIBehavior.Chase);
                }
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
