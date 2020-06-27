using System;
using GameControl.SubModes_1.EditMode;
using GameObjects;
using UnityEngine;

namespace UIControl
{
    // ReSharper disable once InconsistentNaming
    public class UIEventInvoker : MonoBehaviour
    {
        public delegate void ClickAction();

        public static event ClickAction OnAdvance;
        public static event ClickAction AddSpawn;
        public static event ClickAction AddBrain;
        public static event ClickAction AddStructure;
        
        public static event ClickAction PlayPressedInEdit;
        public static event ClickAction EditPressedInPlay;


        public delegate void BlockSelectAction(Block block);

        public delegate void TileSelectAction(Vector2Int coords);
        

        public void advancePressed() => OnAdvance?.Invoke();

        public void addSpawn() => AddSpawn?.Invoke();
        public void addBrain() => AddBrain?.Invoke();

        public void addStructure() => AddStructure?.Invoke();
        

        public void playFromEdit() => PlayPressedInEdit?.Invoke();
        public void editFromPlay() => EditPressedInPlay?.Invoke();

    }
}