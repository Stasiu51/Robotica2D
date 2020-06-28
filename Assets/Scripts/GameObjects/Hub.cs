using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace GameObjects
{
    public class Hub : SingleBlock
    {

        public StructureBlock[] padding = new StructureBlock[6];
        private TimerSource _timerSource = new TimerSource();
        const int HUBMASS = 100;
        private Routing _routing = Routing.newBasicRouting();

        public override bool hasSource() => true;
        public override int getMass() => HUBMASS;
        public override Source getSource() => _timerSource;

        public override bool selectable() => true;

        public override void initialSetup()
        {
            Debug.Log("hi");
            foreach (StructureBlock structureBlock in padding)
            {
                setStuck(structureBlock);
            }
        }

        public override Routing getRouting() => _routing;
    }
}
