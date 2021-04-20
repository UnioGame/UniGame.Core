#if ODIN_INSPECTOR

using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UniModules.UniCore.Runtime.DataFlow;
using UniModules.UniCore.Runtime.Rx.Extensions;
using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
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
        
        public void Initialize(IReadOnlyReactiveCollection<ILifeTime> lifeTimes)
        {
            scriptableLifeTimes.Clear();
            foreach (var lifeTime in lifeTimes)
            {
                AddLifeTime(lifeTime);
            }

            lifeTimes.ObserveAdd()
                .Do(x => AddLifeTime(x.Value))
                .Subscribe()
                .AddTo(_lifeTime);
        }

        public void Dispose() => _lifeTime.Terminate();
        
        private void AddLifeTime(ILifeTime lifeTimeValue)
        {
            if (scriptableLifeTimes.Any(x => x.lifeTime == lifeTimeValue))
            {
                return;
            }
            
            scriptableLifeTimes.Add(new LifeTimeEditorItem().Initialize(lifeTimeValue));
        }

    }
}

#endif