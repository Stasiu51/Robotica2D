using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using ObjectAccess;
using Serialisation;
using UnityEngine;
using UnityEngine.XR.WSA;

namespace GameObjects
{
    [Serializable]
    public abstract class Block
    {
        private readonly Block[] _stuck = new Block[6];
        [NonSerialized]
        private ConnectionsController _connections;
        public const int BLOCKMASS = 1;
        private SerialisableVector2Int _pos = new SerialisableVector2Int(Vector2Int.zero);
        public abstract bool Selectable { get; }
        public abstract Routing Routing { get; }
        public abstract Source Source { get; }
        public abstract bool HasSource { get; }
        public abstract int Mass { get; }
        public Vector2Int Pos
        {
            get => _pos.vector2Int;
            set
            {
                gameObject.transform.localPosition = HexGrid.getPosFromCoords(value);
                Access.managers.Blocks.setBlockAtPos(value,this);
                _pos = new SerialisableVector2Int (value);
            }
        }
        
        
        [NonSerialized]
        protected GameObject gameObject;
        protected abstract GameObject createGameObject();

        protected Block()
        {
            initialSetup();
        }
        
        public abstract void initialSetupOverride();
        public void initialSetup()
        {
            gameObject = createGameObject();
            _connections = gameObject.GetComponentInChildren<ConnectionsController>();
            gameObject.transform.parent = Access.managers.Blocks.transform;
            gameObject.transform.localPosition = HexGrid.getPosFromCoords(_pos.vector2Int);
            Debug.Log("initialsetup");
            
            foreach(Dir dir in Dir.allDirs())
            {
                if (_stuck[dir.N] != null) setStuckDir(dir, true);
            }
            initialSetupOverride();
        }

        public void setStuck(Block stuckTo, bool isStuck)
        {
            Dir dir = HexGrid.relativeDir(Pos, stuckTo.Pos);
            _stuck[dir.N] = isStuck ? stuckTo : null;
            _connections.setConnection(dir,stuckTo != null && isStuck);
            if (stuckTo != null) stuckTo.setStuckInvisible(this, isStuck);
        }

        public void setStuckDir(Dir dir, bool isStuck)
        {
            Block stuckTo = Access.managers.Blocks
                .blockAtPos(Pos + dir.v);
            
            _stuck[dir.N] = isStuck ? stuckTo : null;
            _connections.setConnection(dir,stuckTo != null && isStuck);
            if (stuckTo != null) stuckTo.setStuckInvisible(this, isStuck);
        }

        public void setStuckInvisible(Block stuckFrom, bool isStuck)
        {
            Dir dir = HexGrid.relativeDir(Pos, stuckFrom.Pos);
            _stuck[dir.N] = isStuck? stuckFrom : null;
            _connections.setConnection(dir, false);
        }

        public Block getStuck(Block stuckTo)
        {
            Dir dir = HexGrid.relativeDir(Pos, stuckTo.Pos);
            return _stuck[dir.N];
        }

        public Block getStuckDir(Dir dir)
        {
            return _stuck[dir.N];
        }

        public void SetVisible(bool visible)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = visible;
        }
        
        public void AnimateMoveBlock(Vector2Int start, Vector2Int end, float progress)
        {
            Vector3 startPos = HexGrid.getPosFromCoords(start);
            Vector3 EndPos = HexGrid.getPosFromCoords(end);
            Vector3 pos = Vector3.Lerp(startPos, EndPos, progress);
            gameObject.transform.position = pos;
        }

        public Block clone()
        {
            Block clone;
            using (MemoryStream memory_stream = new MemoryStream())
            {
                // Serialize the object into the memory stream.
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memory_stream, this);

                // Rewind the stream and use it to create a new object.
                memory_stream.Position = 0;
                clone = (Block) formatter.Deserialize(memory_stream);
            }
            clone.initialSetup();
            return clone;
        }

        public void destroy()
        {
            foreach (Dir dir in Dir.allDirs())
            {
                setStuckDir(dir, false);
            }
            GameObject.Destroy(gameObject);
        }
    }
}