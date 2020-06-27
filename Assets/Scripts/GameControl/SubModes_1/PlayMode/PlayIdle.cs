using System;
using UIControl;
using UnityEngine;

namespace GameControl.SubModes_1.PlayMode
{
    class PlayIdle : CBehaviour
    {
        private static PlayIdle _instance ;

        public override CBehaviour getInstance() => _instance ?? (_instance = new PlayIdle());
        public override void onEnter()
        {
            Debug.Log("entered play idle");
            UIEventInvoker.EditPressedInPlay += exitToEdit;
        }

        public override void onExit()
        {
            UIEventInvoker.EditPressedInPlay -= exitToEdit;
            Debug.Log("exited play idle");
        }

        void exitToEdit()
        {
            Debug.Log("exit?");
            Exit(new EditCBehaviour().getInstance());
            
        } 

        protected override CBehaviour updateOverride()
        {
            return null;
        }

        public override CBehaviour ongui(Event e)
        {
            return null;
        }

        public override Type belongsTo() => typeof(PlayCBehaviour);
    }
}