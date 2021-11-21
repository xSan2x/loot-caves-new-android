using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCollector : MonoBehaviour
{
    public static AbilityCollector instance = null; //Singleton object variable
    public List<AbilityPattern> abilityPatterns;
    public GameObject itemHint;
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
}
