using System;
using UniGame.Core.Runtime;
using UnityEngine;

namespace UniModules.Editor.OdinTools.GameEditor.Categories
{
    [Serializable]
    public class CommandObjectCategory : GameEditorCategory
    {
        [SerializeReference]
        public ICommand Command;

        public override object CreateDrawer() => Command;
    }
}
