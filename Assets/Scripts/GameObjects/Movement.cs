using System;
using System.Collections;
using System.Collections.Generic;
using ObjectAccess;
using UnityEngine;

namespace GameObjects
{
    public class Movement
    {
        private IEnumerable _allBlocks;
        private BroadCastResult _broadCastResult;
        private Dictionary<Block,MovementSegment> _map = new Dictionary<Block, MovementSegment>();
        private List<MovementSegment> _movementSegments = new List<MovementSegment>();
        private Proposal _winningProposal;
        public Movement(Blocks allBlocks, BroadCastResult broadCastResult)
        {
            _allBlocks = allBlocks.getAllBlocks();
            _broadCastResult = broadCastResult;
            assignSegments();
        }

        private MovementSegment getSegment(Block b)
        {
            return _map[b];
        }

        private void assignSegments()
        {
            foreach (Block block in _allBlocks)
            {
                if (_map.ContainsKey(block)) continue;
                MovementSegment newSegment = new MovementSegment(this);
                _movementSegments.Add(newSegment);
                dfs(block, newSegment);
            }

            foreach (MovementSegment movementSegment in _movementSegments)
            {
                movementSegment.setConstraints();
            }
            int bestCost = Int32.MaxValue;
            foreach (MovementSegment movementSegment in _movementSegments)
            {
                Proposal newProposal = new Proposal(movementSegment,this, bestCost);
                if (newProposal.cost >= bestCost) continue;
                _winningProposal = newProposal;
                bestCost = newProposal.cost;
            }
        }

        private void dfs(Block block, MovementSegment movementSegment)
        {
            movementSegment.AddBlock(block);
            foreach (Dir dir in Dir.allDirs())
            {
                if (block.GetType() == typeof(SpawnBlock))
                {
                    SpawnBlock sb = (SpawnBlock) block;
                    Dir spawnDir = _broadCastResult.spawnActive(sb);
                    if (spawnDir != null && spawnDir == dir)
                    {
                        movementSegment.AddBoundary(sb,dir);
                        continue;
                    }
                }
                Block nextBlock = block.getStuckDir(dir);
                if (nextBlock == null) continue;

                if (nextBlock.GetType() == typeof(SpawnBlock))
                {
                    SpawnBlock sb = (SpawnBlock) nextBlock;
                    if (_broadCastResult.spawnActive(sb) == dir.Opposite())
                    {
                        continue;
                    }
                }

                if (_map.ContainsKey(nextBlock) && _map[nextBlock] == movementSegment) continue;
                if (_map.ContainsKey(nextBlock) && _map[nextBlock] == movementSegment) throw new Exception("aargh!!");
                dfs(nextBlock,movementSegment);
            }
        }

        public Vector2Int targetCoordsOfBlock(Block block)
        {
            MovementSegment containingSegment = _map[block];
            return block.Pos + _winningProposal.getSegmentTarget(containingSegment);
        }
        
        private class MovementSegment
        {
            public int totalMass { get; private set; }
            private readonly Dictionary<SpawnBlock,Dir> _boundaries = new Dictionary<SpawnBlock, Dir>();
            private readonly Dictionary<MovementSegment,Dir> _constraints = new Dictionary<MovementSegment, Dir>();
            private readonly Blocks _blocks;
            private readonly Movement _movement;

            public MovementSegment(Movement movement)
            {
                _blocks = Access.managers.Blocks;
                _movement = movement;
            }

            public Dictionary<MovementSegment, Dir> getConstraints() => new Dictionary<MovementSegment, Dir>(_constraints);

            public void AddBlock(Block block)
            {
                totalMass += block.Mass;
                _movement._map[block] = this;
            }

            public void AddBoundary(SpawnBlock sb, Dir dir)
            {
                _boundaries[sb] = dir;
            }

            private void addConstraint(MovementSegment movementSegment, Dir constraint)
            {
                if (_constraints.ContainsKey(movementSegment) && _constraints[movementSegment] != constraint) 
                    throw new Exception("Segment constrained twice inconsistently by same segment");
                _constraints[movementSegment] = constraint;
            }

            public void setConstraints()
            {
                foreach (KeyValuePair<SpawnBlock,Dir> kv in _boundaries)
                {
                    Block other = _blocks.blockAtPos(kv.Key.Pos + kv.Value.v);
                    if (other == null) continue;
                    MovementSegment otherSegment = _movement.getSegment(other);
                    otherSegment.addConstraint(this,kv.Value.Opposite());
                    addConstraint(otherSegment,kv.Value);
                }
            }
            
        }

        private class Proposal
        {
            private readonly Dictionary<MovementSegment,Vector2Int> _movements = new Dictionary<MovementSegment, Vector2Int>();
            public int cost;

            public Proposal(MovementSegment stationarySegment, Movement movement, int costLimit, Dictionary<MovementSegment,Vector2Int> movements = null)
            {
                if (movements != null) _movements = movements;
                _movements[stationarySegment] = Vector2Int.zero;
                dfsMovement(stationarySegment);
                if (cost >= costLimit) return;
                if (_movements.Count < movement._movementSegments.Count)
                {
                    Proposal subProposal = null;
                    int bestCost = Int32.MaxValue;
                    foreach (MovementSegment subStationarySegment in movement._movementSegments)
                    {
                        if (_movements.ContainsKey(subStationarySegment)) continue;
                        Proposal newProposal =
                            new Proposal(subStationarySegment, movement, costLimit - cost,
                                new Dictionary<MovementSegment, Vector2Int>(_movements));
                        if (newProposal.cost >= bestCost) continue;
                        subProposal = newProposal;
                        bestCost = newProposal.cost;
                    }
                    if (subProposal == null) throw new Exception("unable to generate subproposal");
                    cost += subProposal.cost;
                    _movements = subProposal._movements;
                }
            }

            private void dfsMovement(MovementSegment segment)
            {
                foreach (KeyValuePair<MovementSegment, Dir> constraint in segment.getConstraints())
                {
                    MovementSegment otherSegment = constraint.Key;
                    Vector2Int totalMove = constraint.Value.v + _movements[segment];
                    if (_movements.ContainsKey(otherSegment) && _movements[otherSegment] != totalMove) 
                        throw new Exception("conflicting constraints!");
                    if (_movements.ContainsKey(otherSegment) && _movements[otherSegment] == totalMove)
                        continue;
                    _movements[otherSegment] = totalMove;
                    if (totalMove != Vector2Int.zero) cost += otherSegment.totalMass;
                    dfsMovement(otherSegment);
                }
            }

            public Vector2Int getSegmentTarget(MovementSegment movementSegment)
            {
                return _movements[movementSegment];
            }
        }
    }
}