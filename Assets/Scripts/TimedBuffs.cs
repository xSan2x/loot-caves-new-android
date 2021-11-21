using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedBuffs : MonoBehaviour
{
    public static TimedBuffs instance = null; //Singleton object variable
    [SerializeField]
    public List<TBuff> timedBuffs = new List<TBuff>();
    // Start is called before the first frame update
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
        Hero hero = Hero.instance;
        hero.OnSuccesfullAttack += AfterSuccesfullAttack;
        hero.OnGetDamage += AfterGetDamage;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        bool recalculating = false;
        foreach (TBuff buff in timedBuffs)
        {
            float durationKF;
            if (buff.actionDuration == "time")
            {
                buff.duration -= Time.fixedDeltaTime;
                durationKF = buff.duration / buff.maxDuration;
                if (buff.duration <= 0)
                {
                    timedBuffs.Remove(buff);
                    recalculating = true;
                }
                buff.ability.gameObject.GetComponent<Item>().buffSlot.transform.GetChild(0).transform.GetChild(3).GetComponent<Image>().fillAmount = durationKF;
            }
        }
        if (recalculating)
            BuffsManager.instance.RecountBuffs();
    }

    void AfterSuccesfullAttack()
    {
        bool recalculating = false;
        foreach (TBuff buff in timedBuffs)
        {
            if (buff.duration <= 0)
            {
                timedBuffs.Remove(buff);
            }
        }
        foreach (TBuff buff in timedBuffs)
        {
            float durationKF;
            if (buff.actionDuration == "attacks")
            {
                buff.duration--;
                durationKF = buff.duration / buff.maxDuration;
                if (buff.duration <= 0)
                {
                    buff.Drop();
                    recalculating = true;
                }
                buff.ability.gameObject.GetComponent<Item>().buffSlot.transform.GetChild(0).transform.GetChild(3).GetComponent<Image>().fillAmount = durationKF;
            }
        }
        if (recalculating)
            BuffsManager.instance.RecountBuffs();
        
    }

    void AfterGetDamage(int damage,bool critical)
    {
        bool recalculating = false;
        foreach (TBuff buff in timedBuffs)
        {
            if (buff.duration <= 0)
            {
                timedBuffs.Remove(buff);
            }
        }
        foreach (TBuff buff in timedBuffs)
        {
            float durationKF;
            if (buff.actionDuration == "takes")
            {
                buff.duration--;
                durationKF = buff.duration / buff.maxDuration;
                if (buff.duration <= 0)
                {
                    buff.Drop();
                    recalculating = true;
                }
                buff.ability.gameObject.GetComponent<Item>().buffSlot.transform.GetChild(0).transform.GetChild(3).GetComponent<Image>().fillAmount = durationKF;
            }
        }
        if (recalculating)
            BuffsManager.instance.RecountBuffs();

    }
}
