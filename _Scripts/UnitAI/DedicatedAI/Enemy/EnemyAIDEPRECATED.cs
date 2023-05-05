using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIDEPRECATED : MonoBehaviour
{
    [SerializeField] List<ScriptableAllyUnit> _allyUnits = new();
    [SerializeField] readonly Transform _target;
    NavMeshAgent _enemyAI;
    Animator _animator;
    public float _currentDistance;
    float _currentSpeed;

    bool _canAttack = true;
    Coroutine _attacking;

    public enum EnemyBehavior
    {
        Idle,
        Patrol,
        Chase,
        Attack
    }

    public EnemyBehavior _State;
    // Start is called before the first frame update
#pragma warning disable IDE0051 // Remove unused private members
    void Start()
#pragma warning restore IDE0051 // Remove unused private members
    {
        _allyUnits = Resources.LoadAll<ScriptableAllyUnit>("Ally").ToList();
        _enemyAI = GetComponent<NavMeshAgent>();
        _animator= GetComponent<Animator>();
        _State = EnemyBehavior.Idle;
    }

    // Update is called once per frame
#pragma warning disable IDE0051 // Remove unused private members
    void Update()
#pragma warning restore IDE0051 // Remove unused private members
    {
        DistanceToTarget();
        Locomotion();
        EnemyBehaviorUpdate();
    }
    void DistanceToTarget()
    {
        
        _currentDistance = Vector3.Distance(_target.position, _enemyAI.transform.position);
       //Debug.Log(_currentDistance);
    }

    void Locomotion()
    {

        if (_enemyAI.velocity.x == 0 && _enemyAI.velocity.z == 0)
        {
            _currentSpeed = 0;
        }
        else if(_enemyAI.velocity.x != 0)
        {
            _currentSpeed = Mathf.Abs(_enemyAI.velocity.x);
        }
        else if(_enemyAI.velocity.z != 0)
        {
            _currentSpeed = Mathf.Abs(_enemyAI.velocity.z);
        }
        else
        {
            _currentSpeed = 1;
        }
        //Debug.Log($"Current enemt speed X is: |{_enemyAI.velocity.x}|, and on Y is: |{_enemyAI.velocity.x}|");
        _animator.SetFloat("Locomotion", _currentSpeed);
    }

    void EnemyBehaviorUpdate()
    {
        switch (_State) // 1. Define each _State. 2. Have corelations between all _States.
        {
            case EnemyBehavior.Idle:
                {
                    if (_currentDistance <= 1000 && _currentDistance >= 10)
                    {
                        _enemyAI.isStopped = true;
                        _State = EnemyBehavior.Chase;
                    }
                    break;
                }
            case EnemyBehavior.Chase:
                {
                    if (_currentDistance <= 1000 && _currentDistance >= 10)
                    {
                        _enemyAI.isStopped = false;
                        _enemyAI.destination = _target.transform.position;
                    }
                    else if (_currentDistance <= 10)
                    {
                        _enemyAI.isStopped = true;
                        _State = EnemyBehavior.Attack;
                    }
                    else
                    {
                        _State = EnemyBehavior.Attack;
                    }
                }
                break;
            case EnemyBehavior.Attack:
                {
                    if (_canAttack)
                    {
                        if(_attacking != null)
                        {
                            StopCoroutine(_attacking);
                        }
                        _attacking = StartCoroutine(Attack());
                        _canAttack = false;
                    }
                    else
                    {
                        _canAttack = false;
                    }
                }
                break;
                /*case EnemyBehavior.Patrol:

                break;*/
                /*case EnemyBehavior.Attack:

                    break;*/
        }
    }

    IEnumerator Attack()
    {
        while (true)
        {
            _animator.SetTrigger("Attack2");
            yield return new WaitForSeconds(1f);
        }
        
    }




}
