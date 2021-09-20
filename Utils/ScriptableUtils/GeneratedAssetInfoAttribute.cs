namespace UniModules.UniGame.Core.Editor.EditorProcessors
{
    using System;

    [AttributeUsage(System.AttributeTargets.Class)]
    public class GeneratedAssetInfoAttribute : Attribute
    {
        public GeneratedAssetInfoAttribute(string location = "")
        {
            Location = location;
        }

        public string Location { get; private set; }

    }
}