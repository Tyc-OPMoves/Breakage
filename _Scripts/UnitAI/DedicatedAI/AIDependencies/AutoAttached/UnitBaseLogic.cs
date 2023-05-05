using System.Collections;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(AIAnimations))]
[RequireComponent(typeof(NavMeshAI))]
public class UnitBaseLogic : MonoBehaviour
{
    [SerializeField] public ScriptableUnitBaseStats.Stats CurrentUnitStats;

    [SerializeField] HealthBar _HealthBar;
    

    NavMeshAI _navMeshAI;
    AIClass _AIClass;
    AIAnimations _AIAnimations;


    Coroutine _currentDamageRoutine;
    Collider _currentTarget;
    bool _isInCombat;
    public void SetStats(ScriptableUnitBaseStats.Stats stats) => CurrentUnitStats = stats;
    private void Start()
    {
        
        _navMeshAI = GetComponent<NavMeshAI>();
        _AIClass = GetComponent<AIClass>();
        _AIAnimations = GetComponent<AIAnimations>();

    }
    void Update()
    {
        LookAtTarget();
        UpdateMovementAnimation();
    }

    void LookAtTarget()
    {
        if (_currentTarget != null && CurrentUnitStats._CurrentHealth > 0)
        {
            transform.LookAt(_currentTarget.GetComponentInParent<Transform>());
        }
    }
    void UpdateMovementAnimation()
    {
        if (!GetCombatStatus())
        {
            _navMeshAI.SetAIMovementSpeed(CurrentUnitStats._MovementSpeed);
            _AIAnimations.AdjustAnimatorMovementSpeed(CurrentUnitStats._MovementSpeed);
        }
        else
        {
            _navMeshAI.SetAIMovementSpeed(0);
        }
    }

    public void OnAttackTarget(Collider unitTouched)
    {
        _AIClass.AIBehaviorUpdate(AIClass.AIBehavior.Attack);
        _currentTarget = unitTouched;

        if (_AIClass._canAttack == false)
        {
            _AIClass.ToggleChaceStatus(false);
            _AIClass.ToggleAttackStatus(true);
        }
        if (_AIClass._canAttack)
        {
            var unitBaseLogic = _currentTarget.GetComponentInParent<UnitBaseLogic>();
            if (!_isInCombat)
            {

                _isInCombat = true;
                _navMeshAI._AIAgent.isStopped = true;
                _currentDamageRoutine = StartCoroutine(DamageRoutine(
                    CurrentUnitStats._Damage,
                    CurrentUnitStats._AttackSpeed,
                    CurrentUnitStats._Armour,
                    unitBaseLogic));
            }
            else return;
        }
    }

