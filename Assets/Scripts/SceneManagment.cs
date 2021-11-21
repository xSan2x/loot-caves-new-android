using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLib;

public class SceneManagment : MonoBehaviour
{
    Hero hero;
    LevelManager levelManage;
    Equipment equipment;
    Inventory inventory;
    GameSettings gameSettings;
    MobsContainer mobs;
    public GameObject wearableDefault;
    public GameObject itemsBuffer;
    float regenerationTick = 0f;
    // Start is called before the first frame update
    void Start()
    {
        hero = Hero.instance;
        levelManage = LevelManager.instance;
        equipment = Equipment.instance;
        inventory = Inventory.instance;
        gameSettings = GameSettings.instance;
        mobs = MobsContainer.instance;
        LoadDataFromFile();
        levelManage.OnLevelFinished += RewardItem;
        hero.OnDeath += HeroDeath;
        RewardItem(1);
        RewardItem(1);
        RewardItem(1);
        RewardItem(1);
        RewardItem(1);
        RewardItem(1);
        RewardItem(1);
        RewardItem(1);
        RewardItem(1);
        RewardItem(1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (levelManage.lvlIsStarted) // check if lvl is started in level manager
        {
            if (mobs.currentMob.isAlive()) //checks current mob is alive?
            {
                //fight the mob
                //calculate hero attack cooldown from his speed rate
                hero.maxAttackCooldown = mobs.currentMob.speedRate/(float)hero.speedRate;
                mobs.currentMob.maxAttackCooldown = hero.speedRate / (float)mobs.currentMob.speedRate;
                if (hero.attackCooldown > hero.maxAttackCooldown) //check is the hero ready to attack
                {
                    hero.Attack(mobs.currentMob);
                    
                    hero.attackCooldown = 0;
                } else
                {
                    hero.attackCooldown += Time.fixedDeltaTime;
                }
                if (mobs.currentMob.attackCooldown > mobs.currentMob.maxAttackCooldown) //check is the mob ready to attack
                {
                    mobs.currentMob.Attack(hero);

                    mobs.currentMob.attackCooldown = 0;
                }
                else
                {
                    mobs.currentMob.attackCooldown += Time.fixedDeltaTime;
                }
            }
            if (regenerationTick >= 1)
            {
                hero.Regenerate();
                regenerationTick = 0;
            } else
            {
                regenerationTick += Time.fixedDeltaTime;
            }
        }
    }
    public void SaveDataToFile()
    {
        SaveLoadSystem.SaveData();
    }
    public void LoadDataFromFile()
    {
        SaveData data = SaveLoadSystem.LoadData();
        if (data == null)
        {
            Debug.Log("Can`t find save file!");
            hero.level = 1;
            levelManage.currLvl = 1;
            inventory.inventorySize = 20;
            hero.exp = 0;
            hero.gold = 0;
            gameSettings.gameDifficulty = 0;
            gameSettings.kfAtkRaise = 1.12f;
            gameSettings.kfExpGather = 1.17f;
            SaveDataToFile();
            LoadDataFromFile();
            return;
        }

        //GameDifficulty change kf`s
        Debug.Log("Saved data found!");
        gameSettings.gameDifficulty = data.difficulty;
        switch (data.difficulty)
        {
            case 0:
                gameSettings.kfAtkRaise = 1.12f;
                gameSettings.kfExpGather = 1.17f;
                break;
            case 1:
                gameSettings.kfAtkRaise = 1.122f;
                gameSettings.kfExpGather = 1.165f;
                break;
            case 2:
                gameSettings.kfAtkRaise = 1.1235f;
                gameSettings.kfExpGather = 1.16f;
                break;
            case 3:
                gameSettings.kfAtkRaise = 1.125f;
                gameSettings.kfExpGather = 1.155f;
                break;
        }
        //Hero init
        hero.level = data.heroLvl;
        inventory.inventorySize = data.inventorySize;
        hero.exp = data.exp;
        hero.gold = data.gold;
        hero.Initialitation();
        levelManage.firstInit(data.caveLvl);
    }
    void RewardItem(int lvl)
    {
        StartCoroutine(RewardItemCor(lvl));
    }
    void HeroDeath()
    {

    }
    IEnumerator RewardItemCor(int lvl)
    {
        GameObject newItem = Instantiate(wearableDefault, itemsBuffer.transform);
        newItem.GetComponent<Wearable>().InitWearable(lvl);
        yield return new WaitForSeconds(0.2f);
        LootHintParent.instance.CreateHint("+" + Evaluator.rarityStr[(int)newItem.GetComponent<Wearable>().rarity] + " item", Evaluator.rarityColor[(int)newItem.GetComponent<Wearable>().rarity]);
        inventory.GiveItemToInventory(newItem.GetComponent<Wearable>());
    }

}
