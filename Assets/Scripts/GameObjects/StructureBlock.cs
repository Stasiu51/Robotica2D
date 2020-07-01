using UnityEngine;

namespace GameObjects
{
    public class StructureBlock : SingleBlock
    {
        private readonly Routing _routing = Routing.newBasicRouting();
        private bool _selectable = true;
        
        public override int getMass() => SINGLEBLOCKMASS;

        public override bool selectable() => _selectable;

        public override void initialSetupOverride()
        {
            Debug.Log("setup");
            _selectable = false;
        }

        public override Routing getRouting()
        {
            return _routing;
        }

        public override Source getSource()
        {
            throw new System.Exception("Structure has no source");
        }

        public override bool hasSource()
        {
            return false;
        }

        
    }
}