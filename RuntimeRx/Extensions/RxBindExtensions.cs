using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UniModules.UniCore.Runtime.Rx.Extensions;
using UniModules.UniCore.Runtime.Utils;
using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
using UniModules.UniGame.Core.Runtime.Interfaces;
using UniModules.UniUiSystem.Runtime.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UniModules.Rx.Extensions
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

        public static TView Bind<TView>(this TView sender, Button source, Action<Unit> command, int throttleTime = 0)
            where TView : ILifeTimeContext
        {
            return source == null ? sender : sender.Bind(source, () => command(Unit.Default), throttleTime);
        }
        
        public static TView Bind<TView>(this TView sender, Button source, Action<Unit> command, TimeSpan throttleTime)
            where TView : ILifeTimeContext
        {
            return source == null ? sender : sender.Bind(source, () => command(Unit.Default), throttleTime);
        }

        public static TView Bind<TView>(this TView sender, Button source, Action command,TimeSpan throttleTime)
            where TView : ILifeTimeContext
        {
            if (!source) return sender;

            var clickObservable = throttleTime.TotalMilliseconds <= 0
                ? source.OnClickAsObservable()
                : source.OnClickAsObservable().ThrottleFirst(throttleTime);

            return Bind(sender, clickObservable, command);
        }

        public static TView Bind<TView>(this TView view, IObservable<bool> source, Button button)
            where TView : ILifeTimeContext
        {
            return !button ? view : view.Bind(source, x => button.interactable = x);
        }
        
        public static TView Bind<TView>(this TView view, Button source, IReactiveCommand<Unit> command, int throttleInMilliseconds = 0)
            where TView : ILifeTimeContext
        {
            return !source ? view : Bind(view, source, () => command.Execute(Unit.Default), TimeSpan.FromMilliseconds(throttleInMilliseconds));
        }
        
        public static TView Bind<TView>(this TView view, IObservable<Unit> source, IReactiveCommand<Unit> command)
                    where TView : ILifeTimeContext
        {
            return Bind(view, source, x => command.Execute(Unit.Default));
        }
        
        public static TView Bind<TView>(this TView sender, Button source, Action command,int throttleInMilliseconds = 0)
            where TView : ILifeTimeContext
        {
            return Bind(sender, source, command,TimeSpan.FromMilliseconds(throttleInMilliseconds));
        }

                        
        public static TView Bind<TView>(this TView view, IObservable<Sprite> source, Button button, int frameThrottle = 0)
            where TView : ILifeTimeContext
        {
            if (!button || !button.image)
                return view;
            
            return view.Bind(source, x => button.image.SetValue(x));
        }
        
        
        public static TView Bind<TView>(this TView view, IObservable<bool> source, Toggle toggle)
            where TView : ILifeTimeContext
        {
            return !toggle ? view : Bind(view,source, x => toggle.isOn = x);
        }
        
        public static TSource Bind<TSource>(this TSource view, Toggle source, IReactiveProperty<bool> value)
            where TSource : ILifeTimeContext
        {
            return !source ? view : Bind(view,source.OnValueChangedAsObservable(), value);
        }

        public static TView Bind<TView>(this TView view, Toggle source, Action<bool> value)
            where TView : ILifeTimeContext
        {
            return !source ? view : view.Bind(source.OnValueChangedAsObservable(), value);
        }

        public static TView Bind<TView>(this TView view, IObservable<bool> source, CanvasGroup group)
            where TView : ILifeTimeContext
        {
            if (!group) return view;
            return view.Bind(source,x => group.interactable = x);
        }
        
        public static TView Bind<TView>(this TView view, IObservable<Sprite> source, Image image)
            where TView : ILifeTimeContext
        {
            return !image ? view : view.Bind(source.Where(x => x!=null), x => image.SetValue(x) );
        }
        
        public static TView Bind<TView>(this TView view, IObservable<string> source, TextMeshProUGUI text)
            where TView : ILifeTimeContext
        {
            return view.Bind(source,x => text.SetValue(x));
        }
        
        public static TView Bind<TView>(this TView view, IObservable<int> source, TextMeshProUGUI text)
            where TView : ILifeTimeContext
        {
            return view.Bind(source, x => text.SetValue(x.ToStringFromCache()));
        }
        
        public static TView Bind<TView,TValue>(this TView view,
            IObservable<TValue> source,
            Func<TValue,string> format, TextMeshProUGUI text)
            where           TView : ILifeTimeContext
        {
            var stringObservable = source.Select(format);
            return view.Bind(stringObservable, text);
        }
        
        public static TView Bind<TView>(this TView view, IObservable<string> source, TMP_Text text)
            where TView : ILifeTimeContext
        {
            return !text ? view : view.Bind(source,x => text.SetValue(x));
        }

        public static TView Bind<TView>(this TView view, IObservable<string> source, TextMeshPro text)
            where TView : ILifeTimeContext
        {
            if (!text) return view;
            return view.Bind(source, x => text.text = x);
        }

        public static TView Bind<TView>(this TView view, IObservable<int> source, TextMeshPro text)
            where TView : ILifeTimeContext
        {
            return view.Bind(source, x => text.text = x.ToStringFromCache());
        }

        
        #endregion
        
        
        #region lifetime context
        
        public static TView Bind<TView,TValue>(this TView view, IObservable<TValue> source, IObserver<Unit> observer)
            where TView : ILifeTimeContext
        {
            return view.Bind(source,x => observer.OnNext(Unit.Default));
        }
        
        public static TView Bind<TView,TValue>(this TView view, IObservable<TValue> source, IObserver<TValue> observer)
            where TView : ILifeTimeContext
        {
            return view.Bind(source, observer.OnNext);
        }
        
        public static TView Bind<TView,TValue>(this TView view, IObservable<TValue> source, IReactiveProperty<TValue> value)
            where TView : ILifeTimeContext
        {
            return view.Bind(source, x => value.Value = x);
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
            return action == null ? sender : Bind<T,TValue>(sender, source, x => action(), sender.LifeTime);
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
        

        
        public static TSource BindCleanUp<TSource>(
            this TSource view, 
            Action target)
            where TSource : ILifeTimeContext
        {
            view.AddCleanUpAction(target);
            return view;
        }
        
        public static TSource BindDispose<TSource>(
            this TSource view,
            IDisposable target)
            where TSource : ILifeTimeContext
        {
            view.AddDisposable(target);
            return view;
        }
        
                
        public static TSource BindLateUpdate<TSource>(
            this TSource view,
            Func<bool> predicate, 
            Action target)
            where TSource : ILifeTimeContext
        {
            var observable = Observable.EveryLateUpdate()
                .Where(x => predicate());
                
            return view.Bind(observable,target);
        }
        
        public static TSource BindIntervalUpdate<TSource>(
            this TSource view,
            TimeSpan interval,
            Action target)
            where TSource : ILifeTimeContext
        {
            var observable = Observable.Interval(interval);
            return view.Bind(observable,target);
        }

        public static TSource BindIntervalUpdate<TSource>(
            this TSource view,
            TimeSpan interval,
            Action target,
            Func<bool> predicate)
            where TSource : ILifeTimeContext
        {
            var observable = Observable.Interval(interval)
                .Where(x => view != null && view.LifeTime.IsTerminated == false && predicate());
            
            return view.Bind(observable,target);
        }
        
        public static TSource BindIntervalUpdate<TSource,TValue>(
            this TSource view,
            TimeSpan interval,
            Func<TValue> source,
            Action<TValue> target,
            Func<bool> predicate = null)
            where TSource : ILifeTimeContext
        {
            var observable = Observable.Interval(interval)
                .Where(x => view.LifeTime.IsTerminated == false && (predicate == null || predicate()))
                .Select(x => source());

            return view.Bind(observable, target);
        }
                
        public static TSource BindIf<TSource,T>(
            this TSource view,
            IObservable<T> source, 
            Func<bool> predicate,
            Action<T> target)
            where TSource : ILifeTimeContext
        {
            if (predicate != null && predicate())
                return view.Bind(source, target);
            return view;
        }
        
        
        #endregion

        #region async
        
        public static TSource Bind<TSource,T,TTaskValue>(
            this TSource view,
            IObservable<T> source, 
            Func<T,UniTask<TTaskValue>> asyncAction)
            where TSource : ILifeTimeContext
        {
            return view.Bind(source, x => asyncAction(x).AttachExternalCancellation(view.LifeTime.TokenSource).Forget());
        }
        
        public static TSource Bind<TSource,T>(this TSource view,
            IObservable<T> source, 
            Func<T,UniTask> asyncAction)
            where TSource : ILifeTimeContext
        {
            return view.Bind(source, x => asyncAction(x).AttachExternalCancellation(view.LifeTime.TokenSource).Forget());
        }

        #endregion
        
        
        #region base 
        
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

        public static TSource Bind<TSource,TValue>(this TSource view, IEnumerable<TValue> source, Action<TValue> action)
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
        
        public static T Bind<T, TValue>(this T sender, IObservable<TValue> source, 
            IReactiveCommand<TValue> action,
            ILifeTime lifeTime)
        {
            if (action == null) return sender;
            source.Where(x => action.CanExecute.Value)
                .Subscribe(x => action.Execute(x))
                .AddTo(lifeTime);
            
            return sender;
        }
        
        public static T Bind<T, TValue>(this T sender, IObservable<TValue> source, 
            IReactiveCommand<Unit> action,
            ILifeTime lifeTime)
        {
            if (action == null) return sender;
            
            source.Where(x => action.CanExecute.Value)
                .Subscribe(x => action.Execute(Unit.Default))
                .AddTo(lifeTime);
            
            return sender;
        }

        #endregion
        
    }
}