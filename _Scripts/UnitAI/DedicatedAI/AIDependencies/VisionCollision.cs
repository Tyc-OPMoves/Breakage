using System.Collections.Generic;
using UnityEngine;

public class VisionCollision : MonoBehaviour
{
    [SerializeField] UnitBaseLogic _unitBaseLogic;
    [SerializeField] List<Collider> _targetUnits = new();

    bool _canRecheckTargetList;
    private void Update()
    {
        if(_canRecheckTargetList)
        {
            foreach(var unit in _targetUnits)
            {
                if(unit == null)
                {
                    _targetUnits.Remove(unit);
                    return;
                }
            }
            if (_targetUnits.Count == 0)
            {
                _canRecheckTargetList = false;
                _unitBaseLogic.ResumeBehavior();
            }
        }
    }
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

                if (_targetUnits.Count > 0)
                {
                    foreach (var unit in _targetUnits)
                    {
                        if (unit == other)
                        {
                            break;
                        }
                    }
                    _unitBaseLogic.OnTargetVision(other.transform);
                    _targetUnits.Add(other);
                }
                else
                {
                    _unitBaseLogic.OnTargetVision(other.transform);
                    _targetUnits.Add(other);
                    _canRecheckTargetList = true;
                }

            }
            else return;
        }
        else return;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Attack"))
        {

            if (!_unitBaseLogic.GetCombatStatus())
            {
                var currentAlly = GetComponentInParent<AllyAI>();
                var currentEnemy = GetComponentInParent<EnemyAI>();
                var targetAlly = other.GetComponentInParent<AllyAI>();
                var targetEnemy = other.GetComponentInParent<EnemyAI>();
                if ((currentAlly != null && targetEnemy != null) || (currentEnemy != null && targetAlly != null))
                {
                    _unitBaseLogic.OnTargetVision(other.transform);

                }
            }
            else return;

        }
        else return;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
            foreach (var unit in _targetUnits)
            {
                if (unit == other)
                {
                    _targetUnits.Remove(other);
                    Debug.Log($"|{unit.name}| was removed from the list of visible targets of |{this.name}|");
                    return;
                }
                else if (unit == null)
                {
                    _targetUnits.Remove(unit);
                    OnTriggerExit(other); 
                    return;

                }
            }
        }
    }
}
