using UnityEngine;
/**
 * This class is attached to the Ground object in GameAction scene that handles
 * all of the tile logic.
 */

public class Ground : MonoBehaviour {

    private Grid currentGrid;

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

    public Camera arialViewCamera;

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
        Vector3 tileScale = new Vector3(tileWidth, tileHeight, tileLength);

        for (int x = 0; x < grid.Width; x++) { // along x-axis in the world
            for (int z = 0; z < grid.Length; z++) { // along z-axis in the world
                Vector3 worldPosition = new Vector3(x*tileWidth, 0, z*tileLength);
                // Vector3 worldPosition = new Vector3(z*tileLength, 0, x*tileWidth);
                GameObject tile;

                switch(grid.At(x,z)) {
                    case Grid.TileType.terrain:
                        tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        tile.name = "Terrain-" + x + "," + z;
                        tile.tag = "TerrainTile";
                        tile.GetComponent<Renderer>().material.color = new Color(0.0f, 0.0f, 0.2f);
                        break;
                    case Grid.TileType.path:
                        tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        tile.name = "Path-" + x + "," + z;
                        tile.tag = "PathTile";
                        tile.GetComponent<Renderer>().material.color = new Color(0.1f, 0.1f, 0.1f);
                        break;
                    case Grid.TileType.start:
                        tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        tile.name = "Start-" + x + "," + z;
                        tile.tag = "StartTile";
                        tile.GetComponent<Renderer>().material.color = new Color(0.0f, 0.5f, 0.0f);
                        break;
                    case Grid.TileType.end:
                        tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        tile.name = "End-" + x + "," + z;
                        tile.tag = "EndTile";
                        tile.GetComponent<Renderer>().material.color = new Color(0.5f, 0.0f, 0.0f);
                        break;
                    case Grid.TileType.border:
                        tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        tile.name = "Border-" + x + "," + z;
                        tile.tag = "BorderTile";
                        tile.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f);
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
            }
        }
    }
}
