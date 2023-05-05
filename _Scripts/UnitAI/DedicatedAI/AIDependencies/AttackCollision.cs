using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{

    [SerializeField] UnitBaseLogic _UnitBaseLogic;
    [SerializeField] List<Collider> _attackableUnits = new();
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
            var currentAlly = GetComponentInParent<AllyAI>();
            var currentEnemy = GetComponentInParent<EnemyAI>();
            var targetAlly = other.GetComponentInParent<AllyAI>();
            var targetEnemy = other.GetComponentInParent<EnemyAI>();

            if ((currentAlly != null && targetEnemy != null) || (currentEnemy != null && targetAlly != null))
            {
                if (!_UnitBaseLogic.GetCombatStatus())
                {
                    _UnitBaseLogic.OnAttackTarget(other);
                }
                _attackableUnits.Add(other);
            }
            else return;   

            Debug.Log($"|{this.name}| will attack |{other.name}|.");
        }
        else return;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Attack") && !_UnitBaseLogic.GetCombatStatus())
        {
            var currentAlly = GetComponentInParent<AllyAI>();
            var currentEnemy = GetComponentInParent<EnemyAI>();
            var targetAlly = other.GetComponentInParent<AllyAI>();
            var targetEnemy = other.GetComponentInParent<EnemyAI>();

            if ((currentAlly != null && targetEnemy != null) || (currentEnemy != null && targetAlly != null))
            {
                _UnitBaseLogic.OnAttackTarget(other);
            }
            else return;

            Debug.Log($"|{this.name}| will attack |{other.name}|.");
        }
        else return;
    }
}
