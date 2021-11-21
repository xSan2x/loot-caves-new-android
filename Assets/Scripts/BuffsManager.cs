using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameLib;

public class BuffsManager : MonoBehaviour
{
    public static BuffsManager instance = null; //Singleton object variable
    Dictionary<string, GameObject> equipment; //Singleton object variable
    public GameObject buffPrefab;
    public Transform buffSlots;
    public Canvas mainCanvas;
    public GameObject buffHint;
    public GameObject currentBuffHint;
    Hero hero;
    //Attributtes from buffs
    public int bSTR = 0;
    public int bAGI = 0;
    public int bINT = 0;
    public int bTRK = 0;
    public int bEND = 0;
    public int bATK = 0;
    public int bATKMult = 0;
    public int bHP = 0;
    public int BspeedRate = 0;
    public int BspeedRateMult = 0;
    public int BaccRate = 0;
    public int BextraMDMG = 0;
    public int BmultMDMG = 0;
    public int BcooldownReduce = 0;
    public int BcritRate = 0;
    public int BcritRateMult = 0;
    public int BcritMultiplier = 0;
    public int BcritChance = 0;
    public int BcritMageMultiplier = 0;
    public int BevasionRate = 0;
    public int BevasionRateMult = 0;
    public int BevasionChance = 0;
    public float Bregeneration = 0;
    public float BregenerationMult = 0;

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
        }
    }
    void ZeroingBuffs()
    {
        bSTR = 0;
        bAGI = 0;
        bINT = 0;
        bTRK = 0;
        bEND = 0;
        bATK = 0;
        bATKMult = 0;
        bHP = 0;
        BspeedRate = 0;
        BspeedRateMult = 0;
        BaccRate = 0;
        BextraMDMG = 0;
        BmultMDMG = 0;
        BcooldownReduce = 0;
        BcritRate = 0;
        BcritRateMult = 0;
        BcritMultiplier = 0;
        BcritChance = 0;
        BcritMageMultiplier = 0;
        BevasionRate = 0;
        BevasionRateMult = 0;
        BevasionChance = 0;
        Bregeneration = 0;
    }
    void DrawBuffsIcons()
    {
        int count = 0;
        foreach (KeyValuePair<string, GameObject> item in equipment)
        {
            if (item.Value != null && item.Value.GetComponent<Wearable>().rarity != 0)
            {
                Wearable stats = item.Value.GetComponent<Wearable>();
                if (stats.buffSlot == null)
                {
                    GameObject newBuff = Instantiate(buffPrefab);
                    newBuff.GetComponent<RectTransform>().anchorMin = new Vector2((0.1f * count), 0);
                    newBuff.GetComponent<RectTransform>().anchorMax = new Vector2((0.1f * (count + 1)), 1);

                    newBuff.GetComponent<BuffClick>().item = stats;
                    newBuff.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("bgitems/" + Evaluator.rarityStr[(int)stats.rarity]);
                    newBuff.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("items/" + stats.type + "/" + stats.type + " (" + stats.imgIndex + ")");
                    count++;
                    newBuff.transform.SetParent(buffSlots, false);
                    stats.buffSlot = newBuff;
                }
                if (stats.ability.stacks > 0)
                {
                    stats.buffSlot.transform.GetChild(0).transform.GetChild(2).GetComponent<Text>().text = stats.ability.stacks.ToString();
                }
            }
        }
        if (currentBuffHint)
            currentBuffHint.GetComponent<BuffHint>().Redraw();
        foreach (Transform child in buffSlots)
        {
            child.GetComponent<RectTransform>().anchorMin = new Vector2((0.1f * count), 0);
            child.GetComponent<RectTransform>().anchorMax = new Vector2((0.1f * (count + 1)), 1);
            count++;
        }
    }
    private void Start()
    {
        equipment = Equipment.instance.equipment;
        //subscribe to all events
        hero = Hero.instance;
        hero.OnKillMob += AfterKillMob;
        hero.OnEvade += AfterEvade;
        hero.OnSuccesfullAttack += AfterSuccesfullAttack;
        hero.OnGetDamage += AfterGetDamage;
        hero.OnRegen += AfterRegen;
        hero.OnDeath += AfterDeath;
        hero.OnCriticalAttack += AfterCriticalAttack;
        hero.OnMissedAttack += AfterMissedAttack;
        LevelManager.instance.OnLevelFinished += AfterLevelFinished;
        LevelManager.instance.OnLevelStarted += AfterLevelStarted;
    }
    public void RecountBuffs()
    {
        ZeroingBuffs();
        foreach (KeyValuePair<string, GameObject> item in equipment)
        {
            if (item.Value != null)
            {
                Wearable stats = item.Value.GetComponent<Wearable>();
                if (stats.rarity > 0)
                {
                    for (int j = 0; j < stats.ability.attributesCount.Count; j++)
                    {
                        switch (stats.ability.abilittyPattern.attributes[j])
                        {
                            case "STR":
                                bSTR += (int)Mathf.Round(stats.ability.attributesCount[j]);
                                break;
                            case "AGI":
                                bAGI += (int)Mathf.Round(stats.ability.attributesCount[j]);
                                break;
                            case "INT":
                                bINT += (int)Mathf.Round(stats.ability.attributesCount[j]);
                                break;
                            case "TRK":
                                bTRK += (int)Mathf.Round(stats.ability.attributesCount[j]);
                                break;
                            case "END":
                                bEND += (int)Mathf.Round(stats.ability.attributesCount[j]);
                                break;
                            case "ALL":
                                bSTR += (int)Mathf.Round(stats.ability.attributesCount[j]);
                                bAGI += (int)Mathf.Round(stats.ability.attributesCount[j]);
                                bINT += (int)Mathf.Round(stats.ability.attributesCount[j]);
                                bTRK += (int)Mathf.Round(stats.ability.attributesCount[j]);
                                bEND += (int)Mathf.Round(stats.ability.attributesCount[j]);
                                break;
                            case "HP":
                                bHP += (int)Mathf.Round(stats.ability.attributesCount[j]);
                                break;
                            case "ATK":
                                bATK += (int)Mathf.Round(stats.ability.attributesCount[j]);
                                break;
                            case "ATKMult":
                                bATKMult += (int)Mathf.Round(stats.ability.attributesCount[j]);
                                break;
                            case "SPD":
                                BspeedRate += (int)Mathf.Round(stats.ability.attributesCount[j]);
                                break;
                            case "SPDMult":
                                BspeedRateMult += (int)Mathf.Round(stats.ability.attributesCount[j]);
                                break;
                            case "CRIT":
                                BcritRate += (int)Mathf.Round(stats.ability.attributesCount[j]);
                                break;
                            case "CRITMult":
                                BcritRate += (int)Mathf.Round(stats.ability.attributesCount[j]);
                                break;
                            case "EVD":
                                BevasionRate += (int)Mathf.Round(stats.ability.attributesCount[j]);
                                break;
                            case "EVDMult":
                                BevasionRateMult += (int)Mathf.Round(stats.ability.attributesCount[j]);
                                break;
                            case "MDMGMult":
                                BmultMDMG += (int)Mathf.Round(stats.ability.attributesCount[j]);
                                break;
                            case "REG":
                                Bregeneration += stats.ability.attributesCount[j];
                                break;
                            case "REGMult":
                                BregenerationMult += stats.ability.attributesCount[j];
                                break;
                            case "CRITM":
                                BcritMultiplier += (int)Mathf.Round(stats.ability.attributesCount[j]);
                                break;
                            case "ChanceCRIT":
                                BcritChance += (int)Mathf.Round(stats.ability.attributesCount[j]);
                                break;
                            case "ChanceEVD":
                                BevasionChance += (int)Mathf.Round(stats.ability.attributesCount[j]);
                                break;
                            case "MCRIT":
                                BcritMageMultiplier += (int)Mathf.Round(stats.ability.attributesCount[j]);
                                break;
                            case "CDR":
                                BcooldownReduce += (int)Mathf.Round(stats.ability.attributesCount[j]);
                                break;
                            default:
                                Debug.LogError("Wrong param in BuffsManager for Recounting Buffs");
                                break;
                        }
                    }
                }
                switch (stats.zeroAttr)
                {
                    case "STR":
                        bSTR += (int)Mathf.Round(stats.zeroAmount);
                        break;
                    case "AGI":
                        bAGI += (int)Mathf.Round(stats.zeroAmount);
                        break;
                    case "INT":
                        bINT += (int)Mathf.Round(stats.zeroAmount);
                        break;
                    case "TRK":
                        bTRK += (int)Mathf.Round(stats.zeroAmount);
                        break;
                    case "END":
                        bEND += (int)Mathf.Round(stats.zeroAmount);
                        break;
                    case "HP":
                        bHP += (int)Mathf.Round(stats.zeroAmount);
                        break;
                    case "ATK":
                        bATK += (int)Mathf.Round(stats.zeroAmount);
                        break;
                    default:
                        Debug.LogError("Wrong param in BuffsManager for Recounting Buffs");
                        break;
                }
                switch (stats.firstAttr)
                {
                    case "STR":
                        bSTR += (int)Mathf.Round(stats.firstAmount);
                        break;
                    case "AGI":
                        bAGI += (int)Mathf.Round(stats.firstAmount);
                        break;
                    case "INT":
                        bINT += (int)Mathf.Round(stats.firstAmount);
                        break;
                    case "TRK":
                        bTRK += (int)Mathf.Round(stats.firstAmount);
                        break;
                    case "END":
                        bEND += (int)Mathf.Round(stats.firstAmount);
                        break;
                    case "ALL":
                        bSTR += (int)Mathf.Round(stats.firstAmount);
                        bAGI += (int)Mathf.Round(stats.firstAmount);
                        bINT += (int)Mathf.Round(stats.firstAmount);
                        bTRK += (int)Mathf.Round(stats.firstAmount);
                        bEND += (int)Mathf.Round(stats.firstAmount);
                        break;
                    case "ATK":
                        bATK += (int)Mathf.Round(stats.firstAmount);
                        break;
                    case "SPD":
                        BspeedRate += (int)Mathf.Round(stats.firstAmount);
                        break;
                    case "CRIT":
                        BcritRate += (int)Mathf.Round(stats.firstAmount);
                        break;
                    case "EVD":
                        BevasionRate += (int)Mathf.Round(stats.firstAmount);
                        break;
                    case "MDMG":
                        BextraMDMG += (int)Mathf.Round(stats.firstAmount);
                        break;
                    case "HP":
                        bHP += (int)Mathf.Round(stats.firstAmount);
                        break;
                    case "REG":
                        Bregeneration += (int)Mathf.Round(stats.firstAmount);
                        break;
                    default:
                        Debug.LogError("Can`t find first param in buffs manager!");
                        break;
                }
                switch (stats.secondAttr)
                {
                    case "STR":
                        bSTR += (int)Mathf.Round(stats.secondAmount);
                        break;
                    case "AGI":
                        bAGI += (int)Mathf.Round(stats.secondAmount);
                        break;
                    case "INT":
                        bINT += (int)Mathf.Round(stats.secondAmount);
                        break;
                    case "TRK":
                        bTRK += (int)Mathf.Round(stats.secondAmount);
                        break;
                    case "END":
                        bEND += (int)Mathf.Round(stats.secondAmount);
                        break;
                    case "ALL":
                        bSTR += (int)Mathf.Round(stats.secondAmount);
                        bAGI += (int)Mathf.Round(stats.secondAmount);
                        bINT += (int)Mathf.Round(stats.secondAmount);
                        bTRK += (int)Mathf.Round(stats.secondAmount);
                        bEND += (int)Mathf.Round(stats.secondAmount);
                        break;
                    case "ATK":
                        bATK += (int)Mathf.Round(stats.secondAmount);
                        break;
                    case "SPD":
                        BspeedRate += (int)Mathf.Round(stats.secondAmount);
                        break;
                    case "CRIT":
                        BcritRate += (int)Mathf.Round(stats.secondAmount);
                        break;
                    case "EVD":
                        BevasionRate += (int)Mathf.Round(stats.secondAmount);
                        break;
                    case "MDMG":
                        BextraMDMG += (int)Mathf.Round(stats.secondAmount);
                        break;
                    case "HP":
                        bHP += (int)Mathf.Round(stats.secondAmount);
                        break;
                    case "REG":
                        Bregeneration += (int)Mathf.Round(stats.secondAmount);
                        break;
                    default:
                        Debug.LogError("Can`t find first param in buffs manager!");
                        break;
                }
            }
        }

        //MULTIPLIERS FOR STATS
        if (bATKMult != 0)
        {
            int PlusAtk = (int)Mathf.Round((((hero.STR + bSTR) * 2) + bATK) * (bATKMult / 100f));
            bATK += PlusAtk;
        }
        if (BspeedRateMult != 0)
        {
            int PlusSPD = (int)Mathf.Round((((hero.AGI + bAGI) * 20) + BspeedRate) * (BspeedRateMult / 100f));
            BspeedRate += PlusSPD;
        }
        if (BcritRateMult != 0)
        {
            int PlusCrit = (int)Mathf.Round((((hero.TRK + bTRK) * 20) + BcritRate) * (BcritRateMult / 100f));
            BcritRate += PlusCrit;
        }
        if (BevasionRateMult != 0)
        {
            int PlusEvd = (int)Mathf.Round((((hero.TRK + bTRK) * 20) + BevasionRate) * (BevasionRateMult / 100f));
            BevasionRate += PlusEvd;
        }
        if (BmultMDMG != 0)
        {
            int PlusMDMG = (int)Mathf.Round((((hero.INT + bINT) * 10) + BextraMDMG) * (BmultMDMG / 100f));
            BextraMDMG += PlusMDMG;
        }
        if (BmultMDMG != 0) 
        {
            int PlusMDMG = (int)Mathf.Round((((hero.INT + bINT) * 10) + BextraMDMG) * (BmultMDMG / 100f));
            BextraMDMG += PlusMDMG;
        }
        if (BregenerationMult != 0)
        {
            int PlusRegen = (int)Mathf.Round((((hero.END + bEND) * 0.5f) + Bregeneration) * (BregenerationMult / 100f));
            Bregeneration += PlusRegen;
        }
        hero.RecalculateStats();
        DrawBuffsIcons();
    }
    void AfterSuccesfullAttack()
    {
        bool recalculating = false;
        foreach (KeyValuePair<string, GameObject> item in equipment)
        {

            if (item.Value != null && item.Value.GetComponent<Wearable>().rarity != 0)
            {
                Wearable stats = item.Value.GetComponent<Wearable>();
                if (stats.ability.abilittyPattern.triggerCondition == "SucAttack")
                {
                    for (int i = 0; i < stats.ability.attributesCount.Count; i++)
                    {
                        if (!stats.ability.isTriggersCountConstant[i])
                        {
                            stats.ability.triggersCount[i] = stats.ability.GetTriggerCount(i);
                        }
                        stats.ability.attributesCount[i] += stats.ability.triggersCount[i];
                        recalculating = true;
                        if (stats.ability.abilittyPattern.isStackable)
                            stats.ability.stacks++;
                    }
                }
                if (stats.ability.abilittyPattern.exitCondition == "SucAttack")
                {
                    stats.ability.ExitTrue();
                    recalculating = true;
                }
            }
        }
        if (recalculating)
        {
            RecountBuffs();
        }
    }
    void AfterGetDamage(int damage,bool critical)
    {
        bool recalculating = false;
        foreach (KeyValuePair<string, GameObject> item in equipment)
        {

            if (item.Value != null && item.Value.GetComponent<Wearable>().rarity != 0)
            {
                Wearable stats = item.Value.GetComponent<Wearable>();
                if (stats.ability.abilittyPattern.triggerCondition == "AfterGetDamage")
                {
                    for (int i = 0; i < stats.ability.attributesCount.Count; i++)
                    {
                        if (!stats.ability.isTriggersCountConstant[i])
                        {
                            stats.ability.triggersCount[i] = stats.ability.GetTriggerCount(i);
                        }
                        stats.ability.attributesCount[i] += stats.ability.triggersCount[i];
                        recalculating = true;
                        if (stats.ability.abilittyPattern.isStackable)
                            stats.ability.stacks++;
                    }
                }
                if (stats.ability.abilittyPattern.exitCondition == "AfterGetDamage")
                {
                    stats.ability.ExitTrue();
                    recalculating = true;
                }
            }
        }
        if (recalculating)
        {
            RecountBuffs();
        }
    }
    void AfterDeath()
    {
        bool recalculating = false;
        foreach (KeyValuePair<string, GameObject> item in equipment)
        {

            if (item.Value != null && item.Value.GetComponent<Wearable>().rarity != 0)
            {
                Wearable stats = item.Value.GetComponent<Wearable>();
                if (stats.ability.abilittyPattern.triggerCondition == "AfterDeath")
                {
                    for (int i = 0; i < stats.ability.attributesCount.Count; i++)
                    {
                        if (!stats.ability.isTriggersCountConstant[i])
                        {
                            stats.ability.triggersCount[i] = stats.ability.GetTriggerCount(i);
                        }
                        stats.ability.attributesCount[i] += stats.ability.triggersCount[i];
                        recalculating = true;
                        if (stats.ability.abilittyPattern.isStackable)
                            stats.ability.stacks++;
                    }
                }
                if (stats.ability.abilittyPattern.exitCondition == "AfterDeath")
                {
                    stats.ability.ExitTrue();
                    recalculating = true;
                }
            }
        }
        if (recalculating)
        {
            RecountBuffs();
        }
    }

    void AfterLevelStarted()
    {
        bool recalculating = false;
        foreach (KeyValuePair<string, GameObject> item in equipment)
        {

            if (item.Value != null && item.Value.GetComponent<Wearable>().rarity != 0)
            {
                Wearable stats = item.Value.GetComponent<Wearable>();
                if (stats.ability.abilittyPattern.triggerCondition == "StartLevel")
                {
                    for (int i = 0; i < stats.ability.attributesCount.Count; i++)
                    {
                        if (!stats.ability.isTriggersCountConstant[i])
                        {
                            stats.ability.triggersCount[i] = stats.ability.GetTriggerCount(i);
                        }
                        stats.ability.attributesCount[i] += stats.ability.triggersCount[i];
                        recalculating = true;
                        if (stats.ability.abilittyPattern.isStackable)
                            stats.ability.stacks++;
                    }
                }
                if (stats.ability.abilittyPattern.exitCondition == "StartLevel")
                {
                    stats.ability.ExitTrue();
                    recalculating = true;
                }
            }
        }
        if (recalculating)
        {
            RecountBuffs();
        }
    }

    void AfterCriticalAttack(int damage)
    {
        bool recalculating = false;
        foreach (KeyValuePair<string, GameObject> item in equipment)
        {

            if (item.Value != null && item.Value.GetComponent<Wearable>().rarity != 0)
            {
                Wearable stats = item.Value.GetComponent<Wearable>();
                if (stats.ability.abilittyPattern.triggerCondition == "AfterCrit")
                {
                    for (int i = 0; i <stats.ability.attributesCount.Count; i++)
                    {
                        if (!stats.ability.isTriggersCountConstant[i])
                        {
                            stats.ability.triggersCount[i] = stats.ability.GetTriggerCount(i);
                        }
                        stats.ability.attributesCount[i] += stats.ability.triggersCount[i];
                        recalculating = true;
                        if (stats.ability.abilittyPattern.isStackable)
                            stats.ability.stacks++;
                    }
                }
                if (stats.ability.abilittyPattern.triggerCondition == "AfterCritT" && TimedBuffs.instance.timedBuffs.Find(x => x.itemName == stats.itemName) == null)
                {
                    for (int i = 0; i < stats.ability.attributesCount.Count; i++)
                    {
                        if (!stats.ability.isTriggersCountConstant[i])
                        {
                            stats.ability.triggersCount[i] = stats.ability.GetTriggerCount(i);
                        }
                        stats.ability.attributesCount[i] += stats.ability.triggersCount[i];
                        recalculating = true;
                        if (stats.ability.abilittyPattern.isStackable)
                            stats.ability.stacks++;
                    }
                    TBuff newBuff = new TBuff();
                    if (stats.ability.abilittyPattern.exitCondition.Contains("AfterAttacks")) //AFTER ATTACKS (COUNT)
                    {
                            string str = stats.ability.abilittyPattern.exitCondition.Remove(0, 13);
                            str = str.Replace(")", "");
                            newBuff.duration = int.Parse(str);
                            newBuff.maxDuration = newBuff.duration;
                            newBuff.actionDuration = "attacks";
                            newBuff.ability = stats.ability;
                            newBuff.itemName = stats.itemName;
                    }
                    if (stats.ability.abilittyPattern.exitCondition.Contains("AfterTime")) //AFTER ATTACKS (COUNT)
                    {
                        string str = stats.ability.abilittyPattern.exitCondition.Remove(0, 10);
                        str = str.Replace(")", "");
                        newBuff.duration = int.Parse(str);
                        newBuff.maxDuration = newBuff.duration;
                        newBuff.actionDuration = "time";
                        newBuff.ability = stats.ability;
                        newBuff.itemName = stats.itemName;
                    }
                    TimedBuffs.instance.timedBuffs.Add(newBuff);
                }
                if (stats.ability.abilittyPattern.exitCondition == "AfterCrit")
                {
                    stats.ability.ExitTrue();
                    recalculating = true;
                }
            }
        }
        if (recalculating)
        {
            RecountBuffs();
        }
    }

    void AfterMissedAttack()
    {
        bool recalculating = false;
        foreach (KeyValuePair<string, GameObject> item in equipment)
        {

            if (item.Value != null && item.Value.GetComponent<Wearable>().rarity != 0)
            {
                Wearable stats = item.Value.GetComponent<Wearable>();
                if (stats.ability.abilittyPattern.exitCondition == "Miss")
                {
                    stats.ability.ExitTrue();
                    recalculating = true;
                }
            }
        }
        if (recalculating)
        {
            RecountBuffs();
        }
    }
    void AfterEvade()
    {
        bool recalculating = false;
        foreach (KeyValuePair<string, GameObject> item in equipment)
        {

            if (item.Value != null && item.Value.GetComponent<Wearable>().rarity != 0)
            {
                Wearable stats = item.Value.GetComponent<Wearable>();
                if (stats.ability.abilittyPattern.triggerCondition == "AfterEvade")
                {
                    for (int i = 0; i < stats.ability.attributesCount.Count; i++)
                    {
                        if (!stats.ability.isTriggersCountConstant[i])
                        {
                            stats.ability.triggersCount[i] = stats.ability.GetTriggerCount(i);
                        }
                        stats.ability.attributesCount[i] += stats.ability.triggersCount[i];
                        recalculating = true;
                        if (stats.ability.abilittyPattern.isStackable)
                            stats.ability.stacks++;
                    }
                }
                if (stats.ability.abilittyPattern.triggerCondition == "AfterEvadeT" && TimedBuffs.instance.timedBuffs.Find(x => x.itemName == stats.itemName) == null)
                {
                    for (int i = 0; i < stats.ability.attributesCount.Count; i++)
                    {
                        if (!stats.ability.isTriggersCountConstant[i])
                        {
                            stats.ability.triggersCount[i] = stats.ability.GetTriggerCount(i);
                        }
                        stats.ability.attributesCount[i] += stats.ability.triggersCount[i];
                        recalculating = true;
                        if (stats.ability.abilittyPattern.isStackable)
                            stats.ability.stacks++;
                    }
                    TBuff newBuff = new TBuff();
                    if (stats.ability.abilittyPattern.exitCondition.Contains("AfterTakes")) //AFTER ATTACKS (COUNT)
                    {
                        string str = stats.ability.abilittyPattern.exitCondition.Remove(0, 11);
                        str = str.Replace(")", "");
                        newBuff.duration = int.Parse(str);
                        newBuff.maxDuration = newBuff.duration;
                        newBuff.actionDuration = "takes";
                        newBuff.ability = stats.ability;
                        newBuff.itemName = stats.itemName;
                    }
                    if (stats.ability.abilittyPattern.exitCondition.Contains("AfterTime")) //AFTER ATTACKS (COUNT)
                    {
                        string str = stats.ability.abilittyPattern.exitCondition.Remove(0, 10);
                        str = str.Replace(")", "");
                        newBuff.duration = int.Parse(str);
                        newBuff.maxDuration = newBuff.duration;
                        newBuff.actionDuration = "time";
                        newBuff.ability = stats.ability;
                        newBuff.itemName = stats.itemName;
                    }
                    TimedBuffs.instance.timedBuffs.Add(newBuff);
                }
                if (stats.ability.abilittyPattern.exitCondition == "AfterEvade")
                {
                    stats.ability.ExitTrue();
                    recalculating = true;
                }
            }
        }
        if (recalculating)
        {
            RecountBuffs();
        }
    }

    void AfterKillMob()
    {
        bool recalculating = false;
        foreach (KeyValuePair<string, GameObject> item in equipment)
        {

            if (item.Value != null && item.Value.GetComponent<Wearable>().rarity != 0)
            {
                Wearable stats = item.Value.GetComponent<Wearable>();
                if (stats.ability.abilittyPattern.triggerCondition == "killMob")
                {
                    for (int i = 0; i < stats.ability.attributesCount.Count; i++)
                    {
                        if (!stats.ability.isTriggersCountConstant[i])
                        {
                            stats.ability.triggersCount[i] = stats.ability.GetTriggerCount(i);
                        }
                        stats.ability.attributesCount[i] += stats.ability.triggersCount[i];
                        recalculating = true;
                        if (stats.ability.abilittyPattern.isStackable)
                            stats.ability.stacks++;
                    }
                }
            }
        }
        if (recalculating)
        {
            RecountBuffs();
        }
    }

    void AfterLevelFinished(int lvl)
    {
        bool recalculating = false;
        foreach (KeyValuePair<string, GameObject> item in equipment)
        {

            if (item.Value != null && item.Value.GetComponent<Wearable>().rarity != 0)
            {
                Wearable stats = item.Value.GetComponent<Wearable>();
                if (stats.ability.abilittyPattern.triggerCondition == "PassLevel")
                {
                    for (int i = 0; i < stats.ability.attributesCount.Count; i++)
                    {
                        if (!stats.ability.isTriggersCountConstant[i])
                        {
                            stats.ability.triggersCount[i] = stats.ability.GetTriggerCount(i);
                        }
                        stats.ability.attributesCount[i] += stats.ability.triggersCount[i];
                        recalculating = true;
                        if (stats.ability.abilittyPattern.isStackable)
                            stats.ability.stacks++;
                    }
                }
                if (stats.ability.abilittyPattern.exitCondition == "EndLevel")
                {
                    stats.ability.ExitTrue();
                    recalculating = true;
                }
            }

        }
        if (recalculating)
        {
            RecountBuffs();
        }
    }

    public void StartTimedBuff(Item stats)
    {
        for (int i = 0; i < stats.ability.attributesCount.Count; i++)
        {
            if (!stats.ability.isTriggersCountConstant[i])
            {
                stats.ability.triggersCount[i] = stats.ability.GetTriggerCount(i);
            }
            stats.ability.attributesCount[i] += stats.ability.triggersCount[i];
            if (stats.ability.abilittyPattern.isStackable)
                stats.ability.stacks++;
        }
        TBuff newBuff = new TBuff();
        if (stats.ability.abilittyPattern.exitCondition.Contains("AfterAttacks")) //AFTER ATTACKS (COUNT)
        {
                newBuff.duration = 0;
                newBuff.actionDuration = "attacks";
                newBuff.ability = stats.ability;
        }
        if (stats.ability.abilittyPattern.exitCondition.Contains("AfterTime")) //AFTER ATTACKS (COUNT)
        {
            newBuff.duration = 0;
            newBuff.actionDuration = "time";
            newBuff.ability = stats.ability;
        }
        TimedBuffs.instance.timedBuffs.Add(newBuff);
        newBuff.Drop();
        RecountBuffs();
    }

    void AfterRegen()
    {
        bool recalculating = false;
        foreach (KeyValuePair<string, GameObject> item in equipment)
        {

            if (item.Value != null && item.Value.GetComponent<Wearable>().rarity != 0)
            {
                Wearable stats = item.Value.GetComponent<Wearable>();
                if (stats.ability.abilittyPattern.triggerCondition == "Regen")
                {
                    for (int i = 0; i < stats.ability.attributesCount.Count; i++)
                    {
                        if (!stats.ability.isTriggersCountConstant[i])
                        {
                            stats.ability.triggersCount[i] = stats.ability.GetTriggerCount(i);
                        }
                        stats.ability.attributesCount[i] += stats.ability.triggersCount[i];
                        recalculating = true;
                        if (stats.ability.abilittyPattern.isStackable)
                            stats.ability.stacks++;
                    }
                }
                if (stats.ability.abilittyPattern.exitCondition == "Regen")
                {
                    stats.ability.ExitTrue();
                    recalculating = true;
                }
            }

        }
        if (recalculating)
        {
            RecountBuffs();
        }
    }
}
