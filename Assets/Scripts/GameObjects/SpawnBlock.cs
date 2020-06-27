﻿using System.Collections.Generic;
using UnityEngine.UI;

namespace GameObjects
{
    public class SpawnBlock : SingleBlock
    {
        private readonly Routing _routing = Routing.newSpawnRouting();
        
        public override int getMass() => SINGLEBLOCKMASS;

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

        public bool isActiveFromDir(int dir, int channel)
        {
            SpawnRouting sr = (SpawnRouting) _routing.AddRouting;
            List<int> funcs = sr.funcsConnectedToDir(dir, channel);
            return funcs.Count > 0;
        }

        public bool isActiveFromAnyDir(List<int> inDirs, int channel)
        {
            foreach (int dir in inDirs)
            {
                if (isActiveFromDir(dir, channel)) return true;
            }

            return false;
        }

        
    }
}