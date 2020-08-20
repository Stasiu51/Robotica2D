using GameObjects;
using UIControl;
using UnityEngine;
using UnityEngine.Serialization;

namespace ObjectAccess
{
    public class Managers : MonoBehaviour
    {
        public SelectorManager selectorManager;
        public Blocks Blocks;
        public HexGrid hexGrid;
        public EditUIEventInvoker editUiEventInvoker;
    }
}
