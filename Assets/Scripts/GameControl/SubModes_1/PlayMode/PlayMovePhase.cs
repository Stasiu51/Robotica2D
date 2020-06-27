using System;
using System.Collections.Generic;
using GameObjects;
using ObjectAccess;
using UnityEngine;

namespace GameControl.SubModes_1.PlayMode
{
    public class PlayMovePhase : CBehaviour
    {
        private static PlayMovePhase _instance ;
        public override Type belongsTo() => typeof(PlayCBehaviour);

        public override CBehaviour getInstance() => _instance ?? (_instance = new PlayMovePhase());
        
        private const float MOVETIME = 3;

        private Blocks _blocks;

        private BroadCastResult _broadCastResult;

        public void setBroadcastResult(BroadCastResult broadCastResult) => _broadCastResult = broadCastResult;
        public override void onEnter()
        {
            Debug.Log("enter move phase");
            if (_blocks == null) _blocks = GameObject.Find("ObjectAccess").GetComponent<Managers>().Blocks;
            List<SpawnBlock> spawnBlocks = new List<SpawnBlock>();
            int turn = ((PlayCBehaviour) new PlayCBehaviour().getInstance()).Turn;
            List<SpawnBlock> activeSpawnBlocks = _broadCastResult.getActiveSpawnBlocks();
            foreach (SpawnBlock block in activeSpawnBlocks)
            {
                block.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
            }

        }

        public override void onExit()
        {
            Debug.Log("exit move");
        }

        protected override CBehaviour updateOverride()
        {
            return null;
        }

        public override CBehaviour ongui(Event e)
        {
            return null;
        }

       
        
    }
}