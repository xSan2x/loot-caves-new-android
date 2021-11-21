using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance = null; //Singleton variable
    //inventory sizes
    public int inventorySize { get; set; }
    private int maxInventorySize = 100;
    private int sortBy = 0; // 0 - by level, 1 - by type, 2 - by cost
    //list of items in inventory
    public List<GameObject> inventory;
    //Grid for DrawItems
    public Transform content;
    public Transform itemsBuffer;

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

    public void GiveItemToInventory(Item item)
    {
        inventory.Add(item.gameObject);
        SortItems();
    }

    public void TakeAwayItemFromInventory(Item item)
    {
        inventory.Remove(item.gameObject);
        SortItems();
    }

    void SortItems()
    {
        switch (sortBy)
        {
            case 0:
                inventory.Sort(SortByLevel);
                break;
            case 1:
                inventory.Sort(SortByType);
                break;
            case 2:
                inventory.Sort(SortByCost);
                break;
            default:
                inventory.Sort(SortByLevel);
                break;
        }
        DrawItems();
    }
    static int SortByLevel(GameObject p1, GameObject p2)
    {
        return p1.GetComponent<Item>().level.CompareTo(p2.GetComponent<Item>().level);
    }
    static int SortByType(GameObject p1, GameObject p2)
    {
        return p1.GetComponent<Item>().type.CompareTo(p2.GetComponent<Item>().type);
    }
    static int SortByCost(GameObject p1, GameObject p2)
    {
        return p1.GetComponent<Item>().cost.CompareTo(p2.GetComponent<Item>().cost);
    }

    void DrawItems()
    {
        foreach (Transform child in content) //Clear the inventory grid
        {
            if (child.childCount > 0)
                child.GetChild(0).parent = itemsBuffer;
        }
        int i = 0;
        foreach (GameObject item in inventory)
        {
            item.transform.parent = content.GetChild(i);
            item.transform.localPosition = Vector3.zero;
            item.transform.localScale = Vector3.one;
            i++;
        }
    }
}
