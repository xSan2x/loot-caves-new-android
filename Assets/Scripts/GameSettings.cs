using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    public static GameSettings instance = null; //Singleton variable

    //for difficulty - 0-easy;1-medium;2-hard;3-insane;
    public int gameDifficulty;
    public readonly float kfStats = 1.1f;
    public readonly float kfEnch = 1.2f;
    public readonly float kfNeedExp = 1.2f;
    public float kfAtkRaise;
    public float kfExpGather;
    public int maxMobIndex = 897;
    public Dictionary<string, int> maxWearableIndex = new Dictionary<string, int>(8);

    public CanvasGroup invPanel;
    public CanvasGroup statsPanel;
    public CanvasGroup equipPanel;

    public AbilityCollector abilityCollector;
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
        maxWearableIndex["helmet"] = 29;
        maxWearableIndex["fullarmor"] = 83;
        maxWearableIndex["gloves"] = 11;
        maxWearableIndex["boots"] = 8;
        maxWearableIndex["shields"] = 30;
        maxWearableIndex["weapons"] = 394;
        maxWearableIndex["ring"] = 49;
        maxWearableIndex["necklace"] = 41;
    }
    private void Start()
    {
        abilityCollector = AbilityCollector.instance;
    }

    public void SwitchPanel(CanvasGroup panelToSwitch)
    {
        invPanel.alpha = 0;
        invPanel.interactable = false;
        invPanel.blocksRaycasts = false;
        statsPanel.alpha = 0;
        statsPanel.interactable = false;
        statsPanel.blocksRaycasts = false;
        equipPanel.alpha = 0;
        equipPanel.interactable = false;
        equipPanel.blocksRaycasts = false;

        panelToSwitch.alpha = 1;
        panelToSwitch.interactable = true;
        panelToSwitch.blocksRaycasts = true;
        Destroy(GameObject.Find("ItemHint(Clone)"));
    }
}
