using System;
using NUnit.Framework;
using UniGame.Runtime.ObjectPool;
using UniGame.Runtime.ObjectPool.Extensions;
using UniGame.Core.Runtime.ObjectPool;
using UnityEngine;

namespace UnioModules.UniGame.CoreModules.Tests
{
    public class ClassPoolTests : MonoBehaviour
    {
        public class PooledTestClass : IPoolable, IDisposable
        {
            public string Message = string.Empty;
            
            public void Release()
            {
                Message = string.Empty;
            }

            public void Dispose()
            {
                Message = string.Empty;
            }
        }
        
        [Test(TestOf = typeof(ClassPoolTests))]
        public void MakePoolItemTest()
        {
            //info
            var messageData = "Demo Message";
            //action
            var classOne = ClassPool.Spawn<PooledTestClass>();
            classOne.Message = messageData;
            classOne.Despawn();
            
            var classTwo = ClassPool.Spawn<PooledTestClass>();
            
            //assert
            Assert.That(classOne == classTwo,"classOne != pooled classTwo");
            Assert.That(classTwo.Message == messageData,"message from pooled class != " + messageData);
        }
        
        [Test(TestOf = typeof(ClassPoolTests))]
        public void MakeReleasePoolItemTest()
        {
            //info
            var messageData = "Demo Message";
            //action
            var classOne = ClassPool.Spawn<PooledTestClass>();
            classOne.Message = messageData;
            classOne.DespawnWithRelease();
            
            var classTwo = ClassPool.Spawn<PooledTestClass>();
            
            //assert
            Assert.That(classOne == classTwo,"classOne != pooled classTwo");
            Assert.That(classTwo.Message == string.Empty,"message not empty");
        }
    }
}
