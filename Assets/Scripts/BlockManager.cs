using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    private Dictionary<Vector2Int, Block> grid; // the grid that holds the blocks
    private Vector2 blockDimension = Vector2.one; // dimension of the blocks (assuming 1x1)

    public void Awake()
    {
        grid = new Dictionary<Vector2Int, Block>();

        // If there are already children blocks in the BlockManager object, add them
        var blocks = GetComponentsInChildren<Block>();
        foreach(Block block in blocks)
        {
            SudoAddBlock(block, block.transform.position);
        }
    }

    // Converts World Position to the internal grid position
    private Vector2Int GetGridIndex(Vector2 worldPosition)
    {
        Vector2 gridPosition = worldPosition - new Vector2(this.transform.position.x, this.transform.position.y);
        gridPosition = new Vector2(gridPosition.x / blockDimension.x, gridPosition.y / blockDimension.y);
        return new Vector2Int((int)gridPosition.x, (int)gridPosition.y);
    }

    // Checks the north, south, west, and east locations for a neighbor
    private bool CanPlaceBlockHere(Vector2Int index)
    {
        // If it's empty, check if it has neighbors we can attach to
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
        Vector2Int gridIndex = GetGridIndex(worldPosition);
        grid[gridIndex] = block;
        block.transform.position = new Vector3(gridIndex.x, gridIndex.y, 0);
        block.transform.SetParent(this.transform);
        return true;
    }

    // Adds a block according to the World Position
    public bool AddBlock(Block block, Vector2 worldPosition)
    {
        Vector2Int gridIndex = GetGridIndex(worldPosition);

        // Check if there is an undamaged neighbor that we can attach to
        if (CanPlaceBlockHere(gridIndex))
        {
            // Add it to the grid
            grid[gridIndex] = block;
            block.transform.position = new Vector3(gridIndex.x, gridIndex.y, 0);
            block.transform.SetParent(this.transform);

            return true;
        }
        return false;
    }

    // Removes the block at the world position, if a valid block is there
    // Returns null if there is no block there
    // NOTE: This will run a check and also disconnect all blocks!
    // How to determine which one is part of the ship: which side the user is standing on
    public Block RemoveBlock(Vector2 worldPosition, Vector3 playerPosition)
    {
        Vector2Int gridIndex = GetGridIndex(worldPosition);
        if (!grid.ContainsKey(gridIndex)) return null;

        Block selectedBlock = grid[gridIndex];
        RemoveDisconnectedBlocks(gridIndex, playerPosition);
        return selectedBlock;
    }

    // A block is considered disconnected if from the block the player is getting to
    // to the current block, it is unable to access it once the current block has been
    // removed.
    // I'm too lazy to read up on decremental connectivity of graphs, so I'll do this sketchily:
    // (1) Run BFS to find all connected components
    // (2) Toss the connected components that the player is not on
    private void RemoveDisconnectedBlocks(Vector2Int gridIndex, Vector3 playerPosition)
    {
        // Convert player position to a grid index
        // TO DO
    }

}
