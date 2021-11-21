using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameLib;

public class Equipment : MonoBehaviour
{
    public static Equipment instance = null; //Singleton variable

    public Dictionary<string,GameObject> equipment = new Dictionary<string, GameObject>(8);
    public List<GameObject> equipSlots = new List<GameObject>(8);
    //public GameObject defaultItem;
    void Awake()
    {
        //Singleton object init
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
            return;
        }
        //init equipment slots
        equipment.Add("helmet", null);
        equipment.Add("fullbody", null);
        equipment.Add("gloves", null);
        equipment.Add("boots", null);
        equipment.Add("shield", null);
        equipment.Add("weapon", null);
        equipment.Add("ring", null);
        equipment.Add("necklace", null);
    }

    public void WearItem(GameObject item,string type)
    {
        item.transform.parent = equipSlots[System.Array.IndexOf(Evaluator.typeStr, type)].transform;
        item.transform.localPosition = Vector3.zero;
        equipment[type] = item;
        string statsTrigger = item.GetComponent<Wearable>().ability.abilittyPattern.triggerCondition;
        
        if (statsTrigger == "AfterCritT" || statsTrigger == "AfterEvadeT")
        {
            BuffsManager.instance.StartTimedBuff(item.GetComponent<Wearable>());
        }
        if (statsTrigger == "AddPassiveModifier")
            Hero.instance.AddModifier(item.GetComponent<Wearable>().ability.abilittyPattern.triggerFormula);
        BuffsManager.instance.RecountBuffs();
    }
    public void UnwearItem(string type)
    {
        Inventory.instance.GiveItemToInventory(equipment[type].GetComponent<Wearable>());
        if (equipment[type].GetComponent<Wearable>().ability.abilittyPattern.triggerCondition == "AddPassiveModifier")
            Hero.instance.DeletePassiveModifier(equipment[type].GetComponent<Wearable>().ability.abilittyPattern.triggerFormula);
        Destroy(equipment[type].GetComponent<Wearable>().buffSlot);
        equipment[type] = null;
        
        BuffsManager.instance.RecountBuffs();
    }
}
