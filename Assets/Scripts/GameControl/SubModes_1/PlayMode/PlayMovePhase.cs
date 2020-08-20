using System;
using System.Collections.Generic;
using GameObjects;
using ObjectAccess;
using UnityEngine;

namespace GameControl.SubModes_1.PlayMode
{
    public class PlayMovePhase : CBehaviour
    {
        private static PlayMovePhase _instance;
        public override Type belongsTo() => typeof(PlayCBehaviour);

        public override CBehaviour getInstance() => _instance ?? (_instance = new PlayMovePhase());

        private const float MOVETIME = 0.6f;

        private Blocks _blocks;

        public BroadCastResult broadCastResult { private get; set; }
        private Dictionary<Block, Vector2Int> _originalPos = new Dictionary<Block, Vector2Int>();
        private Dictionary<Block, Vector2Int> _targetPos = new Dictionary<Block, Vector2Int>();

        private Timer _moveTimer;
        
        public override void onEnter()
        {
            Debug.Log("enter move phase");
            if (_blocks == null) _blocks = Access.managers.Blocks;
            int turn = ((PlayCBehaviour) new PlayCBehaviour().getInstance()).Turn;

            Movement movement = new Movement(_blocks, broadCastResult);
            
            foreach (Block block in _blocks.getAllBlocks())
            {
                _originalPos[block] = block.Pos;
                _targetPos[block] = movement.targetCoordsOfBlock(block);
                if (block.GetType() == typeof(SpawnBlock))
                {
                    Dir dir = broadCastResult.spawnActive((SpawnBlock) block);
                    if (dir == null) continue;
                    block.setStuckDir(dir, false);
                }
            }

            _moveTimer = Timer.MakeTimer(MOVETIME, finishMovement);
        }

        public override void onExit()
        {
            Debug.Log("exit move");
        }

        protected override CBehaviour updateOverride()
        {
            foreach (Block block in _blocks.getAllBlocks())
            {
                block.AnimateMoveBlock(_originalPos[block],_targetPos[block],_moveTimer.getProgress());
            }

            return null;
        }

        private void finishMovement()
        {
            _blocks.forgetBlockPositions();
            foreach (Block block in _blocks.getAllBlocks())
            {
                block.Pos = _targetPos[block];
            }

            foreach (Block block in _blocks.getAllBlocks())
            {
                if (block.GetType() != typeof(SpawnBlock)) continue;
                SpawnBlock spawnBlock = (SpawnBlock) block;
                Dir dir = broadCastResult.spawnActive(spawnBlock);
                if (dir == null) continue;
                Block newBlock = spawnBlock.spawnNewBlock();
                newBlock.Pos = spawnBlock.Pos + dir.v;

                spawnBlock.setStuck(newBlock, true);
                
                newBlock.setStuckDir(dir, true);
            }

            
            Exit(new PlayEvalPhase().getInstance());
        }

        public override CBehaviour ongui(Event e)
        {
            return null;
        }
    }
}