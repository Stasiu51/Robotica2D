using System;
using GameObjects;
using ObjectAccess;
using UIControl;
using UnityEngine;

namespace GameControl.SubModes_1.EditMode
{
    public class EditIdle : CBehaviour
    {
        public override Type belongsTo() => typeof(EditCBehaviour);
        
        private Blocks _blocks;
        private SelectorManager _selectorManager;
        private HexGrid _hexGrid;
        private static EditIdle _instance ;
        public override CBehaviour getInstance()
        {
            if (_instance == null) _instance = new EditIdle();
            return _instance;
        }

        

        public override void onEnter()
        {
            Debug.Log("entered idle");

            Managers managers = GameObject.Find("ObjectAccess").GetComponent<Managers>();
            
            if (_selectorManager == null)
                _selectorManager = managers.selectorManager;
            if (_blocks == null) _blocks = managers.Blocks;
            if (_hexGrid == null) _hexGrid = managers.hexGrid;

            UIEventInvoker.AddSpawn += addSpawn;
            UIEventInvoker.AddBrain += addBrain;
            UIEventInvoker.AddStructure += addStructure;
            
            UIEventInvoker.PlayPressedInEdit += exitToPlay;

            _selectorManager.gameObject.SetActive(true);

        }
        
        void addSpawn() => Exit(new EditAddSpawnBlock().getInstance());
        void addBrain() => Exit(new EditAddBrainBlock().getInstance());
        
        void addStructure() => Exit(new EditAddStructureBlock().getInstance());
        void exitToPlay() => Exit(new PlayCBehaviour().getInstance());

        public override void onExit()
        {
            _selectorManager.gameObject.SetActive(false);
            UIEventInvoker.AddSpawn -= addSpawn;
            UIEventInvoker.AddBrain -= addBrain;
            UIEventInvoker.AddStructure -= addStructure;
            UIEventInvoker.PlayPressedInEdit -= exitToPlay;
            Debug.Log("exited idle");
        }

        protected override CBehaviour updateOverride()
        {
            return null;
        }

        public override CBehaviour ongui(Event e)
        {
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                Vector2Int coords = _hexGrid.mouseCoords();
                Block b = _blocks.blockAtPos(coords);
                if (b == null)
                {
                    _selectorManager.deselectAll();
                    return null;
                }
                if (e.shift) _selectorManager.switchSelected(b);
                    else _selectorManager.selectOnly(b);
            }
            
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Escape) _selectorManager.deselectAll();

            return null;
        }
        
        
        

        
        
    }
}