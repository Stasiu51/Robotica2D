using GameObjects;
using UnityEngine;

namespace UIControl
{
    public class TileManager : MonoBehaviour
    {
        // Start is called before the first frame update
        private Vector2Int _pos;
        private HexGrid _hexGrid;

        public void OnEnable()
        {
            _hexGrid = transform.parent.gameObject.GetComponent<HexGrid>();
        }

        public void setPos(Vector2Int pos)
        {
            _pos = pos;
        }

        void OnMouseEnter()
        {
            _hexGrid.mouseMoved(_pos);
        }
    }
}
