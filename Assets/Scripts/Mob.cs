using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLib;

public class Mob : MonoBehaviour
{
    //Mob variables
    public int level { get; set; }
    public int exp { get; set; }
    public int currentHP { get; set; }
    public int gold { get; set; }
    public Evaluator.Rarity rarity { get; set; }
    public int skin { get; set; }
    public string name { get; set; }
    //Attributes
    public int STR { get; set; }
    public int AGI { get; set; }
    public int INT { get; set; }
    public int TRK { get; set; }
    public int END { get; set; }
    public int ATK { get; set; }
    public int HP { get; set; }
    public int speedRate { get; set; }       //SR
    public int accRate { get; set; }         //ACR
    public int extraMDMG { get; set; }       //EMD
    public int cooldownReduce { get; set; }  //CDR
    public int critRate { get; set; }        //CR
    public int critMultiplier { get; set; }  //CM
    public int critMageMultiplier { get; set; } //CMM
    public int evasionRate { get; set; }     //ER
    public float regeneration { get; set; }  //REG
    public float maxAttackCooldown { get; set; }  
    public float attackCooldown { get; set; } 
    public int index { get; set; } //index of mob in mob list
    public void Initialitation(int lvl)
    {
        level = lvl;
        float legCh = (0.1f * (0.1f + lvl*0.0066f)); //chance to get legendary mob
        rarity = Evaluator.RandRarity(legCh); //Mob obtain random rarity
        GameSettings gameSettings = GameSettings.instance;
        skin = Random.Range(1, gameSettings.maxMobIndex+1);
        STR = AGI = INT = TRK = END = (int)Mathf.Round(10 * Mathf.Pow(gameSettings.kfAtkRaise, level - 1)); //Mob obtain his stats
        gold = (int)Mathf.Round(10 * Mathf.Pow(1.1f, (float)rarity)* Mathf.Pow(1.1f, level - 1)); //Mob obtain his stats
        exp = gold / 2;
        if(rarity != Evaluator.Rarity.Common)
        {
            string AttrName = "";
            int rnd = Random.Range(0, 4);
            switch (rnd)
            {
                case 0:
                    STR = (int)Mathf.Round(STR * Mathf.Pow(1.2f, (float)rarity));
                    AttrName = "Strength";
                    break;
                case 1:
                    AGI = (int)Mathf.Round(AGI * Mathf.Pow(1.2f, (float)rarity));
                    AttrName = "Agilitty";
                    break;
                case 2:
                    TRK = (int)Mathf.Round(TRK * Mathf.Pow(1.2f, (float)rarity));
                    AttrName = "Trickery";
                    break;
                case 3:
                    END = (int)Mathf.Round(END * Mathf.Pow(1.2f, (float)rarity));
                    AttrName = "Endurance";
                    break;
                default:

                    break;
            }
            string rarityName = Evaluator.rarityStr[(int)rarity];
            rarityName = char.ToUpper(rarityName[0]) + rarityName.Substring(1); 
            name = rarityName + " "+AttrName+" creature ";
        } else
        {
            name = "Common creature";
        }
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("mobs/mob (" + skin + ")");
        InitStats();
    }
    void InitStats()
    {
        critMultiplier = 1;
        ATK = STR * 2;
        speedRate = AGI * 20;
        accRate = AGI * 20;
        critRate = TRK * 20;
        evasionRate = TRK * 20;
        HP = END * 2;
        currentHP = HP;
        regeneration = END * 0.5f;
    }
    public bool isAlive()
    {
        if (currentHP <= 0)
            return false;
        return true;
    }
    public bool GetNormalAttackDamage(int damage)
    {
        bool alive = true;
        if (currentHP - damage > 0)
        {
            currentHP -= damage;
        } else
        {
            currentHP = 0;
            alive = false;
        }
        //Place for animation and other
        if (currentHP > 0)
        {
            float kf = (1 / Hero.instance.maxAttackCooldown) / 3;
            GetComponent<Animator>().speed = kf;
            GetComponent<Animator>().SetBool("GetDamage", true);
            StartCoroutine(DamageAnimation());
        }
        return alive;
    }
    public bool GetCriticalAttackDamage(int damage)
    {
        bool alive = true;
        if (currentHP - damage > 0)
        {
            currentHP -= damage;
        }
        else
        {
            currentHP = 0;
            alive = false;
        }
        //Place for animation and other
        if (currentHP > 0)
        {
            float kf = (1 / Hero.instance.maxAttackCooldown) / 3;
            GetComponent<Animator>().speed = kf;
            GetComponent<Animator>().SetBool("GetDamage", true);
            StartCoroutine(DamageAnimation());
        }
        return alive;
    }
    public void GetMissedAttack()
    {
        float kf = (1/Hero.instance.maxAttackCooldown)/ 3;
        GetComponent<Animator>().speed = kf;
        GetComponent<Animator>().SetBool("Evade", true);
        StartCoroutine(DamageAnimation());
    }

    public void Death()
    {
        GetComponent<Animator>().speed = 1;
        GetComponent<Animator>().SetBool("Death", true);
        StartCoroutine(DeathAnimation());
    }
    IEnumerator DeathAnimation()
    {
        int i = 0;
        while (i < 10)
        {
            yield return new WaitForEndOfFrame();
            if (i < 5)
            {
                transform.Translate(new Vector3(0.03f, 0));
            }
            else
            {
                transform.Translate(new Vector3(-0.03f, 0));
            }
            i++;
        }
        yield return new WaitForSeconds(0.332f);
        LootHintParent.instance.MobDeathHint(gold, exp);
        Hero.instance.RewardGoldAndExp(gold, exp);
        Destroy(gameObject);
    }
    IEnumerator DamageAnimation()
    {
        int i = 0;
        while (i < 10)
        {
            yield return new WaitForEndOfFrame();
            if (i < 5)
            {
                transform.Translate(new Vector3(0.03f,0));
            } else
            {
                transform.Translate(new Vector3(-0.03f, 0));
            }
            i++;
        }
        GetComponent<Animator>().SetBool("GetDamage", false);
        GetComponent<Animator>().SetBool("Evade", false);
    }

    public void Attack(Hero target)
    {
        float hitChance = 0.95f-(BuffsManager.instance.BevasionChance/100f);
        float critChance = 0.05f;
        if (accRate < target.evasionRate)
        {
            float hitkf = (float)accRate / target.evasionRate;
            hitkf = Mathf.Pow(hitkf, 1.5f);
            if (hitkf < 0.95f-(BuffsManager.instance.BevasionChance / 100f))
            {
                hitChance = hitkf - (BuffsManager.instance.BevasionChance / 100f);
            }
        }
        Hero.instance.heroEvadeChance = 1 - hitChance;
        float rndHit = Random.Range(0f, 1f);
        if (rndHit <= hitChance)
        {
            if (critRate > target.critRate)
            {
                float critkf = Mathf.Abs(1 - ((float)critRate / target.critRate));
                critkf = Mathf.Pow(critkf, 2);
                if (critkf > 0.90f)
                {
                    critChance = 0.95f;
                }
                else
                {
                    critChance += critkf;
                }
            }
            float rndCrit = Random.Range(0f, 1f);
            if (rndCrit <= critChance)
            {
                //Crit attack!
                target.GetNormalAttackDamage(ATK * (1 + critMultiplier),this,true);
            }
            else
            {
                //Normal attack
                target.GetNormalAttackDamage(ATK,this);
                
            }
        }
        else
        {
            //attack missed
            target.GetMissedAttack(this);
        }
    }
}
