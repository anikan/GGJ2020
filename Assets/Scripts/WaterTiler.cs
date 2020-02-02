using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTiler : MonoBehaviour
{
    public GameObject waterTile;
    public float tileWidth;
    public float totalHorizontalTiles;

    // Start is called before the first frame update
    void Start()
    {
        CreateTiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateTiles()
    {
        float startValue = (totalHorizontalTiles * tileWidth) / 2.0f;

        for (float i = -startValue; i < startValue; i+=tileWidth)
        {
            for (float y = -startValue; y < startValue; y+=tileWidth)
            {
                GameObject newTile = GameObject.Instantiate(waterTile);
                newTile.transform.position = new Vector3(i, y, 2);
                newTile.transform.parent = this.transform;
            }
        }
    }
}
