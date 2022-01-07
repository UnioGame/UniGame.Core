using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniModules.Editor.OdinTools.GameEditor.Categories
{
    [Serializable]
    public class AssetObjectCategory : GameEditorCategory
    {
        public Object asset;

        public override object CreateDrawer() => asset == null 
            ? ScriptableObject.CreateInstance<EmptyDrawer>() 
            : asset;
    }
}
