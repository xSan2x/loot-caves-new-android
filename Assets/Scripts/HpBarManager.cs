using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameLib;

public class HpBarManager : MonoBehaviour
{
    public static HpBarManager instance = null; //Singleton variable

    public MobsContainer mobs;
    public Hero hero;
    public Slider mobSlider;
    public Slider heroSlider;
    public CanvasGroup mobCG;
    public CanvasGroup heroCG;
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
    void Start()
    {
        hero = Hero.instance;
        hero.OnKillMob += HideMobBar;
        hero.OnAttack += Attack;
        hero.OnGetDamage += GetDamage;
        hero.OnDeath += HeroDeath;
        hero.OnRegen += Regeneration;
        hero.OnCriticalAttack += CriticalAttack;
        LevelManager.instance.OnLevelStarted += InitBars;
        mobs = MobsContainer.instance;
        
    }
    void HideMobBar()
    {
        if (mobs.indexMob < 9)
        {
            StartCoroutine(ShowBar(false));
        } else
        {
            StartCoroutine(ShowBar(true));
        }
        
    }
    void InitBars()
    {
        mobCG.alpha = 1;
        mobCG.interactable = true;
        mobCG.blocksRaycasts = true;
        mobSlider.value = 1;
        mobCG.transform.GetChild(2).GetComponent<TextMeshProUGUI>().faceColor = Evaluator.rarityColor[(int)mobs.currentMob.rarity];
        mobCG.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = mobs.currentMob.name;
        heroCG.alpha = 1;
        heroCG.interactable = true;
        heroCG.blocksRaycasts = true;
        heroSlider.value = 1;
    }
    IEnumerator ShowBar(bool hide)
    {
        yield return new WaitForSeconds(0.34f);
        mobCG.alpha = 0;
        mobCG.interactable = false;
        mobCG.blocksRaycasts = false;
        heroCG.alpha = 0;
        heroCG.interactable = false;
        heroCG.blocksRaycasts = false;
        if (!hide)
        {
            yield return new WaitForSeconds(1f);
            mobCG.transform.GetChild(2).GetComponent<TextMeshProUGUI>().faceColor = Evaluator.rarityColor[(int)mobs.currentMob.rarity];
            mobCG.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = mobs.currentMob.name;
            mobCG.alpha = 1;
            mobCG.interactable = true;
            mobCG.blocksRaycasts = true;
            mobSlider.value = 1;
            heroCG.alpha = 1;
            heroCG.interactable = true;
            heroCG.blocksRaycasts = true;
        }
    }
    void Attack(int damage)
    {
        float kf = mobs.currentMob.currentHP / (float)mobs.currentMob.HP;
        mobSlider.value = kf;
        mobCG.transform.GetChild(3).GetComponent<MobHintParent>().CreateHint("-" + Evaluator.CheckInt(damage), new Color(196, 0, 0));
    }
    void GetDamage(int damage,bool critical)
    {
        float kf = hero.currentHP / (float)hero.HP;
        heroSlider.value = kf;
        if (!critical)
        {
            heroCG.transform.GetChild(2).GetComponent<HeroHintParent>().CreateHint("-" + Evaluator.CheckInt(damage), new Color(196, 0, 0));
        } else
        {
            heroCG.transform.GetChild(2).GetComponent<HeroHintParent>().CreateHint("-" + Evaluator.CheckInt(damage) + " !!!", new Color(196, 0, 0));
        }
    }
    void Regeneration()
    {
        float kf = hero.currentHP / (float)hero.HP;
        heroSlider.value = kf;
    }
    void CriticalAttack(int damage)
    {
        float kf = mobs.currentMob.currentHP / (float)mobs.currentMob.HP;
        mobSlider.value = kf;
        mobCG.transform.GetChild(3).GetComponent<MobHintParent>().CreateHint("-" + Evaluator.CheckInt(damage) + " !!!", new Color(196, 0, 0));
    }

    void HeroDeath()
    {
        heroSlider.value = 0;
        StartCoroutine(ShowBar(false));
    }
}
