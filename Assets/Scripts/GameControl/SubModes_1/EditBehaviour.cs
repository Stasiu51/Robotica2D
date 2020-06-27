﻿using System;
using System.Collections.Generic;
using GameControl.SubModes_1.EditMode;
using ObjectAccess;
using UnityEngine;

namespace GameControl.SubModes_1
{
    public class EditCBehaviour : CSubStateMachine
    {
        private static EditCBehaviour _instance ;
        private UIElements uiElements;
        
        public override CBehaviour getInstance()
        {
            if (_instance == null) _instance = new EditCBehaviour();
            return _instance;
        }

        private List<CBehaviour> _allowedBehaviours = new List<CBehaviour>();
        
        public override Type belongsTo() => typeof(GameMachine);

        protected override CBehaviour getInitialBehaviour() => new EditIdle().getInstance();

        public override void onEnterOverride()
        {
            Debug.Log("entering edit mode");
            if (uiElements == null) uiElements = GameObject.Find("ObjectAccess").GetComponent<UIElements>();
            uiElements.EditUI.SetActive(true);
        }

        protected override void onExitOverride()
        {
            uiElements.EditUI.SetActive(false);
            Debug.Log("exiting edit mode");
        }

    }
}