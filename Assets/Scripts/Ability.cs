using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLib;
using System;

public class Ability : MonoBehaviour
{
    public AbilityPattern abilittyPattern;
    public List<float> attributesCount = new List<float>();
    public int stacks;
    public List<float> triggersCount = new List<float>();
    public List<bool> isTriggersCountConstant = new List<bool>();
    
    public void InitAbility(AbilityPattern ap)
    {
        abilittyPattern = ap;
        string[] strings = abilittyPattern.triggerFormula.Split(';');
        for (int i = 0; i < ap.attributesCount.Length; i++)
        {
            string triggerString = strings[i];
            attributesCount.Add(ap.attributesCount[i]);
            triggersCount.Add(GetTriggerCount(i));
            switch (ap.triggerCondition)
            {
                case "killMob":
                    if (!triggerString.Contains("toAmount"))
                    {
                        isTriggersCountConstant.Add(true);
                    } else
                    {
                        isTriggersCountConstant.Add(false);
                    }
                    break;
                case "PassLevel":
                    isTriggersCountConstant.Add(true);
                    break;
                case "SucAttack":
                    if (!triggerString.Contains("toDiff") && !triggerString.Contains("heal"))
                    {
                        isTriggersCountConstant.Add(true);
                    }
                    else
                    {
                        isTriggersCountConstant.Add(false);
                    }
                    break;
                case "AfterCrit":
                    if (triggerString.Contains("plusOrMax") || triggerString.Contains("minusOrZero"))
                    {
                        isTriggersCountConstant.Add(false);
                    }
                    else
                    {
                        isTriggersCountConstant.Add(true);
                    }
                    break;
                case "AfterDeath":
                    if (triggerString.Contains("plusOrMax") || triggerString.Contains("minusOrZero"))
                    {
                        isTriggersCountConstant.Add(false);
                    }
                    else
                    {
                        isTriggersCountConstant.Add(true);
                    }
                    break;
                case "AfterCritT":
                    if (!triggerString.Contains("toAmount"))
                    {
                        isTriggersCountConstant.Add(true);
                    }
                    else
                    {
                        isTriggersCountConstant.Add(false);
                    }
                    break;
                case "AfterEvade":
                    if (triggerString.Contains("plusOrMax") || triggerString.Contains("minusOrZero"))
                    {
                        isTriggersCountConstant.Add(false);
                    }
                    else
                    {
                        isTriggersCountConstant.Add(true);
                    }
                    break;
                case "AfterMAtk":
                    if (triggerString.Contains("toAmount") || triggerString.Contains("minusOrZero"))
                    {
                        isTriggersCountConstant.Add(false);
                    }
                    else
                    {
                        isTriggersCountConstant.Add(true);
                    }
                    break;
                case "AfterEvadeT":
                    if (!triggerString.Contains("toAmount"))
                    {
                        isTriggersCountConstant.Add(true);
                    }
                    else
                    {
                        isTriggersCountConstant.Add(false);
                    }
                    break;
                case "Regen":
                    if (!triggerString.Contains("hpDiff") && !triggerString.Contains("hp50"))
                    {
                        isTriggersCountConstant.Add(true);
                    }
                    else
                    {
                        isTriggersCountConstant.Add(false);
                    }
                    break;
                case "Recalculate":
                    isTriggersCountConstant.Add(true);
                    break;
                default:
                    Debug.LogError("Not accept the param of triggerCondition in InitAbility!");
                    break;
            }
        }

    }
    public string GetDescription()
    {
        string finalString = abilittyPattern.description;

        for(int i=0;i< attributesCount.Count; i++)
        {
            if (!isTriggersCountConstant[i])
                triggersCount[i] = GetTriggerCount(i);

            finalString = finalString.Replace("<n" + (i + 1) + ">", triggersCount[i].ToString("F2"));
            finalString = finalString.Replace("<v" + (i + 1) + ">", Evaluator.CheckFloat(attributesCount[i]));
        }
        return finalString;
    }
    public string GetBuffDescription()
    {
        string finalString = abilittyPattern.buffDescription;

        for (int i = 0; i < attributesCount.Count; i++)
        {
            if (!isTriggersCountConstant[i])
                triggersCount[i] = GetTriggerCount(i);

            //finalString = finalString.Replace("<n" + (i + 1) + ">", triggersCount[i].ToString("F2"));
            finalString = finalString.Replace("<v" + (i + 1) + ">", Evaluator.CheckFloat(attributesCount[i]));
        }
        return finalString;
    }
    public float GetTriggerCount(int index)
    {
        string[] strings = abilittyPattern.triggerFormula.Split(';');
        string triggerString = strings[index];

        if (triggerString.Contains("toAmount"))
        {
            triggerString = triggerString.Remove(0, 9);
            triggerString = triggerString.Remove(triggerString.Length - 1);
            float calcAmount = Evaluator.FormulaSolution(triggerString) - attributesCount[index];
            return calcAmount;
        }
        if (triggerString.Contains("toDiff"))
        {
            triggerString = triggerString.Remove(0, 7);
            triggerString = triggerString.Remove(triggerString.Length - 1);
            string[] paramStrings = triggerString.Split(',');
            float firstParam = 0;
            float secondParam = 0;
            switch (paramStrings[0])
            {
                case "STR":
                    firstParam = Hero.instance.STR+BuffsManager.instance.bSTR;
                    break;
                case "AGI":
                    firstParam = Hero.instance.AGI + BuffsManager.instance.bAGI;
                    break;
                case "INT":
                    firstParam = Hero.instance.INT + BuffsManager.instance.bINT;
                    break;
                case "END":
                    firstParam = Hero.instance.END + BuffsManager.instance.bEND;
                    break;
                case "TRK":
                    firstParam = Hero.instance.TRK + BuffsManager.instance.bTRK;
                    break;
                case "ALL":
                    firstParam = Hero.instance.STR + BuffsManager.instance.bSTR + Hero.instance.AGI + BuffsManager.instance.bAGI + Hero.instance.INT + BuffsManager.instance.bINT + Hero.instance.END + BuffsManager.instance.bEND + Hero.instance.TRK + BuffsManager.instance.bTRK;
                    break;
                default:
                    break;
            }
            switch (paramStrings[1])
            {
                case "mSTR":
                    secondParam = MobsContainer.instance.currentMob.STR;
                    break;
                case "mAGI":
                    secondParam = MobsContainer.instance.currentMob.AGI;
                    break;
                case "mINT":
                    secondParam = MobsContainer.instance.currentMob.INT;
                    break;
                case "mEND":
                    secondParam = MobsContainer.instance.currentMob.END;
                    break;
                case "mTRK":
                    secondParam = MobsContainer.instance.currentMob.TRK;
                    break;
                case "mALL":
                    Mob mob = MobsContainer.instance.currentMob;
                    secondParam = mob.STR + mob.AGI+ mob.INT+ mob.END+ mob.TRK;
                    break;
                default:
                    break;
            }
            float calcAmount = (Mathf.Abs(secondParam - firstParam)/firstParam)*100 - attributesCount[index];
            return calcAmount;
        }
        if (triggerString.Contains("toZero"))
        {
            return 0-attributesCount[index];
        }
        if (triggerString.Contains("plusOrMax"))
        {
            string str = triggerString.Remove(0, 10);
            str = str.Replace(")", "");
            int maxCount = int.Parse(str.Split(',')[1]);
            int plusCount = int.Parse(str.Split(',')[0]);
            if (attributesCount[index] != maxCount)
            {
                return plusCount;
            } else
            {
                return 0;
            }
        }
        if (triggerString.Contains("minusOrZero"))
        {
            string str = triggerString.Remove(0, 12);
            str = str.Replace(")", "");
            int plusCount = int.Parse(str);
            if (attributesCount[index] != 0)
            {
                return -plusCount;
            }
            else
            {
                return 0;
            }
        }
        if (triggerString.Contains("hpDiff"))
        {
            triggerString = triggerString.Remove(0, 7);
            triggerString = triggerString.Remove(triggerString.Length - 1);
            float calcAmount = Evaluator.FormulaSolution(triggerString) * (1 - (Hero.instance.currentHP / (float)Hero.instance.HP));
            calcAmount -= attributesCount[index];
            return calcAmount;
        }
        if(triggerString.Contains("hp50"))
        {
            triggerString = triggerString.Remove(0, 5);
            triggerString = triggerString.Remove(triggerString.Length - 1);
            float calcAmount;
            if ((Hero.instance.currentHP / (float)Hero.instance.HP) <= 0.5f)
            {
                calcAmount = Evaluator.FormulaSolution(triggerString) - attributesCount[index];
            } else
            {
                calcAmount = 0 - attributesCount[index];
            }
            return calcAmount;
        }
        if (triggerString.Contains("lvlCount"))
        {
            triggerString = triggerString.Replace("lvlCount", Mathf.Round(5 * Mathf.Pow(1.1f, GetComponent<Wearable>().level - 1) * Mathf.Pow(1.2f, (int)GetComponent<Wearable>().rarity)).ToString());
        }

        return Evaluator.FormulaSolution(triggerString);
    }
    public void ExitTrue()
    {
        string[] dividedFormulas = abilittyPattern.exitFormula.Split(';');
        for (int i = 0; i < attributesCount.Count; i++)
        {
            if (dividedFormulas[i].Contains("toZero"))
            {
                attributesCount[i] = 0;
                if (BuffsManager.instance.currentBuffHint)
                    if (gameObject.GetComponent<Wearable>() == BuffsManager.instance.currentBuffHint.GetComponent<BuffHint>().item)
                        BuffsManager.instance.currentBuffHint.GetComponent<BuffHint>().DropBuff();
            }
            if (dividedFormulas[i].Contains("minusOrZero"))
            {
                if (attributesCount[i] != 0)
                {
                    string str = dividedFormulas[i].Remove(0,12);
                    str = str.Replace(")", "");
                    attributesCount[i] -= int.Parse(str);
                } else
                {
                    if (BuffsManager.instance.currentBuffHint)
                        if (gameObject.GetComponent<Wearable>() == BuffsManager.instance.currentBuffHint.GetComponent<BuffHint>().item)
                            BuffsManager.instance.currentBuffHint.GetComponent<BuffHint>().DropBuff();
                }
            }
            if (dividedFormulas[i].Contains("plusOrMax"))
            {
                 string str = dividedFormulas[i].Remove(0, 10);
                 str = str.Replace(")", "");
                 int maxCount = int.Parse(str.Split(',')[1]);
                 int plusCount = int.Parse(str.Split(',')[0]);
                 if (attributesCount[i] != maxCount)
                 {
                     attributesCount[i] += plusCount;
                 }
            }
            if (dividedFormulas[i].Contains("toAmount"))
            {
                string str = dividedFormulas[i].Remove(0, 9);
                str = str.Replace(")", "");
                attributesCount[i] = float.Parse(str);
            }
        }
        stacks = 0;
    }
}
