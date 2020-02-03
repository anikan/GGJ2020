using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BlockPrefabs))]
public class BlockManager : MonoBehaviour
{
    public Dictionary<Vector2Int, Block> grid; // the grid that holds the blocks
    private Vector2 blockDimension = Vector2.one; // dimension of the blocks (assuming 1x1)
    private Player player; // a reference to the player
    private KeyValuePair<bool, Vector2Int>[] doesDirectionHaveBlock = new KeyValuePair<bool, Vector2Int>[4]; // for pushing players to nearest safe block

    public void Awake()
    {
        grid = new Dictionary<Vector2Int, Block>();

        // Get a reference to the player from BlockPrefabs.cs
        player = this.GetComponent<BlockPrefabs>().playerRef;

        // If there are already children blocks in the BlockManager object, add them
        var blocks = GetComponentsInChildren<Block>();
        foreach(Block block in blocks)
        {
            SudoAddBlock(block, block.transform.position);

            block.OnAttach(this.transform);

        }
    }

    // Converts World Position to the internal grid position
    public Vector2Int GetGridIndex(Vector2 worldPosition)
    {
        // Scale it down to 1
        Vector2 gridPosition = worldPosition / this.transform.localScale;

        // Rotate in the opposite direction
        Vector2 origin = this.transform.position;
        gridPosition = gridPosition - origin;
        gridPosition = Quaternion.Euler(0, 0, -this.transform.rotation.eulerAngles.z) * gridPosition;

        // Translate it into grid space
        gridPosition = new Vector2(gridPosition.x / blockDimension.x, gridPosition.y / blockDimension.y);
        return new Vector2Int(Mathf.RoundToInt(gridPosition.x), Mathf.RoundToInt(gridPosition.y));
    }

    public Block GetBlockAtWorldPosition(Vector2 worldPosition)
    {
        Vector2Int gridPos = GetGridIndex(worldPosition);
        return grid.ContainsKey(gridPos) ? grid[gridPos] : null;
    }

    // Converts internal grid position to World Position
    public Vector2 GetWorldPosition(Vector2Int gridIndex)
    {
        // Translate it
        Vector2 worldPos = new Vector3(gridIndex.x, gridIndex.y, 0) + this.transform.position;

        // Rotate it
        worldPos = this.transform.rotation * worldPos;

        // Scale it
        worldPos = worldPos * this.transform.localScale;

        return worldPos;
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
        block.transform.position = this.transform.position;
        block.transform.position += this.transform.rotation * new Vector3(gridIndex.x, gridIndex.y, 0);
        block.transform.rotation = this.transform.rotation;
        block.transform.SetParent(this.transform);
        block.OnAttach(transform);

        return true;
    }

    // Adds a block according to the World Position
    public bool AddBlock(Block block, Vector2 worldPosition)
    {
        Vector2Int gridIndex = GetGridIndex(worldPosition);

        // Check if there is an undamaged neighbor that we can attach to
        if (CanPlaceBlockHere(gridIndex))
            return SudoAddBlock(block, worldPosition);

        return false;
    }

