using UnityEngine;

public class Tile : MonoBehaviour {

    private int gridLocX;
    private int gridLocY;

    public int GridLocX {
        get { return gridLocX; }
        internal set { gridLocX = value; }
    }

    public int GridLocY {
        get { return gridLocY; }
        internal set { gridLocY = value; }
    }

    void Start () {

    }

    void Update () {

    }
}
