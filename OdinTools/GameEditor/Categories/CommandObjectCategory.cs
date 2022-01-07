using System;
using UniModules.UniGame.Core.Runtime.Interfaces;
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
