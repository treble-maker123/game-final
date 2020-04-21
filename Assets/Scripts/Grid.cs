/**
 * This class is an abstract representation of the grid. Map generation
 * will be based on the internal state of a Grid object.
 */

public class Grid {
    public enum TileType {
        terrain,
        path,
        border,
        start,
        end
    }

    public enum Difficulty {
        easy,
        medium,
        hard
    }

    private int width;
    private int length;
    private TileType[,] state;

    public int Width {
        get { return width; }
    }

    public int Length {
        get { return length; }
    }

    public Grid(int width, int length) {
        UpdateDimension(width, length);
    }

    /**
     * Returns the TileType at (x,y)
     */
    public TileType At(int x, int y) {
        return state[x,y];
    }

    /**
     * Sets the TileType at (x,y) to the given tile type.
     */
    public void At(int x, int y, TileType newType) {
        state[x,y] = newType;
    }

    /**
     * Utiliy method for checking whether a point is on the border tile.
     */
    public bool IsOnBorder(int x, int y) {
        return x == 0 || y == 0 || x == width - 1 || y == length - 1;
    }

    /**
     * Update the grid with the new dimension. Note that calling this method
     * will reset all internal states;
     */
    public void UpdateDimension(int width, int length) {
        this.width = width;
        this.length = length;
        state = new TileType[width, length];

        // reset all tiles to terrain
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < length; j++) {
                if (IsOnBorder(i, j)) {
                    state[i,j] = TileType.border;
                } else {
                    state[i,j] = TileType.terrain;
                }
            }
        }

        // start and end points will be fixed
        state[1,1] = TileType.start;
        state[width-2,length-2] = TileType.end;
    }

    /**
     * Resets the grid and create a new path. The higher the
     * difficulty, the longer the generated path will be.
     */
    public void GeneratePath(Difficulty d) {
        UpdateDimension(Width, Length);
    }
}
