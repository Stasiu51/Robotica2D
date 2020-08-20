using System;
using ObjectAccess;
using UnityEngine;

namespace GameObjects
{
    [Serializable]
    public class StructureBlock : Block
    {
        private readonly Routing _routing = Routing.newBasicRouting();
        public override int Mass => BLOCKMASS;

        //TODO implement programmable
        public override bool Selectable => true;
        
        protected override GameObject createGameObject() => GameObject.Instantiate(Access.prefabs.structureBlock);

        public override void initialSetupOverride()
        {
        }

        public override Routing Routing
        {
            get { return _routing; }
        }

        public override Source Source
        {
            get { throw new System.Exception("Structure has no source"); }
        }

        public override bool HasSource => false;
    }
}