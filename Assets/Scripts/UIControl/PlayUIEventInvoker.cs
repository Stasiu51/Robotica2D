using System;
using UnityEngine;

namespace UIControl
{
    public class PlayUIEventInvoker : MonoBehaviour
    {

        public static event Action PlayPressedInPlayMode;
        public static event Action PausePressedInPlayMode;
        public static event Action StepPressedInPlayMode;
        
        public static void onPlayPressedInPlayMode() => PlayPressedInPlayMode?.Invoke();
        public static void onPausePressedInPlayMode() => PausePressedInPlayMode?.Invoke();
        public static void onStepPressedInPlayMode() => StepPressedInPlayMode?.Invoke();
    }
}