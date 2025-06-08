using System;

namespace UniGame.DataStructure
{
    [Serializable]
    public class SerializableStringsMap : 
        SerializableDictionary<string,string>
    {
        public SerializableStringsMap(int capacity) : base(capacity)
        {
            
        }
    }
}
