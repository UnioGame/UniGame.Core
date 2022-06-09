using System;
using System.Reflection;
using UniGame.Core.Runtime.Common;
using UniModules.UniCore.Runtime.Utils;

namespace UniModules.UniCore.EditorTools.Editor.PropertiesDrawers
{
    
    public class InspectorEditor : IDisposable
    {
        private const string DisableMethod = "OnDisable";

        private static MemorizeItem<Type, Action> disableMethodsMap = MemorizeTool.Memorize<Type, Action>(x =>
        {
            var methodInfo = x.GetMethod(DisableMethod,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            
            if (methodInfo == null)
                return EmptyDisposable.Empty.Dispose;
            
            var delegateValue = methodInfo.CreateDelegate(typeof(Action));
            return (Action)delegateValue;
        });
        
        public UnityEditor.Editor Editor { get; }

        public Type EditorType { get; }
        
        public Type InspectorType { get; }

        public InspectorEditor(UnityEngine.Object target, Type inspectorInspectorType)
        {
            Editor = UnityEditor.Editor.CreateEditor(target, inspectorInspectorType);
            EditorType = Editor.GetType();
            InspectorType = inspectorInspectorType;
        }

        public void Dispose()
        {
            if (!Editor) return;
            var disableMethod = disableMethodsMap[EditorType];
            disableMethod.Invoke();
            UnityEngine.Object.DestroyImmediate(Editor);
        }
    }
}