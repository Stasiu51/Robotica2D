using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.WSA;

namespace GameObjects
{
    public abstract class Block : MonoBehaviour
    {

        private Vector2Int _pos;

        public abstract void initialSetup();

        public abstract Routing getRouting();

        public abstract Source getSource();

        public abstract bool hasSource();

        public abstract int getMass();

        public Vector2Int getPos()
        {
            return new Vector2Int(_pos.x, _pos.y);
        }

        public void setPos(Vector2Int pos)
        {
            transform.position = HexGrid.getPosFromCoords(pos);
            _pos = pos;
        }

        public abstract void setStuck(Block stuckTo);

        public abstract void setStuckInvisible(Block stuckFrom);

        public abstract Block getStuck(Block stuckTo);
        public abstract Block getStuckDir(Dir dir);



    }

    public abstract class SingleBlock : Block
    {
        private readonly Block[] _stuck = new Block[6];
        public GameObject[] Connections = new GameObject[6];
        public const int SINGLEBLOCKMASS = 1;

        public override void setStuck(Block stuckTo)
        {
            Dir dir = HexGrid.relativeDir(getPos(), stuckTo.getPos());
            _stuck[dir.N] = stuckTo;
            Connections[dir.N]?.SetActive(stuckTo != null);
            stuckTo.setStuckInvisible(this);
        }

        public override void setStuckInvisible( Block stuckFrom)
        {
            Dir dir = HexGrid.relativeDir(getPos(), stuckFrom.getPos());
            _stuck[dir.N] = stuckFrom;
            Connections[dir.N]?.SetActive(false);
        }

        public override Block getStuck(Block stuckTo)
        {
            Dir dir = HexGrid.relativeDir(getPos(), stuckTo.getPos());
            return _stuck[dir.N];
            
        }
        public override Block getStuckDir(Dir dir)
        {
            return _stuck[dir.N];
        }

    }
}