namespace UniModules.UniGame.Core.Runtime.AsyncOperations
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using DataFlow.Interfaces;
    using Interfaces;
    using UniRx;
    using UnityEngine;

    [Serializable]
    public class AsyncScenario<TCommand,TData> : 
        AsyncState<TData>
        where TCommand : IAsyncCommand<TData,AsyncStatus>
    {
        #region inspector
        
        [SerializeReference]
        public List<TCommand> scenarios = new List<TCommand>();

        [SerializeField] 
        private int _activeScenarioIndex = 0;
        
        #endregion

        #region constructor

        public AsyncScenario() { }
        
        public AsyncScenario(IEnumerable<TCommand> nodes) {
            scenarios.AddRange(nodes);
        }
        
        #endregion
        
        
        
        protected sealed override async UniTask<AsyncStatus> OnExecute(TData context, ILifeTime executionLifeTime) {

            var asCancellationSource = executionLifeTime.AsCancellationSource();
            var isCancelled          = false;
            var result               = AsyncStatus.Pending;
            _activeScenarioIndex = 0;
            
            for (var i = 0; i < scenarios.Count; i++) {
                var asyncScenario = scenarios[i];
                var task          = asyncScenario.Execute(context).
                    WithCancellation(asCancellationSource.Token);
                
                result = await task;
                    
                if (result == AsyncStatus.Succeeded) {
                    continue;
                }

                _activeScenarioIndex = i;
                break;
            }

            return result;
        }

        public async UniTask Rollback(IContext source) {
            
            for (var i = _activeScenarioIndex; i >=0 ; i--) 
            {
                switch (scenarios[i]) {
                    case  IAsyncRollback<IContext> contextRollback:
                        await contextRollback.Rollback(source);
                        break;
                    case  IAsyncRollback rollback:
                        await rollback.Rollback();
                        break;
                }
            }
        }
    }
}