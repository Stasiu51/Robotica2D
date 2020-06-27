using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

namespace GameObjects
{
    public class Routing
    {   
    
        public AddRouting AddRouting;

        private bool[,,] _connected;
        private bool[] _transmissionConnect;
        private bool _transmitting;

        private static bool[,,] allTrue(int dim)
        {
            bool[,,] r = new bool[3,dim,dim];
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    r[0, i, j] = true;
                    r[1, i, j] = true;
                    r[2, i, j] = true;
                }
            }

            return r;
        }

        public static Routing newSpawnRouting(bool[,,] initConnected = null)
        {
            if (initConnected == null) initConnected = allTrue(6);
            Routing routing = new Routing();
            routing.AddRouting = new SpawnRouting();
            routing._connected = initConnected;
            return routing;
        }

        public static Routing newBasicRouting(bool[,,] initConnected = null)
        {
            if (initConnected == null) initConnected = allTrue(6);
            Routing routing = new Routing();
            routing._connected = initConnected;
            return routing;
        }

        public void setConnected(bool isconnected, int dir1, int dir2, int channel)
        {
            _connected[channel,dir1, dir2] = isconnected;
            _connected[channel,dir2, dir1] = isconnected;
        }

        public bool getConnected(int dir1, int dir2, int channel)
        {
            return (_connected[channel,dir1, dir2]);
        }

        public List<int> getAllFrom(int dir, int channel)
        {
            return new List<int>(from num in Enumerable.Range(0, 6) where _connected[channel,dir, num] select num);
        }


    }

    public abstract class AddRouting
    {
        public enum ArType
        {
            Spawn,
            Brain,
            Rotate,
            Memory
        }

        public abstract ArType getType();

        public abstract void setConnected(bool isconnected, int dir1, int func, int channel);
        public abstract bool getConnected(int dir1, int func, int channel);
    }

    public class SpawnRouting : AddRouting
    {
        //0-6 are spawns N to NW
        private bool[,,] _spawnConnect;
    
        public SpawnRouting()
        {
            _spawnConnect = new bool[3,6, 6];
            foreach (int dir in Enumerable.Range(0,6))
            {
                _spawnConnect[0, dir, 5] = true;
            }
        }

        public override void setConnected(bool isconnected, int dir1, int func, int channel)
        {
            if (func < 0 || func > 6) { throw new System.ArgumentOutOfRangeException(); }
            _spawnConnect[channel,dir1, func] = isconnected;
        }

        public override bool getConnected(int dir1, int func, int channel)
        {
            if (func < 0 || func > 6) { throw new System.ArgumentOutOfRangeException(); }
            return _spawnConnect[channel, dir1, func];
        }

        public List<int> funcsConnectedToDir(int dir, int channel)
        {
            return new List<int>(from func in Enumerable.Range(0, 6) where getConnected(dir,func,channel) select func);
        }

        public override ArType getType()
        {
            return ArType.Spawn;
        }
    }
}