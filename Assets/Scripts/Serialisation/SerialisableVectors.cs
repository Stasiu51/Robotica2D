using System;
using UnityEngine;

namespace Serialisation
{
    [Serializable]
    public class SerialisableVector3
    {
        private float x;
        private float y;
        private float z;
        public SerialisableVector3(Vector3 vector3)
        {
            (x, y, z) = (vector3.x, vector3.y, vector3.z);
        }
        public Vector3 vector3 => new Vector3(x,y,z);
    }

    [Serializable]
    public class SerialisableVector2Int
    {
        private int x;
        private int y;

        public SerialisableVector2Int(Vector2Int vector2Int)
        {
            (x, y) = (vector2Int.x, vector2Int.y);
        }
        
        public Vector2Int vector2Int => new Vector2Int(x,y);
    }
}