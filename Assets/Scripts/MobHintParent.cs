using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MobHintParent : MonoBehaviour
{
    public GameObject hintPrefab;

    
    private void Start()
    {
        Hero.instance.OnMissedAttack += CreateEvadeHint;
    }
    public void CreateHint(string hintText,Color hintColor)
    {
        GameObject newHint = Instantiate(hintPrefab, transform);
        TextMeshProUGUI mesh = newHint.GetComponent<TextMeshProUGUI>();
        mesh.text = hintText;
        mesh.faceColor = hintColor;
    }
    public void CreateEvadeHint()
    {
        GameObject newHint = Instantiate(hintPrefab, transform);
        TextMeshProUGUI mesh = newHint.GetComponent<TextMeshProUGUI>();
        mesh.text = "Miss!";
    }
    
}
