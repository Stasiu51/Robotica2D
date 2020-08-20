using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameControl
{
    public class CStateMachine
    {
        private CBehaviour _currentCBehaviour = null;
        public CBehaviour initialBehaviour;
        public Type myType;
        

        private void changeTo(CBehaviour newCBehaviour)
        {
            if (_currentCBehaviour == newCBehaviour) return;
            
            _currentCBehaviour?.onExit();

            _currentCBehaviour = newCBehaviour;
            
            newCBehaviour.onEnter();
        }

        private void checkNull()
        {
            if (_currentCBehaviour == null) changeTo(initialBehaviour);
        }

        public CBehaviour update()
        {
            checkNull();
            CBehaviour result = _currentCBehaviour.update();
            return checkChange(result);
        }

        public CBehaviour ongui(Event e)
        {
            checkNull();
            CBehaviour result = _currentCBehaviour.ongui(e);
            return checkChange(result);
        }

        private CBehaviour checkChange(CBehaviour result)
        {
            if (result == null) return null;
            if (result.belongsTo() == myType)
            {
                changeTo(result);
                return null;
            }
            _currentCBehaviour.onExit();
            return result;
        }


    }
    
    
    public abstract class CBehaviour
    {
        private CBehaviour exitB;
        public abstract void onEnter();
        public abstract void onExit();
        protected abstract CBehaviour updateOverride();
        public abstract CBehaviour ongui(Event e);
        public abstract Type belongsTo();
        
        public CBehaviour update()
        {
            if (exitB != null)
            {
                CBehaviour temp = exitB;
                exitB = null;
                return temp;
            }

            return updateOverride();

        }

        protected void Exit(CBehaviour exitWith)
        {
            exitB = exitWith;
        }

        public abstract CBehaviour getInstance();
    }
    

    public abstract class CSubStateMachine: CBehaviour
    {

        public CStateMachine stateMachine { get; private set; }

        protected abstract CBehaviour getInitialBehaviour();

        public abstract void onEnterOverride();

        public sealed override void onEnter()
        {
            stateMachine = new CStateMachine();
            onEnterOverride();
            stateMachine.initialBehaviour = getInitialBehaviour();
            stateMachine.myType = this.GetType();
            
        }

        public sealed override void onExit()
        {
            onExitOverride();
        }

        protected abstract void onExitOverride();

        protected sealed override CBehaviour updateOverride()
        {
            return stateMachine.update();
        }

        public sealed override CBehaviour ongui(Event e)
        {
            return stateMachine.ongui(e);
        }



    }
    
}