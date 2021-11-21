using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "AbilityPattern")]
public class AbilityPattern : ScriptableObject
{
        public bool active;
        public bool isStackable;
        public string[] attributes;
        public float[] attributesCount;
        public string triggerCondition;
        public string triggerFormula;
        public string exitCondition;
        public string exitFormula;
        public bool autoCast;
        public float cooldown;
        public string description;
        public string buffDescription;

    
}

