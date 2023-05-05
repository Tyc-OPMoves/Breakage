using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimations : MonoBehaviour
{
    [SerializeField] Animator _Animator;
    [SerializeField] UnitBaseLogic _UnitBaseLogic;
    [SerializeField] NavMeshAI _NavMeshAI;
    [SerializeField]float _currentSpeed;

    AllyAI _currentAlly;
    
    int _randomAttack;
    private void Start()
    {
        _currentAlly = GetComponent<AllyAI>();
       
    }
    void Update()
    {
        ModifyLocomotionAnimation();
        
    }
    void ModifyLocomotionAnimation()
    {
        if (_NavMeshAI.GetAIDestination() != null && _NavMeshAI.GetAIMovementSpeed() != 0)
        {
            _currentSpeed = _NavMeshAI.GetAIMovementSpeed();
            _Animator.SetFloat("Locomotion", _currentSpeed);
        }
        else
        {
            _Animator.SetFloat("Locomotion", 0);
        }
    }

    public void RandomCombatAnim()
    {
        if(_currentAlly!= null)
        {
            _randomAttack = Random.Range(1, 4);
        }
        else
        {
            _randomAttack = Random.Range(1, 5);
        }
        
        _Animator.SetTrigger($"Attack{_randomAttack}");
        
    }
    public void AdjustAnimatorCombatSpeed(float unitAttackSpeed)
    {
        
        _Animator.speed = 3f/unitAttackSpeed;
    }
    public void AdjustAnimatorMovementSpeed(float unitMovementSpeed)
    {
        if(unitMovementSpeed <= 2)
        {
            _Animator.speed = unitMovementSpeed;
        }
        else
        {
            _Animator.speed = unitMovementSpeed / 1.5f;
        }
        
    }
    public float GetAnimatorSpeed()
    {
        return _Animator.speed;
    }
    public void Die()
    {
        _Animator.SetTrigger("Death");
    }
}
