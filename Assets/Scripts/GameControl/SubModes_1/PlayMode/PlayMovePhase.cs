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

        private const float MOVETIME = 1;

        private Blocks _blocks;

        private BroadCastResult _broadCastResult;
        private Dictionary<Block, Vector2Int> _originalPos = new Dictionary<Block, Vector2Int>();
        private Dictionary<Block, Vector2Int> _targetPos = new Dictionary<Block, Vector2Int>();

        private Timer _moveTimer;

        public void setBroadcastResult(BroadCastResult broadCastResult) => _broadCastResult = broadCastResult;

        public override void onEnter()
        {
            Debug.Log("enter move phase");
            if (_blocks == null) _blocks = GameObject.Find("ObjectAccess").GetComponent<Managers>().Blocks;
            int turn = ((PlayCBehaviour) new PlayCBehaviour().getInstance()).Turn;

            Movement movement = new Movement(_blocks, _broadCastResult);

            GameObject Timers = GameObject.Find("Timers");
            _moveTimer = Timer.MakeTimer(MOVETIME, () => Exit(new PlayIdle().getInstance()));
            foreach (Block block in _blocks.getAllBlocks()) _originalPos[block] = block.getPos();
            foreach (Block block in _blocks.getAllBlocks()) _targetPos[block] = block.getPos() + movement.targetCoordsOfBlock(block);
        }

        public override void onExit()
        {
            Debug.Log("exit move");
        }

        protected override CBehaviour updateOverride()
        {
            foreach (Block block in _blocks.getAllBlocks())
            {
                _blocks.MoveBlock(block,_originalPos[block],_targetPos[block],_moveTimer.getProgress());
            }

            return null;
        }

        public override CBehaviour ongui(Event e)
        {
            return null;
        }
    }
}