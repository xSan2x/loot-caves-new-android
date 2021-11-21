using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GameLib;

public class ItemOnClick : MonoBehaviour, IPointerClickHandler
{
    public void Start()
    {

    }
    public void UseThisItem()
    {
        gameObject.GetComponent<Item>().UseItem();
        Destroy(GameObject.Find("ItemHint(Clone)"));
    }
    public void SellThisItem()
    {
        gameObject.GetComponent<Item>().SellItem();
        Destroy(GameObject.Find("ItemHint(Clone)"));
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject canv = BuffsManager.instance.mainCanvas.gameObject;
        Destroy(GameObject.Find("ItemHint(Clone)"));
        GameObject newHint = Instantiate(AbilityCollector.instance.itemHint, canv.transform);
        GameObject.Find("UseButton").GetComponent<Button>().onClick.AddListener(UseThisItem);
        GameObject.Find("SellButton").GetComponent<Button>().onClick.AddListener(SellThisItem);
        if (gameObject.GetComponent<Wearable>() != null)
        {
            Wearable stats = gameObject.GetComponent<Wearable>();
            newHint.transform.GetChild(0).GetComponent<Text>().text = stats.name;
            newHint.transform.GetChild(0).GetComponent<Text>().color = Evaluator.rarityColor[(int)stats.rarity];
            newHint.transform.GetChild(1).GetComponent<Text>().text = Evaluator.longAttributes[System.Array.IndexOf(Evaluator.shortAttributes,stats.zeroAttr)]+" : "+Evaluator.CheckFloat(stats.zeroAmount);
            newHint.transform.GetChild(2).GetComponent<Text>().text = "Level: " + stats.level;
            if(stats.rarity > 0)
            {
                newHint.transform.GetChild(3).GetComponent<Text>().text = stats.GetSecondaryStats();
                newHint.transform.GetChild(4).GetComponent<Text>().text = stats.ability.GetDescription();
                newHint.transform.GetChild(4).GetComponent<Text>().color = Evaluator.rarityColor[(int)stats.rarity]; 
            } else
            {
                newHint.transform.GetChild(3).GetComponent<Text>().text = "";
                newHint.transform.GetChild(4).GetComponent<Text>().text = "";
            }
            
        }
        float xP = 0.8f;
        float yP = 0.5f;
        if (gameObject.transform.parent.GetComponent<RectTransform>().anchoredPosition.x < 150 && gameObject.transform.parent.name!="Shield" && gameObject.transform.parent.name != "Necklace")
        {
            xP = 0.1f;
        }
        if (gameObject.transform.parent.GetComponent<RectTransform>().anchoredPosition.y > -100)
        {
            yP = 0.8f;
        }
        newHint.GetComponent<RectTransform>().pivot = new Vector2(xP, yP);
        newHint.transform.position = gameObject.transform.position;
    }
}
