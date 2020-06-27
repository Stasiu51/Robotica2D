using System;
using System.Collections.Generic;
using GameObjects;
using ObjectAccess;
using UnityEngine;

namespace GameControl.SubModes_1.PlayMode
{
    public class PlaySignalPhase : CBehaviour
    {
        
        public override CBehaviour getInstance() =>_instance ?? (_instance = new PlaySignalPhase());
        public override Type belongsTo() => typeof(PlayCBehaviour);
        
        private static PlaySignalPhase _instance ;
        private const float SIGNALPAUSETIME = 1f;
        private Managers managers;
        private Blocks blocks;


        public override void onEnter()
        {
            Debug.Log("Enter signal");

            if (managers == null) managers = GameObject.Find("ObjectAccess").GetComponent<Managers>();
            if (blocks == null) blocks = managers.Blocks;
            int turn = ((PlayCBehaviour) new PlayCBehaviour().getInstance()).Turn;
            
            BroadCastResult broadcasts = blocks.getBroadcast(turn);
            ((PlayMovePhase) new PlayMovePhase().getInstance()).setBroadcastResult(broadcasts);

            foreach (Block block in broadcasts.allBlocksThatRecieved(0))
            {
                block.GetComponent<SpriteRenderer>().color = Color.blue;
            }
            
            GameObject Timers = GameObject.Find("Timers");
            Timer.MakeTimer(SIGNALPAUSETIME, () => Exit(new PlayMovePhase().getInstance()));
        }

        public override void onExit()
        {
            Debug.Log("exit signal");
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