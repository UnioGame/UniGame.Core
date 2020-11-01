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
        where TCommand : IAsyncCommand<TData,Unit>
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
        
        
        
        protected sealed override async UniTask<Unit> OnExecute(TData context, ILifeTime executionLifeTime) {

            var asCancellationSource = executionLifeTime.AsCancellationSource();
            var isCancelled       = false;
            _activeScenarioIndex = 0;
            
            for (var i = 0; i < scenarios.Count; i++) {
                var asyncScenario = scenarios[i];
                var task          = asyncScenario.Execute(context).
                    WithCancellation(asCancellationSource.Token);
                var result        = await task;
                    
                if (task.Status == UniTaskStatus.Succeeded) {
                    continue;
                }

                _activeScenarioIndex = i;
                isCancelled          = true;
                break;
            }

            return isCancelled ? 
                await UniTask.FromCanceled<Unit>():
                Unit.Default;
        }

        public async UniTask<Unit> Rollback(IContext source) {
            
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

            return Unit.Default;
        }
    }
}