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
                b.setPos(coords);
                setBlockAtPos(coords,b);
            }

            foreach (GameObject gameObject in existingBlocks)
            {
                gameObject.GetComponent<Block>().initialSetup();
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
            List<Block> redSources = blocksSet.Where(
                b => b.hasSource() && b.getSource().sourceSend(turn, channel)).ToList();

            HashSet<Block> visited = new HashSet<Block>();
            foreach (Block source in redSources)
            {
                foreach (Dir dir in Dir.allDirs())
                {
                    if (!source.getSource().emitToDir(dir, turn, 0)) continue;

                    Block next = source.getStuckDir(dir);

                    if (next != null)
                    {
                        visited.Add(next);
                        broadCastResult.AddBlockFromDir(channel,next,dir);

                        dfsRecurse(visited,broadCastResult, next, 0, turn, dir.Opposite());
                    }
                }
            }

            foreach (Block s in redSources)
            {
                if (!visited.Contains(s)) visited.Add(s);
            }
        }

        private void dfsRecurse(HashSet<Block> visited,BroadCastResult broadCastResult, Block start, int channel, int turn, Dir entryDir)
        {

            foreach (Dir toDir in start.getRouting().getAllFrom(entryDir, channel))
            {
                    
                Block next = start.getStuckDir(toDir);

                if (next == null) continue;
                
                broadCastResult.AddBlockFromDir(channel,next,toDir);
                
                if (!visited.Contains(next))
                {
                    visited.Add(next);
                    dfsRecurse(visited,broadCastResult, next, channel, turn, toDir.Opposite());
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
        private readonly Dictionary<Block, HashSet<Dir>>[] _dictionary = 
        {
            new Dictionary<Block, HashSet<Dir>>(),
            new Dictionary<Block, HashSet<Dir>>(),
            new Dictionary<Block, HashSet<Dir>>(),
        };

        public void AddBlockFromDir(int channel, Block b, Dir toDir)
        {
            Dir dir = toDir.Opposite();
            if (_dictionary[channel].ContainsKey(b)) _dictionary[channel][b].Add(dir);
            else _dictionary[channel][b] = new HashSet<Dir>{dir};
        }

        private IEnumerable<Dir> dirsOfBlock(int channel, Block b)
        {
            if (!_dictionary[channel].ContainsKey(b)) throw new Exception("Block not in result");
            return new List<Dir>(_dictionary[channel][b]);
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
                    
                    IEnumerable<Dir> inDirs = dirsOfBlock(channel,b);
                    
                    if (sb.isActiveFromAnyDir(inDirs,channel)) activeSpawnBlocks.Add(sb);
                }
            }

            return activeSpawnBlocks;
        }

        public IEnumerable<Block> allBlocksThatRecieved(int channel)
        {
            return _dictionary[channel].Keys;
        }
    }
}
