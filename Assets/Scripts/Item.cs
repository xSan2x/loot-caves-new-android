using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public int level;
    public int imgIndex;
    public int cost;
    public string type;
    public string itemName;
    public GameSettings gs;
    public Ability ability;
    public GameObject buffSlot;

    private void Start()
    {
        gs = GameSettings.instance;
    }

    public virtual void UseItem()
    {
        Debug.Log("Called wrong!");
    }

    public virtual void SellItem()
    {

    }

}
