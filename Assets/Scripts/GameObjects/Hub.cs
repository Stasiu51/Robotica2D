using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using ObjectAccess;
using UnityEngine;

namespace GameObjects
{
    public class Hub : Block
    {

        public StructureBlock[] padding = new StructureBlock[6];
        private TimerSource _timerSource = new TimerSource();
        const int HUBMASS = 100;
        private Routing _routing = Routing.newBasicRouting();

        public override bool HasSource => true;
        public override int Mass => HUBMASS;

        public override Source Source => _timerSource;

        public override bool Selectable => true;
        //TODO HUB PREFAB
        protected override GameObject createGameObject() => GameObject.Instantiate(Access.prefabs.structureBlock);
        public override void initialSetupOverride()
        {
            Debug.Log("hi");
            foreach (StructureBlock structureBlock in padding)
            {
                setStuck(structureBlock, true);
            }
        }

        public override Routing Routing => _routing;
    }
}
