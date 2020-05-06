using UnityEngine;
using System.Collections.Generic;
/**
 * This class is attached to the Ground object in GameAction scene that handles
 * all of the tile logic.
 */

public class Ground : MonoBehaviour {

    private Grid currentGrid;
    // these references are assigned in the script for convenience
    public GameObject startTile;
    public GameObject endTile;
    public GameObject[,] tiles;
    // position and orientation of all of the waypoint GameObjects
    public List<GameObject> waypointTransforms;

    // variables to be configured in unity
    public int gridWidth;
    public int gridLength;
    public float tileWidth;
    public float tileLength;
    public float tileHeight;
    // world pos of the top left-most point, default to (0,0)
    public float topLeftMostX;
    public float topLeftMostZ;
    public float arialCameraHeight;
    public GameObject sceneController;
    public GameObject player;

    public Camera arialViewCamera;

    public static readonly string BorderTexture = "Environment/TerrainAssets/SurfaceTextures/GrassRockyAlbedo";
    public static readonly string PathTexture = "Environment/TerrainAssets/SurfaceTextures/SandAlbedo";
    public static readonly string TerrainTexture = "Environment/TerrainAssets/SurfaceTextures/GrassHillAlbedo";
    public static readonly string BuildableTileTexture = "Environment/TerrainAssets/SurfaceTextures/MudRockyAlbedoSpecular";

    void Start () {
        AssertCorrectConfiguration();
        InitializeVariables();
        PositionCameraForArialView();

        currentGrid.GeneratePath(GameState.Difficulty.easy);
        DrawTiles();
    }

    void Update () {

    }

    /**
     * Checks that the public variables are initialized correctly.
     */
    private void AssertCorrectConfiguration() {
        Debug.Assert(gridWidth > 1);
        Debug.Assert(gridLength > 1);
        Debug.Assert(tileWidth > 1.0f);
        Debug.Assert(tileLength > 1.0f);
    }

    /**
     * Initialize all of the private member variables.
     */
    private void InitializeVariables() {
        currentGrid = new Grid(gridWidth, gridLength);
    }

    /**
     * Position the arial view camera to give us a good view.
     */
    private void PositionCameraForArialView() {
        Camera camera = arialViewCamera;
        float fieldWidth = tileWidth * currentGrid.Width;
        float fieldLength = tileLength * currentGrid.Length;
        camera.transform.position = new Vector3(fieldLength / 2.0f - 4f, arialCameraHeight, fieldWidth / 2.0f - 2f);
        camera.transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
    }

    /**
     * Generate all of GameObjects from currentGrid.
     */
    public void DrawTiles() {
        Grid grid = currentGrid;
        tiles = new GameObject[grid.Width,grid.Length];
        List<Grid.Position> waypoints = grid.Waypoints;
        Grid.Position currentPos = new Grid.Position(0, 0);
        bool isWaypoint = false;
        Grid.TileType currentTile;

        Vector3 tileScale = new Vector3(tileWidth, tileHeight, tileLength);

        for (int x = 0; x < grid.Width; x++) { // along x-axis in the world
            for (int z = 0; z < grid.Length; z++) { // along z-axis in the world
                isWaypoint = false;
                currentPos.UpdatePosition(x, z);
                currentTile = grid.At(x,z);

                // figure out if current position is a waypoint if on path
                if (currentTile == Grid.TileType.path || currentTile == Grid.TileType.end) {
                    foreach(Grid.Position p in waypoints) {
                        if (p == currentPos) {
                            isWaypoint = true;
                            break;
                        }
                    }
                }

                Vector3 worldPosition = new Vector3(x*tileWidth, 0, z*tileLength);
                // Vector3 worldPosition = new Vector3(z*tileLength, 0, x*tileWidth);
                GameObject tile;

                switch(currentTile) {
                    case Grid.TileType.terrain:
                        tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        tile.name = "Terrain-" + x + "," + z;
                        tile.tag = "TerrainTile";

                        tile.GetComponent<Renderer>().material.mainTexture =
                            Resources.Load<Texture2D>(TerrainTexture);
                        break;
                    case Grid.TileType.path:
                        tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        tile.name = "Path-" + x + "," + z;
                        tile.tag = "PathTile";

                        tile.GetComponent<Renderer>().material.mainTexture =
                            Resources.Load<Texture2D>(PathTexture);
                        break;
                    case Grid.TileType.start:
                        tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        startTile = tile;
                        tile.name = "Start-" + x + "," + z;
                        tile.tag = "StartTile";
                        tile.GetComponent<Renderer>().material.color = new Color(0.0f, 0.5f, 0.0f);

                        tile.AddComponent<SpawnPoint>();
                        tile.GetComponent<SpawnPoint>().ground = this;
                        tile.GetComponent<SpawnPoint>().player = player;
                        break;
                    case Grid.TileType.end:
                        tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        endTile = tile;
                        tile.name = "End-" + x + "," + z;
                        tile.tag = "EndTile";
                        tile.GetComponent<Renderer>().material.color = new Color(0.5f, 0.0f, 0.0f);
                        break;
                    case Grid.TileType.border:
                        tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        tile.name = "Border-" + x + "," + z;
                        tile.tag = "BorderTile";

                        tile.AddComponent<Border>();
                        tile.GetComponent<Renderer>().material.mainTexture =
                            Resources.Load<Texture2D>(BorderTexture);
                        break;
                    default:
                        Debug.LogError("Unrecognized tile type: " + grid.At(x,z).ToString());
                        return;
                }

                 // setting this as child of ground object
                tile.transform.parent = transform;
                // update object position in world space
                tile.transform.localScale = tileScale;
                tile.transform.position = worldPosition;
                // add Tile component and update its internal state
                tile.AddComponent<Tile>();
                tile.GetComponent<Tile>().GridLocX = x;
                tile.GetComponent<Tile>().GridLocY = z;
                tile.GetComponent<Tile>().IsWaypoint = isWaypoint;
                // add it to the tiles variable to keep track it
                tiles[x, z] = tile;
            }
        }

        // save the waypoints transformations so mobs can follow
        foreach (Grid.Position w in waypoints) {
            GameObject tile = tiles[w.X, w.Y];
            Debug.Assert(tile.name.Contains("Path") || tile.name.Contains("End"));
            Debug.Assert(tile.GetComponent<Tile>().IsWaypoint);
            waypointTransforms.Add(tile.GetComponent<Tile>().waypoint);
        }


        // identify neighboring tiles as buildable
        for (int x = 0; x < grid.Width; x++) {
            for (int z = 0; z < grid.Length; z++) {
                currentPos.UpdatePosition(x, z);
                GameObject tile = tiles[currentPos.X, currentPos.Y];

                if (!tile.tag.Contains("Path")) {
                    continue;
                }

                List<Grid.Position> neighbors = currentPos
                    .BuildNeighbors()
                    .FindAll(n => !n.OutOfBound(grid));

                foreach (Grid.Position n in neighbors)  {
                    GameObject neighbor = tiles[n.X, n.Y];
                    if (neighbor.tag.Contains("Terrain")) {
                        neighbor.GetComponent<Tile>().IsBuildable = true;

                        neighbor.GetComponent<Renderer>().material.mainTexture =
                            Resources.Load<Texture2D>("Environment/TerrainAssets/SurfaceTextures/MudRockyAlbedoSpecular");
                    }
                }
            }
        }
    }
}
