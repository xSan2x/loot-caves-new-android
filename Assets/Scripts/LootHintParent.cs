using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LootHintParent : MonoBehaviour
{
    public GameObject hintPrefab;
    public Animator lootAnimator;
    public static LootHintParent instance = null;

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
    private void Start()
    {
        
    }
    public void CreateHint(string hintText, Color hintColor)
    {
        GameObject newHint = Instantiate(hintPrefab, transform);
        TextMeshProUGUI mesh = newHint.GetComponent<TextMeshProUGUI>();
        mesh.text = hintText;
        mesh.faceColor = hintColor;
    }
    public void MobDeathHint(int gold, int exp)
    {
        StartCoroutine(GetLoot(gold, exp));
    }
    IEnumerator GetLoot(int gold, int exp)
    {
        lootAnimator.SetBool("Gold", true);
        CreateHint("+" + exp + " EXP", new Color(0.5f, 0.5f, 0.5f));
        yield return new WaitForSeconds(0.2f);
        CreateHint("+" + gold + " gold", new Color32(255, 215, 0, 255));
        lootAnimator.SetBool("Gold", false);
    }
}
