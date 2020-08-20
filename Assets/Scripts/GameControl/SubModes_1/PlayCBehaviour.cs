using System;
using GameControl.SubModes_1.PlayMode;
using ObjectAccess;
using UIControl;
using UnityEngine;

namespace GameControl.SubModes_1
{
    public class PlayCBehaviour: CSubStateMachine

    {
        public int Turn;
        
        private static PlayCBehaviour _instance ;
        private UIElements _uiElements;
        public PauseState pauseState;

        public override CBehaviour getInstance()
        {
            if (_instance == null) _instance = new PlayCBehaviour();
            return _instance;
        }
        public override Type belongsTo() => typeof(GameMachine);
        

        protected override CBehaviour getInitialBehaviour()
        {
            switch (pauseState)
            {
                case PauseState.Play:
                    return new PlaySignalPhase().getInstance();
                case PauseState.Pause:
                    return new PlayPaused().getInstance();
                case PauseState.Step:
                    return new PlaySignalPhase().getInstance();
            }
            throw new Exception("pause state not set at get initial behaviour call");
        }

        public override void onEnterOverride()
        {
            Debug.Log("entered play mode");
            if (_uiElements == null) _uiElements = Access.uiElements;
            _uiElements.PlayUI.SetActive(true);
            Turn = 0;
            PlayUIEventInvoker.PlayPressedInPlayMode += PlayPressed;
            PlayUIEventInvoker.PausePressedInPlayMode += PausePressed;
            PlayUIEventInvoker.StepPressedInPlayMode += StepPressed;
        }

        protected override void onExitOverride()
        {
            _uiElements.PlayUI.SetActive(false);
            PlayUIEventInvoker.PlayPressedInPlayMode -= PlayPressed;
            PlayUIEventInvoker.PausePressedInPlayMode -= PausePressed;
            PlayUIEventInvoker.StepPressedInPlayMode -= StepPressed;
            Debug.Log("exited play mode");
        }

        public void PlayPressed() => pauseState = PauseState.Play;
        public void PausePressed() => pauseState = PauseState.Pause;
        public void StepPressed() => pauseState = PauseState.Step;
    }

    public enum PauseState
    {
        Play,
        Pause,
        Step
    }
}