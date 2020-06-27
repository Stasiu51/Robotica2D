using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace GameObjects
{
    public class Hub : Block
    {

        private Block[] _stuck = new Block[12];
        public GameObject[] connections = new GameObject[12];
        private TimerSource _timerSource = new TimerSource();
        const int HUBMASS = 100;

        private readonly Dictionary<Vector2Int, int> YDirs = new Dictionary<Vector2Int, int>
        {
            {new Vector2Int(2, 0), 0},
            {new Vector2Int(2, -1), 1},
            {new Vector2Int(2, -2), 2},
            {new Vector2Int(1, -2), 3},
            {new Vector2Int(0, -2), 4},
            {new Vector2Int(-1, -1), 5},
            {new Vector2Int(-2, 0), 6},
            {new Vector2Int(-2, 1), 7},
            {new Vector2Int(-2, 2), 8},
            {new Vector2Int(-1, 2), 9},
            {new Vector2Int(0, 2), 10},
            {new Vector2Int(1, 1), 11},
        };

        private int relativeDir(Vector2Int from, Vector2Int to)
        {
            Vector2Int d = to - from;
            return YDirs[d];
        }
        public override void setStuck(Block stuckTo)
        {
            int dir = relativeDir(getPos(), stuckTo.getPos());
            _stuck[dir] = stuckTo;
            connections[dir].SetActive(stuckTo != null);

        }

        public override void setStuckInvisible(Block stuckFrom)
        {
            int dir = HexGrid.relativeDir(getPos(), stuckFrom.getPos());
            _stuck[dir] = stuckFrom;
            connections[dir].SetActive(false);
        }

        public override Block getStuck(Block stuckTo)
        {
            int dir = HexGrid.relativeDir(getPos(), stuckTo.getPos());
            return _stuck[dir];
        }
        public override Block getStuckDir(int dir)
        {
            return _stuck[dir];
        }

        public override int getNSides()
        {
            return 12;
        }

        public override bool hasSource()
        {
            return true;
        }

        public override int getMass() => HUBMASS;

        public override Source getSource()
        {
            return _timerSource;
        }

        public override Routing getRouting()
        {
            throw new System.NotImplementedException();
        }
    }
}
