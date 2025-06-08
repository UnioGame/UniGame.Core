﻿namespace UniGame.Runtime.ScriptableObjects
{
    using System;
    using System.Collections;
    using global::UniGame.Core.Runtime;
    using UnityEngine;

    public abstract class ScriptableObjectRoutine<TContext> : 
        ScriptableObject ,IRoutine<TContext,IEnumerator>
    {
	
        [NonSerialized]
        private bool _initialized = false;
	
        public IEnumerator Execute(TContext context) {
		
            if (_initialized == false)
            {
                _initialized = true;
                OnInitialize();
            }

            yield return OnExecute(context);
		
        }
	
	
        #region private methods

        protected virtual void OnInitialize() {}

        protected abstract IEnumerator OnExecute(TContext context);

        #endregion


    }
}
