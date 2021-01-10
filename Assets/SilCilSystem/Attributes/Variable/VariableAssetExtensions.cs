﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEditor;
using SilCilSystem.Variables.Base;

namespace SilCilSystem.Editors
{
    public static class VariableAssetExtensions
    {
        public static T GetSubVariable<T>(this VariableAsset parent) where T : VariableAsset
        {
            return GetSubVariableCollection<T>(parent).FirstOrDefault();
        }

        public static VariableAsset GetSubVariable(this VariableAsset parent, Type type)
        {
            return GetSubVariableCollection(parent, type).FirstOrDefault();
        }

        public static T[] GetSubVariables<T>(this VariableAsset parent) where T : VariableAsset
        {
            return GetSubVariableCollection<T>(parent).ToArray();
        }

        public static VariableAsset[] GetSubVariables(this VariableAsset parent, Type type)
        {
            return GetSubVariableCollection(parent, type).ToArray();
        }

        public static VariableAsset[] GetAllVariables(this VariableAsset parent)
        {
            return GetSubVariableCollection<VariableAsset>(parent).ToArray();
        }

        [Conditional("UNITY_EDITOR")]
        public static void AddSubVariable<T>(this VariableAsset parent, bool registerUndo = true) where T : VariableAsset
        {
            var asset = ScriptableObject.CreateInstance<T>();
            if (registerUndo) Undo.RegisterCreatedObjectUndo(asset, "Add Sub Variable");
            SetSubVariable(parent, asset);
        }

        [Conditional("UNITY_EDITOR")]
        public static void AddSubVariable(this VariableAsset parent, Type type, bool registerUndo = true)
        {
            var asset = ScriptableObject.CreateInstance(type) as VariableAsset;
            if (registerUndo) Undo.RegisterCreatedObjectUndo(asset, "Add Sub Variable");
            SetSubVariable(parent, asset);
        }
        
        // Conditional属性はメソッドの"呼び出し"を条件付きでコンパイルする.
        // メソッド自体のコンパイルはされるらしく、AssetDatabaseでコンパイルエラー吐く.

        [Conditional("UNITY_EDITOR")]
        public static void AddSubVariables(this VariableAsset parent, bool registerUndo, params Type[] types)
        {
#if UNITY_EDITOR
            List<VariableAsset> assets = new List<VariableAsset>();
            foreach(var type in types)
            {
                var asset = ScriptableObject.CreateInstance(type) as VariableAsset;
                if (registerUndo) Undo.RegisterCreatedObjectUndo(asset, "Add Sub Variable");
                SetSubVariable(parent, asset, false);
                assets.Add(asset);
            }

            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(parent));
            foreach(var asset in assets)
            {
                VariableAttributeList.CallAttached(asset, parent);
            }
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(parent));
#endif
        }

        [Conditional("UNITY_EDITOR")]
        public static void RenameSubVariables(this VariableAsset parent)
        {
#if UNITY_EDITOR
            if (AssetDatabase.IsSubAsset(parent)) return;
            
            // 名前が変わった場合のみImportする.
            bool import = false;
            foreach (var asset in parent.GetAllVariables())
            {
                if (asset == parent) continue;

                string name = asset.name;
                SetName(asset, parent.name);

                if (asset.name == name) continue;
                import = true;
            }

            if (import) AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(parent));
#endif
        }

        [Conditional("UNITY_EDITOR")]
        private static void SetSubVariable(VariableAsset parent, VariableAsset asset, bool importAndOnAttached = true)
        {
#if UNITY_EDITOR
            SetName(asset, parent.name);
            asset.hideFlags = HideFlags.HideInHierarchy;

            AssetDatabase.AddObjectToAsset(asset, parent);

            if (importAndOnAttached)
            {
                VariableAttributeList.CallAttached(asset, parent);
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(parent));
            }
#endif
        }

        [Conditional("UNITY_EDITOR")]
        private static void SetName(VariableAsset asset, string parentName)
        {
            string name = VariableAttributeList.GetName(asset, parentName);
            if (name == null) return;
            if (name == asset.name) return;
            asset.name = name;
        }

        private static IEnumerable<T> GetSubVariableCollection<T>(VariableAsset parent)
        {
#if UNITY_EDITOR
            string path = AssetDatabase.GetAssetPath(parent);
            foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(path))
            {
                if (asset is T value)
                {
                    yield return value;
                }
            }
#endif
            yield break;
        }

        private static IEnumerable<VariableAsset> GetSubVariableCollection(VariableAsset parent, Type type)
        {
#if UNITY_EDITOR
            string path = AssetDatabase.GetAssetPath(parent);
            foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(path))
            {
                if (asset is VariableAsset value && type.IsAssignableFrom(asset.GetType()))
                {
                    yield return value;
                }
            }
#endif
            yield break;
        }
    }
}