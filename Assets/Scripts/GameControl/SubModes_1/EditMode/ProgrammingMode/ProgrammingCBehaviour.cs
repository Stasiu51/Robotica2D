using System;
using System.Runtime.CompilerServices;
using GameObjects;
using ObjectAccess;
using Serialisation;
using UnityEngine;

namespace GameControl.SubModes_1.EditMode.ProgrammingMode
{
    public class ProgrammingCBehaviour : CSubStateMachine
    {
        private static ProgrammingCBehaviour _instance ;
        
        private Block _blockToProgram;
        public Block blockToProgram
        {
            get => _blockToProgram;
            set => _blockToProgramNext = value;
        }
        private Block _blockToProgramNext;
        public override CBehaviour getInstance()
        {
            if (_instance == null) _instance = new ProgrammingCBehaviour();
            return _instance;
        }
        public override Type belongsTo() => typeof(EditCBehaviour);

        private UIElements _uiElements;


        protected override CBehaviour getInitialBehaviour()
        {
            if (blockToProgram.GetType() == typeof(SpawnBlock))
            {
                return new ProgramSpawnIdle().getInstance();
            }

            Debug.Log("cannot program");
            return null;
        }

        public override void onEnterOverride()
        {
            if (_uiElements == null) _uiElements = Access.uiElements;
            _uiElements.ProgrammingUI.SetActive(true);
            _blockToProgram = _blockToProgramNext;
            Debug.Log("enter program");
        }

        protected override void onExitOverride()
        {
            _uiElements.ProgrammingUI.SetActive(false);
            UndoSystem.saveUndo();
            Debug.Log("exit program");
        }
    }
}