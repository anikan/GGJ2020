using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The Boulder is a dangerous obstacle. It doesn't move, but it will slice through your ship
 */
[RequireComponent(typeof(BoxCollider2D))]
// [RequireComponent(typeof(MeshRenderer))]
// [RequireComponent(typeof(MeshFilter))]
public class Boulder : Damaging
{
    public List<GameObject> meshes = null;
    private GameObject boulder = null;

    public void Awake()
    {
        // Select a random Boulder mesh
        boulder = meshes[Random.Range(0, meshes.Count)];
        boulder = Instantiate(boulder);
        boulder.transform.SetParent(this.transform);
        boulder.transform.localPosition = Vector3.zero;

        MeshFilter meshFilter = boulder.GetComponent<MeshFilter>();
        this.GetComponent<MeshRenderer>().enabled = false;

        /*
        this.transform.rotation = Quaternion.Euler(270, 0, 0);
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = selectedBoulder.GetComponent<MeshFilter>().sharedMesh;
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = selectedBoulder.GetComponent<MeshRenderer>().sharedMaterial;
        */

        // Adjust BoxCollider2D to the boulder mesh size
        Bounds meshBounds = meshFilter.mesh.bounds;
        Debug.Log(meshBounds.extents);
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.offset = new Vector2(meshBounds.center.x, meshBounds.center.z);
        collider.size = new Vector2(meshBounds.extents.x, meshBounds.extents.z) * 2;
    }
}
