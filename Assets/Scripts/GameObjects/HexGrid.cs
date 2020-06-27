using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using ObjectAccess;
using UIControl;
using UnityEngine;

namespace GameObjects
{
    public class HexGrid : MonoBehaviour
    {
        private const float Sidelen = 1;
        private static readonly float Ratio = Mathf.Sqrt(3) / 2;
        public int extent = 10;
        private Vector2Int _currentMousePos = Vector2Int.zero;
        public UIEventInvoker _uiEventInvoker;

        private static readonly Dictionary<int, Vector2Int> evenYDirs = new Dictionary<int, Vector2Int>()
        {
            {0, new Vector2Int(1, 0)},
            {1, new Vector2Int(0, -1)},
            {2, new Vector2Int(-1, -1)},
            {3, new Vector2Int(-1, 0)},
            {4, new Vector2Int(-1, 1)},
            {5, new Vector2Int(0, 1)}
        };
        private static readonly Dictionary<int, Vector2Int> oddYDirs = new Dictionary<int, Vector2Int>()
        {
            {0, new Vector2Int(1, 0)},
            {1, new Vector2Int(1, -1)},
            {2, new Vector2Int(0, -1)},
            {3, new Vector2Int(-1, 0)},
            {4, new Vector2Int(0, 1)},
            {5, new Vector2Int(1, 1)}
        };
        private static readonly Dictionary<Vector2Int,int> evenYDirsInv = new Dictionary<Vector2Int,int>()
        {
            {new Vector2Int(1, 0),0},
            {new Vector2Int(0, -1),1},
            {new Vector2Int(-1, -1),2},
            {new Vector2Int(-1, 0),3},
            {new Vector2Int(-1, 1),4},
            {new Vector2Int(0, 1),5}
        };
        private static readonly Dictionary<Vector2Int, int> oddYDirsInv = new Dictionary<Vector2Int, int>()
        {
            { new Vector2Int(1, 0),0},
            { new Vector2Int(1, -1),1},
            { new Vector2Int(0, -1),2},
            { new Vector2Int(-1, 0),3},
            { new Vector2Int(0, 1),4},
            { new Vector2Int(1, 1),5}
        };
        public static Vector3 getPosFromCoords(Vector2Int pos)
        {
            return getPosFromCoords(pos.x, pos.y);
        }
        public static Vector3 getPosFromCoords(int col, int row)
        {
            if (row % 2 == 0)
            {
                return new Vector3(Sidelen * 2 * col*Ratio, row* Sidelen*1.5f, 0);
            }
            return new Vector3(Sidelen * 2 * (col + 0.5f)*Ratio, row * Sidelen*1.5f, 0);
        }

        public static Vector2Int getCoordsFromPos(Vector3 pos)
        {
            int y = Mathf.RoundToInt(pos.y / (Sidelen * 1.5f));
            int x;
            if (y % 2 == 0) x = Mathf.RoundToInt(pos.x / (Sidelen * 2 * Ratio));
            else x = Mathf.RoundToInt((pos.x - Sidelen*Ratio) / (Sidelen * 2 * Ratio));
            return new Vector2Int(x,y);
        }

        public void Start()
        {
            GameObject tile = GameObject.Find("ObjectAccess").GetComponent<Prefabs>().backgroundHex;
            for (int x = -extent; x < extent; x++)
            {
                for (int y = -extent; y < extent; y++)
                {
                    GameObject go = Instantiate(tile,parent: transform);
                    TileManager tm = go.GetComponent<TileManager>();
                    tm.setPos(new Vector2Int(x,y));
                    go.transform.localPosition = getPosFromCoords(x, y);
                }
            }
        }

        public Vector2Int mouseCoords()
        {
            return _currentMousePos;
        }

        public Vector3 mousePos() => getPosFromCoords(_currentMousePos);

        public void mouseMoved(Vector2Int newCoords)
        {
            _currentMousePos = newCoords;
        }

        public static List<Vector2Int> adjacentTo(Vector2Int pos)
        {
            IEnumerable<int> numbers = Enumerable.Range(0, 6);
            if (pos.y % 2 == 0)
            {
                return new List<Vector2Int>(from num in numbers select pos + evenYDirs[num]);
            }
            else
            {
                return new List<Vector2Int>( from num in numbers select pos + oddYDirs[num]);
            }
        }

        public static int relativeDir(Vector2Int from, Vector2Int to)
        {
            return @from.y % 2 == 0 ? evenYDirsInv[to - @from] : oddYDirsInv[to - @from];
        }
    }
    
    
}
