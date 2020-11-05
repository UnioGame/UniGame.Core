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
