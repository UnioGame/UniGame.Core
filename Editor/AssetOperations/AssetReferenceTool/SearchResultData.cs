namespace UniModules.UniGame.Core.EditorTools.Editor.AssetOperations.AssetReferenceTool
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using EditorResources;
    using Runtime.DataFlow.Interfaces;
    using Runtime.Interfaces;
    using UniModules.UniCore.EditorTools.Editor;
    using UniModules.UniCore.Runtime.DataFlow;
    using UniModules.UniCore.Runtime.Rx.Extensions;
    using UniRx;
    using Object = UnityEngine.Object;

    public class SearchResultData : IDisposable, ILifeTimeContext
    {
        private LifeTimeDefinition _lifeTime = new LifeTimeDefinition();
        private Subject<ProgressData> _progressData = new Subject<ProgressData>();
        
        #region constructor

        public SearchResultData()
        {
            _progressData.AddTo(_lifeTime);
        }
        
        #endregion
        
        public ConcurrentDictionary<Object, List<ResourceHandle>> referenceMap = new ConcurrentDictionary<Object, List<ResourceHandle>>();
        
        public AssetResourcesMap assets  = new AssetResourcesMap(2);

        public IObservable<ProgressData> Progression => _progressData;

        public ILifeTime LifeTime => _lifeTime;
        
        public void Dispose() => _lifeTime.Release();

        public SearchResultData AddKey(Object asset)
        {
            if (referenceMap.ContainsKey(asset)) {
                return this;
            }
            referenceMap[asset] = new List<ResourceHandle>();
            var resource = new EditorResource();
            resource.Update(asset);
            assets[asset] = resource;
            return this;
        }

        public SearchResultData ReportProgress(ProgressData progress)
        {
            _progressData.OnNext(progress);
            return this;
        }

        public void Complete()
        {
            _lifeTime.Release();
            _progressData = new Subject<ProgressData>().AddTo(_lifeTime);
        }
    }
}