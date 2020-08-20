using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using GameControl.SubModes_1.EditMode;
using ObjectAccess;
using UIControl;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

namespace GameObjects
{
    public class HexGrid : MonoBehaviour
    {
        private const float Sidelen = 1;
        private static readonly float Ratio = Mathf.Sqrt(3) / 2;
        public int extent = 10;
        private Vector2Int _currentMousePos = Vector2Int.zero;
        private Camera _camera;

        public static Vector3 getPosFromCoords(Vector2Int pos)
        {
            return getPosFromCoords(pos.x, pos.y);
        }
        public static Vector3 getPosFromCoords(int col, int row)
        {
            return new Vector3(Sidelen * 2 * (col + 0.5f*row) *Ratio, row* Sidelen*1.5f, 0);
        }

        public static Vector2Int getCoordsFromPos(Vector3 pos)
        {
            int y = Mathf.RoundToInt(pos.y / (Sidelen * 1.5f));
            int x = Mathf.RoundToInt((pos.x - y*Sidelen*Ratio) / (Sidelen * 2 * Ratio));
            return new Vector2Int(x,y);
        }

        public void Start()
        {
            // GameObject tile = Access.prefabs.backgroundHex;
            // for (int x = -extent; x < extent; x++)
            // {
            //     for (int y = -extent; y < extent; y++)
            //     {
            //         GameObject go = Instantiate(tile,parent: transform);
            //         TileManager tm = go.GetComponent<TileManager>();
            //         tm.setPos(new Vector2Int(x,y));
            //         go.transform.localPosition = getPosFromCoords(x, y);
            //     }
            // }
        }

        public Vector2Int mouseCoords()
        {
            Vector3 mousePos = Input.mousePosition;
            // Debug.Log(mousePos);
            mousePos.z = Mathf.Abs(Camera.main.transform.position.z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
            // Debug.Log(worldPosition);
            return getCoordsFromPos(worldPosition);
        }

        


        public Vector3 mousePos() => getPosFromCoords(mouseCoords());

        public void mouseMoved(Vector2Int newCoords)
        {
            _currentMousePos = newCoords;
        }

        public static List<Vector2Int> adjacentTo(Vector2Int pos)
        {
            return new List<Vector2Int>(from dir in Dir.allDirs() select pos + dir.v);
        }

        public static Dir relativeDir(Vector2Int from, Vector2Int to)
        {
            return Dir.getDir(to - from);
        }
    }

    public class Dir
    {
        public readonly Vector2Int v;
        public readonly int N;

        public static IEnumerable<Dir> allDirs() => new List<Dir> {E, SE, SW, W, NE, NW, W};
        private Dir(Vector2Int d ,int n)
        {
            v = d;
            N = n;
        }
        public static Dir E = new Dir(new Vector2Int(1,0) ,0);
        public static Dir SE = new Dir(new Vector2Int(1,-1),1);
        public static Dir SW = new Dir(new Vector2Int(0,-1),2);
        public static Dir W = new Dir(new Vector2Int(-1, 0), 3);
        public static Dir NW = new Dir(new Vector2Int(-1, 1), 4);
        public static Dir NE = new Dir(new Vector2Int(0, 1), 5);

        public static Dir getDir(Vector2Int vector)
        {
            if (vector == E.v) return E;
            if (vector == SE.v) return SE;
            if (vector == SW.v) return SW;
            if (vector == W.v) return W;
            if (vector == NW.v) return NW;
            if (vector == NE.v) return NE;
            throw new ArgumentException("not a valid direction");
        }

        public static Dir dirFromN(int n)
        {
            switch (n)
            {
                case 0:
                    return E;
                case 1:
                    return SE;
                case 2:
                    return SW;
                case 3:
                    return W;
                case 4:
                    return NW;
                case 5:
                    return NE;
                
            }
            throw new ArgumentException("must be 0 - 6");
        }

        public Dir Opposite()
        {
            return dirFromN((N + 3) % 6);
        }
        
    }

    public class Chn
    {
        public readonly int N;

        private Chn(int n)
        {
            N = n;
        }
        
        public static Chn Red = new Chn(0);
        public static Chn Yellow = new Chn(1);
        public static Chn Blue = new Chn(2);

        public static IEnumerable<Chn> allChannels() => new List<Chn> {Red, Yellow, Blue};

        public static Chn chnFromN(int n)
        {
            return ((List<Chn>) Chn.allChannels())[n];
        }
    }
    
    
}
