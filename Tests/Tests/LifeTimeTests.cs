using NUnit.Framework;
using UniModules.UniCore.Runtime.DataFlow;

public class LifeTimeTests 
{
    [Test]
    public void LifeTimeMergeTest()
    {
        //info
        var lifeTime1 = new LifeTimeDefinition();
        var lifeTime2 = new LifeTimeDefinition();
        var lifeTime3 = new LifeTimeDefinition();
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
}
