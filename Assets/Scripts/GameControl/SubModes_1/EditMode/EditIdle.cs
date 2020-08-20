using System;
using System.Collections.Generic;
using GameControl.SubModes_1.EditMode.ProgrammingMode;
using GameObjects;
using ObjectAccess;
using Serialisation;
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

            Managers managers = Access.managers;
            
            if (_selectorManager == null)
                _selectorManager = managers.selectorManager;
            if (_blocks == null) _blocks = managers.Blocks;
            if (_hexGrid == null) _hexGrid = managers.hexGrid;

            EditUIEventInvoker.AddSpawn += addSpawn;
            EditUIEventInvoker.AddBrain += addBrain;
            EditUIEventInvoker.AddStructure += addStructure;
            EditUIEventInvoker.PlayPressedInEditMode += playPressed;
            EditUIEventInvoker.PausePressedInEditMode += pausePressed;
            EditUIEventInvoker.StepPressedInEditMode += stepPressed;
            EditUIEventInvoker.UndoPressed += undo;
            EditUIEventInvoker.RedoPressed += redo;

            _selectorManager.gameObject.SetActive(true);

        }
        
        void addSpawn() => Exit(new EditAddSpawnBlock().getInstance());
        void addBrain() => Exit(new EditAddBrainBlock().getInstance());
        
        void addStructure() => Exit(new EditAddStructureBlock().getInstance());

        void playPressed()
        {
            PlayCBehaviour playB = (PlayCBehaviour) new PlayCBehaviour().getInstance();
            playB.PlayPressed();
            Exit(new PlayCBehaviour().getInstance());
        } 
        void pausePressed()
        {
            PlayCBehaviour playB = (PlayCBehaviour) new PlayCBehaviour().getInstance();
            playB.PausePressed();
            Exit(new PlayCBehaviour().getInstance());
        } 
        void stepPressed()
        {
            PlayCBehaviour playB = (PlayCBehaviour) new PlayCBehaviour().getInstance();
            playB.StepPressed();
            Exit(new PlayCBehaviour().getInstance());
        } 

        public override void onExit()
        {
            _selectorManager.gameObject.SetActive(false);
            EditUIEventInvoker.AddSpawn -= addSpawn;
            EditUIEventInvoker.AddBrain -= addBrain;
            EditUIEventInvoker.AddStructure -= addStructure;
            EditUIEventInvoker.PlayPressedInEditMode -= playPressed;
            EditUIEventInvoker.PausePressedInEditMode -= pausePressed;
            EditUIEventInvoker.StepPressedInEditMode -= stepPressed;
            EditUIEventInvoker.UndoPressed -= undo;
            EditUIEventInvoker.RedoPressed -= redo;
            Debug.Log("exited idle");
        }

        protected override CBehaviour updateOverride()
        {
            return null;
        }
        
        private void undo()
        {
            UndoSystem.undo();
        }

        private void redo()
        {
            UndoSystem.redo();
        }


        public override CBehaviour ongui(Event e)
        {
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                Vector2Int coords = _hexGrid.mouseCoords();
                Block b = _blocks.blockAtPos(coords);
                if (b == null || !b.Selectable)
                {
                    _selectorManager.deselectAll();
                    return null;
                }
                if (e.shift) _selectorManager.switchSelected(b);
                    else _selectorManager.switchSelectOnly(b);
            }
            
            if (e.type == EventType.MouseDown && e.button == 1)
            {
                Vector2Int coords = _hexGrid.mouseCoords();
                Block b = _blocks.blockAtPos(coords);
                if (b == null || !b.Selectable)
                {
                    _selectorManager.deselectAll();
                    return null;
                }
                _selectorManager.selectOnly(b);
                var progBev = (ProgrammingCBehaviour) new ProgrammingCBehaviour().getInstance();
                progBev.blockToProgram = b;
                return progBev;
            }
            
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Escape) _selectorManager.deselectAll();

            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.P)
            {
                Block b = _selectorManager.RecentSelect;
                if (b == null) return null;
                var progBev = (ProgrammingCBehaviour) new ProgrammingCBehaviour().getInstance();
                progBev.blockToProgram = b;
                return progBev;
            }
            
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Z && e.modifiers == EventModifiers.Control)
            {
                undo();
                return null;
            }
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Z 
                                            && e.modifiers == (EventModifiers.Control | EventModifiers.Shift))
            {
                redo();
                return null;
            }

            
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Backspace)
            {
                List<Block> blocks = _selectorManager.getSelected();
                if (blocks.Count == 0) return null;
                _selectorManager.deselectAll();
                foreach (Block block in blocks) _blocks.destroyBlock(block);
                UndoSystem.saveUndo();
                return null;
            }

            

            return null;
        }
        
        
        

        
        
    }
}