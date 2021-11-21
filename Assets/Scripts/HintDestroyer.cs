using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HintDestroyer : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Destroy(GameObject.Find("ItemHint(Clone)"));
        Destroy(GameObject.Find("BuffHint(Clone)"));
    }
    
}
