using UnityEngine;

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

    void Start () {
        AssertCorrectConfiguration();
        InitializeVariables();
        PositionMainCameraForArialView();

        DrawMaze(currentGrid);
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
     * Position the main camera to give us an arial view.
     */
    private void PositionMainCameraForArialView() {
        Camera camera = Camera.main;
        float fieldWidth = tileWidth * currentGrid.Width;
        float fieldLength = tileLength * currentGrid.Length;
        // TODO: Maybe height can be adjusted according to how wide the field is
        camera.transform.position = new Vector3(fieldWidth / 2.0f, 100.0f, fieldLength / 2.0f);
        camera.transform.eulerAngles = new Vector3(90.0f, -90.0f, 0.0f);
    }

    /**
     * Takes a Grid object and draws all the tiles.
     */
    public void DrawMaze(Grid grid) {
        Vector3 floorTileScale = new Vector3(tileWidth, tileHeight, tileLength);

        for (int x = 0; x < grid.Width; x++) {
            for (int z = 0; z < grid.Length; z++) {
                Vector3 worldPosition = new Vector3(x*tileWidth, 0, z*tileWidth);

                switch(grid.At(x,z)) {
                    case Grid.TileType.terrain:
                        GameObject terrain = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        terrain.name = "Terrain-(" + x + "," + z + ")";
                        terrain.transform.parent = transform; // setting this as child of ground
                        terrain.transform.localScale = floorTileScale;
                        terrain.transform.position = worldPosition;
                        terrain.GetComponent<Renderer>().material.color = new Color(0.0f, 0.0f, 0.5f);
                        break;
                    case Grid.TileType.path:
                        break;
                    case Grid.TileType.start:
                        GameObject start = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        start.name = "Terrain-(" + x + "," + z + ")";
                        start.transform.parent = transform;
                        start.transform.localScale = floorTileScale;
                        start.transform.position = worldPosition;
                        start.GetComponent<Renderer>().material.color = new Color(0.0f, 0.5f, 0.0f);
                        break;
                    case Grid.TileType.end:
                        break;
                    case Grid.TileType.border:
                        GameObject border = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        border.name = "Border-(" + x + "," + z + ")";
                        border.transform.parent = transform;
                        border.transform.localScale = floorTileScale;
                        border.transform.position = worldPosition;
                        border.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f);
                        break;
                    default:
                        Debug.LogError("Unrecognized tile type: " + grid.At(x,z).ToString());
                        break;
                }
            }
        }
    }
}
