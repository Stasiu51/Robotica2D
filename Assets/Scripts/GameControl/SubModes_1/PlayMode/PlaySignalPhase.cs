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

            BroadCastResult broadcasts = blocks.getBroadcast(((PlayCBehaviour) new PlayCBehaviour().getInstance()).Turn);
            ((PlayMovePhase) new PlayMovePhase().getInstance()).setBroadcastResult(broadcasts);
            
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