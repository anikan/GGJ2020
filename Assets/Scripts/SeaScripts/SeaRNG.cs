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

    public float timeToSpawn;

    public bool gameActive = true;

    Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        currentTable = startSpawnTable;
        mainCam = Camera.main;

        StartCoroutine(SpawnItem());
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

            if (rangeSoFar > randomVal)
            {
                GameObject spawnedObject = GameObject.Instantiate<GameObject>(entry.objectToSpawn);

                Vector3 playerPosition = BlockPrefabs.instance.playerRef.transform.position;

                spawnedObject.transform.position = playerPosition + GetRandomRelativePosition();
                spawnedObject.transform.parent = this.transform;

                return;
            }
        }
    }

    Vector3 GetRandomRelativePosition()
    {
        Vector3 topBoundaryPoint = mainCam.ViewportToWorldPoint(new Vector3(0.5f, 0.0f, mainCam.transform.position.z));
        return new Vector3(Random.Range(-100.0f, 100.0f), topBoundaryPoint.y + Random.Range(10.0f, 20.0f));
    }

    IEnumerator SpawnItem()
    {
        while (gameActive)
        {
            Debug.Log("SPAWNING");
            SpawnObject();

            float speed = Mathf.Max(.1f, BlockPrefabs.instance.playerRef.boat.GetComponent<Rigidbody2D>().velocity.magnitude);

            float minWaitTime = .1f;

            float waitTime = Mathf.Max(minWaitTime, timeToSpawn / speed);

            yield return new WaitForSeconds(waitTime);
        }
    }
}
