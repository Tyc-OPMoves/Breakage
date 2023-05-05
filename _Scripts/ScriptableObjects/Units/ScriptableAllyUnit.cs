using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ScriptableAllyUnit")]
public class ScriptableAllyUnit : ScriptableUnitBaseStats
{
    public Type AllyType;
    public Image _shopImage;
    public Image _handImage;
    [Serializable]
    public enum Type
    {
        Goblin,
        Fishman,
        Anubis,
        Tower
    }

}
