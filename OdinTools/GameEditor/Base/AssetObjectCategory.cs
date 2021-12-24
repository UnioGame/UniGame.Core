using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniModules.OdinTools.GameEditor.Base
{
    [Serializable]
    public class AssetObjectCategory : GameEditorCategory<GameEditorConfiguration>
    {
        public Object asset;

        public override object CreateDrawer() => asset == null 
            ? ScriptableObject.CreateInstance<EmptyDrawer>() 
            : asset;
    }
}
