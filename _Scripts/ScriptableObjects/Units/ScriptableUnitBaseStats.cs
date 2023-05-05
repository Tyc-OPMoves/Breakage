using System;
using UnityEngine;


public abstract class ScriptableUnitBaseStats : ScriptableObject
{
    [SerializeField] Stats _baseStats;
    public Stats BaseStats => _baseStats;

    
    public UnitBaseLogic _unitPrefab;
    

    [Serializable]
    public struct Stats
    {
        public int _MaxHealth;
        public int _CurrentHealth;
        public int _Armour;
        public int _Damage;
        public float _AttackSpeed;
        public float _MovementSpeed;
        public int _Price;
    }

}


