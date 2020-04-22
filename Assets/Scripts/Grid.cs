using System;
using System.Collections.Generic;
using UnityEngine;
/**
 * This class is an abstract representation of the grid. Map generation
 * will be based on the internal state of a Grid object.
 */

public class Grid {
    private int width;
    private int length;
    private TileType[,] state;
    private List<Position> waypoints;
    private Position startPos = new Position(1,1);
    private Position endPos;

    public int Width {
        get { return width; }
    }

    public int Length {
        get { return length; }
    }

    /**
     * Returns a copy of the waypoints for the current state. A waypoint
     * is a turn in the path, which can be used to help guide NPCs
     * along the path.
     */
    public List<Position> Waypoints {
        get { return waypoints.ConvertAll(w => (Position) w.Clone()); }
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
     * Update the grid with the new dimension. Note that calling this method
     * will reset all internal states;
     */
    public void UpdateDimension(int width, int length) {
        this.width = width;
        this.length = length;
        endPos = new Position(width-2, length-2);
        state = new TileType[width, length];

        // reset all tiles
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < length; j++) {
                if (Position.IsOnBorder(i,j,width,length)) {
                    state[i,j] = TileType.border;
                } else {
                    state[i,j] = TileType.terrain;
                }
            }
        }

        // start and end points will be fixed
        state[startPos.X,startPos.Y] = TileType.start;
        state[endPos.X,endPos.Y] = TileType.end;
        waypoints = new List<Position>();
    }

    /**
     * Resets the grid and creates a new path. The higher the
     * difficulty, the shorter the generated path will be.
     */
    public void GeneratePath(GameState.Difficulty diff) {
        UpdateDimension(Width, Length);

        Position pos = (Position) startPos.Clone();
        Position waypoint = (Position) startPos.Clone();

        Direction dir = Direction.right;
        int tilesToExpand = 0;

        while (true) {
            // reached the end, done
            if (pos == endPos) return;

            // figure out how many tiles we CAN expand in the current direction
            int availableTiles = pos.AvailableUnitsUntilBorder(this, dir);
            Debug.Assert(availableTiles > 0);
            Debug.Assert(dir != Direction.up); // should not go up!

            // figure out how many tiles we WILL expand based on difficulty
            switch (diff) {
                case (GameState.Difficulty.easy):
                    if (dir == Direction.right || dir == Direction.left)
                        tilesToExpand = UnityEngine.Random.Range(
                                Math.Min(1, (int) (availableTiles * 0.9f)),
                                availableTiles + 1); // +1 because Random.Range is exclusive for end
                    else
                        tilesToExpand = 2; // go down two at a time to ensure maximum length
                    break;
                case (GameState.Difficulty.medium):
                    if (dir == Direction.right || dir == Direction.left)
                        tilesToExpand = UnityEngine.Random.Range(
                                Math.Min(1, (int) (availableTiles * 0.4f)),
                                Math.Min(1, (int) (availableTiles * 0.8f)) + 1);
                    else
                        tilesToExpand = UnityEngine.Random.Range(2, 4);
                    break;
                case (GameState.Difficulty.hard):
                    if (dir == Direction.right || dir == Direction.left)
                        tilesToExpand = UnityEngine.Random.Range(
                                Math.Min(1, (int) (availableTiles * 0.4f)),
                                Math.Min(1, (int) (availableTiles * 0.2f)) + 1);
                    else
                        tilesToExpand = 3;
                    break;
                default:
                    Debug.LogError("Invalid difficulty received: " + diff.ToString());
                    return;
            }
            tilesToExpand = Math.Min(tilesToExpand, availableTiles);

            // move the waypoint to the destination
            waypoint.Move(tilesToExpand, dir);
            // double check that all assumptions are valid
            // Debug.Log("# tiles to expand: " + tilesToExpand + ", destination: " + waypoint.ToString() + ", direction: " + dir.ToString());
            Debug.Assert(!waypoint.IsOnBorder(this));
            Debug.Assert(!waypoint.OutOfBound(this));
            Debug.Assert(
                    state[waypoint.X, waypoint.Y] == TileType.terrain || state[waypoint.X, waypoint.Y] == TileType.end);
            Debug.Assert(waypoint.X <= endPos.X);
            Debug.Assert(waypoint.Y <= endPos.Y);

            // if at the bottom-most row and moving right, then bring the path to the finish line
            if (waypoint.Y == endPos.Y && waypoint.X != endPos.X && dir == Direction.right) {
                waypoint.UpdatePosition(endPos.X, endPos.Y);
            }

            // if one or two units above end point, bring the path to finish line
            if (endPos.Y - waypoint.Y <= 2 && waypoint.X == endPos.X && dir == Direction.down) {
                waypoint.UpdatePosition(endPos.X, endPos.Y);
            }

            // save the waypoint
            waypoints.Add((Position) waypoint.Clone());

            // update the state to build the path
            while (true) {
                pos.Move(1, dir);

                if (pos == endPos) break;

                state[pos.X, pos.Y] = TileType.path;

                if (pos == waypoint) break;
            }

            // ============================================
            // figure out direction for the next iteration
            // ============================================

            // if at the bottom row, just go right next time so it will hit the end
            if (pos.Y == length - 2) {
                dir = Direction.right;
                continue;
            }

            if (dir == Direction.right || dir == Direction.left) {
                dir = Direction.down;
                continue;
            }

            if (dir == Direction.down) {
                // if current row is one above the end point, go down one more
                if (pos.Y + 1 == endPos.Y) {
                    dir = Direction.down;
                    continue;
                }

                int leftRoom = pos.AvailableUnitsUntilBorder(this, Direction.left);
                int rightRoom = pos.AvailableUnitsUntilBorder(this, Direction.right);
                dir = leftRoom > rightRoom ? Direction.left : Direction.right;
                continue;
            }
        }
    }

    public enum TileType {
        terrain,
        path,
        border,
        start,
        end
    }

    public enum Direction {
        up,
        down,
        left,
        right
    }

    public class Position : ICloneable{
        private int x;
        private int y;
        private List<Position> neighbors;

        public int X {
            get { return x; }
        }

        public int Y {
            get { return y; }
        }

        public Position(int x, int y) {
            this.x = x;
            this.y = y;
        }

        /**
         * Updates the x, y with the new values.
         */
        public void UpdatePosition(int x, int y) {
            this.x = x;
            this.y = y;
        }

        /**
         * Move the position in the given direction for n units.
         */
        public void Move(int n, Direction dir) {
            int newX = x;
            int newY = y;

            switch (dir) {
                case (Direction.up):
                    newY -= n;
                    break;
                case (Direction.down):
                    newY += n;
                    break;
                case (Direction.left):
                    newX -= n;
                    break;
                case (Direction.right):
                    newX += n;
                    break;
                default:
                    Debug.LogError("Unrecognized direction: " +  dir.ToString());
                    break;
            }

            UpdatePosition(newX, newY);
        }

        /**
         * Check if the point is along the border on the grid.
         */
        public bool IsOnBorder(Grid g) {
            return Position.IsOnBorder(x, y, g.Width, g.Length);
        }

        /**
         * Check if the position is outside the bounds of this grid.
         */
        public bool OutOfBound(Grid g) {
            return x < 0 || y < 0 || x >= g.Width || y >= g.Length;
        }

        /**
         * Calculates the number of tiles available between this position
         * and the border in the given direction.
         *
         * Example:
         *      'p - - - - |', Direction.right
         *      Where 'p' is the current position, '-' is the empty tile, and
         *      '|' is the border, this function returns 4.
         */
        public int AvailableUnitsUntilBorder(Grid g, Direction dir) {
            switch (dir) {
                case (Direction.right):
                    return (g.Width - 2) - X;
                case (Direction.left):
                    return (X - 1);
                case (Direction.down):
                    return (g.length - 2) - Y;
                case (Direction.up):
                    return (Y - 1);
                default:
                    Debug.LogError("Invalid direction received: " + dir.ToString());
                    return 0;
            }

        }

        public override bool Equals(object other) {
            if (other == null) {
                return false;
            }

            Position c = (Position) other;

            return !object.ReferenceEquals(c, null) && (c.X == X) && (c.Y == Y);
        }

        public override int GetHashCode() {
            return string.Format("X:{0},Y:{1}", X, Y).GetHashCode();
        }

        public object Clone() {
            return new Position(X, Y);
        }

        public static bool IsOnBorder(int x, int y, int width, int length) {
            return x == 0 || y == 0 || x == width - 1 || y == length - 1;
        }

        public static bool operator ==(Position first, Position second) {
            return first.Equals(second);
        }

        public static bool operator !=(Position first, Position second) {
            return !first.Equals(second);
        }

        public override string ToString() {
            return "("+X+","+Y+")";
        }
    }
}
