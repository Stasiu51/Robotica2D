using System;
using GameObjects;
using ObjectAccess;
using UIControl;
using UnityEngine;

namespace GameControl.SubModes_1.EditMode.ProgrammingMode
{
    public class ProgramSpawnIdle : CBehaviour
    {
        private static ProgramSpawnIdle _instance ;
        
        public override CBehaviour getInstance()
        {
            if (_instance == null) _instance = new ProgramSpawnIdle();
            return _instance;
        }
        public override Type belongsTo() => typeof(ProgrammingCBehaviour); 
        
        private UIElements _uiElements;
        
        private SpawnBlock _blockToProgram;
        


        public override void onEnter()
        {
            Debug.Log("prog idle");
            _blockToProgram = (SpawnBlock) ((ProgrammingCBehaviour) new ProgrammingCBehaviour().getInstance()).blockToProgram;
            if (_uiElements == null) _uiElements = Access.uiElements;
            _uiElements.SpawnProgramUI.SetActive(true);
            _uiElements.SpawnProgramUI.GetComponent<SpawnUIManager>().setSpawnToggles(_blockToProgram.spawnRouting);
            
            
            EditUIEventInvoker.SpawnToggle += spawnToggle;

        }

        public override void onExit()
        {
            _uiElements.SpawnProgramUI.SetActive(false);
            EditUIEventInvoker.SpawnToggle -= spawnToggle;
            Debug.Log("exit prog idle");
        }

        protected override CBehaviour updateOverride()
        {
            return null;
        }

        public override CBehaviour ongui(Event e)
        {
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Escape) return new EditIdle().getInstance();
            return null;
        }

        private void spawnToggle(Chn channel, Dir dir, bool connected)
        {
            foreach (var fromDir in Dir.allDirs())
            {
                _blockToProgram.spawnRouting.setConnected(connected,fromDir, dir,channel);
            }
        }
        

    }
}