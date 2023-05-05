using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
class SlotClass
{
    public Image _creatureImage;
    public ScriptableAllyUnit _unitData;
}
public class GUIManager : StaticInstance<GUIManager> 
{
    [SerializeField] Image _shop;
    [SerializeField] Image _bottomBar;
  
    [SerializeField] List<SlotClass> _shopCreatureInSlot;
    [SerializeField] List<SlotClass> _handCreatureInSlot;
    [SerializeField] List<ScriptableAllyUnit> _creaturesToBuy;

    void Start()
    {
        Reroll();
        ShopInit();
    }

    public void ToggleShop()
    {
        if(_shop.gameObject.activeSelf)
        {
            _shop.gameObject.SetActive(false);
        }
        else
        {
            _shop.gameObject.SetActive(true);
        }
        
    }
    public void ToggleBottomBar()
    {
        if (_bottomBar.gameObject.activeSelf)
        {
            _bottomBar.gameObject.SetActive(false);
        }
        else
        {
            _bottomBar.gameObject.SetActive(true);
        }
    }
    public void Reroll()
    {
        foreach (var slot in _shopCreatureInSlot)
        {
            int random = UnityEngine.Random.Range(0, _creaturesToBuy.Count);
            slot._creatureImage.sprite = _creaturesToBuy[random]._shopImage.sprite;
            slot._unitData = _creaturesToBuy[random];
            
        }
    }
    public void BuyUnitInShop(int slotNumber)
    {
        int slotCounter = 0;
        foreach(var slot in _handCreatureInSlot)
        {

            if(slot._unitData == null)
            {
                slot._unitData = _shopCreatureInSlot[slotNumber]._unitData;
                slot._creatureImage.sprite = _shopCreatureInSlot[slotNumber]._creatureImage.sprite;
                Debug.Log(slot._unitData.name);
                break;
            }
            else
            {
                if(slotCounter == 9)
                {
                    Debug.Log("I can't take it anymore senpai...");
                    slotCounter = 0;
                    break;
                }
            }
            slotCounter++;
        }
    }
    public void DragUnit(int slot)
    {
        //_handCreatureInSlot[slot]._unitData
    }
    void ShopInit()
    {
        foreach (var slot in _shopCreatureInSlot)
        {
            slot._creatureImage.gameObject.SetActive(true);
        }
        _shop.gameObject.SetActive(false);
    }

    
}
