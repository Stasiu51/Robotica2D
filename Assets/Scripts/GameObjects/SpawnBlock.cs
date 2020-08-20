using System;
using System.Collections.Generic;
using System.Linq;
using ObjectAccess;
using Serialisation;
using UnityEngine;
using UnityEngine.UI;

namespace GameObjects
{
    [Serializable]
    public class SpawnBlock : Block
    {
        private readonly Routing _routing = Routing.newBasicRouting();
        public readonly SpawnRouting spawnRouting = new SpawnRouting();
        private Block template;
        
        [SerializeField]
        public override int Mass => BLOCKMASS;

        public override bool Selectable => true;

        protected override GameObject createGameObject() => GameObject.Instantiate(Access.prefabs.spawnBlock);
        public override void initialSetupOverride()
        {
            //TODO get template
            template = new StructureBlock();
            template.SetVisible(false);
        }

        public override Routing Routing => _routing;

        public override Source Source => throw new System.Exception("Spawn has no source");

        public override bool HasSource => false;

        public Dir activeFromDir(Dir dir, Chn channel)
        {
            List<Dir> funcs = spawnRouting.funcsConnectedToDir(dir, channel);
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

        public Block spawnNewBlock()
        {
            return template.clone();
        }
        [Serializable]

        public class SpawnRouting
        {
            private bool[,,] _spawnConnect;
    
            public SpawnRouting()
            {
                Debug.Log("spawnblock constructor");
                _spawnConnect = new bool[3,6, 6];
            }
            public void setConnected(bool isConnected, Dir dir1, Dir func, Chn channel)
            {
                _spawnConnect[channel.N,dir1.N, func.N] = isConnected;
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
