using System;
using UniGame.Core.Runtime;
using UnityEngine;

namespace UniModules.GameEditor.Categories
{
    [Serializable]
    public class CommandObjectCategory : GameEditorCategory
    {
        [SerializeReference]
        public ICommand Command;

        public override object CreateDrawer() => Command;
    }
}
