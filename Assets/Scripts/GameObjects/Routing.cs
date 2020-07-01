using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

namespace GameObjects
{
    public class Routing
    {

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
        public static Routing newBasicRouting(bool[,,] initConnected = null)
        {
            if (initConnected == null) initConnected = allTrue(6);
            Routing routing = new Routing();
            routing._connected = initConnected;
            return routing;
        }

        public void setConnected(bool isconnected, Dir dir1, Dir dir2, Chn channel)
        {
            _connected[channel.N,dir1.N, dir2.N] = isconnected;
            _connected[channel.N,dir2.N, dir1.N] = isconnected;
        }

        public bool getConnected(Dir dir1, Dir dir2, Chn channel)
        {
            return (_connected[channel.N,dir1.N, dir2.N]);
        }

        public IEnumerable<Dir> getAllFrom(Dir fromDir, Chn channel)
        {
            return new List<Dir>(from toDir in Dir.allDirs() where getConnected(fromDir,toDir,channel) select toDir);
        }


    }


}