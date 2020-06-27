using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameObjects
{
    public class Blocks : MonoBehaviour
    {
        private HexGrid hexGrid;

        private Block[,] blocksArray;
        private HashSet<Block> blocksSet = new HashSet<Block>();

        public void Start()
        {
            hexGrid = GameObject.Find("HexGrid").GetComponent<HexGrid>();

            blocksArray = new Block[hexGrid.extent*2,hexGrid.extent*2];
            alignStartingBlocks();
        }

        private void alignStartingBlocks()
        {
            GameObject[] existingBlocks = GameObject.FindGameObjectsWithTag("Block");
            foreach (GameObject go in existingBlocks)
            {
                Block b = go.GetComponent<Block>();
                Vector2Int coords = HexGrid.getCoordsFromPos(go.transform.position);
                if (b.GetType().IsSubclassOf(typeof(SingleBlock)))
                {
                    Debug.Log("added");
                    b.setPos(coords);
                    setBlockAtPos(coords,b);
                }
                else if (b.GetType() == typeof(Hub))
                {
                    b.setPos(coords);
                    setBlockAtPos(coords,b);
                    foreach (Vector2Int vAdj in HexGrid.adjacentTo(coords)) setBlockAtPos(vAdj,b);
                }
            }
        }

        public BroadCastResult getBroadcast(int turn)
        {
            BroadCastResult broadCastResult = new BroadCastResult();
            received(0, turn, broadCastResult);
            received(1, turn, broadCastResult);
            received(2, turn, broadCastResult);
            return broadCastResult;
        }
        private void received (int channel, int turn, BroadCastResult broadCastResult)
        {
            List<Block> redSources = new List<Block>();

            foreach (Block b in blocksSet) {
                if (b.hasSource() && b.getSource().sourceSend(turn,0))
                {
                    redSources.Add(b);
                }
            }


            HashSet<Block> visited = new HashSet<Block>();
            foreach (Block source in redSources)
            {
                int nSides = source.getNSides();
                for (int dir = 0; dir < nSides; dir++)
                {
                    if (!source.getSource().emitToDir(dir, turn, 0)) continue;

                    Block next = source.getStuckDir(dir);

                    if (next != null)
                    {
                        visited.Add(next);
                        broadCastResult.AddBlockFromDir(channel,next,dir);

                        dfsRecurse(visited,broadCastResult, next, 0, turn, (dir + 3) % 6);
                    }
                }
            }

            foreach (Block s in redSources)
            {
                if (!visited.Contains(s)) visited.Add(s);
            }
        }

        private void dfsRecurse(HashSet<Block> visited,BroadCastResult broadCastResult, Block start, int channel, int turn, int entryDir)
        {
            int nSides = start.getNSides();
        
            foreach (int toDir in start.getRouting().getAllFrom(entryDir, channel))
            {
                    
                Block next = start.getStuckDir(toDir);
            
                if (next != null && !visited.Contains(next))
                {
                    visited.Add(next);
                    broadCastResult.AddBlockFromDir(channel,next,toDir);

                    dfsRecurse(visited,broadCastResult, next, channel, turn, (toDir + 3) % 6);
                }

            }
        
        }
        
        public Block blockAtPos(Vector2Int pos)
        {
            return blocksArray[pos.x + hexGrid.extent, pos.y + hexGrid.extent];
        }

        private void setBlockAtPos(Vector2Int pos, Block block)
        {
            blocksArray[pos.x + hexGrid.extent, pos.y + hexGrid.extent] = block;
            blocksSet.Add(block);
        }

        public bool placeBlock(Vector2Int pos, Block block)
        {
            if (blockAtPos(pos) != null) return false;
            block.gameObject.transform.parent = transform;
            block.gameObject.transform.position = HexGrid.getPosFromCoords(pos);
            setBlockAtPos(pos,block);
            block.setPos(pos);
            foreach (Vector2Int pAdj in HexGrid.adjacentTo(pos))
            {
                Block blockAdj = blockAtPos(pAdj);
                if (blockAdj == null) continue;
                blockAdj.setStuck(block);
            }
            return true;
        }
    }

    public class BroadCastResult
    {
        private readonly Dictionary<Block, HashSet<int>>[] _dictionary = new Dictionary<Block, HashSet<int>>[]
        {
            new Dictionary<Block, HashSet<int>>(),
            new Dictionary<Block, HashSet<int>>(),
            new Dictionary<Block, HashSet<int>>(),
        };

        public void AddBlockFromDir(int channel, Block b, int dir)
        {
            if (_dictionary[channel].ContainsKey(b)) _dictionary[channel][b].Add(dir);
            else _dictionary[channel][b] = new HashSet<int>{dir};
        }

        private List<int> dirsOfBlock(int channel, Block b)
        {
            if (!_dictionary[channel].ContainsKey(b)) throw new Exception("Block not in result");
            return new List<int>(_dictionary[channel][b]);
        }

        public List<SpawnBlock> getActiveSpawnBlocks()
        {
            List<SpawnBlock> activeSpawnBlocks = new List<SpawnBlock>();
            for (int channel = 0; channel < 3; channel++)
            {
                foreach (Block b in _dictionary[channel].Keys)
                {
                    if (b.GetType() != typeof(SpawnBlock)) continue;
                    SpawnBlock sb = (SpawnBlock) b;
                    if (activeSpawnBlocks.Contains(sb)) continue;
                    
                    List<int> inDirs = dirsOfBlock(channel,b);
                    
                    if (sb.isActiveFromAnyDir(inDirs,channel)) activeSpawnBlocks.Add(sb);
                }
            }

            return activeSpawnBlocks;
        }
    }
}
