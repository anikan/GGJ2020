using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class GPSBlock : Block
{
    public TextMeshProUGUI kilometerText;
    public float unityToFakeMeterScale = 1.0f;
    public Transform boatCenter;

    private float startingYValue = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        startingYValue = boatCenter.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float kmElapsed = unityToFakeMeterScale * (boatCenter.position.y - startingYValue);

        kilometerText.text = kmElapsed.ToString("F1");


    }
}
