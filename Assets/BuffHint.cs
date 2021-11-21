using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffHint : MonoBehaviour
{
    public Item item;
    // Start is called before the first frame update
    void Start()
    {
        BuffsManager.instance.currentBuffHint = gameObject;
    }

    public void Redraw()
    {
        transform.GetChild(0).GetComponent<Text>().text = item.ability.GetBuffDescription();
    }

    public void DropBuff()
    {
        Destroy(gameObject);
    }
}
