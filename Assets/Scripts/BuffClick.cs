using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GameLib;

public class BuffClick : MonoBehaviour, IPointerClickHandler
{
    BuffsManager bm;
    public string buffDescription;
    public Item item;
    // Start is called before the first frame update
    void Start()
    {
        bm = BuffsManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject canv = BuffsManager.instance.mainCanvas.gameObject;
        Destroy(GameObject.Find("BuffHint(Clone)"));
        GameObject newHint = Instantiate(BuffsManager.instance.buffHint, canv.transform);
        newHint.transform.GetChild(0).GetComponent<Text>().text = buffDescription;
        float xP = 0.85f;
        float yP = -0.35f;
        if (gameObject.transform.GetComponent<RectTransform>().anchoredPosition.x < 150)
        {
            xP = 0.15f;
        }
        newHint.GetComponent<RectTransform>().pivot = new Vector2(xP, yP);
        newHint.transform.position = gameObject.transform.position;
        newHint.GetComponent<BuffHint>().item = item;
        newHint.GetComponent<BuffHint>().Redraw();
    }


}
