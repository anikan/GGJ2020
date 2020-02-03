using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class TextColorChanger : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    [RangeAttribute(0.01f, 2.0f)]
    public float changeDuration = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeToRandomColor());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ChangeToRandomColor()
    {
        changeDuration = Mathf.Clamp(changeDuration, 0.1f, changeDuration);
        Color startColor = textMesh.color;
        Color endColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));

        for (float i = 0; i < changeDuration; i+=Time.deltaTime)
        {
            textMesh.color = Color.Lerp(startColor, endColor, i / changeDuration);
            yield return null;
        }
        textMesh.color = endColor;
        StartCoroutine(ChangeToRandomColor());
    }
}
