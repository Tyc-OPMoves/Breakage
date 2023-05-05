using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshAI : MonoBehaviour
{
    [NonSerialized] public NavMeshAgent _AIAgent;
    AIClass _AIClass;
    Transform _currentTarget;
    Transform _desiredTarget;


    

    float _patrolScoutingTime;
    float _patrolScoutingDuration = 1f;
    Coroutine _patrolRoutine;
    void Start()
    {
        _AIAgent = GetComponent<NavMeshAgent>();
        _AIClass = GetComponent<AIClass>();
        _AIAgent.autoRepath = true;
        _AIAgent.isStopped = true;
        _AIAgent.angularSpeed = 360f;
        _AIAgent.acceleration = 30f;
        _AIAgent.speed = 1;
        _AIAgent.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;

        _AIAgent.autoBraking = true;

        _currentTarget = null;
        


    }
    void Update()
    {
        ChaseTarget();
        ChangePatrolPos();

    }
   

    void ChaseTarget()
    {
        if (_desiredTarget != null)
        {

            _AIAgent.isStopped = false;
            _currentTarget = _desiredTarget;
            _currentTarget.position = _desiredTarget.position;

            _AIAgent.destination = _currentTarget.position;


            //Debug.Log($"|{this.name}| is chasing |{_currentTarget.name}| at pos |{_AIAgent.destination}|");

        }
        else
        {
            if (_AIAgent.isOnNavMesh)
            {
                _AIAgent.isStopped = true;
                _currentTarget = null;
                //Debug.Log($"Current target is |{_currentTarget}|");
            }
        }
    }
    void ChangePatrolPos()
    {
        if (_AIClass.GetCurrentBehavior() == AIClass.AIBehavior.Patrol)
        {

            //Debug.Log($"|{this.name}| remaining distance to |{_desiredTarget.name}| is |{_AIAgent.remainingDistance}|");
            if (_AIAgent.remainingDistance <= _AIAgent.stoppingDistance )
            {
                if (_patrolScoutingTime <= Time.time)
                {
                    _patrolScoutingTime = (_patrolScoutingDuration+0.2f) + Time.time;
                    
                    PatrolScouting(_patrolScoutingDuration);
                    Debug.Log($"|{this.name}| PatrolScoutingRoutine will be initiated.");
                }
            }
        }
    }
    void PatrolScouting(float timer)
    {

        _patrolRoutine = StartCoroutine(PatrolScoutingRoutine(timer));
    }
    public IEnumerator PatrolScoutingRoutine(float timer)
    {
        yield return new WaitForSeconds(timer);
        ChangeAIPatrolPos(_currentTarget);
        Debug.Log("PatrolScoutingRoutine should have finished.");
        
    }
    public void ChangeAIPatrolPos(Transform currentTarget)
    {
        ModifyStoppingDistance(1f);

        _desiredTarget = UnitDatabase.Instance.GetPatrolPos(currentTarget);
        Debug.Log($"Current target is |{_desiredTarget.name}|.");
    }
    public void ChangeAITarget(Transform target)
    {
        ModifyStoppingDistance(2f);

        _desiredTarget = target;


    }
   
    public void ModifyStoppingDistance(float distance)
    {
        _AIAgent.stoppingDistance = distance;
    }
    public Transform GetAIDestination()
    {
        return _currentTarget;
    }
    public float GetAIMovementSpeed()
    {
        return _AIAgent.velocity.magnitude;
    }
    public void SetAIMovementSpeed(float unitMovementSpeed)
    {
        _AIAgent.speed = unitMovementSpeed;
    }
    public float GetStoppingDistance()
    {
        return _AIAgent.stoppingDistance;
    }




}