    public void OnTargetVision(Transform targetSeen)
    {
        //Debug.Log($"Unit |{transform.name}| sees unit |{unitSeen.name}|");
        if (_AIClass.GetCurrentBehavior() == AIClass.AIBehavior.Idle) return;
        if (_AIClass._canChase == false)
        {
            _AIClass.ToggleAttackStatus(false);
            _AIClass.ToggleChaceStatus(true);

            //Debug.Log($"OnTargetVision changed Chanse status.");
        }
        _navMeshAI.ChangeAITarget(targetSeen.transform);
        //Debug.Log($"Unit |{this.name}| is chasing |{targetSeen.name}| at pos|{targetSeen.position}|.");
    }
    public bool GetCombatStatus()
    {
        return _isInCombat;
    }
    void CheckCombatStats(int damageInRoutine, float atkSpeedInRoutine, int armourInRoutine, UnitBaseLogic unitToAttack)
    {
        if (CurrentUnitStats._Damage != damageInRoutine ||
            CurrentUnitStats._AttackSpeed != atkSpeedInRoutine ||
            CurrentUnitStats._Armour != armourInRoutine)
        {
            StopCoroutine(_currentDamageRoutine);
            _currentDamageRoutine = StartCoroutine(DamageRoutine(
                CurrentUnitStats._Damage, 
                CurrentUnitStats._AttackSpeed, 
                CurrentUnitStats._Armour, 
                unitToAttack));
            Debug.Log("DamageRoutine was modified.");
        }
        else return;
    }
    IEnumerator DamageRoutine(int damageToTake, float atkSpeed, int armour, UnitBaseLogic unitBaseLogic)
    {
        _AIAnimations.AdjustAnimatorCombatSpeed(atkSpeed);

        while (true)
        {
            
            if (unitBaseLogic.CurrentUnitStats._CurrentHealth - damageToTake <= 0)
            {
                unitBaseLogic.TakeDamage(damageToTake);
                _AIAnimations.RandomCombatAnim();
                DisengageAttack();
            }
            else
            {
                CheckCombatStats(damageToTake, atkSpeed, armour, unitBaseLogic);
                _AIAnimations.RandomCombatAnim();
                unitBaseLogic.TakeDamage(damageToTake);
            }
            yield return new WaitForSeconds(atkSpeed);
        }
    }
    void TakeDamage(int damageToTake)
    {
        if (CurrentUnitStats._CurrentHealth > 0)
        {
            if (CurrentUnitStats._CurrentHealth - damageToTake <= 0)
            { 

                CurrentUnitStats._CurrentHealth -= damageToTake - (damageToTake*CurrentUnitStats._Armour)/100;

                _HealthBar.UpdateHealthbar(CurrentUnitStats._MaxHealth, CurrentUnitStats._CurrentHealth);
                Debug.Log($"Unit |{this.name}| took |{damageToTake}| damage, " +
                    $"blocked |{(damageToTake * CurrentUnitStats._Armour) / 100}|. " +
                    $"Total Damage |{damageToTake - (damageToTake * CurrentUnitStats._Armour) / 100}|");
                Debug.Log($"Unit |{this.name}| has |{CurrentUnitStats._CurrentHealth}| health remaining.");
                Die();
            }
            else
            {
                var temporaryArmour = CurrentUnitStats._Armour;
                if (temporaryArmour > 80)
                {
                    temporaryArmour = 80;
                }
                var temporaryDamage = damageToTake - damageToTake * temporaryArmour / 100;
                CurrentUnitStats._CurrentHealth -= temporaryDamage;

                _HealthBar.UpdateHealthbar(CurrentUnitStats._MaxHealth, CurrentUnitStats._CurrentHealth);
                Debug.Log($"Unit |{this.name}| took |{damageToTake}| damage, " +
                    $"blocked |{damageToTake * temporaryArmour / 100}|. " +
                    $"Total Damage |{temporaryDamage}|");
                Debug.Log($"Unit |{this.name}| has |{CurrentUnitStats._CurrentHealth}| health remaining.");
            }
        }
    }

    public void DisengageAttack()
    {
        StopCoroutine(_currentDamageRoutine);
        _AIAnimations.AdjustAnimatorCombatSpeed(1);
        _isInCombat = false;
        //_AIClass.ToggleAttackStatus(false);
        _currentTarget = null;
        _navMeshAI._AIAgent.isStopped = false;

    }
    public void ResumeBehavior()
    {
        if (TryGetComponent<EnemyAI>(out EnemyAI enemyAI))
        {
            enemyAI.ToggleAttackStatus(false);
            enemyAI.AIBehaviorUpdate(AIClass.AIBehavior.Chase);
        }
        else if (TryGetComponent<AllyAI>(out AllyAI allyAI))
        {
            allyAI.ToggleAttackStatus(false);
            allyAI.ToggleChaceStatus(false);
            allyAI.AIBehaviorUpdate(AIClass.AIBehavior.Patrol);
        }
    }
    public void Die()
    {

        if (CurrentUnitStats._CurrentHealth <= 0)
        {
            CurrentUnitStats._CurrentHealth = 0;

            StopAllCoroutines();
            
            _AIAnimations.AdjustAnimatorMovementSpeed(1);

            _navMeshAI.ChangeAITarget(transform);
            _navMeshAI._AIAgent.isStopped = true;
            _navMeshAI._AIAgent.ActivateCurrentOffMeshLink(false);
            _navMeshAI._AIAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;

            _HealthBar.DisableHealthBar();
            Destroy(GetComponent<Rigidbody>());
            Destroy(transform.GetChild(0).gameObject);
            Destroy(transform.GetChild(1).gameObject);
            Debug.Log($"Unit |{this.name}| dieded.");
            UnitManager.Instance.UpdateEnemiesCounter();
            Destroy(this.gameObject, 4);
            _AIAnimations.Die();
        }
    }



}
