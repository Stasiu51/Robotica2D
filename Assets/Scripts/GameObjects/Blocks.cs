using System;
using System.Collections.Generic;
using System.Linq;
using Serialisation;
using UnityEditor;
using UnityEngine;

namespace GameObjects
{
    public class Blocks : MonoBehaviour
    {
        private HexGrid _hexGrid;

        private HashSet<Block> _blocksSet = new HashSet<Block>();
        private Dictionary<Vector2Int, Block> _blocksByPos = new Dictionary<Vector2Int, Block>();

        public void Start()
        {
            _hexGrid = GameObject.Find("HexGrid").GetComponent<HexGrid>();

            // _blocksArray = new Block[_hexGrid.extent * 2, _hexGrid.extent * 2];
            _blocksByPos = new Dictionary<Vector2Int, Block>();
            alignStartingBlocks();
        }

        private void alignStartingBlocks()
        {
            GameObject[] existingBlocks = GameObject.FindGameObjectsWithTag("Block");
            foreach (GameObject go in existingBlocks)
            {
                Block b = go.GetComponent<Block>();
                Vector2Int coords = HexGrid.getCoordsFromPos(go.transform.position);
                b.Pos = coords;
            }

            foreach (GameObject gameObject in existingBlocks)
            {
                gameObject.GetComponent<Block>().initialSetup();
            }
        }

        public BroadCastResult getBroadcast(int turn)
        {
            BroadCastResult broadCastResult = new BroadCastResult();
            propagateSignal(Chn.Red, turn, broadCastResult);
            propagateSignal(Chn.Yellow, turn, broadCastResult);
            propagateSignal(Chn.Blue, turn, broadCastResult);
            return broadCastResult;
        }

        private void propagateSignal(Chn channel, int turn, BroadCastResult broadCastResult)
        {
            List<Block> channelSources = _blocksSet.Where(
                b => b.HasSource && b.Source.sourceSend(turn, channel)).ToList();

            HashSet<Block> visited = new HashSet<Block>();
            foreach (Block source in channelSources)
            {
                foreach (Dir dir in Dir.allDirs())
                {
                    if (!source.Source.emitToDir(dir, turn, channel)) continue;

                    Block next = source.getStuckDir(dir);

                    if (next != null)
                    {
                        visited.Add(next);
                        broadCastResult.AddBlockFromDir(channel, next, dir);

                        dfsRecurse(visited, broadCastResult, next, channel, turn, dir.Opposite());
                    }
                }
            }

            foreach (Block s in channelSources)
            {
                if (!visited.Contains(s)) visited.Add(s);
            }
        }

        private void dfsRecurse(HashSet<Block> visited, BroadCastResult broadCastResult, Block start, Chn channel,
            int turn, Dir entryDir)
        {
            foreach (Dir toDir in start.Routing.getAllFrom(entryDir, channel))
            {
                Block next = start.getStuckDir(toDir);

                if (next == null) continue;

                broadCastResult.AddBlockFromDir(channel, next, toDir);

                if (!visited.Contains(next))
                {
                    visited.Add(next);
                    dfsRecurse(visited, broadCastResult, next, channel, turn, toDir.Opposite());
                }
            }
        }

        public Block blockAtPos(Vector2Int pos)
        {
            if (!_blocksByPos.ContainsKey(pos)) return null;
            return _blocksByPos[pos];
        }

        public void setBlockAtPos(Vector2Int pos, Block block)
        {
            _blocksByPos[pos] = block;
            _blocksSet.Add(block);
        }

        public void forgetBlockPositions()
        {
            _blocksByPos = new Dictionary<Vector2Int, Block>();
        }

        public bool placeBlock(Vector2Int pos, Block block)
        {
            if (blockAtPos(pos) != null) return false;
            block.Pos = pos;
            Debug.Log("setpos");
            foreach (Vector2Int pAdj in HexGrid.adjacentTo(pos))
            {
                Block blockAdj = blockAtPos(pAdj);
                if (blockAdj == null) continue;
                blockAdj.setStuck(block, true);
            }
            return true;
        }

        public IEnumerable<Block> getAllBlocks()
        {
            return new List<Block>(_blocksSet);
        }

        private void forgetAll()
        {
            _blocksSet = new HashSet<Block>();
            forgetBlockPositions();
        }

        public void destroyBlock(Block block)
        {
            _blocksSet.Remove(block);
            _blocksByPos[block.Pos] = null;
            block.destroy();
        }

        public void loadFromSave(SavedBlocks save)
        {
            foreach (Block block in getAllBlocks())
            {
                block.destroy();
            }
            forgetAll();
            foreach (Block block in save.GetBlocks)
            {
                block.initialSetup();
                setBlockAtPos(block.Pos,block);
            }
        }

        
        [Serializable]
        public class SavedBlocks
        {
            private List<Block> _blockSet;
            public IEnumerable<Block> GetBlocks
            {
                get => new List<Block>(_blockSet);
                private set => _blockSet = new List<Block>(value);
            }
            public SavedBlocks(Blocks b)
            {
                GetBlocks = b.getAllBlocks();
            }
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

        public void AddBlockFromDir(Chn channel, Block b, Dir toDir)
        {
            Dir dir = toDir.Opposite();
            if (_dictionary[channel.N].ContainsKey(b)) _dictionary[channel.N][b].Add(dir);
            else _dictionary[channel.N][b] = new HashSet<Dir> {dir};
        }

        private IEnumerable<Dir> dirsOfBlock(Chn channel, Block b)
        {
            if (!_dictionary[channel.N].ContainsKey(b)) return new List<Dir>();
            return new List<Dir>(_dictionary[channel.N][b]);
        }

        public List<SpawnBlock> getActiveSpawnBlocks()
        {
            List<SpawnBlock> activeSpawnBlocks = new List<SpawnBlock>();
            foreach (var channel in Chn.allChannels())
            {
                {
                    foreach (Block b in _dictionary[channel.N].Keys)
                    {
                        if (b.GetType() != typeof(SpawnBlock)) continue;
                        SpawnBlock sb = (SpawnBlock) b;
                        if (activeSpawnBlocks.Contains(sb)) continue;

                        IEnumerable<Dir> inDirs = dirsOfBlock(channel, b);

                        if (sb.isActiveFromAnyDir(inDirs, channel)) activeSpawnBlocks.Add(sb);
                    }
                }
            }

            return activeSpawnBlocks;
        }

        public Dir spawnActive(SpawnBlock spawnBlock)
        {
            //rule priority determined by first red, then by E->NE signal, then by E->NE func
            foreach (Chn channel in Chn.allChannels())
            {
                List<Dir> inDirs = new List<Dir>(dirsOfBlock(channel, spawnBlock));
                foreach (Dir signalDir in Dir.allDirs())
                {
                    if (!inDirs.Contains(signalDir)) continue;
                    Dir func = spawnBlock.activeFromDir(signalDir, channel);
                    if (func != null) return func;
                }
            }
            return null;
        }

        public IEnumerable<Block> allBlocksThatRecieved(int channel)
        {
            return _dictionary[channel].Keys;
        }
        
        
    }
}