using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameLib;
using System;

public class Wearable : Item
{
    public int enchant;
    //TYPE = 0 - helmet; 1 - fullarmor; 2 - gloves; 3 - boots; 4 - shield; 5 - weapon; 6 - ring; 7 - necklace
    public Evaluator.Rarity rarity; //0 - common; 1 - uncommon; 2 - rare; 3 - epic; 4 - legendary;
    public string zeroAttr; // 0-STR,1-AGI,2-INT,3-TRK,4-END,5-ALL,6-ATK,7-SPD,8-CRIT,9-EVD,10-MDMG,11-HP,12-REG
    public string firstAttr;
    public string secondAttr;
    public float zeroAmount;
    public float firstAmount;
    public float secondAmount;

    public void Start()
    {
        gs = GameSettings.instance;
    }

    public void InitWearable(int lvl)
    {
        enchant = 0;
        level = lvl;
        rarity = Evaluator.RandRarity(0.0667f);
        type = Evaluator.typeStr[UnityEngine.Random.Range(0,8)];
        imgIndex = UnityEngine.Random.Range(1, Evaluator.maxWearableIndex[Array.IndexOf(Evaluator.typeStr,type)]);
        if (rarity > 0)
        {
            int firstParam = UnityEngine.Random.Range(0, 12);
            int secondParam = UnityEngine.Random.Range(0, 12);
            switch (firstParam)
            {
                case 0:
                    firstAttr = "STR";
                    firstAmount = (float)Mathf.Round(5 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
                case 1:
                    firstAttr = "AGI";
                    firstAmount = (float)Mathf.Round(5 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
                case 2:
                    firstAttr = "INT";
                    firstAmount = (float)Mathf.Round(5 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
                case 3:
                    firstAttr = "TRK";
                    firstAmount = (float)Mathf.Round(5 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
                case 4:
                    firstAttr = "END";
                    firstAmount = (float)Mathf.Round(5 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
                case 5:
                    firstAttr = "ALL";
                    firstAmount = (float)Mathf.Round(1 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
                case 6:
                    firstAttr = "ATK";
                    firstAmount = (float)Mathf.Round(10 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
                case 7:
                    firstAttr = "SPD";
                    firstAmount = (float)Mathf.Round(100 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
                case 8:
                    firstAttr = "CRIT";
                    firstAmount = (float)Mathf.Round(100 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
                case 9:
                    firstAttr = "EVD";
                    firstAmount = (float)Mathf.Round(100 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
                case 10:
                    firstAttr = "MDMG";
                    firstAmount = (float)Mathf.Round(50 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
                case 11:
                    firstAttr = "HP";
                    firstAmount = (float)Mathf.Round(100 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
                case 12:
                    firstAttr = "REG";
                    firstAmount = (float)Mathf.Round(2.5f * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
            }
            switch (secondParam)
            {
                case 0:
                    secondAttr = "STR";
                    secondAmount = (float)Mathf.Round(5 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
                case 1:
                    secondAttr = "AGI";
                    secondAmount = (float)Mathf.Round(5 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
                case 2:
                    secondAttr = "INT";
                    secondAmount = (float)Mathf.Round(5 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
                case 3:
                    secondAttr = "TRK";
                    secondAmount = (float)Mathf.Round(5 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
                case 4:
                    secondAttr = "END";
                    secondAmount = (float)Mathf.Round(5 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
                case 5:
                    secondAttr = "ALL";
                    secondAmount = (float)Mathf.Round(1 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
                case 6:
                    secondAttr = "ATK";
                    secondAmount = (float)Mathf.Round(10 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
                case 7:
                    secondAttr = "SPD";
                    secondAmount = (float)Mathf.Round(100 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
                case 8:
                    secondAttr = "CRIT";
                    secondAmount = (float)Mathf.Round(100 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
                case 9:
                    secondAttr = "EVD";
                    secondAmount = (float)Mathf.Round(100 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
                case 10:
                    secondAttr = "MDMG";
                    secondAmount = (float)Mathf.Round(50 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
                case 11:
                    secondAttr = "HP";
                    secondAmount = (float)Mathf.Round(100 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
                case 12:
                    secondAttr = "REG";
                    secondAmount = (float)Mathf.Round(2.5f * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                    break;
            }
        }
        switch (type)
        {
            case "helmet": 
                zeroAttr = "HP";
                zeroAmount = (float)Mathf.Round(40 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                break;
            case "fullbody":
                zeroAttr = "HP";
                zeroAmount = (float)Mathf.Round(80 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                break;
            case "gloves":
                zeroAttr = "HP";
                zeroAmount = (float)Mathf.Round(40 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                break;
            case "boots":
                zeroAttr = "HP";
                zeroAmount = (float)Mathf.Round(40 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                break;
            case "shield":
                zeroAttr = "HP";
                zeroAmount = (float)Mathf.Round(100 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                break;
            case "weapon":
                zeroAttr = "ATK";
                zeroAmount = (float)Mathf.Round(10 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                break;
            case "ring":
                int attr = UnityEngine.Random.Range(0, 4);
                switch (attr)
                {
                    case 0:
                        zeroAttr = "STR";
                        break;
                    case 1:
                        zeroAttr = "AGI";
                        break;
                    case 2:
                        zeroAttr = "INT";
                        break;
                    case 3:
                        zeroAttr = "TRK";
                        break;
                    case 4:
                        zeroAttr = "END";
                        break;
                }
                zeroAmount = (float)Mathf.Round(5 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                break;
            case "necklace":
                int attr2 = UnityEngine.Random.Range(0, 4);
                switch (attr2)
                {
                    case 0:
                        zeroAttr = "STR";
                        break;
                    case 1:
                        zeroAttr = "AGI";
                        break;
                    case 2:
                        zeroAttr = "INT";
                        break;
                    case 3:
                        zeroAttr = "TRK";
                        break;
                    case 4:
                        zeroAttr = "END";
                        break;
                }
                zeroAmount = (float)Mathf.Round(5 * Mathf.Pow(1.1f, level - 1) * Mathf.Pow(1.2f, (int)rarity));
                break;
        }
        name = "";
        if (enchant > 0)
        {
            name += "+" + enchant + " ";
        }
        string bigType = char.ToUpper(type[0]) + type.Substring(1);
        name += bigType;
        if (rarity > 0)
        {
            name += " of";
        }
        if (rarity > 0)
            if (firstAttr != secondAttr)
            {
                switch (firstAttr)
                {
                    case "STR":
                        name += " Strength";
                        break;
                    case "AGI":
                        name += " Agility";
                        break;
                    case "INT":
                        name += " Intelligence";
                        break;
                    case "TRK":
                        name += " Trickery";
                        break;
                    case "END":
                        name += " Endurance";
                        break;
                    case "ALL":
                        name += " All stats";
                        break;
                    case "ATK":
                        name += " Attack";
                        break;
                    case "SPD":
                        name += " Speed Rate";
                        break;
                    case "CRIT":
                        name += " Crit.Rate";
                        break;
                    case "EVD":
                        name += " Evasion Rate";
                        break;
                    case "MDMG":
                        name += " Magic damage";
                        break;
                    case "HP":
                        name += " Health";
                        break;
                    case "REG":
                        name += " Regeneration";
                        break;
                    default:
                        name += " Something!)";
                        break;
                }
                switch (secondAttr)
                {
                    case "STR":
                        name += " and Strength";
                        break;
                    case "AGI":
                        name += " and Agility";
                        break;
                    case "INT":
                        name += " and Intelligence";
                        break;
                    case "TRK":
                        name += " and Trickery";
                        break;
                    case "END":
                        name += " and Endurance";
                        break;
                    case "ALL":
                        name += " and All stats";
                        break;
                    case "ATK":
                        name += " and Attack";
                        break;
                    case "SPD":
                        name += " and Speed Rate";
                        break;
                    case "CRIT":
                        name += " and Crit.Rate";
                        break;
                    case "EVD":
                        name += " and Evasion Rate";
                        break;
                    case "MDMG":
                        name += " and Magic damage";
                        break;
                    case "HP":
                        name += " and Health";
                        break;
                    case "REG":
                        name += " and Regeneration";
                        break;
                    default:
                        name += " and Something!)";
                        break;
                }
            }
            else
            {
                switch (firstAttr)
                {
                    case "STR":
                        name += " Strength";
                        break;
                    case "AGI":
                        name += " Agility";
                        break;
                    case "INT":
                        name += " Intelligence";
                        break;
                    case "TRK":
                        name += " Trickery";
                        break;
                    case "END":
                        name += " Endurance";
                        break;
                    case "ALL":
                        name += " All stats";
                        break;
                    case "ATK":
                        name += " Attack";
                        break;
                    case "SPD":
                        name += " Speed Rate";
                        break;
                    case "CRIT":
                        name += " Crit.Rate";
                        break;
                    case "EVD":
                        name += " Evasion Rate";
                        break;
                    case "MDMG":
                        name += " Magic damage";
                        break;
                    case "HP":
                        name += " Health";
                        break;
                    case "REG":
                        name += " Regeneration";
                        break;
                    default:
                        name += " Something!)";
                        break;
                }
            }
        //ABILITY
        //----------------
        if (rarity > 0)
        {
            string assetName = "";
            //Names of files: "rarity"-"string first attr"-"string second attr". Example: "uncommon-STR-AGI"
            assetName = Evaluator.rarityStr[(int)rarity]+ "-" + firstAttr + "-" + secondAttr;
            assetName = "uncommon-ATK-REG";
           
            

            if(AbilityCollector.instance.abilityPatterns.Find(x => x.name == assetName) == null)
                assetName = Evaluator.rarityStr[(int)rarity] + "-" + secondAttr + "-" + firstAttr;
            AbilityPattern abilityPattern = AbilityCollector.instance.abilityPatterns.Find(x => x.name == assetName);
            ability = gameObject.AddComponent<Ability>();
            ability.InitAbility(abilityPattern);
        }

        transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("bgitems/" + Evaluator.rarityStr[(int)rarity]);
        transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(64, 64);
        transform.GetChild(1).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(64, 64);
        transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("items/" + type + "/" + type + " (" + imgIndex + ")");
    }

    public string GetSecondaryStats()
    {
        string finalString = "";
        if (firstAttr != secondAttr)
        {
            finalString = Evaluator.longAttributes[System.Array.IndexOf(Evaluator.shortAttributes, firstAttr)] + " : " + Evaluator.CheckFloat(firstAmount)+" | "+ Evaluator.longAttributes[System.Array.IndexOf(Evaluator.shortAttributes, secondAttr)] + " : " + Evaluator.CheckFloat(secondAmount);
        } else
        {
            finalString = Evaluator.longAttributes[System.Array.IndexOf(Evaluator.shortAttributes, firstAttr)] + " : " + Evaluator.CheckFloat(firstAmount * 2);
        }
        return finalString;
    }

    public override void UseItem()
    {
        if (transform.parent.name.Contains("Slot"))
        {
            Inventory.instance.TakeAwayItemFromInventory(this);
            if (Equipment.instance.equipment[type]!=null)
            {
                Equipment.instance.UnwearItem(type);
            }
            Equipment.instance.WearItem(gameObject, type);
        } else
        {
            Equipment.instance.UnwearItem(type);
        }
    }
}
