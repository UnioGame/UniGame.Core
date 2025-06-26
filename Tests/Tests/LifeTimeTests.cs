using NUnit.Framework;
using UniGame.Runtime.DataFlow;

public class LifeTimeTests 
{
    [Test]
    public void LifeTimeMergeTest()
    {
        //info
        var lifeTime1 = new LifeTime();
        var lifeTime2 = new LifeTime();
        var lifeTime3 = new LifeTime();
        var isDisposed = false;
        
        //action
        var mergeLifeTime = lifeTime1.MergeLifeTime(lifeTime2, lifeTime3);
        mergeLifeTime.AddCleanUpAction(() => isDisposed = true);
        
        lifeTime1.Terminate();
        lifeTime2.Terminate();
        lifeTime3.Terminate();
        
        //check
        Assert.That(isDisposed);
    }
    
    [Test]
    public void LifeTimeMergeWaitTest()
    {
        //info
        var lifeTime1 = new LifeTime();
        var lifeTime2 = new LifeTime();
        var lifeTime3 = new LifeTime();
        var isDisposed = false;
        
        //action
        var mergeLifeTime = lifeTime1.MergeLifeTime(lifeTime2, lifeTime3);
        mergeLifeTime.AddCleanUpAction(() => isDisposed = true);
        
        lifeTime1.Terminate();
        lifeTime2.Terminate();
        
        //check
        Assert.That(!isDisposed);
        
        lifeTime3.Terminate();
        
        Assert.That(isDisposed);
    }
    
    
}
