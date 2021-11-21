using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobsContainer : MonoBehaviour
{
    public static MobsContainer instance = null; //Singleton variable

    private List<Mob> mobs = new List<Mob>();
    public Mob currentMob;
    public GameObject mobPrefab;
    public int indexMob;
    public Text mobCountUI;
    private void Start()
    {
        Hero.instance.OnKillMob += MobDead;
        Hero.instance.OnDeath += MobClear;
    }
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
    }
    void MobClear()
    {
        GameObject[] mobics = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject go in mobics)
        {
            Destroy(go);
        }
    }
    public void InitMobs(int level)
    {
        indexMob = 0;
        mobs.Clear();
        for(var i = 0; i < 10; i++)
        {
            GameObject newMob = Instantiate(mobPrefab);
            newMob.transform.Translate(new Vector3(3 * i, 0));
            newMob.GetComponent<Mob>().Initialitation(level);
            mobs.Add(newMob.GetComponent<Mob>());
        }
        currentMob = mobs[0];
        for(int i=0;i<10;i++)
        {
            mobs[i].index = i;
        }
    }
    
    void MobDead()
    {
        mobs[indexMob].Death();
        mobs[indexMob] = null;
        mobCountUI.text = (indexMob+1)+"/10";
        if (indexMob == 9)
        {
            // next level
            LevelManager.instance.LevelFinished();
        } else
        {
            StartCoroutine(CameraMove());
        }
    }
    IEnumerator CameraMove()
    {
        yield return new WaitForSeconds(0.25f);
        float timeline = 0;
        float passedX = 0;
        while (timeline < 1f) // move camera to next mob for 1 second
        {
            yield return new WaitForFixedUpdate();
            passedX += Time.deltaTime * 3f;
            if(passedX<3)
                Camera.main.transform.Translate(new Vector3(Time.deltaTime*3f, 0));
            timeline += Time.deltaTime;
        }
        indexMob++;
        currentMob = mobs[indexMob];
        yield return null;
    }
    public void Restart()
    {
        indexMob = 0;
        mobCountUI.text = "0/10";
        currentMob = mobs[0];
    }
}
