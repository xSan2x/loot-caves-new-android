using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBuff 
{
    public float duration;
    public float maxDuration;
    public string actionDuration;
    public Ability ability;
    public string itemName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Drop()
    {
        ability.ExitTrue();
    }
}
