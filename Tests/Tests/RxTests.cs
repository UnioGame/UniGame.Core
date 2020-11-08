using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UniModules.UniGame.Core.Runtime.Rx;
using UniRx;
using UnityEngine;

public class RxTests 
{
    [Test]
    public void ReactivePropertyDisposeTest()
    {
        //info
        var value = new RecycleReactiveProperty<int>();
            
        //action
        var disposable = value.Subscribe(x => Assert.That(x == int.MaxValue));
        disposable.Dispose();
            
        value.Value = 0;
        Assert.True(true);
    }
    
    [Test]
    public void ReactivePropertyReceiveTest()
    {
        //info
        var value     = new RecycleReactiveProperty<int>();
        var testValue = 100;
        
        //action
        value.Value = testValue;
        
        //check
        var disposable = value.Subscribe(x => Assert.That(x == testValue));
        disposable.Dispose();
            
    }
    
    [Test]
    public void ReactivePropertyFirstTest()
    {
        //info
        var value     = new RecycleReactiveProperty<int>();
        var testValue = 100;
        
        //action
        var disposable = value.First().
            Subscribe(x => Assert.That(x == testValue));
        value.Value = testValue;
        
        //check
        disposable.Dispose();
    }
    
    [Test]
    public void UniRxReactiveFirstTwiceAfterTest()
    {
        //info
        var value       = new ReactiveProperty<int>();
        var testValue   = 100;
        var resultValue = 0;
        
        //action
        var disposable2 = value.
            First().
            Subscribe();
        
        var disposable = value.
            Do(x => Debug.Log($"Receive {x}")).
            Subscribe(x => resultValue = x);

        value.Value = testValue;

        Assert.That(resultValue == testValue);
    }
    
    [Test]
    public void UniRxReactiveFirstTwiceTest()
    {
        //info
        var value       = new ReactiveProperty<int>();
        var testValue   = 100;
        var resultValue = 0;
        
        //action
        value.Value = testValue;
        
        var disposable2 = value.
            First().
            Subscribe();
        
        var disposable = value.
            First().
            Subscribe(x => resultValue = x);
        
        Assert.That(resultValue == testValue);
        
        //check
        disposable.Dispose();
        disposable2.Dispose();
    }
    
    [Test]
    public void ReactivePropertyFirstTwicePublishAfterTest()
    {
        //info
        var value       = new RecycleReactiveProperty<int>();
        var testValue   = 100;
        var resultValue = 0;
        
        //action

        var disposable2 = value.
            First().
            Subscribe();
        
        var disposable = value.
            Do(x=> Debug.Log($"Receive VALUE {x}")).
            Subscribe(x => resultValue = x);
        
        value.Value = testValue;

        //check
        Assert.That(resultValue == testValue);
    }
    
    [Test]
    public void ReactivePropertyFirstTwiceTest()
    {
        //info
        var value       = new RecycleReactiveProperty<int>();
        var testValue   = 100;
        var resultValue = 0;
        
        //action
        value.Value = testValue;
        
        var disposable2 = value.
            First().
            Subscribe();
        
        var disposable = value.
            First().
            Subscribe(x => resultValue = x);
        
        Assert.That(resultValue == testValue);
        
        //check
        disposable.Dispose();
        disposable2.Dispose();
    }

    [Test]
    public void ReactivePropertyValueChangeTest()
    {
        //info
        var value     = new RecycleReactiveProperty<int>();
        var testValue = 333;
        //action
        var disposable1 = value.Subscribe(x => Assert.That(x == testValue));
        var disposable2 = value.Subscribe(x => Assert.That(x == testValue));

        value.Value = testValue;
        
        disposable1.Dispose();
        disposable2.Dispose();
        
        Assert.True(true);
    }
    
    [Test]
    public void ReactivePropertyValueChangeDisposeTest()
    {
        //info
        var value     = new RecycleReactiveProperty<int>();
        var testValue = 333;
        //action
        var disposable1 = value.Subscribe(x => Assert.That(x == testValue));
        var disposable2 = value.Subscribe(x => Assert.That(x == testValue));

        value.Value = testValue;
        
        disposable1.Dispose();
        disposable2.Dispose();
        
        value.Value = int.MaxValue;
        
        Assert.True(true);
    }
    
    [Test]
    public void ReactivePropertyValueChangeRevertDisposeTest()
    {
        //info
        var value     = new RecycleReactiveProperty<int>();
        var testValue = 333;
        //action
        var disposable1 = value.Subscribe(x => Assert.That(x == testValue));
        var disposable2 = value.Subscribe(x => Assert.That(x == testValue));

        value.Value = testValue;
        
        disposable2.Dispose();
        disposable1.Dispose();
        
        value.Value = int.MaxValue;
        
        Assert.True(true);
    }
    
    [Test]
    public void ReactivePropertySubscribeAfterTest()
    {
        //info
        var value     = new RecycleReactiveProperty<int>();
        var testValue = 333;
        //action
        var disposable1 = value.Subscribe(x => Assert.That(x == testValue));
        value.Value = testValue;
        disposable1.Dispose();
        
        var disposable2 = value.Subscribe(x => Assert.That(x == testValue));
        disposable2.Dispose();
        value.Value = int.MaxValue;
        disposable2 = value.Subscribe(x => Assert.That(x == int.MaxValue));
        disposable2.Dispose();
        
        Assert.True(true);
    }
}
