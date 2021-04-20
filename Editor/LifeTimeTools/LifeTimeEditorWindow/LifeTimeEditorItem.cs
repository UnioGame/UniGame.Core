﻿#if ODIN_INSPECTOR

using System;
using Sirenix.OdinInspector;
using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniModules.UniGame.Context.Editor.LifeTimeEditorWindow
{
    [Serializable]
    [InlineProperty]
    public class LifeTimeEditorItem
    {
        #region inspector
        
        [HideIf("@this.asset == null")]
        [InlineEditor]
        public Object asset;

        public string type;
        
        [GUIColor("GetStatusColor")]
        public string status;
        
        #endregion

        public ILifeTime lifeTime;
        
        public bool IsAlive => lifeTime != null && lifeTime.IsTerminated == false;
        
        public LifeTimeEditorItem Initialize(ILifeTime lifeTimeValue)
        {
            lifeTime = lifeTimeValue;
            type = lifeTime?.GetType().Name;
            asset = lifeTime as Object;
            
            lifeTime?.AddCleanUpAction(UpdateStatus);
            
            UpdateStatus();
            
            return this;
        }

        public Color GetStatusColor() => IsAlive ? Color.green : Color.red;

        public void UpdateStatus()
        {
            status = IsAlive ? "alive" : "dead";
        }
    }
}

#endif