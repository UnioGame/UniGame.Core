namespace UniModules.Editor
{
    using UnityEngine;

    public static class EditorResourceExtensions 
    {

        public static EditorResource ToEditorResource(this Object asset)
        {
            var resource = new EditorResource();
            resource.Update(asset);
            return resource;
        }

    }
}
