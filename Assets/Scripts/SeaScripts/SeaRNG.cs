using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RNGEntry
{
    public GameObject objectToSpawn;
    public float probability;
}
public class SeaRNG : MonoBehaviour
{
    public List<RNGEntry> startSpawnTable;
    public List<RNGEntry> quarterSpawnTable;
    public List<RNGEntry> midSpawnTable;
    public List<RNGEntry> endSpawnTable;

    List<RNGEntry> currentTable;

    // Start is called before the first frame update
    void Start()
    {
        currentTable = startSpawnTable;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnObject()
    {
        float randomVal = Random.Range(0, 1.0f);

        float rangeSoFar = 0;

        foreach (RNGEntry entry in currentTable)
        {
            rangeSoFar += entry.probability;

            if (randomVal > rangeSoFar)
            {
                GameObject spawnedObject = GameObject.Instantiate<GameObject>(entry.objectToSpawn);

                spawnedObject.transform.position = new Vector3(GetRandomHorizontalPosition(), BlockPrefabs.instance.playerRef.transform.position.y);
            }
        }
    }

    float GetRandomHorizontalPosition()
    {
        return Random.Range(-100, 100);
    }

    IEnumerator SpawnItem()
    {
        yield return null;
    }
}
