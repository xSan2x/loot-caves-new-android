using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLib;
using UnityEngine.UI;

[System.Serializable]
public class SaveData
{
    public int[] saveTime;
    public int caveLvl;
    public int heroLvl;
    public int difficulty;
    public int inventorySize;
    public int exp;
    public int gold;

    //slots = 0 head; 1 right arm; 2 chest; 3 left arm; 4 hands; 5 legs; 6 ring; 7 necklace
    public bool[] isSlotNotEmpty = new bool[8];
    public string[] slotItemType = new string[8];
    public Evaluator.Rarity[] slotItemRarity = new Evaluator.Rarity[8];
    public int[] slotItemLvl = new int[8];
    public int[] slotItemEnchant = new int[8];
    public int[] slotItemIndex = new int[8];
    public string[] slotItem0Attr = new string[8];
    public string[] slotItem1Attr = new string[8];
    public string[] slotItem2Attr = new string[8];
    public float[] slotItem0Amount = new float[8];
    public float[] slotItem1Amount = new float[8];
    public float[] slotItem2Amount = new float[8];

    //inventory
    public string[] invItemType;
    public Evaluator.Rarity[] invItemRarity;
    public string[] invItem0Attr;
    public string[] invItem1Attr;
    public string[] invItem2Attr;
    public float[] invItem0Amount;
    public float[] invItem1Amount;
    public float[] invItem2Amount;
    public int[] invItemLvl;
    public int[] invItemEnchant;
    public int[] invItemIndex;

    public SaveData()
    {
        //save saveDate! 0-year;1-month;2-day;3-hour;4-minutes;5-seconds;
        DateTime date = DateTime.Now;
        saveTime = new int[6];
        saveTime[0] = date.Year;
        saveTime[1] = date.Month;
        saveTime[2] = date.Day;
        saveTime[3] = date.Hour;
        saveTime[4] = date.Minute;
        saveTime[5] = date.Second;

        Hero hero = Hero.instance;
        Equipment equipment = Equipment.instance;
        Inventory inventory = Inventory.instance;
        LevelManager levelManage = LevelManager.instance;
        GameSettings gameSettings = GameSettings.instance;

        //saving MAIN params
        caveLvl = levelManage.currLvl;
        heroLvl = hero.level;
        difficulty = gameSettings.gameDifficulty;
        inventorySize = inventory.inventorySize;
        exp = hero.exp;
        gold = hero.gold;

        //saving EQUIPMENT
        int iterator = 0;
        foreach(KeyValuePair<string, GameObject> equip in equipment.equipment)
        {
            if(equip.Value.GetComponent<Wearable>().type != "default")
            {
                isSlotNotEmpty[iterator] = true;
                slotItemType[iterator] = equip.Value.GetComponent<Wearable>().type;
                slotItemRarity[iterator] = equip.Value.GetComponent<Wearable>().rarity;
                slotItemLvl[iterator] = equip.Value.GetComponent<Wearable>().level;
                slotItemEnchant[iterator] = equip.Value.GetComponent<Wearable>().enchant;
                slotItemIndex[iterator] = equip.Value.GetComponent<Wearable>().imgIndex;
                slotItem0Attr[iterator] = equip.Value.GetComponent<Wearable>().zeroAttr;
                slotItem1Attr[iterator] = equip.Value.GetComponent<Wearable>().firstAttr;
                slotItem2Attr[iterator] = equip.Value.GetComponent<Wearable>().secondAttr;
                slotItem0Amount[iterator] = equip.Value.GetComponent<Wearable>().zeroAmount;
                slotItem1Amount[iterator] = equip.Value.GetComponent<Wearable>().firstAmount;
                slotItem2Amount[iterator] = equip.Value.GetComponent<Wearable>().secondAmount;
            } else
            {
                isSlotNotEmpty[iterator] = false;
            }
            iterator++;
        }


        invItemType = new string[inventory.inventory.Count];
        invItemRarity = new Evaluator.Rarity[inventory.inventory.Count];
        invItemLvl = new int[inventory.inventory.Count];
        invItemEnchant = new int[inventory.inventory.Count];
        invItemIndex = new int[inventory.inventory.Count];
        invItem0Attr = new string[inventory.inventory.Count];
        invItem1Attr = new string[inventory.inventory.Count];
        invItem2Attr = new string[inventory.inventory.Count];
        invItem0Amount = new float[inventory.inventory.Count];
        invItem1Amount = new float[inventory.inventory.Count];
        invItem2Amount = new float[inventory.inventory.Count];
        for (var i = 0; i < inventory.inventory.Count; i++)
        {
            if (inventory.inventory[i] != null)
            {
                if(inventory.inventory[i].GetComponent<Item>() is Wearable)
                {
                    invItemType[i] = inventory.inventory[i].GetComponent<Wearable>().type;
                    invItemRarity[i] = inventory.inventory[i].GetComponent<Wearable>().rarity;
                    invItemLvl[i] = inventory.inventory[i].GetComponent<Wearable>().level;
                    invItemEnchant[i] = inventory.inventory[i].GetComponent<Wearable>().enchant;
                    invItemIndex[i] = inventory.inventory[i].GetComponent<Wearable>().imgIndex;
                    invItem0Attr[i] = inventory.inventory[i].GetComponent<Wearable>().zeroAttr;
                    invItem1Attr[i] = inventory.inventory[i].GetComponent<Wearable>().firstAttr;
                    invItem2Attr[i] = inventory.inventory[i].GetComponent<Wearable>().secondAttr;
                    invItem0Amount[i] = inventory.inventory[i].GetComponent<Wearable>().zeroAmount;
                    invItem1Amount[i] = inventory.inventory[i].GetComponent<Wearable>().firstAmount;
                    invItem2Amount[i] = inventory.inventory[i].GetComponent<Wearable>().secondAmount;
                } else
                {
                    invItemType[i] = inventory.inventory[i].GetComponent<Item>().type;
                    invItemLvl[i] = inventory.inventory[i].GetComponent<Item>().level;
                    invItemIndex[i] = inventory.inventory[i].GetComponent<Item>().imgIndex;
                }
                
            }
        }
    }
}
