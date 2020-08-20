using GameObjects;
using ObjectAccess;
using Serialisation;
using UnityEngine;

namespace GameControl.SubModes_1.EditMode
{
    public class EditAddStructureBlock : EditAddObject
    {

        private Block toPlace;
        private static EditAddStructureBlock _instance ;
        
        public override CBehaviour getInstance()
        {
            if (_instance == null) _instance = new EditAddStructureBlock();
            return _instance;
        }
        
        protected override bool placeblock()
        {
            bool success = blocks.placeBlock(hexGrid.mouseCoords(), toPlace);
            if (success)
            {
                toPlace.SetVisible(true);
                toPlace = null;
                
                return true;
            }

            return false;
        }

        protected override void setup()
        {
            toPlace = new StructureBlock();
            toPlace.SetVisible(false);
        }

        protected override void cancel()
        {
            // if (toPlace != null) GameObject.Destroy(toPlace.gameObject);
            
        }
    }
}