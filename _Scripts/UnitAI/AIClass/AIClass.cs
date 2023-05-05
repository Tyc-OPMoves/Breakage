using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AIClass : MonoBehaviour
{
    protected NavMeshAI _navMeshAI;
    public Transform _target;
    public Transform _patrolPos;
    public bool _canChase;
    public bool _canAttack;
    [Serializable]
    public enum AIBehavior
    {
        Idle,
        Patrol,
        Chase,
        Attack
    }
    [SerializeField] AIBehavior _State;


    private void Start()
    {
        _navMeshAI = GetComponent<NavMeshAI>();
        
        Debug.Log($"Current AI |{this.name}| state is: |{_State}|.");
        
    }

    public void AIBehaviorUpdate(AIBehavior behavior)
    {
        if (_State != behavior)
        {
            _State = behavior;
            switch (_State)
            {
                case AIBehavior.Idle:
                    {
                        Debug.Log($"Current AI | {this.name} | state is: |{_State}|.");
                        _navMeshAI.ChangeAITarget(null);

                    }
                    break;
                case AIBehavior.Patrol:
                    {
                        Debug.Log($"Current AI | {this.name} | state is: |{_State}|.");
                        
                        _navMeshAI.ChangeAIPatrolPos(GetNewPatrolPos());
                       
                    }
                    break;
                case AIBehavior.Chase:
                    {   
                        Debug.Log($"Current AI | {this.name} | state is: |{_State}|.");
                        if(TryGetComponent<AllyAI>(out AllyAI allyAI))
                        {
                            return;
                        }
                        else
                        {
                            _navMeshAI.ChangeAITarget(GetAlliedMainBaseTarget());
                        }
                        
                    }
                    break;
                case AIBehavior.Attack:
                    {
                        Debug.Log($"Current AI | {this.name} | state is: |{_State}|.");

                    }
                    break;
            }
        }
    }

    Transform GetNewPatrolPos()
    {
        
        _navMeshAI.ModifyStoppingDistance(1);
        
        return _patrolPos;
    }

    Transform GetAlliedMainBaseTarget()
    {
        //to add way to change target if requested
        
        _navMeshAI.ModifyStoppingDistance(2);
        
        return _target;
    }

     public AIBehavior GetCurrentBehavior()
    {
        return _State;
    }

    public void ToggleChaceStatus(bool canChase)
    {
        if (canChase)
        {
            _canChase = true;
        }
        else
        {
            _canChase = false;
        }
    }

    public void ToggleAttackStatus(bool canAttack)
    {
        if (canAttack)
        {
            _canAttack = true;
            Debug.Log($"Unit |{this.name}| can now attack.");
        }
        else
        {
            _canAttack = false;
            Debug.Log($"Unit |{this.name}| cannot attack.");
        }
    }
}

