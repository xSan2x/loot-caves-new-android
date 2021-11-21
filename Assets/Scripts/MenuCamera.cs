using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MenuCamera : MonoBehaviour
{
    public Tilemap tilemap;
    Tile newTile;

    private void Start()
    {
        ChangeBackground();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0, 0.1f));
    }

    void ChangeBackground()
    {
        int mobCount = 897;

        int bg = Random.Range(0, mobCount);
        for (var i = -11; i < 12; i++)
        {
            for (var j = -11; j < 12; j++)
            {
                int rnd = Random.Range(1, bg);
                newTile = Resources.Load<Tile>("mobs/mob (" + rnd + ")");
                tilemap.SetTile(new Vector3Int(j, i, 0), newTile);
            }
        }
    }
}
