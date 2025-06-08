using UniGame.Runtime.Utils;

namespace UniModules.UniGame.Core.Editor.EditorProcessors
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UniModules.Editor;
    using global::UniGame.Core.Runtime.Extension;
    using global::UniCore.Runtime.Attributes;
    using global::UniGame.Runtime.ReflectionUtils;
    using UniGameFlow.GameFlowEditor.Editor.UiElementsEditor.Styles;
    using Unity.EditorCoroutines.Editor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public class ScriptableEditorProcessor<TProcessor, TDataProcessor, TData> :
        GeneratedAsset<TProcessor>,
        IEditorProcess
        where TDataProcessor : class, IEditorProcessor<TData>
        where TProcessor : ScriptableEditorProcessor<TProcessor, TDataProcessor, TData>
    {
        #region inscpector

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
#endif
        [SerializeReference]
        public List<TDataProcessor> processors = new List<TDataProcessor>();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineEditor()]
#endif
        public List<Object> assetsProcessors = new List<Object>();

        [SerializeField] public List<TData> data = new List<TData>();

        [SerializeField] [ReadOnlyValue] public bool isActive = false;

        #endregion

        private EditorCoroutine           _coroutine;
        private List<TData> _removedData  = new List<TData>();

        public bool IsRunning => _coroutine != null;

        public void Add(TData dataItem)
        {
            if (data.Contains(dataItem)) return;
            data.Add(dataItem);
        }

        public void Remove(TData item)
        {
            _removedData.Add(item);
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Start()
        {
            if (_coroutine != null)
                return;
            _coroutine = EditorCoroutineUtility.StartCoroutine(Execute(), this);
            isActive   = true;
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Stop()
        {
            if (_coroutine != null)
            {
                EditorCoroutineUtility.StopCoroutine(_coroutine);
            }

            _coroutine = null;

            OnProcessorAction(x => x.Disable());
            isActive = false;
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void RefillProcessors()
        {
            processors.Clear();
            assetsProcessors.Clear();
            UpdateProcessors();
        }
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void UpdateProcessors()
        {
            var wasActive = isActive;

            Stop();
            ValidateProcessors();
            
            var targetProcessors = typeof(TDataProcessor).GetAssignableTypes();
            foreach (var targetProcessor in targetProcessors)
            {
                if (!ValidateProcessorType(targetProcessor))
                    continue;

                switch (targetProcessor)
                {
                    case { } assetType when assetType.IsScriptableObject():
                        var assetProcessor = CreateInstance(targetProcessor);
                        assetProcessor.name = targetProcessor.Name;
                        assetProcessor      = assetProcessor.SaveAsset(assetProcessor.name, AssetPath);
                        assetsProcessors.Add(assetProcessor);
                        break;
                    case { }:
                        var processor = Activator.CreateInstance(targetProcessor) as TDataProcessor;
                        processors.Add(processor);
                        break;
                }
            }

            if (wasActive)
                Start();
        }

        private bool ValidateProcessorType(Type targetProcessor)
        {
            if (targetProcessor.IsAbstract || targetProcessor.HasDefaultConstructor() == false)
                return false;
            if (processors.Any(x => x.GetType() == targetProcessor))
                return false;
            if (assetsProcessors.Any(x => x.GetType() == targetProcessor))
                return false;
            if (targetProcessor.IsAsset() && targetProcessor.IsScriptableObject() == false)
                return false;

            return true;
        }

        private void ValidateProcessors()
        {
            processors.RemoveAll(x => x == null);
            assetsProcessors.RemoveAll(x => !x);
        }

        private IEnumerator Execute()
        {
            UpdateProcessors();
            
            OnProcessorAction(x => x.Start());

            while (isActive)
            {
                data.RemoveAll(x => x == null);

                foreach (var removedNode in _removedData)
                    data.Remove(removedNode);

                if (data.Count > 0)
                    OnProcessorAction(x => x.Proceed(data));
                
                yield return null;
            }
        }

        private void Awake()
        {
            Stop();
            UpdateProcessors();
        }

        private void OnDisable()
        {
            Stop();
            OnProcessorAction(x => x.Disable());
        }

        private void OnProcessorAction(Action<TDataProcessor> processorAction)
        {
            foreach (var dataProcessor in processors)
            {
                if (dataProcessor == null)
                    continue;
                processorAction(dataProcessor);
            }
            
            foreach (var dataProcessor in assetsProcessors)
            {
                if (dataProcessor == null)
                    continue;
                processorAction(dataProcessor as TDataProcessor);
            }
        }
    }
    
    
}