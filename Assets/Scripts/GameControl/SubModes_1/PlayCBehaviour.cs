using System;
using GameControl.SubModes_1.PlayMode;
using ObjectAccess;
using UnityEngine;

namespace GameControl.SubModes_1
{
    public class PlayCBehaviour: CSubStateMachine

    {
        public int Turn { get; private set; }
        
        private static PlayCBehaviour _instance ;
        private UIElements _uiElements;

        public override CBehaviour getInstance()
        {
            if (_instance == null) _instance = new PlayCBehaviour();
            return _instance;
        }
        public override Type belongsTo() => typeof(GameMachine);
        

        protected override CBehaviour getInitialBehaviour()
        {
            return new PlaySignalPhase().getInstance();
        }

        public override void onEnterOverride()
        {
            Debug.Log("entered play mode");
            if (_uiElements == null) _uiElements = GameObject.Find("ObjectAccess").GetComponent<UIElements>();
            _uiElements.PlayUI.SetActive(true);
            Turn = 0;
        }

        protected override void onExitOverride()
        {
            _uiElements.PlayUI.SetActive(false);
            Debug.Log("exited play mode");
        }

        public void turnInc()
        {
            Debug.Log("next turn");
            Turn++;
        }
    }
}