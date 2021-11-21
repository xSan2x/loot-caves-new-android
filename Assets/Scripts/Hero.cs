using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameLib;

public class Hero : MonoBehaviour
{
    public static Hero instance = null; //Singleton variable
    BuffsManager buffs; //Singleton variable
    //Hero variables
    public int level { get; set; }
    public int exp { get; set; }
    public int needExp { get; set; }
    public int currentHP { get; set; }
    public int gold { get; set; }
    public int skin { get; set; }
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
    //Attributtes from buffs
    private int ownSTR = 0;
    private int ownAGI = 0;
    private int ownINT = 0;
    private int ownTRK = 0;
    private int ownEND = 0;
    //Stats draw slots
    public Text[] statsTexts = new Text[18];

    //MODIFIERS
    List<string> modifiers = new List<string>();

    //fast changing params
    public float attackCooldown = 0;
    public float maxAttackCooldown = 0;
    float heroHitChance = 0;
    float heroCritChance = 0;
    public float heroEvadeChance = 0;

    public Text goldCountUI;

    //delegates
    public delegate void KillMobMethod();
    public delegate void AttackMethod(int damage);
    public delegate void HealMethod(int heal);
    public delegate void SucAttackMethod();
    public delegate void CriticalAttackMethod(int damage);
    public delegate void MissedAttackMethod();
    public delegate void DeathMethod();
    public delegate void LevelUpMethod();
    public delegate void RegenMethod();
    public delegate void EvadeMethod();
    public delegate void GetDamageMethod(int damage,bool critical);

    //events
    public event KillMobMethod OnKillMob;
    public event AttackMethod OnAttack;
    public event DeathMethod OnDeath;
    public event HealMethod OnHeal;
    public event RegenMethod OnRegen;
    public event EvadeMethod OnEvade;
    public event LevelUpMethod OnLevelUp;
    public event GetDamageMethod OnGetDamage;
    public event SucAttackMethod OnSuccesfullAttack;
    public event CriticalAttackMethod OnCriticalAttack;
    public event MissedAttackMethod OnMissedAttack;

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
    private void Start()
    {
        buffs = BuffsManager.instance;
        LevelManager.instance.OnLevelStarted += LevelStarted;
    }
    public void Attack(Mob target)
    {
        float hitChance= 0.95f;
        float critChance= 0.05f+(buffs.BcritChance/100f);
        if (accRate < target.evasionRate)
        {
            float hitkf = (float)accRate / target.evasionRate;
            hitkf = Mathf.Pow(hitkf, 1.5f);
            if (hitkf < 0.95f)
            {
                hitChance = hitkf;
            } 
        }
        float rndHit = Random.Range(0f, 1f);
        if (rndHit <= hitChance)
        {
            if (critRate > target.critRate)
            {
                float critkf = Mathf.Abs(1-((float)critRate / target.critRate));
                critkf = Mathf.Pow(critkf, 2);
                if (critkf > 0.90f)
                {
                    critChance = 0.95f;
                } else
                {
                    critChance += critkf;
                }
            }
            float rndCrit = Random.Range(0f, 1f);
            if (rndCrit <= critChance)
            {
                //Crit attack!
                if (!target.GetCriticalAttackDamage(ATK*(1+critMultiplier)))
                {
                    if (modifiers.Contains("_corpse_heal_5"))
                    {
                        Heal(HP * 0.05f);
                    }
                    OnKillMob();
                }
                if (modifiers.Contains("_lifesteal_10"))
                {
                    Heal(ATK * (1 + critMultiplier) * 0.1f);
                }
                if (modifiers.Contains("_crit_heal_5"))
                {
                    Heal(HP*0.05f);
                }
                OnCriticalAttack(ATK * (1 + critMultiplier));
                OnSuccesfullAttack();
            } else
            {
                //Normal attack

                if (!target.GetNormalAttackDamage(ATK))
                {
                    if (modifiers.Contains("_corpse_heal_5"))
                    {
                        Heal(HP * 0.05f);
                    }
                    OnKillMob();
                }
                if (modifiers.Contains("_lifesteal_10"))
                {
                    Heal(ATK * 0.1f);
                }
                OnAttack(ATK);
                OnSuccesfullAttack();
            }
        } else
        {
            //attack missed
            target.GetMissedAttack();
            OnMissedAttack();
        }
        heroCritChance = critChance;
        heroHitChance = hitChance;
        RedrawStats();
    }

