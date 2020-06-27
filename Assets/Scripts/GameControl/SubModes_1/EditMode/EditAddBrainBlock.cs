using GameObjects;
using ObjectAccess;
using UnityEngine;

namespace GameControl.SubModes_1.EditMode
{
    public class EditAddBrainBlock : EditAddObject
    {

        private Block toPlace;
        private static EditAddBrainBlock _instance ;

        public override CBehaviour getInstance()
        {
            if (_instance == null) _instance = new EditAddBrainBlock();
            return _instance;
        }

        protected override bool placeblock()
        {
            bool success = blocks.placeBlock(hexGrid.mouseCoords(), toPlace);
            if (success)
            {
                toPlace.GetComponent<SpriteRenderer>().enabled = true;
                toPlace = null;
                return true;
            }

            return false;
        }

        protected override void setup()
        {
            toPlace = GameObject.Instantiate(GameObject.Find("ObjectAccess").GetComponent<Prefabs>().brainBlock)
                .GetComponent<SpawnBlock>();
            toPlace.GetComponent<SpriteRenderer>().enabled = false;
        }

        protected override void cancel()
        {
            if (toPlace != null) GameObject.Destroy(toPlace.gameObject);
            
        }
    }
}