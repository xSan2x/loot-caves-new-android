using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameLib;

public class HeroHintParent : MonoBehaviour
{
    public GameObject hintPrefab;


    private void Start()
    {
        Hero.instance.OnEvade += CreateEvadeHint;
        Hero.instance.OnHeal += CreateHealHint;
    }
    public void CreateHint(string hintText, Color hintColor)
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
    public void CreateHealHint(int heal)
    {
        CreateHint("+" + Evaluator.CheckInt(heal), new Color(0, 196, 0));
    }
}
