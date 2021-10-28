#if ODIN_INSPECTOR

using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UniModules.UniCore.Runtime.DataFlow;
using UniModules.UniCore.Runtime.Rx.Extensions;
using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
using UniModules.UniGame.Core.Runtime.ScriptableObjects;
using UniRx;

namespace UniModules.UniGame.Context.Editor.LifeTimeEditorWindow
{
    [Serializable]
    public class LifeTimeEditorData
    {
        #region indspector

        [Searchable]
        [ListDrawerSettings(Expanded = true)]
        public List<LifeTimeEditorItem> scriptableLifeTimes = new List<LifeTimeEditorItem>();

        #endregion

        private LifeTimeDefinition _lifeTime = new LifeTimeDefinition();
        
        public void Initialize(IReadOnlyList<ILifeTime> lifeTimes)
        {
            UpdateList(lifeTimes);
        }

        public void Dispose() => _lifeTime.Terminate();
        
        private void UpdateList(IReadOnlyList<ILifeTime> lifeTimeValue)
        {
            scriptableLifeTimes.Clear();
            foreach (var lifeTime in lifeTimeValue)
            {
                if (lifeTime is LifetimeScriptableObject lifetimeScriptableObject)
                {
                    scriptableLifeTimes.Add(new LifeTimeEditorItem().Initialize(lifetimeScriptableObject));
                }
            }
        }

    }
}

#endif