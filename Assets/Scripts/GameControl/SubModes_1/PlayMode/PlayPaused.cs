using System;
using UIControl;
using UnityEngine;

namespace GameControl.SubModes_1.PlayMode
{
    class PlayPaused : CBehaviour
    {
        private static PlayPaused _instance ;
        public override CBehaviour getInstance() => _instance ?? (_instance = new PlayPaused());
        public override Type belongsTo() => typeof(PlayCBehaviour);
        public override void onEnter()
        {
            Debug.Log("entered play paused");
            PlayUIEventInvoker.PlayPressedInPlayMode += playOrStepPressed;
            PlayUIEventInvoker.StepPressedInPlayMode += playOrStepPressed;
        }

        public override void onExit()
        {
            PlayUIEventInvoker.PlayPressedInPlayMode -= playOrStepPressed;
            PlayUIEventInvoker.StepPressedInPlayMode -= playOrStepPressed;
            Debug.Log("exited play paused");
        }
        
        protected override CBehaviour updateOverride() => null;
        public override CBehaviour ongui(Event e) => null;

        void playOrStepPressed()
        {
            Exit(new PlaySignalPhase().getInstance());
        }


    }
}