    // Removes the block at the world position, if a valid block is there
    // Returns null if there is no block there
    // NOTE: This will run a check and also disconnect all blocks!
    // How to determine which one is part of the ship: which side the user is standing on
    public Block RemoveBlock(Vector2 worldPosition)
    {
        Vector2Int gridIndex = GetGridIndex(worldPosition);
        if (!grid.ContainsKey(gridIndex)) return null;

        Block selectedBlock = grid[gridIndex];
        grid[gridIndex].transform.SetParent(null);
        grid.Remove(gridIndex);
        selectedBlock.OnStopUsing(player);

        // Move the player to the nearest block if the player is standing on the destroyed block
        Vector2Int playerGridIndex = GetGridIndex(player.transform.position);

        if(playerGridIndex == gridIndex)
        {
            // TEMPORARY: Assumes (0, 0) is steering wheel
            player.transform.position = GetWorldPosition(new Vector2Int(0, 0));

            /*
            // We know that the ship is a connected component, which means we can push the player to a neighboring block
            // After we grab the available neighbors, get their actual positions and find the one the player is closest to
            // Force the player to be on that position
            Vector2Int north = new Vector2Int(gridIndex.x, gridIndex.y + 1);
            Vector2Int south = new Vector2Int(gridIndex.x, gridIndex.y - 1);
            Vector2Int east = new Vector2Int(gridIndex.x - 1, gridIndex.y);
            Vector2Int west = new Vector2Int(gridIndex.x + 1, gridIndex.y);

            // Update array with whether or not a block is there
            doesDirectionHaveBlock[0] = new KeyValuePair<bool, Vector2Int>(grid.ContainsKey(north), north);
            doesDirectionHaveBlock[1] = new KeyValuePair<bool, Vector2Int>(grid.ContainsKey(south), south);
            doesDirectionHaveBlock[2] = new KeyValuePair<bool, Vector2Int>(grid.ContainsKey(east), east);
            doesDirectionHaveBlock[3] = new KeyValuePair<bool, Vector2Int>(grid.ContainsKey(west), west);

            // TODO: Push to nearest. Right now, just picking an arbtirary one
            for (int i = 0; i < doesDirectionHaveBlock.Length; ++i)
            {
                if (doesDirectionHaveBlock[i].Key)
                {
                    player.transform.position = GetWorldPosition(doesDirectionHaveBlock[i].Value);
                    break;
                }
            }
            */
        }

        RemoveDisconnectedBlocks(player.transform.position);
        return selectedBlock;
    }

    public bool IsPositionAvailable(Vector2 worldPosition)
    {
        return CanPlaceBlockHere(GetGridIndex(worldPosition));
    }

    // A block is considered disconnected if from the block the player is getting to
    // to the current block, it is unable to access it once the current block has been
    // removed.
    // I'm too lazy to read up on decremental connectivity of graphs, so I'll do this sketchily:
    // (1) Run BFS to find all connected components
    // (2) Toss the connected components that the player is not on
    // Runtime is O(N) for number of nodes
    private void RemoveDisconnectedBlocks(Vector2 playerWorldPosition)
    {
        // Convert player position to a grid index
        Vector2Int playerIndex = GetGridIndex(playerWorldPosition);

        // Something is VERY wrong if the player is not on a grid
        Debug.Assert(grid.ContainsKey(playerIndex));
        Block rootNode = grid[playerIndex];

        // Iterate through all blocks and set visited to false
        // Run BFS from the player position's block
        // Iterate through all blocks and unparent all those that still hasn't been visited
        var keys = new List<Vector2Int>(grid.Keys);
        foreach(var key in keys)
        {
            grid[key].visited = false;
        }

        // BFS
        Queue<KeyValuePair<Vector2Int, Block>> toVisit = new Queue<KeyValuePair<Vector2Int, Block>>();
        toVisit.Enqueue(new KeyValuePair<Vector2Int, Block>(playerIndex, rootNode));

        while(toVisit.Count > 0)
        {
            var currentNode = toVisit.Dequeue();
            currentNode.Value.visited = true;

            // Get the neighboring nodes
            Vector2Int north = new Vector2Int(currentNode.Key.x, currentNode.Key.y + 1);
            Vector2Int south = new Vector2Int(currentNode.Key.x, currentNode.Key.y - 1);
            Vector2Int east = new Vector2Int(currentNode.Key.x - 1, currentNode.Key.y);
            Vector2Int west = new Vector2Int(currentNode.Key.x + 1, currentNode.Key.y);

            if (grid.ContainsKey(north) && !grid[north].visited) toVisit.Enqueue(new KeyValuePair<Vector2Int, Block>(north, grid[north]));
            if (grid.ContainsKey(south) && !grid[south].visited) toVisit.Enqueue(new KeyValuePair<Vector2Int, Block>(south, grid[south]));
            if (grid.ContainsKey(east) && !grid[east].visited) toVisit.Enqueue(new KeyValuePair<Vector2Int, Block>(east, grid[east]));
            if (grid.ContainsKey(west) && !grid[west].visited) toVisit.Enqueue(new KeyValuePair<Vector2Int, Block>(west, grid[west]));
        }

        // iterate through all blocks and unparent those
        foreach (var key in keys)
        {
            if(!grid[key].visited)
            {
                grid[key].transform.SetParent(null);
                grid.Remove(key);
            }
        }
    }
}
