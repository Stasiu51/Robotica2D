using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace GameObjects
{
    public class SpawnBlock : SingleBlock
    {
        private readonly Routing _routing = Routing.newBasicRouting();
        private readonly SpawnRouting _spawnRouting = new SpawnRouting();
        
        public override int getMass() => SINGLEBLOCKMASS;

        public override bool selectable() => true;
        public override void initialSetupOverride()
        {
        }

        public override Routing getRouting()
        {
            return _routing;
        }

        public override Source getSource()
        {
            throw new System.Exception("Spawn has no source");
        }

        public override bool hasSource()
        {
            return false;
        }

        public Dir activeFromDir(Dir dir, Chn channel)
        {
            List<Dir> funcs = _spawnRouting.funcsConnectedToDir(dir, channel);
            if (funcs.Count == 0) return null;
            return funcs[0];
        }

        public bool isActiveFromAnyDir(IEnumerable<Dir> inDirs, Chn channel)
        {
            foreach (Dir dir in inDirs)
            {
                if (activeFromDir(dir, channel) != null) return true;
            }

            return false;
        }
        
        private class SpawnRouting
        {
            private bool[,,] _spawnConnect;
    
            public SpawnRouting()
            {
                _spawnConnect = new bool[3,6, 6];
                foreach (Dir dir in Dir.allDirs())
                {
                    _spawnConnect[0, dir.N, 0] = true;
                }
            }
            public void setConnected(bool isconnected, Dir dir1, Dir func, Chn channel)
            {
                _spawnConnect[channel.N,dir1.N, func.N] = isconnected;
            }

            public bool getConnected(Dir dir1, Dir func, Chn channel)
            {
                return _spawnConnect[channel.N, dir1.N, func.N];
            }

            public List<Dir> funcsConnectedToDir(Dir signalDir, Chn channel)
            {
                return new List<Dir>(from funcDir in Dir.allDirs() where getConnected(signalDir,funcDir,channel) select funcDir);
            }
        }
        

        
    }
}