    public bool GetNormalAttackDamage(int damage, Mob receiver, bool isCritical=false)
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
            OnGetDamage(damage,isCritical);
            float kf = (1 / receiver.maxAttackCooldown) / 3;
            GetComponent<Animator>().speed = kf;
            GetComponent<Animator>().SetBool("GetDamage", true);
            StartCoroutine(DamageAnimation());
        }
        else
        {
            Death();
        }
        RedrawStats();
        return alive;
    }
    public void GetMissedAttack(Mob receiver)
    {
        float kf = (1 / receiver.maxAttackCooldown) / 3;
        GetComponent<Animator>().speed = kf;
        GetComponent<Animator>().SetBool("Evade", true);
        if (modifiers.Contains("_evade_heal_5"))
        {
            Heal(HP*0.05f);
        }
        OnEvade();
        StartCoroutine(DamageAnimation());
    }
    void Heal(float amount)
    {
        int heal = (int)Mathf.Round(amount);
        if (currentHP + heal >= HP)
        {
            heal = HP - currentHP;
            currentHP = HP;
        }
        if(heal>0)
            OnHeal(heal);
    }

    public void Initialitation()
    {
        ownSTR = ownAGI = ownINT = ownTRK = ownEND = (int)Mathf.Round(10 * Mathf.Pow(1.1f, level - 1));
        for (var i = 1; i <= level; i++)
        {
            needExp += (int)Mathf.Round(100 * Mathf.Pow(1.1f, i - 1));
        }
        RecalculateStats();
    }
    public void LevelStarted()
    {
        currentHP = HP;
        RecalculateStats();
    }
    public void RecalculateStats()
    {
        STR = ownSTR + buffs.bSTR;
        AGI = ownAGI + buffs.bAGI;
        INT = ownINT + buffs.bINT;
        TRK = ownTRK + buffs.bTRK;
        END = ownEND + buffs.bEND;
        float hpkf;
        if (HP != 0)
        {
            hpkf = currentHP / (float)HP;
        }
        else
        {
            hpkf = 1;
        }
        HP = END * 20 + buffs.bHP;
        currentHP = (int)Mathf.Round(HP * hpkf);
        ATK = STR * 2 + buffs.bATK;
        speedRate = AGI * 20 + buffs.BspeedRate;
        accRate = AGI * 20 + buffs.BaccRate;
        extraMDMG = INT * 10 + buffs.BextraMDMG;
        cooldownReduce = INT + buffs.BcooldownReduce;
        critRate = TRK * 20 + buffs.BcritRate;
        critMultiplier = 1 + buffs.BcritMultiplier;
        critMageMultiplier = 0 + buffs.BcritMageMultiplier;
        evasionRate = TRK * 20 + buffs.BevasionRate;
        regeneration = END * 0.5f + buffs.Bregeneration;
        RedrawStats();
    }
    void RedrawStats()
    {
        statsTexts[0].text = "Level: "+level;
        statsTexts[1].text = "Strength: "+ Evaluator.CheckInt(STR);
        statsTexts[2].text = "Agility: "+ Evaluator.CheckInt(AGI);
        statsTexts[3].text = "Intelligence: "+ Evaluator.CheckInt(INT);
        statsTexts[4].text = "Trickery: "+ Evaluator.CheckInt(TRK);
        statsTexts[5].text = "Endurance: "+ Evaluator.CheckInt(END);
        statsTexts[6].text = "Attack: "+ Evaluator.CheckInt(ATK);
        statsTexts[7].text = "Health: "+ Evaluator.CheckInt(currentHP) +"/"+ Evaluator.CheckInt(HP);
        statsTexts[8].text = "Atk. Cooldown: "+ maxAttackCooldown.ToString("F2") + " sec";
        if (heroHitChance == 0)
            heroHitChance = 0.95f;
        statsTexts[9].text = "Accuracy: "+ (heroHitChance*100).ToString("F1") + "%";
        statsTexts[10].text = "ExtraMageDMG: " + Evaluator.CheckInt(extraMDMG);
        statsTexts[11].text = "CD Reduce: " + (cooldownReduce/100).ToString("F2")+"%";
        statsTexts[12].text = "Evasion: " + (heroEvadeChance*100).ToString("F2")+"%";
        if (heroCritChance == 0)
            heroCritChance = 0.05f;
        statsTexts[13].text = "Crit. Chance: " + (heroCritChance*100).ToString("F1")+"%";
        statsTexts[14].text = "Crit. Multiplier: " + (critMultiplier+1)+"x";
        statsTexts[15].text = "Regen: " + Evaluator.CheckFloat(regeneration) +" HP/sec";
        statsTexts[16].text = "EXP: " + Evaluator.CheckInt(exp) + "/" + Evaluator.CheckInt(needExp);
        statsTexts[17].text = "Cave Level: " + LevelManager.instance.currLvl;

    }
    public void RewardGoldAndExp(int goldReward,int expReward)
    {
        gold += goldReward;
        exp += expReward;
        statsTexts[16].text = "EXP: " + Evaluator.CheckInt(exp) + "/" + Evaluator.CheckInt(needExp);
        if (exp >= needExp)
        {
            LevelUp();
        }
        StartCoroutine(GoldRedraw());
    }
    public void Regenerate()
    {
        if (currentHP > 0)
        {
            if(currentHP + (int)Mathf.Round(regeneration) < HP)
            {
                currentHP += (int)Mathf.Round(regeneration);
                
            } else
            {
                currentHP = HP;
            }
            RedrawStats();
            OnRegen();
        }
    }
    void LevelUp()
    {
        if (level < 100)
        {
            level++;
            currentHP = HP;
            needExp += (int)Mathf.Round(100 * Mathf.Pow(1.1f, level - 1));
            ownSTR = ownAGI = ownINT = ownTRK = ownEND = (int)Mathf.Round(10 * Mathf.Pow(1.1f, level - 1));
            RecalculateStats();
        }
    }
    void Death()
    {
        OnDeath();
        GetComponent<Animator>().speed = 1;
        GetComponent<Animator>().SetBool("Death", true);
        StartCoroutine(DeathAnimation());
    }
    public void AddModifier(string mod)
    {
        if (!modifiers.Contains(mod))
            modifiers.Add(mod);
    }
    public void DeletePassiveModifier(string mod)
    {
        bool hadMod = false;
        foreach(KeyValuePair<string, GameObject> item in Equipment.instance.equipment)
        {
            if (item.Value!= null && item.Value.GetComponent<Wearable>().ability.abilittyPattern.triggerFormula == mod)
            {
                hadMod = true;
            }
        }
        if (!hadMod)
            modifiers.Remove(mod);
    }

    IEnumerator DeathAnimation()
    {
        int i = 0;
        while (i < 10)
        {
            yield return new WaitForEndOfFrame();
            if (i < 5)
            {
                transform.Translate(new Vector3(-0.03f, 0));
            }
            else
            {
                transform.Translate(new Vector3(0.03f, 0));
            }
            i++;
        }
        yield return new WaitForSeconds(1f);
        GetComponent<Animator>().SetBool("Death", false);
    }
    IEnumerator DamageAnimation()
    {
        int i = 0;
        while (i < 10)
        {
            yield return new WaitForEndOfFrame();
            if (i < 5)
            {
                transform.Translate(new Vector3(-0.03f, 0));
            }
            else
            {
                transform.Translate(new Vector3(0.03f, 0));
            }
            i++;
        }
        GetComponent<Animator>().SetBool("GetDamage", false);
        GetComponent<Animator>().SetBool("Evade", false);
    }
    IEnumerator GoldRedraw()
    {
        yield return new WaitForSeconds(1f);
        goldCountUI.text = Evaluator.CheckInt(gold);
    }
}
