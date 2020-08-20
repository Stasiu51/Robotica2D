using System;
using ObjectAccess;
using UnityEngine;

namespace GameObjects
{
    [Serializable]
    public class BrainBlock : Block
    {
        private readonly Routing _routing = Routing.newBasicRouting();
        private Source _source = new TestSource();
        public override bool Selectable => true;
        
        protected override GameObject createGameObject() => GameObject.Instantiate(Access.prefabs.brainBlock);

        public override void initialSetupOverride()
        {
        }

        public override Routing Routing => _routing;
        public override Source Source => _source;
        public override bool HasSource => true;
        public override int Mass => BLOCKMASS;
    }
}