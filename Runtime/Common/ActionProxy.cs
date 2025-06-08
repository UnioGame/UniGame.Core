﻿namespace UniGame.Runtime.Common
{
    using System;
    using global::UniGame.Core.Runtime.ObjectPool;

    public class ActionProxy<T> : IPoolable
    {
        private Action _action;

        public void Initialize(Action action) {
            _action = action;
        }

        public virtual void Release() {
            _action?.Invoke();
        }
    }
}
