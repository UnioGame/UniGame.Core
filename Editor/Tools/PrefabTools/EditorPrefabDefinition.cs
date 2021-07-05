namespace UniModules.UniCore.EditorTools.Editor.PrefabTools
{
    using System.Text;
    using UnityEditor;
    using UnityEngine;

    public struct EditorPrefabDefinition
    {
        private static StringBuilder builder = new StringBuilder(100);

        public GameObject Source;
        public string SourcePath;
        public GameObject Asset;
        public string     AssetPath;
        
        public bool IsInstance;
        public bool IsRegularPrefab;
        public bool IsVariantPrefab;

        public PrefabInstanceStatus InstanceStatus;
        public PrefabAssetType      PrefabAssetType;

        private GameObject assetPrefab;
        public GameObject AssetPrefab {
            get {
                if (assetPrefab) return assetPrefab;
                assetPrefab = string.IsNullOrEmpty(AssetPath) ? null : 
                        AssetDatabase.LoadAssetAtPath<GameObject>(AssetPath);
                return assetPrefab;
            }
            set => assetPrefab = value;
        }

        public override string ToString()
        {
            builder.AppendLine($"ASSET: {Asset}");
            builder.AppendLine($"PATH: {AssetPath}");
            builder.AppendLine($"SOURCE: {Source}");
            builder.AppendLine($"SOURCE PATH: {SourcePath}");
            builder.AppendLine($"IS INSTANCE: {IsInstance}");
            builder.AppendLine($"IS REGULAR:{IsRegularPrefab}");
            builder.AppendLine($"IS VARIANT: {IsVariantPrefab}");
            builder.AppendLine($"STATUS: {InstanceStatus}");
            builder.AppendLine($"TYPE: {PrefabAssetType}");
            
            var result = builder.ToString();
            builder.Clear();
            return result;
            
        }
    }
}