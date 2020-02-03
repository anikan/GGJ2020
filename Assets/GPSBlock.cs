using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class GPSBlock : Block
{
    public TextMeshProUGUI kilometerText;
    public TextMeshProUGUI goalText;
    public float cameraIncrease = 5.0f;

    private bool turnedOn = true;

    // Start is called before the first frame update
    void Start()
    {
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
        float kmElapsed = TopDownLazyFollow.gameCamera.GetDistanceTraveled();
        kilometerText.text = kmElapsed.ToString("F1");

        int goal = Mathf.FloorToInt(TopDownLazyFollow.gameCamera.winMeters);

        goalText.text = string.Format("{0}km", goal);

        if (this.hp <= 0.0f && turnedOn)
        {
            turnedOn = false;
            foreach (TextMeshProUGUI textMesh in GetComponentsInChildren<TextMeshProUGUI>())
            {
                textMesh.enabled = turnedOn;
            }
            TopDownLazyFollow.gameCamera.steeringViewYOffset -= turnedOn ? cameraIncrease : -cameraIncrease;
        }
    }
}
