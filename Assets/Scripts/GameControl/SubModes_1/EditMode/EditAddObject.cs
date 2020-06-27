using System;
using GameObjects;
using UnityEngine;

namespace GameControl.SubModes_1.EditMode
{
    public abstract class EditAddObject : CBehaviour
    {
        private GameObject _selectCircle;
        protected HexGrid hexGrid;
        protected Blocks blocks;
        protected abstract bool placeblock();
        protected abstract void setup();
        protected abstract void cancel();

        public override Type belongsTo()
        {
            return typeof(EditCBehaviour);
        }
        public override void onEnter()
        {
            Debug.Log("placemode");
            if (_selectCircle == null) _selectCircle = GameObject.Find("select");

            if (hexGrid == null) hexGrid = GameObject.Find("HexGrid").GetComponent<HexGrid>();

            if (blocks == null) blocks = GameObject.Find("Grid").GetComponent<Blocks>();

            _selectCircle.transform.position = Vector3.zero;
            _selectCircle.GetComponent<SpriteRenderer>().enabled = true;
            
            setup();

        }
        public override void onExit()
        {
            _selectCircle.GetComponent<SpriteRenderer>().enabled = false;
            cancel();
            Debug.Log("exit placemode");
        }

        protected override CBehaviour updateOverride()
        {
            _selectCircle.transform.position = hexGrid.mousePos();
            return null;
        }

        public override CBehaviour ongui(Event e)
        {
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                return placeblock() ? new EditIdle().getInstance() :null;
            }

            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Escape) return new EditIdle().getInstance();
            return null;
        }
    }
}