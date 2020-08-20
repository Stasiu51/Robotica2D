using GameObjects;
using ObjectAccess;
using UnityEngine;

namespace GameControl.SubModes_1.EditMode
{
    public class EditAddSpawnBlock : EditAddObject
    {

        private Block toPlace;
        private static EditAddSpawnBlock _instance ;
        
        public override CBehaviour getInstance()
        {
            if (_instance == null) _instance = new EditAddSpawnBlock();
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
            toPlace = new SpawnBlock();
            toPlace.SetVisible(false);
        }

        protected override void cancel()
        {
            //TODO fix destroy
            // if (toPlace != null) GameObject.Destroy(toPlace.gameObject);
            
        }
    }
}