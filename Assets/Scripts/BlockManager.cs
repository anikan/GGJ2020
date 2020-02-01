using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    // The grid that holds the blocks
    private Dictionary<Vector2Int, Block> grid;

    // Dimension of the blocks
    private Vector2 blockDimension = Vector2.one;

    // Add every prefabricated block
    public void Awake()
    {
        grid = new Dictionary<Vector2Int, Block>();
        var blocks = GetComponentsInChildren<Block>();
        foreach(Block block in blocks)
        {
            SudoAddBlock(block, block.transform.position);
        }
    }

    // Checks the north, south, west, and east locations for a neighbor
    private bool CanPlaceBlockHere(Vector2Int index)
    {
        // if it's empty, check if it has neighbors we can attach to
        if(!grid.ContainsKey(index))
        {
            Vector2Int north = new Vector2Int(index.x, index.y + 1);
            Vector2Int south = new Vector2Int(index.x, index.y - 1);
            Vector2Int east = new Vector2Int(index.x - 1, index.y);
            Vector2Int west = new Vector2Int(index.x + 1, index.y);

            bool isNorthGood = grid.ContainsKey(north) && grid[north].isSafeToAttachTo;
            bool isSouthGood = grid.ContainsKey(south) && grid[south].isSafeToAttachTo;
            bool isEastGood = grid.ContainsKey(east) && grid[east].isSafeToAttachTo;
            bool isWestGood = grid.ContainsKey(west) && grid[west].isSafeToAttachTo;

            return (isNorthGood || isSouthGood || isEastGood || isWestGood);
        }

        return false;
    }

    // Forcefully adds a block without any checks
    private bool SudoAddBlock(Block block, Vector2 worldPosition)
    {
        // Convert the world position to the local grid index
        Vector2 gridPosition = worldPosition - new Vector2(this.transform.position.x, this.transform.position.y);
        gridPosition = new Vector2(gridPosition.x / blockDimension.x, gridPosition.y / blockDimension.y);
        Vector2Int gridIndex = new Vector2Int((int)gridPosition.x, (int)gridPosition.y);

        // Add it to the grid
        grid[gridIndex] = block;
        block.transform.position = new Vector3(gridIndex.x, gridIndex.y, 0);
        block.transform.SetParent(this.transform);
        return true;
    }
    public bool AddBlock(Block block, Vector2 worldPosition)
    {
        // Convert the world position to the local grid index
        Vector2 gridPosition = worldPosition - new Vector2(this.transform.position.x, this.transform.position.y);
        gridPosition = new Vector2(gridPosition.x / blockDimension.x, gridPosition.y / blockDimension.y);
        Vector2Int gridIndex = new Vector2Int((int)gridPosition.x, (int)gridPosition.y);

        // Check if there is an undamaged neighbor that we can attach to
        if(CanPlaceBlockHere(gridIndex))
        {
            // Add it to the grid
            grid[gridIndex] = block;
            block.transform.position = new Vector3(gridIndex.x, gridIndex.y, 0);
            block.transform.SetParent(this.transform);

            return true;
        }
        return false;
    }
}
