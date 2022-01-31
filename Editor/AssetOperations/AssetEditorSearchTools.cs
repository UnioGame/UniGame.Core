using System.IO;
using System.Text.RegularExpressions;
using UniModules.UniCore.Runtime.Utils;

namespace UniModules.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UniGame.Core.Runtime.Extension;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public partial class AssetEditorTools
    {
        #region asset loading

        public readonly static List<string> assetExtensions = new List<string>()
        {
            ".prefab",".unity",".asset"
        };
        public const string guidRegExpr = @"(guid:(\s)* (?<guid_group>[\w|\d]*))";
        public const string guidGroupName = @"guid_group";
        public const string FilterTemplate = "t: {0} {1}";
        
        static AssetEditorTools()
        {
            GuidRegexpr = new Regex(guidRegExpr,
                RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }

        public static MemorizeItem<string, List<string>> guidsReferencies = MemorizeTool.Memorize<string, List<string>>(
            path =>
            {
                return GetDependenciesNonCached(path);
            });
        
        public readonly static Regex GuidRegexpr;
        private static string[] EmptyDirFilter = new string[0];

        public static List<Object> GetAssets(Type assetType, string[] folders = null, int count = 0)
        {
            var filterText = GetTypeFilter(assetType);
            var assets     = GetAssets<Object>(filterText, folders,count);
            return assets;
        }

        public static string GetTypeFilter<T>()
        {
            return GetTypeFilter(typeof(T));
        }

        public static string GetTypeFilter(Type filter)
        {
            var filterText = $"t:{filter.Name}";
            return filterText;
        }

        public static IEnumerable<string> GetDependenciesFromAsset(string path)
        {
            return guidsReferencies[path];
        }

        public static List<string> GetDependenciesNonCached(string path)
        {
            var result = new List<string>();
            var globalPath = path.ToAbsoluteProjectPath();
            if(!File.Exists(globalPath))
                return result;
            var fileExt = Path.GetExtension(path);
            if(!assetExtensions.Any(x => x.Equals(fileExt,StringComparison.InvariantCultureIgnoreCase)))
                return result;

            var content = File.ReadAllText(globalPath);
            var matchResult = GuidRegexpr.Matches(content);
            foreach (Match match in matchResult)
            {
                if(!match.Success)
                    continue;

                var group = match.Groups[guidGroupName];
                if(group == null) continue;
            
                var matches = group.Captures;
                foreach (Capture  capture in matches)
                {
                    var guid = capture.Value;
                    if(string.IsNullOrEmpty(guid))
                        continue;
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    if(string.IsNullOrEmpty(assetPath)) continue;
                    result.Add(assetPath);
                }
            }

            return result;
        }

        public static Object GetAsset(string filter, string[] folders = null)
        {
            return GetAssets(filter, folders,1).FirstOrDefault();
        }

        public static List<Object> GetAssets(Type type, string filter, string[] folders = null)
        {
            var isComponent = type.IsComponent();
            if (isComponent) {
                return GetComponentsAssets(type, filter, folders);
            }

            var filterValue = string.Format(FilterTemplate, type.Name, filter);

            return GetAssets(filterValue, folders).Where(x => x && type.IsInstanceOfType(x)).ToList();
        }

        public static List<Object> GetAssets(string filter, string[] folders = null, int count = 0)
        {
            if (string.IsNullOrEmpty(filter))
                return new List<Object>();

            var path = AssetDatabase.GUIDToAssetPath(filter);
            return !string.IsNullOrEmpty(path) 
                ? new List<Object>() {AssetDatabase.LoadAssetAtPath<Object>(path)} 
                : GetAssets<Object>(filter, folders,count);
        }

        public static List<T> GetAssets<T>(string filter, string[] folders = null, int count = 0) where T : Object
        {
            var assets = GetAssets(new List<T>(), filter, folders,count);
            return assets;
        }

        public static List<T> GetAssetsByPaths<T>(List<string> paths) where T : Object
        {
            var assets = new List<T>();
            foreach (var path in paths) {
                var asset = AssetDatabase.LoadAssetAtPath<T>(path);
                if (!asset) continue;
                assets.Add(asset);
            }

            return assets;
        }

        public static List<T> GetAssets<T>(List<T> resultContainer, string filter, string[] folders = null,int count = 0) where T : Object
        {
            var type = typeof(T);
            
            var ids  = folders == null ? 
                AssetDatabase.FindAssets(filter) : 
                AssetDatabase.FindAssets(filter, folders.
                    Where(x => !string.IsNullOrEmpty(x)).
                    Apply(x => x.TrimEndPath()).
                    ToArray());

            foreach (var id in ids) {
                var assetPath = AssetDatabase.GUIDToAssetPath(id);
                if (string.IsNullOrEmpty(assetPath)) {
                    Debug.LogErrorFormat("Asset importer {0} with NULL path detected", id);
                    continue;
                }

                var asset = AssetDatabase.LoadAssetAtPath(assetPath, type) as T;
                if (asset) resultContainer.Add(asset);
                
                if(count<=0) continue;
                if(resultContainer.Count >= count) break;
            }

            return resultContainer;
        }

        /// <summary>
        /// load components
        /// </summary>
        /// <param name="type">component type</param>
        /// <param name="filter"></param>
        /// <param name="folders">folder filter</param>
        /// <returns>list of found items</returns>
        public static List<Object> GetComponentsAssets(Type type, string filter = "", string[] folders = null)
        {
            if (type.IsComponent() == false) return new List<Object>();

            var filterText   = string.Format(FilterTemplate, UnityTypeExtension.gameObjectType.Name, filter);
            var assets       = GetAssets<GameObject>(filterText, folders);
            var resultAssets = new List<Object>();

            foreach (var t in assets) {
                var targetComponents = t.GetComponents(type);
                resultAssets.AddRange(targetComponents);
            }

            return resultAssets;
        }

        #endregion
    }
}