using System;
using GameControl.SubModes_1.EditMode;
using GameObjects;
using UnityEngine;

namespace UIControl
{
    // ReSharper disable once InconsistentNaming
    public class EditUIEventInvoker : MonoBehaviour
    {
        public delegate void WireAction(Chn channel, Dir dir, bool Connected);
        
        public static event Action AddSpawn;
        public static event Action AddBrain;
        public static event Action AddStructure;

        public static event Action UndoPressed;
        public static event Action RedoPressed;
        
        public static event WireAction WirePointClicked;

        public static event WireAction SpawnToggle;

        public static event Action PlayPressedInEditMode;
        public static event Action PausePressedInEditMode;
        public static event Action StepPressedInEditMode;



        public delegate void BlockSelectAction(Block block);

        public delegate void TileSelectAction(Vector2Int coords);
        public void addSpawn() => AddSpawn?.Invoke();
        public void addBrain() => AddBrain?.Invoke();
        public void addStructure() => AddStructure?.Invoke();
        public void spawnToggleClicked(Dir dir, Chn channel, bool Connected) => SpawnToggle?.Invoke(channel, dir, Connected);


        public static void onPlayPressedInEditMode() => PlayPressedInEditMode?.Invoke();
        public static void onPausePressedInEditMode() => PausePressedInEditMode?.Invoke();
        public static void onStepPressedInEditMode() => StepPressedInEditMode?.Invoke();

        public static void onUndoPressed() => UndoPressed?.Invoke();
        public static void onRedoPressed() => RedoPressed?.Invoke();
    }
}