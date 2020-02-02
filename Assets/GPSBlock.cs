using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class GPSBlock : Block
{
    public TextMeshProUGUI kilometerText;
    private const float unityToFakeMeterScale = 0.01f;

    public static float startingYValue = -1000.0f;

    private Transform boatCenter;
    public float cameraIncrease = 5.0f;

    private bool turnedOn = true;

    // Start is called before the first frame update
    void Start()
    {
        if (boatCenter == null)
        {
            boatCenter = BlockPrefabs.instance.playerRef.boat.transform;
        }
        if (startingYValue < -100.0f)
        {
            startingYValue = boatCenter.position.y;
        }

        TopDownLazyFollow.gameCamera.steeringViewYOffset -= cameraIncrease;
    }

    public override void OnUse(Player player)
    {
        base.OnUse(player);
        turnedOn = !turnedOn;
        foreach (TextMeshProUGUI textMesh in GetComponentsInChildren<TextMeshProUGUI>())
        {
            textMesh.enabled = turnedOn;
        }
        TopDownLazyFollow.gameCamera.steeringViewYOffset -= turnedOn ? cameraIncrease : -cameraIncrease;
    }

    // Update is called once per frame
    void Update()
    {
        if (boatCenter == null)
        {
            boatCenter = BlockPrefabs.instance.playerRef.boat.transform;
        }

        float kmElapsed = unityToFakeMeterScale * (boatCenter.position.y - startingYValue);
        kilometerText.text = kmElapsed.ToString("F1");
    }
}
