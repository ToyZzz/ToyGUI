using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ToyGUI
{
    public class FUIPackageMgr
    {
        private static FUIPackageMgr _instance = null;

        public Dictionary<string, FUIPackage> _packageInstById = new Dictionary<string, FUIPackage>();
        public Dictionary<string, FUIPackage> _packageInstByName = new Dictionary<string, FUIPackage>();
        public List<FUIPackage> _packageList = new List<FUIPackage>();

     
        public static FUIPackageMgr Instance()
        {
            if (_instance == null)
            {
                _instance = new FUIPackageMgr();
            }
            return _instance;
        }

        public FUIPackage AddPackage(string descFilePath)
        {
#if UNITY_EDITOR
            if (descFilePath.StartsWith("Assets/"))
            {
                return AddPackage(descFilePath,(string name, string extension, System.Type type) =>
                    {
                        return AssetDatabase.LoadAssetAtPath(name + extension, type);
                    });
            }
#endif
            return AddPackage(descFilePath, (string name, string extension, System.Type type) =>
            {
                return Resources.Load(name, type);
            });
        }

        public FUIPackage AddPackage(string assetPath, FUIPackage.LoadResource loadFunc)
        {
            if (_packageInstById.ContainsKey(assetPath))
                return _packageInstById[assetPath];

         
            TextAsset asset = (TextAsset)loadFunc(assetPath + "_fui", ".bytes", typeof(TextAsset));
            if (asset == null)
            {
                if (Application.isPlaying)
                    throw new Exception("FairyGUI: Cannot load ui package in '" + assetPath + "'");
                else
                    Debug.LogWarning("FairyGUI: Cannot load ui package in '" + assetPath + "'");
            }
            
            FUIByteBuffer buffer = new FUIByteBuffer(asset.bytes);

            FUIPackage pkg = new FUIPackage();
            pkg.m_loadFunc = loadFunc;
            pkg.AssetPath = assetPath;
            if (!pkg.LoadPackage(buffer, assetPath, assetPath))
            {
                return null;
            }
            _packageInstById[pkg.Id] = pkg;
            _packageInstByName[pkg.Name] = pkg;
            _packageInstById[assetPath] = pkg;
            _packageList.Add(pkg);
            return pkg;
        }
    }
}