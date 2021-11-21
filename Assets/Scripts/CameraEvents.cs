using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class CameraEvents : MonoBehaviour
{
    public Tilemap tilemap;
    Tile newTile;
    public Image crossfade;

    readonly string[] bTypes = new string[8] { "acidic", "black_cobalt", "bog_green", "cage", "cobble_blood", "crypt", "crystal_floor", "demonic_red"};
    readonly int[] bCount = new int[8] { 4, 12, 8, 6, 24, 12, 6, 9 };

    private void Start()
    {
        CrossfadeIn();
        LevelManager.instance.OnLevelStarted += CrossfadeIn;
        LevelManager.instance.OnLevelFinished += CrossfadeOut;
        Hero.instance.OnDeath += HeroCrossfadeOut;
    }
    void ChangeBackground()
    {
        int bg = Random.Range(0, 8);
        for (var i = -9; i < 0; i++)
        {
            for (var j = -6; j < 70; j++)
            {
                int rnd = Random.Range(1, bCount[bg]);
                newTile = Resources.Load<Tile>("dungeon/" + bTypes[bg] + " (" + rnd + ")");
                tilemap.SetTile(new Vector3Int(j, i, 0), newTile);
            }
        }
    }

    public void CrossfadeIn()
    {
        ChangeBackground();
        Camera.main.transform.position = new Vector3(0, 0, -10);
        crossfade.GetComponent<Animator>().SetBool("In", true);
        crossfade.GetComponent<Animator>().SetBool("Out", false);
    }

    public void CrossfadeOut(int num)
    {
        crossfade.GetComponent<Animator>().speed = 1;
        crossfade.GetComponent<Animator>().SetBool("In", false);
        crossfade.GetComponent<Animator>().SetBool("Out", true);
        crossfade.transform.GetChild(0).GetComponent<Text>().text = "Level complete!";
    }
    public void HeroCrossfadeOut()
    {
        crossfade.GetComponent<Animator>().speed = 2;
        crossfade.GetComponent<Animator>().SetBool("In", false);
        crossfade.GetComponent<Animator>().SetBool("Out", true);
        crossfade.transform.GetChild(0).GetComponent<Text>().text = "Unsuccess! Going back.";
    }
}
