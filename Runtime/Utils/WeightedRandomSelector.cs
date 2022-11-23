namespace UniGame.Utils.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Runtime;
    using Random = UnityEngine.Random;

    public class WeightedRandomSelector<T> : ICollectionSelector<T>
    {
        private readonly Func<T, float> _weightFunction;
        private readonly Func<T,bool>   _filter;
        private readonly System.Random  _rng;

        private float[] _weights;
        
        public WeightedRandomSelector(Func<T, float> weightFunction,Func<T,bool> filter = null)
        {
            var randomSeed = Random.Range(int.MinValue, int.MaxValue);
            
            _weightFunction = weightFunction;
            _filter    = filter;
            _rng            = new System.Random(randomSeed);
        }
        
        public WeightedRandomSelector(Func<T, float> weightFunction, int seed)
        {
            _weightFunction = weightFunction;
            _rng = new System.Random(seed);
        }
        
        public T Select(IEnumerable<T> source)
        {
            var collection = _filter != null 
                ? source.Where(_filter).ToList()
                : source.ToList();
            
            _weights = new float[collection.Count];

            var weightTotal = 0f;
            
            for (var i = 0; i < collection.Count; i++)
            {
                var element = collection[i];
                _weights[i] = _weightFunction(element);

                weightTotal += _weights[i];
            }
            
            var random = _rng.NextDouble() * weightTotal;
            
            var accumulatedWeight = 0f;
            var selected = default(T);

            for (var i = 0; i < collection.Count; i++)
            {
                var element = collection[i];
                accumulatedWeight += _weights[i];
                
                if (accumulatedWeight >= random)
                {
                    selected = element;
                    break;
                }
            }

            return selected;
        }
    }
}