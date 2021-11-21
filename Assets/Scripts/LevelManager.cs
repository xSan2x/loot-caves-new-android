using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLib;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null; //Singleton object variable
    MobsContainer mobsCont;
    //Camera mainCamera;

    public bool lvlIsStarted = false;
    public int currLvl = 0;
    private int currLvlCheckpoint = 1;

    //delegates
    public delegate void StartLevelMethod();
    public delegate void FinishLevelMethod(int lvl);

    //events
    public event StartLevelMethod OnLevelStarted;
    public event FinishLevelMethod OnLevelFinished;

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
        mobsCont = MobsContainer.instance;
        Hero.instance.OnDeath += ReturnToPreviousLevel;
    }

    public void firstInit(int level)
    {
        currLvl = level;
        currLvlCheckpoint = CheckpointCalc(level);
        InitLvl(currLvlCheckpoint);
    }
    int CheckpointCalc(int lvl)
    {
        int checkpoint = 1;
        int dozens = 0;
        while (lvl > 10)
        {
            lvl -= 10;
            dozens++;
        }
        if (dozens != 0)
        {
            checkpoint = (10 * dozens)+1;
        }
        return checkpoint;
    }
    void InitLvl(int lvl)
    {
        mobsCont.InitMobs(lvl);
        if (OnLevelStarted != null)
            OnLevelStarted();
        //Clear level modificators
        StartCoroutine(InitDelay());
    }
    public void LevelFinished()
    {
        lvlIsStarted = false;
        OnLevelFinished(currLvl);
        StartCoroutine(StartNextLevel());
    }
    void ReturnToPreviousLevel()
    {
        lvlIsStarted = false;
        StartCoroutine(StartPreviousLevel());
    }
    IEnumerator StartNextLevel()
    {
        yield return new WaitForSeconds(1f);
        currLvl++;
        currLvlCheckpoint = CheckpointCalc(currLvl);
        InitLvl(currLvl);
    }
    IEnumerator StartPreviousLevel()
    {
        yield return new WaitForSeconds(1f);
        if(currLvl> currLvlCheckpoint)
            currLvl--;
        InitLvl(currLvl);
    }
    IEnumerator InitDelay()
    {
        yield return new WaitForSeconds(1f);
        lvlIsStarted = true;
        mobsCont.Restart();
    }
}
