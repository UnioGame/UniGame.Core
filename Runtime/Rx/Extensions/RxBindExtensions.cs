using System;
using System.Collections.Generic;
using TMPro;
using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
using UniModules.UniGame.Core.Runtime.Interfaces;
using UniRx;
using UnityEngine.UI;

namespace UniModules.UniCore.Runtime.Rx.Extensions
{
    public static class RxBindExtensions
    {

        #region ugui
        
        public static TView Bind<TView,TValue>(this TView sender, IObservable<TValue> source, Button command)
            where TView : ILifeTimeContext
        {
            if (command == null) return sender;
            return Bind(sender,source,x => command.onClick?.Invoke());
        }
        
        public static TView Bind<TView>(this TView sender, Button source, Action command,int throttleInMilliseconds = 0)
            where TView : ILifeTimeContext
        {
            if (!source) return sender;

            var clickObservable = throttleInMilliseconds <= 0
                ? source.OnClickAsObservable()
                : source.OnClickAsObservable().ThrottleFirst(TimeSpan.FromMilliseconds(throttleInMilliseconds));

            return Bind(sender, clickObservable, command);
        }

        #endregion
        
        
        public static TView Bind<TView,TValue>(this TView view, IEnumerable<TValue> source, Action<TValue> action)
        {
            if (source == null || action == null) return view;

            foreach (var value in source)
                action(value);
            
            return view;
        }
        
        public static T Bind<T, TValue>(this T sender, IObservable<TValue> source, Action<TValue> action, ILifeTime lifeTime)
        {
            if (action == null) return sender;
            source.Subscribe(action).AddTo(lifeTime);
            return sender;
        }
        
        public static T Bind<T, TValue>(this T sender, IObservable<TValue> source, 
            Action<T,TValue> action,
            ILifeTime lifeTime)
        {
            if (action == null) return sender;
            source.Subscribe(x => action(sender,x)).AddTo(lifeTime);
            return sender;
        }
        
        public static T Bind<T, TValue>(this T sender, IObservable<TValue> source, Action<TValue> action)
            where T : ILifeTimeContext
        {
            return Bind<T,TValue>(sender, source, action, sender.LifeTime);
        }
        
        public static T Bind<T, TValue>(this T sender, IObservable<TValue> source, Action<T,TValue> action)
            where T : ILifeTimeContext
        {
            return Bind<T,TValue>(sender, source, action, sender.LifeTime);
        }
        
        public static T Bind<T, TValue>(this T sender, IObservable<TValue> source, Action action)
            where T : ILifeTimeContext
        {
            if (action == null)
                return sender;
            return Bind<T,TValue>(sender, source, x => action(), sender.LifeTime);
        }
        
        public static TResult BindConvert<TResult,T, TValue>(this T sender,Func<T,TResult> converter, IObservable<TValue> source, Action action)
            where T : ILifeTimeContext
        {
            TResult result = default;
            if (action != null)
                sender = Bind<T,TValue>(sender, source, x => action(), sender.LifeTime);
            return converter == null ? result : converter(sender);
        }
        
        public static TSource BindWhere<TSource,T>(
            this TSource sender,
            IObservable<T> source, 
            Func<bool> predicate,
            Action<T> target)
            where TSource : ILifeTimeContext
        {
            if (predicate != null && predicate())
                sender.Bind(source, target);
            return sender;
        }
        
        public static TSource BindWhere<TSource,T>(
            this TSource sender,
            IObservable<T> source, 
            Func<bool> predicate,
            Action<T> target,
            ILifeTime lifeTime)
        {
            if (predicate != null && predicate())
                sender.Bind(source, target,lifeTime);
            return sender;
        }
        
    }
}