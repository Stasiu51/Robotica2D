using System;
using System.Collections;
using System.Collections.Generic;
using GameControl.SubModes_1;
using UnityEngine;
using UnityEngine.UI;

namespace GameControl
{
    
    public class GameMachine : CSubStateMachine
    {
        public  GameMachine()
        {
        }

        public override Type belongsTo()
        {
            throw new NotImplementedException();
        }

        private static GameMachine _instance ;

        public override CBehaviour getInstance()
        {
            if (_instance == null) _instance = new GameMachine();
            return _instance;
        }

        protected override CBehaviour getInitialBehaviour()
        {
            return new EditCBehaviour().getInstance();
        }

        public override void onEnterOverride()
        {
            Debug.Log("entering superstate");
        }

        protected override void onExitOverride()
        {
            Debug.Log("exited superstate");
        }
    }

    public class GameManager : MonoBehaviour
    {
        private readonly GameMachine _gameMachine = new GameMachine();
        public void Start() => _gameMachine.onEnter();
        public void Update() => _gameMachine.update();
        public void OnGUI() => _gameMachine.ongui(Event.current);
    }


}