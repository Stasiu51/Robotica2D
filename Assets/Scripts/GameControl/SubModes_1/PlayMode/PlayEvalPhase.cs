using System;
using UnityEngine;

namespace GameControl.SubModes_1.PlayMode
{
    public class PlayEvalPhase : CBehaviour
    {
        private static PlayEvalPhase _instance;
        public override CBehaviour getInstance() => _instance ?? (_instance = new PlayEvalPhase());
        private const float EVALTIME = 0.5f;
        public override Type belongsTo() => typeof(PlayCBehaviour);
        public override void onEnter()
        {
            Debug.Log("eval");
            Timer.MakeTimer(EVALTIME, leaveEval);
        }

        public override void onExit()
        {
            Debug.Log("exit eval");
        }

        void leaveEval()
        {
            var playB = (PlayCBehaviour) new PlayCBehaviour().getInstance();
            playB.Turn++;
            PauseState pauseState = playB.pauseState;
            switch (pauseState)
            {
                case PauseState.Play:
                    Exit(new PlaySignalPhase().getInstance());
                    break;
                case PauseState.Step:
                case PauseState.Pause:
                    Exit(new PlayPaused().getInstance());
                    break;
            }
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