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

        public void setConnected(bool isconnected, Dir dir1, Dir dir2, int channel)
        {
            _connected[channel,dir1.N, dir2.N] = isconnected;
            _connected[channel,dir2.N, dir1.N] = isconnected;
        }

        public bool getConnected(Dir dir1, Dir dir2, int channel)
        {
            return (_connected[channel,dir1.N, dir2.N]);
        }

        public IEnumerable<Dir> getAllFrom(Dir fromDir, int channel)
        {
            return new List<Dir>(from toDir in Dir.allDirs() where _connected[channel,fromDir.N, toDir.N] select toDir);
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

        public abstract void setConnected(bool isconnected, Dir dir1, int func, int channel);
        public abstract bool getConnected(Dir dir1, int func, int channel);
    }

    public class SpawnRouting : AddRouting
    {
        private bool[,,] _spawnConnect;
    
        public SpawnRouting()
        {
            _spawnConnect = new bool[3,6, 6];
            foreach (int dir in Enumerable.Range(0,6))
            {
                _spawnConnect[0, dir, 5] = true;
            }
        }

        public override void setConnected(bool isconnected, Dir dir1, int func, int channel)
        {
            if (func < 0 || func > 6) { throw new System.ArgumentOutOfRangeException(); }
            _spawnConnect[channel,dir1.N, func] = isconnected;
        }

        public override bool getConnected(Dir dir1, int func, int channel)
        {
            if (func < 0 || func > 6) { throw new System.ArgumentOutOfRangeException(); }
            return _spawnConnect[channel, dir1.N, func];
        }

        public List<int> funcsConnectedToDir(Dir dir, int channel)
        {
            return new List<int>(from func in Enumerable.Range(0, 6) where getConnected(dir,func,channel) select func);
        }

        public override ArType getType()
        {
            return ArType.Spawn;
        }
    }
}