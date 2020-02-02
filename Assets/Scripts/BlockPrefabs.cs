using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPrefabs : MonoBehaviour
{
    public static BlockPrefabs instance;

    public GameObject needResourceIconPrefab;
    public GameObject blockPrefab;
    public Player playerRef;

    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
